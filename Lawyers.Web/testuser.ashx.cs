using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lawyers.Web
{
    /// <summary>
    /// testuser 的摘要说明
    /// </summary>
    public class testuser : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var resData = new OperationResult<CustomerLoginData>();
            var uid = context.Request["id"] ?? "100002";
            var service = new CustomerBP();
            resData = service.GetUserById(new RequestOperation<int>()
            {
                Body = int.Parse(uid),
                Header = new HeaderInfo()
                {

                }
            });
            if (resData.ErrCode == 0)
            {

                LoginManage.SaveUserInfo(resData.Body.UserID);
            }

            context.Response.Redirect("index.html");
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