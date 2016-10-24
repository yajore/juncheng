<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="UserRoleMgmt.aspx.cs" Inherits="Lawyers.Web.manage.UserRoleMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>用户角色分配</title>

    <style>
        .img-td img { width: 90px; }
        td p { margin-top: 2px; margin-bottom: 2px; }
        .img-face { width: 60px; }
        .table .btn-group { width: 100px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHContent" runat="server">
    <div class="row" id="page-userlist">
        <div class="col-md-12">

            <!-- Begin: life time stats -->
            <div class="portlet ">
                <div class="portlet-body">
                    <form role="form" class="form-inline">
                        <div class="form-body" v-cloak>

                            <div class="form-group">
                                <label>角色类型</label>
                                <select class="form-control" v-model="query.UserRoleID">
                                    <option v-for="option in roles | filterBy 'A' in 'Status'"
                                        v-bind:value="option.RoleID">{{ option.RoleName }}
                                    </option>
                                </select>
                            </div>


                            <div class="form-group">
                                <label>登录账号</label>
                                <input type="text" class="form-control" placeholder="登录账号"
                                    v-model="query.Account" />
                            </div>


                            <button type="submit" class="btn btn-default"
                                v-on:click.prevent="search">
                                <i class="fa fa-search"></i>
                                查询</button>


                        </div>
                    </form>
                    <div class="table-scrollable">
                        <table class="table table-striped table-bordered table-hover table-advance">
                            <thead>
                                <tr>
                                    <th>操作</th>
                                    <th>ID</th>
                                    <th>头像</th>
                                    <th>姓名</th>
                                    <th>账号</th>
                                    <th>角色权限</th>
                                    <th>用户类型</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(index,item) in items">
                                    <td>
                                        <button type="button" class="btn btn-default btn-xs"
                                            v-on:click.prevent="setuserrole(item)">
                                            分配角色
                                        </button>
                                        <p style="margin-top: 10px;">
                                            <button type="button" class="btn btn-default btn-xs"
                                                v-on:click.prevent="showsetpwdmodal(item)">
                                                设置密码
                                            </button>
                                        </p>
                                    </td>
                                    <td>{{item.UserID}}</td>
                                    <td class="img-td">
                                        <img v-bind:src="getDefaultFace(item.Face)" />
                                    </td>
                                    <td>{{item.Name}}</td>
                                    <td>{{item.Account}}</td>
                                    <td>{{ getrolebyid(item.UserRoleID)}}</td>
                                    <td>{{gettypebyid(item.CustomerType)}}</td>
                                </tr>

                            </tbody>
                        </table>
                    </div>

                    <div class="pagination pagination-sm" id="pagination">
                        <div class="btn-group btn-group-sm" role="group">
                        </div>
                    </div>

                </div>
            </div>
            <!-- End: life time stats -->
        </div>


        <div class="modal fade" id="modal-rolesend">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">用户角色权限分配</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-body">
                            <div class="form-group">
                                <label>角色类型</label>
                                <input type="hidden" v-model="user.UserID" />
                                <select class="form-control" v-model="user.RoleID">
                                    <option v-for="option in roles | filterBy 'A' in 'Status'"
                                        v-bind:value="option.RoleID">{{ option.RoleName }}
                                    </option>
                                </select>
                                <p class="help-block">
                                    建议先分清各个角色所拥有的权限再分配
                                    <br />
                                    <a href="RoleRightMgmt.aspx">点此查看具体角色权限</a>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-primary" v-on:click.prevent="saveuserrole">保存</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <div class="modal fade" id="modal-setpwd">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">用户登录密码设置</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-body">
                            <div class="form-group">
                                <label>登录密码（6-16位）</label>
                                <input type="hidden" v-model="userpwd.UserID" />
                                <input type="text" v-model="userpwd.Password" class="form-control" />
                            </div>
                            <div class="form-group">
                                <label>授权密码验证</label>
                                <input type="text" v-model="userpwd.MgrPassword" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-primary" v-on:click.prevent="setpassword">保存</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>


    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JSContent" runat="server">
    <script src="static/js/page/user-role.js"></script>
</asp:Content>
