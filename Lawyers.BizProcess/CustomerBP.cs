using Lawyers.BizEntities;
using Lawyers.Utilities;
using Mango.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lawyers.BizProcess
{
    public class CustomerBP
    {
        public QueryResultList<CustomerData> GetCustomers(QueryRequest<CustomerQueryData> query)
        {
            var result = new QueryResultList<CustomerData>();

            try
            {

                result = CustomerDA.GetCustomers(query);
                if (result.Body == null)
                {
                    result.Body = new List<CustomerData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetCustomers", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<CustomerLawyerData> GetLawyers(QueryRequest<CustomerQueryData> query)
        {
            var result = new QueryResultList<CustomerLawyerData>();

            try
            {

                result = CustomerDA.GetLawyers(query);
                if (result.Body == null)
                {
                    result.Body = new List<CustomerLawyerData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetLawyers", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<CustomerLawyerShowData> GetShowLawyers(QueryRequest<CustomerQueryData> query)
        {
            var result = new QueryResultList<CustomerLawyerShowData>();

            try
            {

                result = CustomerDA.GetShowLawyers(query);
                if (result.Body == null)
                {
                    result.Body = new List<CustomerLawyerShowData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetShowLawyers", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<CustomerLawyerData> GetCustomerById(RequestOperation<int> request)
        {
            var result = new OperationResult<CustomerLawyerData>();

            try
            {

                var customer = CustomerDA.GetCustomerById(request.Body);
                if (customer == null)
                {
                    throw new Exception("不存在该用户");
                }
                result.Body = customer;

                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetCustomerById", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<CustomerLoginData> GetUserById(RequestOperation<int> request)
        {
            var result = new OperationResult<CustomerLoginData>();

            try
            {

                var customer = CustomerDA.GetUserById(request.Body);
                if (customer == null)
                {
                    throw new Exception("不存在该用户");
                }

                if (customer.UserStatus != 10)
                {
                    result.Message = "当前用户被冻结，请联系客服";
                    return result;

                }

                result.Body = customer;

                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetCustomerById", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult VerifyAccount(RequestOperation<CustomerAccountQueryData> request)
        {
            var result = new OperationResult();

            try
            {

                var row = CustomerDA.VerifyAccount(request);

                if (row == null || row.UserID <= 0)
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }
                else
                {
                    result.ErrCode = 1;
                    result.Message = "已存在此用户";
                }


            }
            catch (Exception ex)
            {
                //System.Reflection.MethodInfo.GetCurrentMethod().Name
                Logger.WriteException("VerifyAccount", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<CustomerLoginData> LoginByAccount(RequestOperation<UserLoginData> request)
        {
            var result = new OperationResult<CustomerLoginData>();

            try
            {
                request.Body.Password = MD5Helper.Encode(request.Body.Password);
                result.Body = CustomerDA.LoginByAccount(request.Body);

                if (result.Body == null)
                {

                    result.ErrCode = 1;
                    result.Message = "用户名或密码错误";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("LoginByAccount", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<CustomerLoginData> LoginByWechatAccount(RequestOperation<string> request)
        {
            var result = new OperationResult<CustomerLoginData>();

            try
            {

                result.Body = CustomerDA.LoginByWechatAccount(request);

                if (result.Body == null)
                {

                    result.ErrCode = 1;
                    result.Message = "不存在此用户";
                }
                else
                {

                    if (result.Body.UserStatus != 10)
                    {
                        result.Message = "当前用户被冻结，请联系客服";
                        return result;

                    }
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("LoginByWechatAccount", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<CustomerLoginData> Register(RequestOperation<RegisterData> request)
        {
            var result = new OperationResult<CustomerLoginData>();

            try
            {
                var requestTran = new RequestOperation<CustomerRegisterData>();
                requestTran.Header = request.Header;
                requestTran.Body = new CustomerRegisterData();
                requestTran.Body.Customer = new CustomerData();
                requestTran.Body.Customer.AuditStatus = 10;
                requestTran.Body.Customer.CustomerType = 1;
                requestTran.Body.Customer.Face = request.Body.Face;
                requestTran.Body.Customer.Mobile = request.Body.Account;
                requestTran.Body.Customer.Name = request.Body.Name;
                requestTran.Body.Customer.NickName = request.Body.NickName;

                requestTran.Body.Login = new LoginData();
                requestTran.Body.Login.Account = request.Body.Account;
                requestTran.Body.Login.QQAccount = request.Body.QQAccount;
                requestTran.Body.Login.WechatAccount = request.Body.WechatAccount;
                requestTran.Body.Login.Password = String.IsNullOrEmpty(request.Body.Password) ? MD5Helper.Encode("123456") : MD5Helper.Encode(request.Body.Password);
                requestTran.Body.Login.UserRoleID = 1;
                requestTran.Body.Login.UserStatus = 10;
                requestTran.Body.Login.UserFrom = request.Header.DeviceID;

                result.Body = CustomerDA.Register(requestTran);

                if (result.Body == null)
                {

                    result.ErrCode = 1;
                    result.Message = "注册失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("Register", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SeCustomerPwd(RequestOperation<CustomerPwdData> request)
        {
            var result = new OperationResult();

            try
            {

                request.Body.Password = MD5Helper.Encode(request.Body.Password);

                var rows = CustomerDA.SetCustomerPwd(request);

                if (rows == 0)
                {

                    result.ErrCode = 1;
                    result.Message = "设置失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("SeCustomerPwd", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetCustomerStatus(RequestOperation<CustomerStatusData> request)
        {
            var result = new OperationResult();

            try
            {

                var row = CustomerDA.SetCustomerStatus(request);

                if (row == 0)
                {
                    result.ErrCode = 1;
                    result.Message = "设置失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                //System.Reflection.MethodInfo.GetCurrentMethod().Name
                Logger.WriteException("SetCustomerStatus", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        string[] fields = { "", "UserID", "Status", "InDate", "InUser", "AuditStatus", "CustomerType" };
        string[] noEmptyFields = { "Name", "NickName", "Face" };
        public OperationResult ModifyCustomerByFields(RequestOperation<Dictionary<string, string>> request)
        {
            var result = new OperationResult();

            try
            {

                string sql = "";
                string sqlTemp = "{0}='{1}',";
                foreach (var keys in request.Body)
                {
                    if (fields.Contains(keys.Key) || keys.Value == null)
                    {
                        throw new Exception("FieldName错误");
                    }
                    if (noEmptyFields.Contains(keys.Key) && String.IsNullOrEmpty(keys.Value))
                    {
                        throw new Exception("部分值为空");
                    }
                    sql += String.Format(sqlTemp, keys.Key, keys.Value);
                }
                var row = CustomerDA.ModifyCustomerByFields(request.Header.UserID, sql.TrimEnd(','));

                if (row == 0)
                {
                    result.ErrCode = 1;
                    result.Message = "设置失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                //System.Reflection.MethodInfo.GetCurrentMethod().Name
                Logger.WriteException("ModifyCustomerByFields", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<CustomerSkillData> GetCustomerSkill(RequestOperation request)
        {
            var result = new QueryResultList<CustomerSkillData>();

            try
            {

                var data = CustomerDA.GetCustomerSkill(request);

                if (data == null)
                {
                    data = new List<CustomerSkillData>();
                }
                result.Body = data;

                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                //System.Reflection.MethodInfo.GetCurrentMethod().Name
                Logger.WriteException("GetCustomerSkill", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetCustomerSkill(RequestOperation<CustomerSkillData> request)
        {
            var result = new OperationResult();

            try
            {

                var rows = CustomerDA.SetCustomerSkill(request);

                if (rows <= -1)
                {

                    result.ErrCode = 1;

                    if (request.Body.Sysno == 0)
                    {
                        result.Message = "设置失败(或存在相同的领域名称)";
                    }
                    else
                    {
                        result.Message = "设置失败";

                    }

                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("SetCustomerSkill", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<int> AddCustomer(RequestOperation<CustomerLawyerData> request)
        {
            var result = new OperationResult<int>();

            try
            {

                if (String.IsNullOrEmpty(request.Body.Name) && String.IsNullOrEmpty(request.Body.NickName))
                {
                    result.Message = "姓名不能为空";
                    return result;
                }

                if (String.IsNullOrEmpty(request.Body.Name))
                {
                    request.Body.Name = request.Body.NickName;
                }

                if (String.IsNullOrEmpty(request.Body.NickName))
                {
                    request.Body.NickName = request.Body.Name;
                }

                var data = CustomerDA.AddCustomer(request);

                if (data == null || data.UserID <= 0)
                {
                    result.ErrCode = 1;
                    result.Message = "新增用户失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("AddCustomer", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetCustomer(RequestOperation<CustomerLawyerData> request)
        {
            var result = new OperationResult();

            try
            {

                var rows = CustomerDA.SetCustomer(request);

                if (rows <= -1)
                {

                    result.ErrCode = 1;

                    result.Message = "更新失败";

                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("SetCustomer", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetCustomerSort(RequestOperation<CustomerSortData> request)
        {
            var result = new OperationResult();

            try
            {

                var rows = CustomerDA.SetCustomerSort(request);

                if (rows <= -1)
                {

                    result.ErrCode = 1;

                    result.Message = "更新排序失败";

                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("SetCustomerSort", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }


    }
}
