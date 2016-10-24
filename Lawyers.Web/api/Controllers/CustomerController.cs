using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Lawyers.Web.Controllers
{
    /// <summary>
    /// 用户api
    /// </summary>
    public class CustomerController : ApiController
    {
        CustomerBP service = new CustomerBP();
        //GetHomePageData(RequestOperation request)
        [HttpPost]
        public OperationResult VerifyAccount([FromBody]string queryString)
        {
            var result = new OperationResult();

            var request = JsonHelper.Build<RequestOperation<CustomerAccountQueryData>>(queryString);

            var verify = ValidaQueryString.ValidaDevice(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.VerifyAccount(request);
            return result;
        }

        [HttpPost]
        public OperationResult<CustomerLoginData> LoginByAccount([FromBody]string queryString)
        {
            var result = new OperationResult<CustomerLoginData>();

            var request = JsonHelper.Build<RequestOperation<UserLoginData>>(queryString);

            var verify = ValidaQueryString.ValidaDevice(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.LoginByAccount(request);
            return result;
        }

        [HttpPost]
        public OperationResult SeCustomerPwd(RequestOperation<CustomerPwdData> request)
        {
            var result = new OperationResult();

            if (request == null || request.Body == null || request.Body.UserID == 0 || String.IsNullOrEmpty(request.Body.Password))
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var verify = ValidaQueryString.ValidaDevice(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SeCustomerPwd(request);


            return result;
        }

        [HttpPost]
        public OperationResult<CustomerLawyerData> GetInfoById([FromBody]string queryString)
        {
            var result = new OperationResult<CustomerLawyerData>();

            if (string.IsNullOrEmpty(queryString) || queryString == "{}")
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var request = JsonHelper.Build<RequestOperation<int>>(queryString);

            if (request.Body <= -1)
            {
                result.Message = "请求参数为NULL";
                return result;
            }


            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetCustomerById(request);
            return result;
        }

        [HttpPost]
        public OperationResult ModifyInfoByFields([FromBody]string queryString)
        {
            var result = new OperationResult();

            if (string.IsNullOrEmpty(queryString) || queryString == "{}")
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var request = JsonHelper.Build<RequestOperation<Dictionary<string, string>>>(queryString);

            if (request.Body == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }
            if (request.Body.Count == 0)
            {
                result.ErrCode = 0;
                result.Message = "ok";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.ModifyCustomerByFields(request);
            return result;
        }

        [HttpPost]
        public QueryResultList<CustomerLawyerShowData> Lawyers([FromBody]string queryString)
        {
            var result = new QueryResultList<CustomerLawyerShowData>();


            var request = JsonHelper.Build<QueryRequest<CustomerQueryData>>(queryString);

            if (request.Body == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            if (request.PageInfo == null)
            {
                result.Message = "分页参数为NULL";
                return result;
            }


            var verify = ValidaQueryString.ValidaDevice(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new CustomerBP().GetShowLawyers(request);
            return result;
        }

        [HttpPost]
        public OperationResult<CustomerLawyerData> GetCustomerById([FromBody]string queryString)
        {
            var result = new OperationResult<CustomerLawyerData>();

            var request = JsonHelper.Build<RequestOperation<int>>(queryString);

            if (request == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var verify = ValidaQueryString.ValidaDevice(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetCustomerById(request);
            return result;
        }

    }
}
