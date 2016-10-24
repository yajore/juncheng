using Lawyers.BizEntities;
using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lawyers.Web.manage
{
    public partial class admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {

                    ReferUrl = Request.Url.AbsolutePath;

                    var user = CookiesHelper.GetCookieValue("juncheng_platform_user", "uinfo");
                    if (user == null || user == "")
                    {
                        Page.Response.Redirect("login.html?referUrl=" + ReferUrl);
                        return;
                    }
                    user = HttpUtility.UrlDecode(user);
                    UInfo = user;
                    UserData = JsonHelper.Build<CustomerLoginData>(user);
                    if (UserData == null)
                    {
                        Page.Response.Redirect("login.html?referUrl=" + ReferUrl);
                        return;
                    }


                    var menus = Utilities.Cache.Instance["juncheng_platform_menu_" + UserData.UserRoleID];
                    if (menus == null)
                    {
                        Page.Response.Redirect("login.html?referUrl=" + ReferUrl);
                        Page.Response.End();
                        return;
                    }
                    MenuDatas = (List<MenuData>)menus;

                    var extends = Utilities.Cache.Instance["juncheng_rolemenuextends_" + UserData.UserRoleID];
                    if (extends != null)
                    {

                        RightKeys = JsonHelper.ReBuilder(((List<MenuExtentData>)extends).Where(c => c.Url == ReferUrl).Select(c => c.RightKey).ToArray());
                    }

                }
                catch (Exception ex)
                {
                    if (ex.GetType().Name != "ThreadAbortException")
                    {
                        Logger.WriteToFile("【登录】", ex);
                    }

                }

                //Page.DataBind();

            }
        }

        public string ReferUrl { get; set; }
        public string UInfo { get; set; }
        public string RightKeys { get; set; }
        ///manage/views/PublishActivity.aspx
        ///
        public CustomerLoginData UserData { get; set; }

        public List<MenuData> MenuDatas { get; set; }

    }
}