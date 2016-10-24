using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System.Web;

namespace Lawyers.Web.api
{
    /// <summary>
    /// customerhander 的摘要说明
    /// </summary>
    public class customerhander : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var resData = new OperationResult<CustomerLoginData>();

            var cookieUser = LoginManage.GetUId();

            //
            if (cookieUser <= 0)
            {
                resData.Message = "用户未登录";
            }
            else
            {


                var service = new CustomerBP();
                var res = service.GetUserById(new RequestOperation<int>()
                {
                    Body = cookieUser,
                    Header = new HeaderInfo()
                    {

                    }
                });
                if (res.ErrCode == 0)
                {
                    resData.Body = new CustomerLoginData()
                    {
                        UserID = res.Body.UserID,
                        NickName = res.Body.NickName,
                        Name = res.Body.Name,
                        Face = res.Body.Face
                    };
                    resData.ErrCode = 0;
                }
            }
            context.Response.Write(JsonHelper.ReBuilder(resData));
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