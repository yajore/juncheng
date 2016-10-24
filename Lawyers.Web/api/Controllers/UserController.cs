using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System.Web.Http;

namespace Lawyers.Web.Controllers
{
    /// <summary>
    /// 用户api
    /// </summary>
    public class UserController : ApiController
    {
        CustomerBP service = new CustomerBP();

        [HttpPost]
        public QueryResultList<CustomerData> GetCustomers(QueryRequest<CustomerQueryData> request)
        {
            var result = new QueryResultList<CustomerData>();

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


            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetCustomers(request);
            return result;
        }

        [HttpPost]
        public QueryResultList<CustomerLawyerData> GetLawyers(QueryRequest<CustomerQueryData> request)
        {
            var result = new QueryResultList<CustomerLawyerData>();

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


            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetLawyers(request);
            return result;
        }


        [HttpPost]
        public OperationResult<CustomerLawyerData> GetCustomerById(RequestOperation<int> request)
        {
            var result = new OperationResult<CustomerLawyerData>();

            if (request == null)
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
        public OperationResult SetCustomerStatus(RequestOperation<CustomerStatusData> request)
        {
            var result = new OperationResult();

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SetCustomerStatus(request);
            return result;
        }

        [HttpPost]
        public QueryResultList<CustomerSkillData> GetCustomerSkill(RequestOperation request)
        {
            var result = new QueryResultList<CustomerSkillData>();

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new CustomerBP().GetCustomerSkill(request);
            return result;
        }

        [HttpPost]
        public OperationResult SetCustomerSkill(RequestOperation<CustomerSkillData> request)
        {
            var result = new OperationResult();

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SetCustomerSkill(request);
            return result;
        }

        [HttpPost]
        public OperationResult<int> SetCustomer([FromBody]string requestStr)
        {
            var result = new OperationResult<int>();

            var request = JsonHelper.Build<RequestOperation<CustomerLawyerData>>(requestStr);

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }

            if (request.Body.UserID == 0)
            {
                result = new CustomerBP().AddCustomer(request);
            }
            else
            {
                var setData = new CustomerBP().SetCustomer(request);
                result.ErrCode = setData.ErrCode;
                result.Message = setData.Message;
            }

            return result;
        }

        [HttpPost]
        public OperationResult SetCustomerSort([FromBody]string requestStr)
        {
            var result = new OperationResult();

            var request = JsonHelper.Build<RequestOperation<CustomerSortData>>(requestStr);

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }

            result = new CustomerBP().SetCustomerSort(request);
            return result;
        }
    }
}
