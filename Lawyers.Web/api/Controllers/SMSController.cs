using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System.Web.Http;

namespace Lawyers.Web.Controllers
{
    /// <summary>
    /// 用户api
    /// </summary>
    public class SMSController : ApiController
    {
        MsgBP service = new MsgBP();


        [HttpPost]
        public OperationResult Send2([FromBody]string queryString)
        {
            var result = new OperationResult();

            if (string.IsNullOrEmpty(queryString) || queryString == "{}")
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            //RequestOperation

            var request = JsonHelper.Build<RequestOperation<ReqMsgData>>(queryString);
            if (request.Body == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }
            if (request.Body.Mobile.Length != 11 ||
               request.Body.MsgType == 0)
            {
                result.Message = "请求参数错误";
                return result;
            }


            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SendMsg(request);
            return result;
        }

        [HttpPost]
        public OperationResult Send([FromBody]string queryString)
        {
            var result = new OperationResult();

            if (string.IsNullOrEmpty(queryString))
            {
                result.Message = "请求参数为NULL";
                return result;
            }


            //queryString = ASEDecrypt.Decrypt(queryString);
            //RequestOperation

            var request = JsonHelper.Build<RequestOperation<ReqMsgData>>(queryString);
            if (request == null || request.Body == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }
            if (request.Body.Mobile.Length != 11 ||
               request.Body.MsgType == 0)
            {
                result.Message = "请求参数错误";
                return result;
            }


            var verify = ValidaQueryString.ValidaDevice(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SendMsg(request);
            return result;
        }


    }
}
