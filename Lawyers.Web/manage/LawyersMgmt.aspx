<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="LawyersMgmt.aspx.cs" Inherits="Lawyers.Web.manage.LawyersMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .img-td { width: 100px; }
            .img-td img { width: 90px; }
        td p { margin-top: 2px; margin-bottom: 2px; }
        td .input-group input { width: 90px; }
        .img-face { width: 60px; }
        .table .btn-group { width: 100px; }
        .table-scrollable { min-height: 300px; }
        .mr-20 { margin-right: 20px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHContent" runat="server">
    <div class="row" id="page-lawyerlist">
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
                                            <option value="10">展示</option>
                                            <option value="11">禁用</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>姓名</label>
                                        <input type="text" class="form-control input-sm" placeholder="姓名"
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

                            </div>
                            <div class="form-actions">
                                <button type="button" class="btn btn-primary mr-20" v-on:click.prevent="search()">
                                    <i class="fa fa-search"></i>
                                    查询</button>

                                <a href="LawyersDetail.aspx" class="btn btn-primary" target="_blank">
                                    <i class="fa fa-plus"></i>
                                    新增律师</a>
                            </div>
                        </div>
                    </form>
                    <div class="table-scrollable">
                        <table class="table table-striped table-bordered table-hover table-advance">
                            <thead>
                                <tr>
                                    <th style="width: 90px;">操作</th>
                                    <th style="width: 90px;">ID</th>
                                    <th>头像</th>
                                    <th style="width: 120px;">姓名</th>
                                    <th>联系方式</th>
                                    <th>资料</th>
                                    <th style="width: 180px;">排序(1排最前)</th>
                                    <th>状态</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(index,item) in items">
                                    <td>

                                        <div class="btn-group btn-group-sm" v-bind:class="index>1&&index==items.length-1?'dropup':''">
                                            <a class="btn btn-default" href="#" v-bind:href="'LawyersDetail.aspx?uid='+item.UserID" target="_blank">查看详情                                           
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
                                                        禁用账号</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td>{{item.UserID}}</td>
                                    <td class="img-td">
                                        <img v-bind:src="getDefaultFace(item.Face)" />
                                    </td>
                                    <td>
                                        <%--<p class="text-nowrap">昵称：{{item.NickName}}</p>--%>
                                        <p>{{item.Name}}</p>
                                    </td>

                                    <td>
                                        <p class="text-nowrap">手机：{{item.Mobile}}</p>
                                        <p>微信：{{item.WexinNo}}</p>
                                        <p>QQ：{{item.QQNo}}</p>
                                    </td>
                                    <td>
                                        <p class="text-nowrap">地址：{{item.Address}}</p>
                                        <p>公司：{{item.Company}}</p>
                                        <p>领域：{{item.Skills}}</p>
                                    </td>
                                    <td>
                                        <div class="input-group">
                                            <input type="number" class="form-control"
                                                v-model="item.SortNo"
                                              
                                                />
                                            <span class="input-group-btn">
                                                <button class="btn" type="button"
                                                    v-on:click="savesortno(item)">
                                                    保存</button>
                                            </span>
                                        </div>
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
    <script src="static/js/page/lawyer-list.js"></script>
</asp:Content>
