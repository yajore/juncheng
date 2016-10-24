using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using Lawyers.Web.Controllers;
using System;
using System.Linq;
using System.Web;

namespace Lawyers.Web.manage.handler
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var service = new CustomerBP();
            var result = new OperationResult<CustomerLoginData>();

            var queryString = context.Request["queryString"];
            if (String.IsNullOrEmpty(queryString))
            {
                result.Message = "请求参数不正确";
                context.Response.Write(JsonHelper.ReBuilder(result));
                context.Response.End();
            }
            var req = JsonHelper.Build<RequestOperation<UserLoginData>>(queryString);

            //req.Body.Account = "18857303534";
            //req.Body.Password = "123123";


            var verify = ValidaQueryString.ValidaDevice(req.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                context.Response.Write(JsonHelper.ReBuilder(result));
                context.Response.End();
            }
            result = service.LoginByAccount(req);
            if (result.ErrCode == 0)
            {
                //如果是平台方
                //获取权限
                var setcookie = HttpUtility.UrlEncode(JsonHelper.ReBuilder(result.Body), System.Text.Encoding.GetEncoding("UTF-8"));
                CookiesHelper.AddCookie("juncheng_platform_user", "uinfo", setcookie, System.DateTime.Now.AddDays(1));

                if (result.Body.UserRoleID > 0)
                {

                    if (String.IsNullOrEmpty(req.Body.ReferUrl))
                    {
                        result.Message = "UserMgmt.aspx";
                    }
                    else
                    {
                        result.Message = req.Body.ReferUrl;
                    }

                    if (Cache.Instance["juncheng_platform_menu_" + result.Body.UserRoleID] == null)
                    {
                        var menus = new RoleRightBP().GetTreeMenus(result.Body.UserRoleID);
                        if (menus.ErrCode == 0)
                        {

                            Cache.Instance.Add("juncheng_platform_menu_" + result.Body.UserRoleID, menus.Body);

                        }
                        else
                        {
                            result.ErrCode = menus.ErrCode;
                            result.Message = menus.Message;
                        }

                    }

                    if (Cache.Instance["juncheng_rolemenuextends_" + result.Body.UserRoleID] == null)
                    {
                        //result.Body.UserRoleID
                        var extends = new RoleRightBP().GetRoleMenuExtends();
                        if (extends.ErrCode == 0)
                        {
                            var rightKeys = extends.Body.Where(c => c.RoleID == result.Body.UserRoleID).ToList();
                            Cache.Instance.Add("juncheng_rolemenuextends_" + result.Body.UserRoleID, rightKeys);

                        }
                        else
                        {
                            result.ErrCode = extends.ErrCode;
                            result.Message = extends.Message;
                        }

                    }

                }
                else
                {
                    result.ErrCode = -1;
                    result.Message = "抱歉，您没有相关权限";
                }
            }

            context.Response.Write(JsonHelper.ReBuilder(result));
            context.Response.End();
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