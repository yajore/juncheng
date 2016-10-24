<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="UserMgmt.aspx.cs" Inherits="Lawyers.Web.manage.UserMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .img-td { width: 100px; }
            .img-td img { width: 90px; }
        td p { margin-top: 2px; margin-bottom: 2px; }
        .img-face { width: 60px; }
        .table .btn-group { width: 100px; }
        .table-scrollable { min-height: 300px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHContent" runat="server">
    <div class="row" id="page-userlist">
        <div class="col-md-12">

            <!-- Begin: life time stats -->
            <div class="portlet ">
                <div class="portlet-body">
                    <form role="form">
                        <div class="form-body">
                            <div class="row">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>状态</label>
                                        <select class="form-control input-sm" v-model="query.AuditStatus">
                                            <option value="0">全部</option>
                                            <option value="1">待审核</option>
                                            <option value="10">审核通过</option>
                                            <option value="11">冻结</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>客户类型</label>
                                        <select class="form-control input-sm" v-model="query.CustomerType">
                                            <option value="0">全部</option>
                                            <option value="1">普通用户</option>
                                            <option value="2">律师</option>

                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>用户昵称</label>
                                        <input type="text" class="form-control input-sm" placeholder="用户昵称"
                                            v-model="query.NickName" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>手机号码</label>
                                        <input type="text" class="form-control input-sm" placeholder="手机号码"
                                            v-model="query.Mobile" />
                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>注册时间</label>
                                        <div class="input-group input-group-sm">
                                            <input type="date" class="form-control input-sm input-small" name="from"
                                                v-model="query.RegStartDate" />
                                            <span class="input-group-addon">—</span>
                                            <input type="date" class="form-control input-sm input-small" name="to"
                                                v-model="query.RegEndDate" />
                                        </div>
                                    </div>

                                </div>

                            </div>
                            <div class="form-actions">
                                <button type="submit" class="btn blue" v-on:click.prevent="search">
                                    <i class="fa fa-search"></i>
                                    查询</button>
                            </div>
                        </div>
                    </form>
                    <div class="table-scrollable">
                        <table class="table table-striped table-bordered table-hover table-advance">
                            <thead>
                                <tr>
                                    <th style="width:90px;">操作</th>
                                    <th>用户ID</th>
                                    <th>头像</th>
                                    <th>昵称/姓名</th>
                                    <th>性别/生日</th>
                                    <th>注册时间</th>
                                    <th>状态</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(index,item) in items">
                                    <td>

                                        <div class="btn-group btn-group-sm" v-bind:class="index>1&&index==items.length-1?'dropup':''">
                                            <a class="btn btn-default" href="#" v-bind:href="'UserMgmt.aspx?uid='+item.UserID" target="_blank">查看详情                                           
                                            </a>
                                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                                <i class="fa" v-bind:class="index>1&&index==items.length-1?'fa-angle-up':'fa-angle-down'"></i>
                                            </button>
                                            <ul class="dropdown-menu" role="menu">
                                                <li>
                                                    <a href="#" v-on:click.prevent="audit(item,10)">
                                                        <i class="fa fa-check"></i>
                                                        审核通过</a>
                                                </li>
                                                <li>
                                                    <a href="#" v-on:click.prevent="audit(item,11)">
                                                        <i class="fa fa-remove"></i>
                                                        冻结账号</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td>{{item.UserID}}</td>
                                    <td class="img-td">
                                        <img v-bind:src="getDefaultFace(item.Face)" />
                                    </td>
                                    <td>
                                        <p class="text-nowrap">昵称：{{item.NickName}}</p>
                                        <p>姓名：{{item.Name}}</p>
                                    </td>

                                    <td>
                                        <p class="text-nowrap">性别：{{getSex(item.Sex)}}</p>
                                        <p>生日：{{getShortDate(item.BirthDay)}}</p>
                                    </td>
                                    <td>{{getLongDate(item.InDate)}}
                                    </td>
                                    <td>{{getUserStatus(item.AuditStatus)}}
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </div>


                    <div class="pagination pagination-sm pull-right" id="pagination">
                        <div class="btn-group btn-group-sm" role="group">
                        </div>
                    </div>

                </div>
            </div>
            <!-- End: life time stats -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JSContent" runat="server">
    <script src="static/js/page/user-list.js"></script>
</asp:Content>
