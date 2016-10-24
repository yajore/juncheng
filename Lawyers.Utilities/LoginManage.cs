using System;
using System.Web;

namespace Lawyers.Utilities
{
    public class LoginManage
    {
        private const string cookieUidKey = "juncheng_h5_uid";
        private const string cookieOpenidKey = "juncheng_h5_openid";
        private const string uid = "UId";


        public static int GetUId()
        {
            if (System.Web.HttpContext.Current.Request.Cookies[cookieUidKey] == null)
            {
                return 0;
            }
            string text = ASEDecrypt.Decrypt(System.Web.HttpContext.Current.Request.Cookies[cookieUidKey].Value);

            int uId = 0;
            //解析：部分
            string[] values = text.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            switch (values[0])
            {
                case uid:
                    uId = Convert.ToInt32(values[1]);
                    break;
            }
            return uId;
        }

        public static string GetOpenId()
        {
            if (System.Web.HttpContext.Current.Request.Cookies[cookieOpenidKey] == null)
            {
                return String.Empty;
            }

            return ASEDecrypt.Decrypt(System.Web.HttpContext.Current.Request.Cookies[cookieOpenidKey].Value);
        }

        public static void SaveUserInfo(int customerno)
        {
            string value = string.Format("{0}:{1}", uid, customerno);//用户编号
            HttpCookie cookie = new HttpCookie(cookieUidKey, ASEDecrypt.Encrypt(value));
            //cookie.Domain = cookie_domain;
            cookie.Path = "/";
            cookie.Expires = DateTime.Now.AddDays(15);//15天之内面登录
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SaveUserWeixinOpenId(string openid)
        {
            HttpCookie cookie = new HttpCookie(cookieOpenidKey, ASEDecrypt.Encrypt(openid));
            //cookie.Domain = cookie_domain;
            cookie.Path = "/";
            //cookie.Expires = DateTime.Now.AddDays(15);//15天之内面登录
            HttpContext.Current.Response.Cookies.Add(cookie);
        }


    }
}
