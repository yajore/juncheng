using Lawyers.Utilities;
using System.Web;

namespace Lawyers.Web.manage.handler
{
    /// <summary>
    /// logout 的摘要说明
    /// </summary>
    public class logout : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["clear"] == "true")
            {
                Cache.Instance.Remove("juncheng_menus");
            }
            CookiesHelper.RemoveCookie("juncheng_platform_user");

            context.Response.Redirect("../login.html?referUrl=" + context.Request.UrlReferrer == null ? string.Empty : context.Request.UrlReferrer.AbsolutePath);
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