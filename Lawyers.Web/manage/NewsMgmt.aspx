<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="NewsMgmt.aspx.cs" Inherits="Lawyers.Web.manage.NewsMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>新闻</title>
    <link href="static/css/plugins/datetime/bootstrap-datetimepicker.css" rel="stylesheet" />
      <style>
        td p { margin-top: 2px; margin-bottom: 2px; }
        td .input-group input { width: 90px; }
        .img-cover { width: 120px; }
        .table .btn-group { width: 100px; }
        .table-scrollable { min-height: 300px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHContent" runat="server">
    <div class="row" id="page-newslist">
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
                                        <select class="form-control input-sm" v-model="query.ArtStatus">
                                            <option value="0">全部</option>
                                            <option value="1">待审核</option>
                                            <option value="10">上架</option>
                                            <option value="11">下架</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>新闻类型</label>
                                        <select class="form-control input-sm" v-model="query.ArtType">
                                            <option value="0">全部</option>
                                            <option value="1">新闻</option>
                                            <option value="2">每日焦点</option>
                                            <option value="3">家事国事天下事</option>
                                            <option value="4">每日普法</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>新闻标题</label>
                                        <input type="text" class="form-control input-sm" placeholder="新闻标题"
                                            v-model="query.Title" />
                                    </div>
                                </div>

                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>时间范围</label>
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control input-sm input-small datetimepicker" name="from"
                                                v-model="query.StartDate" />
                                            <span class="input-group-addon">—</span>
                                            <input type="text" class="form-control input-sm input-small datetimepicker" name="to"
                                                v-model="query.EndDate" />
                                        </div>
                                    </div>

                                </div>

                            </div>
                            <div class="form-actions">
                                <button type="button" class="btn btn-primary mr-20" v-on:click.prevent="search()">
                                    <i class="fa fa-search"></i>
                                    查询</button>

                                <a href="ArticleDetail.aspx" class="btn btn-primary" target="_blank">
                                    <i class="fa fa-plus"></i>
                                    发布新文章</a>

                            </div>
                        </div>
                    </form>
                    <div class="table-scrollable">
                        <table class="table table-striped table-bordered table-hover table-advance">
                            <thead>
                                <tr>
                                    <th style="width: 90px;">操作</th>
                                    <th style="width: 90px;">ID</th>
                                    <th>封面</th>
                                    <th style="width: 200px;">标题</th>
                                    <th style="width: 280px;">摘要</th>
                                    <th>新闻类型</th>
                                    <th>发布信息</th>
                                    <th style="width: 180px;">排序(1排最前)</th>
                                    <th>状态</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(index,item) in items">
                                    <td>

                                        <div class="btn-group btn-group-sm" v-bind:class="index>1&&index==items.length-1?'dropup':''">
                                            <a class="btn btn-default" href="#" v-bind:href="'ArticleDetail.aspx?id='+item.ID" target="_blank">查看详情                                           
                                            </a>
                                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                                <i class="fa" v-bind:class="index>1&&index==items.length-1?'fa-angle-up':'fa-angle-down'"></i>
                                            </button>
                                            <ul class="dropdown-menu" role="menu">
                                                <li>
                                                    <a href="#" v-on:click.prevent="audit(item,10)">
                                                        <i class="fa fa-check"></i>
                                                        上架</a>
                                                </li>
                                                <li>
                                                    <a href="#" v-on:click.prevent="audit(item,11)">
                                                        <i class="fa fa-remove"></i>
                                                        下架</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td>{{item.ID}}</td>
                                    <td class="img-td">
                                        <img v-bind:src="item.Cover" class="img-cover" />
                                    </td>
                                    <td>{{item.Title}}
                                    </td>
                                    <td>{{item.Summary}}
                                    </td>
                                    <td>{{getArtType(item.ArtType)}}
                                    </td>
                                    <td>
                                        <p class="text-nowrap">发布人：{{item.InUser}}</p>
                                        <p>发布时间：{{item.PublisherDate}}</p>
                                    </td>
                                    <td>
                                        <div class="input-group">
                                            <input type="number" class="form-control"
                                                v-model="item.SortNo" />
                                            <span class="input-group-btn">
                                                <button class="btn" type="button"
                                                    v-on:click="savesortno(item)">
                                                    保存</button>
                                            </span>
                                        </div>
                                    </td>
                                    <td>{{getNewsStatus(item.ArtStatus)}}
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
    <script src="static/js/plugins/datetime/bootstrap-datetimepicker.js"></script>
    <script src="static/js/page/news-list.js"></script>
</asp:Content>
