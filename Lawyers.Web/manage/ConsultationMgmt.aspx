<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="ConsultationMgmt.aspx.cs" Inherits="Lawyers.Web.manage.ConsultationMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>消息</title>
    <style>
        .img-face { width: 60px; }
        .table .btn-group { width: 100px; }
        .table-scrollable { min-height: 300px; }
        .mr-20 { margin-right: 20px; }
        .img-left { float: left; }
        .p-left { float: left; margin-left: 5px; }
        td p { margin-top: 0; margin-bottom: 5px; }
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
                                        <select class="form-control input-sm" v-model="query.ConStatus">
                                            <option value="0">全部</option>
                                            <option value="1">待处理</option>
                                            <option value="10">已处理</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>咨询律师</label>
                                        <input type="text" class="form-control input-sm" placeholder="咨询律师"
                                            v-model="query.LawyerName" />
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
                            </div>
                        </div>
                    </form>
                    <div class="table-scrollable">
                        <table class="table table-striped table-bordered table-hover table-advance">
                            <thead>
                                <tr>
                                    <th style="width: 90px;">操作</th>
                                    <th style="width: 60px;">ID</th>
                                    <th style="width: 280px;">用户</th>
                                    <th style="width: 220px;">咨询律师</th>
                                    <th style="width: 360px;">咨询内容</th>
                                    <th style="width: 180px;">咨询时间</th>
                                    <th>状态</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(index,item) in items">
                                    <td>

                                        <a href="#" v-on:click.prevent="audit(item,10)"
                                             class="btn btn-sm">
                                                        <i class="fa fa-check"></i>
                                                        处理</a>
                                    </td>
                                    <td>{{item.Sysno}}</td>
                                    <td>
                                        <img v-bind:src="item.CustomerFace" class="img-face img-left" />

                                        <div class="p-left  text-nowrap">
                                            <p>姓名：{{item.CustomerName}}</p>
                                            <p>手机号码：{{item.Mobile}}</p>
                                        </div>
                                    </td>
                                    <td>
                                        <img v-bind:src="item.LawyerFace" class="img-face img-left" />

                                        <div class="p-left  text-nowrap">
                                            <p>律师：{{item.LawyerName}}</p>
                                        </div>
                                    </td>
                                    <td>{{item.Contents}}
                                    </td>
                                    <td>{{getLongDate(item.InDate)}}
                                    </td>
                                    <td>{{getStatus(item.ConStatus)}}
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
    <script src="static/js/page/consultation.js"></script>
</asp:Content>
