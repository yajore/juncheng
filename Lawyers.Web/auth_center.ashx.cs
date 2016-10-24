using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Vaolon.Wechat.Framework.Api;
using Vaolon.Wechat.Framework.Model;
using Vaolon.Wechat.Framework.Model.ApiRequests;
using Vaolon.Wechat.Framework.Model.ApiResponses;

namespace Lawyers.Web
{
    /// <summary>
    /// auth_center 的摘要说明
    /// </summary>
    public class auth_center : IHttpHandler
    {
        private static string wxappid = ConfigurationManager.AppSettings["wxappid"];
        private static string wxappsecret = ConfigurationManager.AppSettings["wxappsecret"];
        private static string domian = ConfigurationManager.AppSettings["domain"];
        private static string accesstokenKey = "webpage_accesstoken";
        public void ProcessRequest(HttpContext context)
        {

            var accesstoken = CookiesHelper.GetCookie(accesstokenKey);
            SnsOAuthAccessTokenResponse AccessToken = null;
            var m_client = new DefaultApiClient();
            var m_appIdent = new AppIdentication(wxappid, wxappsecret);

            string State = context.Request["state"];

            string jumpUrl = "index.html";
            try
            {

                if (accesstoken == null)
                {
                    string code = context.Request["Code"];




                    var request = new SnsOAuthAccessTokenRequest
                    {
                        AppID = m_appIdent.AppID,
                        AppSecret = m_appIdent.AppSecret,
                        Code = code
                    };

                    AccessToken = m_client.Execute(request);

                    if (AccessToken.IsError)
                    {
                        throw new Exception("获取网页授权accesstoken失败。" +
                            JsonHelper.ReBuilder(request) + "\r\n" +
                            AccessToken.ErrorMessage);
                    }

                    CookiesHelper.AddCookie("webpage_accesstoken",
                        JsonHelper.ReBuilder(AccessToken),
                        DateTime.Now.AddSeconds(AccessToken.ExpiresIn - 600));
                }
                else
                {
                    AccessToken = JsonHelper.Build<SnsOAuthAccessTokenResponse>(accesstoken.Value);
                }

                string unionid = AccessToken.UnionId;

                if (String.IsNullOrEmpty(unionid))
                {
                    unionid = AccessToken.OpenId;
                }

                string openid = AccessToken.OpenId;
                var query = new RequestOperation<string>();

                query.Header = new HeaderInfo()
                {
                    DeviceID = 5,
                    DisplayName = "customer",
                    UserID = 1
                };

                query.Body = unionid;

                var service = new CustomerBP();

                var data = service.LoginByWechatAccount(query);
                //不存在此用户
                if (data.ErrCode == 1)
                {
                    var wexinInfo = new SnsUserInfoRequest
                    {
                        OAuthToken = AccessToken.AccessToken,
                        OpenId = AccessToken.OpenId,
                        Lang = Language.CN
                    };

                    var userinfo_res = m_client.Execute(wexinInfo);
                    if (userinfo_res.IsError)
                    {
                        throw new Exception("获取用户信息失败2。" +
                            JsonHelper.ReBuilder(wexinInfo) + "\r\n" +
                            JsonHelper.ReBuilder(userinfo_res));
                    }


                    #region 注册

                    string url = HttpUtility.UrlDecode(State);
                    int invateUserId = 0;
                    if (!string.IsNullOrEmpty(State))
                    {
                        url = domian + unescape(url);
                        invateUserId = getInveteUser(url);
                    }

                    var register = new RequestOperation<RegisterData>();
                    register.Header = query.Header;
                    register.Body = new RegisterData();
                    register.Body.Account = "";
                    register.Body.WechatAccount = unionid;
                    register.Body.QQAccount = "";
                    register.Body.Face = userinfo_res.HeadImageUrl;
                    register.Body.NickName = register.Body.Name = userinfo_res.NickName;
                    register.Body.Password = "123456";

                    var register_res = service.Register(register);

                    if (register_res.ErrCode != 0)
                    {
                        throw new Exception("注册用户失败：" + register_res.Message);
                    }


                    LoginManage.SaveUserWeixinOpenId(userinfo_res.OpenId);
                    LoginManage.SaveUserInfo(register_res.Body.UserID);


                    if (!string.IsNullOrEmpty(State))
                    {
                        jumpUrl = replaceInveteUserParam(url);
                    }


                    #endregion


                }
                else
                {

                    //不准修改，此处用于微信支付！！！
                    LoginManage.SaveUserWeixinOpenId(AccessToken.OpenId);

                    LoginManage.SaveUserInfo(data.Body.UserID);

                    if (!string.IsNullOrEmpty(State))
                    {
                        string url = domian + HttpUtility.UrlDecode(State);
                        jumpUrl = replaceInveteUserParam(url);
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.WriteException("【微信网页授权】", ex, "");
            }

            context.Response.Redirect(jumpUrl);
        }


        private string unescape(string s)
        {
            if (s.Contains("%u"))
            {
                string str = s.Remove(0, 2);//删除最前面两个＂%u＂  
                string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔  
                byte[] byteArr = new byte[strArr.Length * 2];
                for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
                {
                    byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //把十六进制形式的字串符串转换为二进制字节  
                    byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
                }
                str = System.Text.Encoding.Unicode.GetString(byteArr); //把字节转为unicode编码  
                return str;
            }
            return s;

        }

        private int getInveteUser(string url)
        {
            Uri urls = new Uri(url);
            int userid = 0;


            string query = urls.Query.Contains("?") ? urls.Query.Split('?')[1] : urls.Query;
            foreach (var param in query.Split('&'))
            {

                string[] queryString = param.Split('=');
                if (queryString[0] == "sid" && queryString.Length > 1)
                {
                    if (int.TryParse(queryString[1], out userid))
                    {
                        return userid;
                    }
                }

            }

            return 0;
        }

        private string replaceInveteUserParam(string url)
        {
            int index = url.IndexOf("&sid");
            if (index > -1)
            {
                return url.Remove(index, url.Length - index);
            }
            return url;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}