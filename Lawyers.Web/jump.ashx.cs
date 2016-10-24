using System.Configuration;
using System.Web;
using Vaolon.Wechat.Framework.Model;
using Vaolon.Wechat.Framework.OAuth;

namespace Lawyers.Web
{
    /// <summary>
    /// jump 的摘要说明
    /// </summary>
    public class jump : IHttpHandler
    {
        static string resutl_url = ConfigurationManager.AppSettings["return_url"];
        static string wxappid = ConfigurationManager.AppSettings["wxappid"];
        static string wx_auth = ConfigurationManager.AppSettings["wx_auth_url"];
        public void ProcessRequest(HttpContext context)
        {

            var manager = new OAuthHelper(wxappid);
            var return_url = context.Request["return_url"] ?? "";
            var url = manager.BuildOAuthUrl(HttpUtility.UrlEncode(wx_auth),
                OAuthScope.UserInfo, HttpUtility.UrlEncode(return_url));
            context.Response.Redirect(url);
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