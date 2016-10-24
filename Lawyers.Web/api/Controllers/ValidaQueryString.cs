using Lawyers.BizEntities;
using System;

namespace Lawyers.Web.Controllers
{
    public class ValidaQueryString
    {


        public static OperationResult ValidaDevice(HeaderInfo headinfo)
        {
            var result = new OperationResult();

            if (headinfo == null)
            {
                result.ErrCode = 4;
                result.Message = "HeaderInfo参数不正确";
                return result;
            }

            if (headinfo.DeviceID == 0)
            {
                result.ErrCode = 4;
                result.Message = "HeaderInfo.DeviceID参数不正确";
                return result;
            }

            result.ErrCode = 0;
            result.Message = "ok";
            return result;
        }

        public static OperationResult Valida(HeaderInfo headinfo)
        {
            var result = new OperationResult();

            if (headinfo == null)
            {
                result.ErrCode = 4;
                result.Message = "HeaderInfo参数不正确";
                return result;
            }

            if (headinfo.DeviceID <= 0)
            {
                result.ErrCode = 4;
                result.Message = "HeaderInfo.DeviceID参数不正确";
                return result;
            }

            if (headinfo.UserID <= 0)
            {
                result.ErrCode = 4;
                result.Message = "HeaderInfo.UserID参数不正确";
                return result;
            }

            if (String.IsNullOrEmpty(headinfo.DisplayName))
            {
                result.ErrCode = 4;
                result.Message = "HeaderInfo.DisplayName参数不正确";
                return result;
            }
            result.ErrCode = 0;
            result.Message = "ok";
            return result;
        }
    }
}