using Lawyers.BizEntities;
using Lawyers.BizProcess;
using System.Web.Http;

namespace Lawyers.Web.Controllers
{
    public class RoleRightController : ApiController
    {
        RoleRightBP service = new RoleRightBP();

        [HttpPost]
        public QueryResultList<RoleData> GetRoles(RequestOperation<string> query)
        {
            var result = new QueryResultList<RoleData>();


            var verify = ValidaQueryString.Valida(query.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetRoles(query.Body);
            return result;
        }


        [HttpPost]
        public QueryResultList<RoleData> SetRole(RequestOperation<RoleData> request)
        {
            var result = new QueryResultList<RoleData>();

            if (request == null ||
                request.Body == null)
            {
                result.Message = "参数有误";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SetRole(request);
            return result;
        }

        [HttpPost]
        public OperationResult<MenuTreeData> GetTree(RequestOperation request)
        {
            var result = new OperationResult<MenuTreeData>();

            if (request == null)
            {
                result.Message = "参数有误";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetWebTreeMenus();
            return result;
        }

        [HttpPost]
        public QueryResultList<RoleRightListData> GetUserRoleRightList(RequestOperation<int> request)
        {
            var result = new QueryResultList<RoleRightListData>();

            if (request == null || request.Body == 0)
            {
                result.Message = "参数有误";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetUserRoleRightList(request);
            return result;
        }


        [HttpPost]
        public OperationResult SetUserRoleRightList(RequestOperation<SetRoleRightData> request)
        {
            var result = new OperationResult();

            if (request == null || request.Body == null ||
                request.Body.RightIDs == null || request.Body.RightIDs.Count == 0 ||
                request.Body.RoleID <= 0)
            {
                result.Message = "参数有误";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SetUserRoleRightList(request);
            return result;
        }


        [HttpPost]
        public QueryResultList<UserRoleData> GetUserRoles(QueryRequest<UserRoleQueryData> request)
        {
            var result = new QueryResultList<UserRoleData>();

            if (request == null || request.Body == null)
            {
                result.Message = "参数有误";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.GetUserRoles(request);
            return result;
        }

        [HttpPost]
        public OperationResult UpdateUserRole(RequestOperation<UserRoleUpdateData> request)
        {
            var result = new OperationResult();

            if (request == null)
            {
                result.Message = "参数有误";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.UpdateUserRoles(request);
            return result;
        }
        [HttpPost]
        public OperationResult UpdatePwd(RequestOperation<UserPasswordSetData> request)
        {
            var result = new OperationResult();

            if (request == null || request.Body == null)
            {
                result.Message = "参数有误";
                return result;
            }

            if (request.Body.UserID == 0 || string.IsNullOrEmpty(request.Body.Password) || string.IsNullOrEmpty(request.Body.MgrPassword))
            {
                result.Message = "参数有误";
                return result;
            }
            if (request.Body.MgrPassword != "888")
            {
                result.Message = "参数有误";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = service.SetCustomerPwd(request);
            return result;
        }
    }
}