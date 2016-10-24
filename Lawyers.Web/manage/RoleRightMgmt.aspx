<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="RoleRightMgmt.aspx.cs" Inherits="Lawyers.Web.manage.RoleRightMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>角色权限分配 </title>
    <link href="static/css/plugins/jstree/default/style.min.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHContent" runat="server">
    <div id="page-role">
        <div class="row">
            <div class="col-md-6">
                <div class="portlet">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class="fa fa-registered"></i>角色信息
                           <small>双击表格行，查看权限</small>
                        </div>
                        <div class="actions">
                            <div class="btn-group btn-group-devided" data-toggle="buttons">
                                <button class="btn btn-default btn-outline btn-circle btn-sm"
                                    v-on:click="create()">
                                    <i class="fa fa-registered"></i>
                                    新增角色</button>
                                <%--                                <button class="btn btn-default red btn-outline btn-circle btn-sm">
                                    <i class="fa fa-user-plus"></i>
                                    新增用户</button>--%>
                            </div>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="table-scrollable">
                            <table class="table table-striped table-bordered table-advance table-hover">
                                <thead>
                                    <tr>
                                        <th>
                                            <i class="fa fa-angle-right"></i>
                                            角色ID

                                        </th>
                                        <th class="hidden-xs">
                                            <i class="fa fa-user"></i>
                                            角色 

                                        </th>
                                        <th>
                                            <i class="fa fa-question"></i>
                                            备注

                                        </th>
                                        <th style="width: 60px">
                                            <i class="fa fa-edit"></i>
                                            操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="role in roles" v-on:dblclick.prevent="getusertole(role.RoleID, $event)">
                                        <td>{{role.RoleID}}
                                        </td>
                                        <td>{{role.RoleName}}</td>
                                        <td>{{role.Remark}}</td>
                                        <td>
                                            <a href="#" v-on:click.prevent="edit(role)"
                                                class="btn btn-outline btn-circle btn-xs purple active">编辑</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="portlet">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class="fa fa-bell-o"></i>角色权限
                            <small>请先选择左边角色</small>
                        </div>
                        <div class="actions">
                            <div class="btn-group btn-group-devided" data-toggle="buttons">
                                <button class="btn btn-default btn-outline btn-circle btn-sm"
                                    v-on:click="saveroleright">
                                    <i class="fa fa-save"></i>
                                    保存角色权限</button>
                                <%--                                <button class="btn btn-default red btn-outline btn-circle btn-sm">
                                    <i class="fa fa-user-plus"></i>
                                    新增用户</button>--%>
                            </div>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="note note-info">
                                    <p>页面权限</p>
                                </div>
                                <div id="tree_1" class="tree-demo"></div>
                            </div>
                            <div class="col-md-6">
                                <div class="note note-info">
                                    <p>按钮权限</p>
                                </div>
                                <div id="tree_2" class="tree-demo"></div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>




        </div>


        <div class="modal fade" id="modal-edit">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">角色设置</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-body">
                            <div class="form-group">
                                <label>角色</label>
                                <input type="hidden" v-model="role.RoleID" />
                                <input type="text" class="form-control" placeholder="角色名称"
                                    v-model="role.RoleName" />
                            </div>
                            <div class="form-group">
                                <label>备注</label>
                                <input type="text" class="form-control" placeholder="备注"
                                    v-model="role.Remark" maxlength="50" />
                            </div>
                            <div class="form-group">
                                <label>状态</label>
                                <select class="form-control" v-model="role.Status">
                                    <option value="A">启用</option>
                                    <option value="D">禁用</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-primary" v-on:click="save()">保存</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JSContent" runat="server">
    <script src="static/js/plugins/jstree/dist/jstree.js"></script>
    <script src="static/js/page/role-right.js"></script>
</asp:Content>
