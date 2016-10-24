using Lawyers.BizEntities;
using System.Collections.Generic;
using Valon.Framework.DataAccess;

namespace Mango.DataAccess
{
    public class CustomerDA
    {
        /// <summary>
        /// 获取用户信息，平台方管理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryResultList<CustomerData> GetCustomers(QueryRequest<CustomerQueryData> query)
        {
            var result = new QueryResultList<CustomerData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_GetCustomers");
            cmd.SetParameterValue("@UserID", query.Body.UserID);
            cmd.SetParameterValue("@NickName", query.Body.NickName);
            cmd.SetParameterValue("@Name", query.Body.Name);
            cmd.SetParameterValue("@Mobile", query.Body.Mobile);
            cmd.SetParameterValue("@CustomerType", query.Body.CustomerType);
            cmd.SetParameterValue("@RegStartDate", query.Body.RegStartDate);
            cmd.SetParameterValue("@RegEndDate", query.Body.RegEndDate);
            cmd.SetParameterValue("@AuditStatus", query.Body.AuditStatus);

            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<CustomerData>();
            result.TotalCount = (int)cmd.GetParameterValue("@TotalCount");
            return result;
        }

        public static QueryResultList<CustomerLawyerData> GetLawyers(QueryRequest<CustomerQueryData> query)
        {
            var result = new QueryResultList<CustomerLawyerData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_GetLawyers");
            cmd.SetParameterValue("@UserID", query.Body.UserID);
            cmd.SetParameterValue("@NickName", query.Body.NickName);
            cmd.SetParameterValue("@Name", query.Body.Name);
            cmd.SetParameterValue("@Mobile", query.Body.Mobile);
            cmd.SetParameterValue("@CustomerType", query.Body.CustomerType);
            cmd.SetParameterValue("@AuditStatus", query.Body.AuditStatus);

            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<CustomerLawyerData>();
            result.TotalCount = (int)cmd.GetParameterValue("@TotalCount");
            return result;
        }

        public static QueryResultList<CustomerLawyerShowData> GetShowLawyers(QueryRequest<CustomerQueryData> query)
        {
            var result = new QueryResultList<CustomerLawyerShowData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_GetShowLawyers");
            cmd.SetParameterValue("@CustomerType", query.Body.CustomerType);
            cmd.SetParameterValue("@AuditStatus", query.Body.AuditStatus);
            cmd.SetParameterValue("@Mobile", query.Body.Mobile);

            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<CustomerLawyerShowData>();
            return result;
        }

        public static List<CustomerLawyerShowData> GetCustomerByKey(QueryRequest<string> query)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_SearchKey");
            cmd.SetParameterValue("@KeyWord", "%" + query.Body + "%");
            return cmd.ExecuteEntityList<CustomerLawyerShowData>();
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CustomerLawyerData GetCustomerById(int userid)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_GetCustomerById");
            cmd.SetParameterValue("@UserID", userid);
            return cmd.ExecuteEntity<CustomerLawyerData>();
        }

