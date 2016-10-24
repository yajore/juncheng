using Lawyers.BizEntities;
using System.Collections.Generic;
using Valon.Framework.DataAccess;

namespace Mango.DataAccess
{
    public class RoleRightDA
    {
        public static List<MenuData> GetRoleMenus()
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_GetRoleMenus");
            return cmd.ExecuteEntityList<MenuData>();
        }

        public static List<RoleData> GetRoles(string status)
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_GetRoles");
            cmd.SetParameterValue("@Status", status);
            return cmd.ExecuteEntityList<RoleData>();
        }


        public static List<MenuExtentData> GetRoleMenuExtends()
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_GetRoleMenuExtends");
            return cmd.ExecuteEntityList<MenuExtentData>();
        }


        public static MenusData GetMenus()
        {

            var result = new MenusData();
            var cmd = DataCommandManager.GetDataCommand("RoleRight_GetMenus");
            var dt = cmd.ExecuteDataSet();
            if (dt != null && dt.Tables.Count > 0)
            {
                result.PageRight = Valon.Framework.Data.EntityBuilder.BuildEntityList<MenuData>(dt.Tables[0]);
                if (dt.Tables.Count > 1)
                {
                    result.PageRightExtend = Valon.Framework.Data.EntityBuilder.BuildEntityList<MenuExtentData>(dt.Tables[1]);
                }
            }

            return result;
        }

        public static int SetRole(RequestOperation<RoleData> request)
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_SetRole");
            cmd.SetParameterValue("@RoleID", request.Body.RoleID);
            cmd.SetParameterValue("@RoleName", request.Body.RoleName);
            cmd.SetParameterValue("@Status", request.Body.Status);
            cmd.SetParameterValue("@Remark", request.Body.Remark);
            return cmd.ExecuteNonQuery();
        }

        public static List<RoleRightListData> GetUserRoleRightList(int roleId)
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_GetUserRoleRightList");
            cmd.SetParameterValue("@RoleID", roleId);
            return cmd.ExecuteEntityList<RoleRightListData>();
        }

        public static int SetUserRoleRight(int roleId, string sql1, string extsql2)
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_SetUserRoleRight");
            cmd.SetParameterValue("@RoleID", roleId);
            cmd.ReplaceParameterValue("@RoleMenu_Mapping", sql1);
            cmd.ReplaceParameterValue("@RoleMenuExtent_Mapping", extsql2);
            return cmd.ExecuteNonQuery();
        }

        public static QueryResultList<UserRoleData> GetUserRoles(QueryRequest<UserRoleQueryData> query)
        {
            var result = new QueryResultList<UserRoleData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("RoleRight_GetUserRoles");
            cmd.SetParameterValue("@UserRoleID", query.Body.UserRoleID);
            cmd.SetParameterValue("@Account", query.Body.Account);

            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<UserRoleData>();
            return result;
        }

        public static int UpdateUserRoles(RequestOperation<UserRoleUpdateData> request)
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_UpdateUserRoles");
            cmd.SetParameterValue("@UserRoleID", request.Body.RoleID);
            cmd.SetParameterValue("@UserID", request.Body.UserID);
            return cmd.ExecuteNonQuery();
        }

        public static int SetCustomerPwd(RequestOperation<UserPasswordSetData> request)
        {
            var cmd = DataCommandManager.GetDataCommand("RoleRight_SetCustomerPwd");
            cmd.SetParameterValue("@Password", request.Body.Password);
            cmd.SetParameterValue("@UserID", request.Body.UserID);
            return cmd.ExecuteNonQuery();
        }
    }
}
