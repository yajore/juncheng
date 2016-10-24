using Lawyers.BizEntities;
using Lawyers.Utilities;
using Mango.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lawyers.BizProcess
{
    public class RoleRightBP 
    {

        delegate void RefreshRoleMenu();

        public QueryResultList<MenuData> GetRoleMenus()
        {
            var result = new QueryResultList<MenuData>();

            try
            {
                var menus = Cache.Instance["mango_rolemenus"];
                if (menus == null)
                {
                    result.Body = RoleRightDA.GetRoleMenus();
                    if (result.Body == null || result.Body.Count == 0)
                    {
                        throw new Exception("您没有相关权限");
                    }

                    Cache.Instance.Add("mango_rolemenus", result.Body);
                }
                else
                {
                    result.Body = menus as List<MenuData>;
                }


                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetRoleMenus", ex, "");
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<MenuExtentData> GetRoleMenuExtends()
        {
            var result = new QueryResultList<MenuExtentData>();

            try
            {
                var menus = Cache.Instance["mango_rolemenuextends"];
                if (menus == null)
                {
                    result.Body = RoleRightDA.GetRoleMenuExtends();

                    Cache.Instance.Add("mango_rolemenuextends", result.Body);
                }
                else
                {
                    result.Body = menus as List<MenuExtentData>;
                }


                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetRoleMenuExtends", ex, "");
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<MenuData> GetTreeMenus(int role)
        {
            var result = new QueryResultList<MenuData>();

            try
            {

                var menus = GetRoleMenus();
                if (menus.ErrCode != 0)
                {
                    throw new Exception(menus.Message);
                }

                var rolemenus = menus.Body.Where(c => c.RoleID == role);
                if (rolemenus.Count() == 0)
                {
                    throw new Exception("没有相关权限");
                }

                result.Body = new List<MenuData>();

                var level1 = rolemenus.Where(c => c.Levels == 1).ToList();

                var level2 = rolemenus.Where(c => c.Levels == 2).ToList();

                var level3 = rolemenus.Where(c => c.Levels == 3).ToList();

                //模块

                foreach (var menu in level1)
                {
                    var nmenu = new MenuData();
                    nmenu.MID = menu.MID;
                    nmenu.MName = menu.MName;
                    nmenu.MIcon = menu.MIcon;
                    nmenu.MDesc = menu.MDesc;
                    nmenu.Url = menu.Url;
                    nmenu.IsShow = menu.IsShow;
                    if (nmenu.Items == null)
                    {
                        nmenu.Items = new List<MenuData>();
                    }
                    nmenu.Items.AddRange(level2.Where(c => c.PID == menu.MID));

                    foreach (var item in nmenu.Items)
                    {
                        if (item.Items == null)
                        {
                            item.Items = new List<MenuData>();
                        }
                        item.Items.AddRange(level3.Where(c => c.PID == item.MID));
                    }
                    result.Body.Add(nmenu);
                }



                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetTreeMenus", ex, "");
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public MenusData GetMenus()
        {
            var result = new MenusData();

            try
            {
                var menus = Cache.Instance["mango_menus"];
                if (menus == null)
                {
                    result = RoleRightDA.GetMenus();
                    if (result == null || result.PageRight == null ||
                        result.PageRight.Count == 0)
                    {
                        throw new Exception("没有配置菜单项");
                    }

                    Cache.Instance.Add("mango_menus", result);
                }
                else
                {
                    result = menus as MenusData;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetMenus", ex, "");
                throw new Exception("没有配置菜单项");
            }

            return result;
        }

        public OperationResult<MenuTreeData> GetWebTreeMenus()
        {
            var result = new OperationResult<MenuTreeData>();

            try
            {

                var menus = GetMenus();
                var rolemenus = menus.PageRight;

                result.Body = new MenuTreeData();
                result.Body.PageRight = new List<MenuTreeItemData>();
                foreach (var menu in rolemenus)
                {
                    var child = new MenuTreeItemData();
                    child.id = menu.MID;
                    if (menu.PID == 0)
                    {
                        child.parent = "#";
                    }
                    else
                    {
                        child.parent = menu.PID + "";
                    }

                    child.icon = menu.MIcon;
                    child.state = new MenuTreeStateData() { opened = true };
                    child.text = menu.MName;
                    child.li_attr = new Dictionary<string, string>() { { "dtype", menu.MType + "" } };
                    child.url = menu.Url;
                    result.Body.PageRight.Add(child);
                }

                var extends = menus.PageRightExtend;

                if (extends != null && extends.Count > 0)
                {

                    result.Body.PageRightExtend = new List<MenuTreeItemData>();
                    foreach (var menu in extends)
                    {
                        var parentPage = result.Body.PageRight.FirstOrDefault(c => c.id == menu.MID);
                        if (parentPage == null)
                        {
                            continue;
                        }

                        if (result.Body.PageRightExtend.Exists(c => c.id == parentPage.id) == false)
                        {
                            var pagePar = new MenuTreeItemData()
                            {
                                parent = "#",
                                id = parentPage.id,
                                icon = parentPage.icon,
                                li_attr = parentPage.li_attr,
                                text = parentPage.text,
                                state = parentPage.state,
                            };
                            result.Body.PageRightExtend.Add(pagePar);
                        }
                        var child = new MenuTreeItemData();
                        child.id = menu.ExtendID;
                        child.parent = menu.MID + "";
                        child.icon = "fa fa-chain";
                        child.li_attr = new Dictionary<string, string>() { { "dtype", "2" } };
                        child.text = menu.ExtendName;
                        result.Body.PageRightExtend.Add(child);
                    }
                }


                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetWebTreeMenusV2", ex, "");
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取角色id
        /// </summary>
        /// <returns></returns>
        public QueryResultList<RoleData> GetRoleTree()
        {
            var result = new QueryResultList<RoleData>();

            try
            {
                var roles = Cache.Instance["mango_roles"];
                if (roles == null)
                {
                    result.Body = RoleRightDA.GetRoles("A");
                    if (result.Body == null || result.Body.Count == 0)
                    {
                        throw new Exception("没有获取到相关角色");
                    }

                    Cache.Instance.Add("mango_roles", result.Body);
                }
                else
                {
                    result.Body = roles as List<RoleData>;
                }


                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetRoles", ex, "");
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<RoleData> GetRoles(string status)
        {
            var result = new QueryResultList<RoleData>();

            try
            {
                result.Body = RoleRightDA.GetRoles(status);
                if (result.Body == null || result.Body.Count == 0)
                {
                    throw new Exception("没有获取到相关角色");
                }
                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetRoles", ex, "");
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<RoleData> SetRole(RequestOperation<RoleData> request)
        {
            var result = new QueryResultList<RoleData>();

            try
            {
                int row = RoleRightDA.SetRole(request);
                if (row != 1)
                {
                    throw new Exception("设置角色失败");
                }


                result = GetRoles("A");
            }
            catch (Exception ex)
            {
                Logger.WriteException("SetRole", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<RoleRightListData> GetUserRoleRightList(RequestOperation<int> request)
        {
            var result = new QueryResultList<RoleRightListData>();

            try
            {

                result.Body = RoleRightDA.GetUserRoleRightList(request.Body);
                if (result.Body == null || result.Body.Count == 0)
                {
                    result.Message = "没有角色权限";
                    return result;
                }
                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("GetUserRoleRightList", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetUserRoleRightList(RequestOperation<SetRoleRightData> request)
        {
            var result = new OperationResult();

            try
            {
                if (request.Body == null ||
                    request.Body.RightIDs == null ||
                    request.Body.RightIDs.Count == 0)
                {
                    result.Message = "没有可保存的数据";
                    return result;
                }

                string sqlTemplate = "({0},{1}),";
                string sql1 = "";
                string extsql2 = "";
                foreach (var right in request.Body.RightIDs)
                {
                    if (right < 0)
                    {
                        extsql2 += String.Format(sqlTemplate, request.Body.RoleID, right);
                    }
                    else
                    {
                        sql1 += String.Format(sqlTemplate, request.Body.RoleID, right);
                    }
                }

                if (sql1.Length > 0)
                {
                    sql1 = " INSERT INTO [dbo].[T_RoleMenu_Mapping]([RoleID],[MID]) VALUES "
                   + sql1.TrimEnd(',');
                }

                if (extsql2.Length > 0)
                {
                    extsql2 = " INSERT INTO [dbo].[T_RoleMenuExtent_Mapping]([RoleID],[ExtendID]) VALUES "
                   + extsql2.TrimEnd(',');
                }

                int rows = RoleRightDA.SetUserRoleRight(request.Body.RoleID, sql1, extsql2);
                if (rows == 0)
                {
                    throw new Exception("设置角色权限失败");
                }
                result.ErrCode = 0;
                result.Message = "ok";

                Task.Factory.StartNew(() => RefreshRightCache(request.Body.RoleID));

            }
            catch (Exception ex)
            {
                Logger.WriteException("SetUserRoleRightList", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<UserRoleData> GetUserRoles(QueryRequest<UserRoleQueryData> query)
        {
            var result = new QueryResultList<UserRoleData>();

            try
            {

                result = RoleRightDA.GetUserRoles(query);
                if (result.Body == null)
                {
                    result.Body = new List<UserRoleData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetUserRoles", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult UpdateUserRoles(RequestOperation<UserRoleUpdateData> request)
        {
            var result = new OperationResult();

            try
            {
                int row = RoleRightDA.UpdateUserRoles(request);
                if (row != 1)
                {
                    throw new Exception("变更角色失败");
                }
                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("UpdateUserRoles", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetCustomerPwd(RequestOperation<UserPasswordSetData> request)
        {
            var result = new OperationResult();

            try
            {
                request.Body.Password = MD5Helper.Encode(request.Body.Password);
                int row = RoleRightDA.SetCustomerPwd(request);
                if (row != 1)
                {
                    throw new Exception("设置密码失败");
                }
                result.ErrCode = 0;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.WriteException("SetCustomerPwd", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        private void RefreshRightCache(int roleId)
        {
            try
            {

                Cache.Instance.Remove("mango_rolemenus");

                var rolemenus = RoleRightDA.GetRoleMenus();

                Cache.Instance.Add("mango_rolemenus", rolemenus);

                Cache.Instance.Remove("mango_rolemenuextends");

                var menuextends = RoleRightDA.GetRoleMenuExtends();

                Cache.Instance.Add("mango_rolemenuextends", menuextends);

                // mango_menuextends
                //清楚缓存，让用户重新登录
                Cache.Instance.Remove("juncheng_platform_menu_" + roleId);


            }
            catch (Exception ex)
            {
                Logger.WriteException("【刷新角色才能】", ex, "");
            }
        }
    }
}