        public static CustomerLoginData GetUserById(int userid)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_GetUserById");
            cmd.SetParameterValue("@UserID", userid);
            return cmd.ExecuteEntity<CustomerLoginData>();
        }

        /// <summary>
        /// 检查是否注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CustomerIDEntry VerifyAccount(RequestOperation<CustomerAccountQueryData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_VerifyAccount");
            cmd.SetParameterValue("@Account", request.Body.Account);
            cmd.SetParameterValue("@QQAccount", request.Body.QQAccount);
            cmd.SetParameterValue("@WechatAccount", request.Body.WechatAccount);
            cmd.SetParameterValue("@WeiboAccount", request.Body.WeiboAccount);
            return cmd.ExecuteEntity<CustomerIDEntry>();
        }

        /// <summary>
        /// 手机号码登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CustomerLoginData LoginByAccount(UserLoginData request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_LoginByAccount");
            cmd.SetParameterValue("@Account", request.Account);
            cmd.SetParameterValue("@Password", request.Password);
            return cmd.ExecuteEntity<CustomerLoginData>();
        }


        public static CustomerLoginData LoginByWechatAccount(RequestOperation<string> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_LoginByWechatAccount");
            cmd.SetParameterValue("@WechatAccount", request.Body);
            return cmd.ExecuteEntity<CustomerLoginData>();
        }

        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int SetCustomerPwd(RequestOperation<CustomerPwdData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_SetCustomerPwd");
            cmd.SetParameterValue("@UserID", request.Body.UserID);
            cmd.SetParameterValue("@Password", request.Body.Password);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 保存用户爱好
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int SetCustomerHobby(int userid, string request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_SetCustomerHobby");
            cmd.SetParameterValue("@UserID", userid);

            cmd.ReplaceParameterValue("@Customer_Hobbys", request);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 禁用、启用员工账号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int SetCustomerStatus(RequestOperation<CustomerStatusData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_SetCustomerStatus");
            cmd.SetParameterValue("@UserID", request.Body.UserID);
            cmd.SetParameterValue("@UserStatus", request.Body.UserStatus);
            return cmd.ExecuteNonQuery();
        }


        public static int ModifyCustomerByFields(int usrid, string fileds)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_ModifyCustomerByFields");
            cmd.ReplaceParameterValue("@FieldNameAndFieldValue", fileds);
            cmd.SetParameterValue("@UserID", usrid);
            return cmd.ExecuteNonQuery();
        }

        public static List<CustomerSkillData> GetCustomerSkill(RequestOperation request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_GetCustomerSkill");
            return cmd.ExecuteEntityList<CustomerSkillData>();
        }

        public static int SetCustomerSkill(RequestOperation<CustomerSkillData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_SetCustomerSkill");
            cmd.SetParameterValue("@Skill", request.Body.Skill);
            cmd.SetParameterValue("@Status", request.Body.Status);
            cmd.SetParameterValue("@Sysno", request.Body.Sysno);
            return cmd.ExecuteNonQuery();
        }

        public static CustomerIDEntry AddCustomer(RequestOperation<CustomerLawyerData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_AddCustomer");
            cmd.SetParameterValue("@Name", request.Body.Name);
            cmd.SetParameterValue("@NickName", request.Body.NickName);
            cmd.SetParameterValue("@Face", request.Body.Face);
            cmd.SetParameterValue("@Sex", request.Body.Sex);
            cmd.SetParameterValue("@BirthDay", request.Body.BirthDay);
            cmd.SetParameterValue("@Mobile", request.Body.Mobile);
            cmd.SetParameterValue("@WexinNo", request.Body.WexinNo);
            cmd.SetParameterValue("@WexinQrcode", request.Body.WexinQrcode);
            cmd.SetParameterValue("@QQNo", request.Body.QQNo);
            cmd.SetParameterValue("@Email", request.Body.Email);
            cmd.SetParameterValue("@HomeTownCode", request.Body.HomeTownCode);
            cmd.SetParameterValue("@HomeTown", request.Body.HomeTown);
            cmd.SetParameterValue("@PCDCode", request.Body.PCDCode);
            cmd.SetParameterValue("@PCDDesc", request.Body.PCDDesc);
            cmd.SetParameterValue("@Address", request.Body.Address);
            cmd.SetParameterValue("@Job", request.Body.Job);
            cmd.SetParameterValue("@Company", request.Body.Company);
            cmd.SetParameterValue("@Signature", request.Body.Signature);
            cmd.SetParameterValue("@CustomerType", request.Body.CustomerType);
            cmd.SetParameterValue("@AuditStatus", request.Body.AuditStatus);
            cmd.SetParameterValue("@Skills", request.Body.Skills);
            cmd.SetParameterValue("@CaseSeries", request.Body.CaseSeries);
            cmd.SetParameterValue("@Resume", request.Body.Resume);
            cmd.SetParameterValue("@Subscribe", request.Body.Subscribe);
            cmd.SetParameterValue("@InUser", request.Header.DisplayName);
            return cmd.ExecuteEntity<CustomerIDEntry>();
        }

        public static int SetCustomer(RequestOperation<CustomerLawyerData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_SetCustomer");
            cmd.SetParameterValue("@Name", request.Body.Name);
            cmd.SetParameterValue("@NickName", request.Body.NickName);
            cmd.SetParameterValue("@Face", request.Body.Face);
            cmd.SetParameterValue("@Sex", request.Body.Sex);
            cmd.SetParameterValue("@BirthDay", request.Body.BirthDay);
            cmd.SetParameterValue("@Mobile", request.Body.Mobile);
            cmd.SetParameterValue("@WexinNo", request.Body.WexinNo);
            cmd.SetParameterValue("@WexinQrcode", request.Body.WexinQrcode);
            cmd.SetParameterValue("@QQNo", request.Body.QQNo);
            cmd.SetParameterValue("@Email", request.Body.Email);
            cmd.SetParameterValue("@HomeTownCode", request.Body.HomeTownCode);
            cmd.SetParameterValue("@HomeTown", request.Body.HomeTown);
            cmd.SetParameterValue("@PCDCode", request.Body.PCDCode);
            cmd.SetParameterValue("@PCDDesc", request.Body.PCDDesc);
            cmd.SetParameterValue("@Address", request.Body.Address);
            cmd.SetParameterValue("@Job", request.Body.Job);
            cmd.SetParameterValue("@Company", request.Body.Company);
            cmd.SetParameterValue("@Signature", request.Body.Signature);
            cmd.SetParameterValue("@CustomerType", request.Body.CustomerType);
            cmd.SetParameterValue("@AuditStatus", request.Body.AuditStatus);
            cmd.SetParameterValue("@Skills", request.Body.Skills);
            cmd.SetParameterValue("@CaseSeries", request.Body.CaseSeries);
            cmd.SetParameterValue("@Subscribe", request.Body.Subscribe);
            cmd.SetParameterValue("@Resume", request.Body.Resume);
            cmd.SetParameterValue("@EditUser", request.Header.DisplayName);
            cmd.SetParameterValue("@UserID", request.Body.UserID);
            return cmd.ExecuteNonQuery();
        }

        public static int SetCustomerSort(RequestOperation<CustomerSortData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_SetCustomerSort");
            cmd.SetParameterValue("@SortNo", request.Body.SortNo);
            cmd.SetParameterValue("@UserID", request.Body.UserID);
            return cmd.ExecuteNonQuery();
        }

        public static CustomerLoginData Register(RequestOperation<CustomerRegisterData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Customer_Register");
            cmd.SetParameterValue("@Account", request.Body.Login.Account);
            cmd.SetParameterValue("@Email", request.Body.Login.Email);
            cmd.SetParameterValue("@Password", request.Body.Login.Password);
            cmd.SetParameterValue("@QQAccount", request.Body.Login.QQAccount);
            cmd.SetParameterValue("@WechatAccount", request.Body.Login.WechatAccount);
            cmd.SetParameterValue("@WeiboAccount", request.Body.Login.WeiboAccount);
            cmd.SetParameterValue("@BaiduAccount", request.Body.Login.BaiduAccount);
            cmd.SetParameterValue("@UserRoleID", request.Body.Login.UserRoleID);
            cmd.SetParameterValue("@UserFrom", request.Body.Login.UserFrom);
            cmd.SetParameterValue("@UserStatus", request.Body.Login.UserStatus);

            cmd.SetParameterValue("@Name", request.Body.Customer.Name);
            cmd.SetParameterValue("@NickName", request.Body.Customer.NickName);
            cmd.SetParameterValue("@Face", request.Body.Customer.Face);
            cmd.SetParameterValue("@Sex", request.Body.Customer.Sex);
            cmd.SetParameterValue("@BirthDay", request.Body.Customer.BirthDay);
            cmd.SetParameterValue("@Mobile", request.Body.Customer.Mobile);
            cmd.SetParameterValue("@CustomerType", request.Body.Customer.CustomerType);
            cmd.SetParameterValue("@AuditStatus", request.Body.Customer.AuditStatus);
            cmd.SetParameterValue("@InUser", request.Header.DisplayName);
            return cmd.ExecuteEntity<CustomerLoginData>();
        }

    }
}
