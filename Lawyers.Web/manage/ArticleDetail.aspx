<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="ArticleDetail.aspx.cs" Inherits="Lawyers.Web.manage.ArticleDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .img-td img { width: 90px; }
        td p { margin-top: 2px; margin-bottom: 2px; }
        .img-face { width: 60px; }
        a.thumbnail { display: inline-block; position: relative; margin-right: 10px; margin-bottom: 10px; }
            a.thumbnail img { width: 132px; height: 132px; }
        .selected_mask { position: absolute; width: 100%; height: 100%; top: 0px; left: 0px; opacity: 0.6; background-color: rgb(0, 0, 0); }
        .selected_mask_icon { position: absolute; top: 0px; left: 0px; width: 132px; height: 132px; vertical-align: middle; display: inline-block; background: url(static/img/pages/icon-selected.png) 50% 50% no-repeat transparent; background-position: 50% 50%; }
    </style>
    <link href="static/css/plugins/wangeditor/wangEditor.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHContent" runat="server">

    <div id="page-lawyerdetail">

        <div class="row">
            <div class="col-md-8">
                <div class="form-group form-md-line-input">
                    <div class="col-md-8">
                        <input type="text"
                            class="form-control"
                            placeholder="请在这里输入标题"
                            name="Title"
                            v-model="item.Title"
                            maxlength="50"
                            style="font-size: 22px;" />
                    </div>
                    <div class="col-md-4">
                        标题：{{item.Title.length}}/50
                    </div>
                </div>
                <div class="form-group form-md-line-input">
                    <div class="col-md-8">
                        <input type="text"
                            class="form-control"
                            placeholder="封面图片式:大图片建议尺寸：900像素 * 500像素"
                            name="Cover"
                            v-model="item.Cover"
                            readonly="readonly" />
                    </div>
                    <div class="col-md-4">
                        <button class="btn" v-on:click="setCover()">从正文选择</button>
                        <button class="btn" v-on:click="chooseCover()">从图片库选择</button>
                    </div>
                </div>
                <div class="form-group form-md-line-input">
                    <div class="col-md-8">
                        <textarea rows="3" class="form-control"
                            placeholder="摘要：选填，如果不填写会默认抓取正文前54个字"
                            name="Summary"
                            v-model="item.Summary"
                            maxlength="120"></textarea>
                    </div>
                    <div class="col-md-4">
                        摘要：{{item.Summary.length}}/120
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="form-group form-md-line-input">
                    <div class="col-md-2">
                        <div class="checkbox" style="margin-left: 20px;">
                            <label>
                                <input type="checkbox" v-model="showlink" value="false" />
                                原文链接
                            </label>
                        </div>
                    </div>
                    <div class="col-md-6" v-show="showlink">
                        <input type="text"
                            class="form-control"
                            placeholder="请在这里输入原文链接"
                            name="Link"
                            v-model="item.Link"
                            maxlength="255" />
                    </div>
                </div>

                <div class="form-group form-md-line-input">
                    <div class="col-md-2">
                        <div class="checkbox" style="margin-left: 20px;">
                            <label>
                                <input type="checkbox" v-model="showLawyer" value="false" />
                                关联律师
                            </label>
                        </div>
                    </div>
                    <div class="col-md-6" v-show="showLawyer">
                        <input type="text"
                            class="form-control"
                            placeholder="设置律师相关案例"
                            name="Lawyers"
                            v-model="item.Lawyers"
                            readonly="readonly" />
                    </div>
                    <div class="col-md-4" v-show="showLawyer">
                        <button class="btn" v-on:click.prevent="chooseLaws()">选择律师</button>

                        <button class="btn" v-on:click.prevent="clearLaws()">清空关联律师</button>
                    </div>
                </div>

                <div class="form-group form-md-line-input">
                    <div class="col-md-1">
                        文章类型
                    </div>
                    <div class="col-md-8">
                        <label class="radio-inline">
                            <input type="radio" name="inlineRadioOptions" value="1" v-model="item.ArtType">
                            新闻
                        </label>
                        <label class="radio-inline">
                            <input type="radio" name="inlineRadioOptions" value="2" v-model="item.ArtType">
                            每日焦点
                        </label>
                        <label class="radio-inline">
                            <input type="radio" name="inlineRadioOptions" value="3" v-model="item.ArtType">
                            家事国事天下事
                        </label>
                          <label class="radio-inline">
                            <input type="radio" name="inlineRadioOptions" value="4" v-model="item.ArtType">
                            每日普法
                        </label>
                    </div>
                </div>



                <div class="form-group form-md-line-input" style="margin-bottom: 10px;">
                    <p style="margin-top: 0; margin-bottom: 20px;">
                        正文内容:
                        <button class="btn pull-right"
                            v-on:click="chooseArticlePic()">
                            从图片库选择插入</button>
                    </p>
                    <div id="content1" style="width: 100%; height: 600px; margin-top: 10px;">
                    </div>
                </div>

                <div class="form-group form-md-line-input">
                    <div class="col-md-2">
                        <div class="checkbox" style="margin-left: 20px;">
                            <label>
                                <input type="checkbox" v-model="toPublish" value="false" />
                                是否直接发布
                            </label>
                        </div>
                    </div>
                </div>

                <div class="form-group form-md-line-input" style="margin-bottom: 20px;">
                    <div class="col-md-offset-3 col-md-6">

                        <button class="btn btn-primary btn-lg"
                            v-on:click.prevent="save()">
                            保存</button>
                        <button class="btn btn-link" style="margin-left: 60px;"
                            v-on:click.prevent="preview()">
                            预览</button>
                    </div>
                    <div class="clearfix"></div>
                </div>

            </div>
        </div>



        <div class="modal fade" id="modal-laws">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">选择关联律师</h4>
                    </div>
                    <div class="modal-body">
                        <form class="form-inline" role="form">
                            <div class="form-group">
                                <label class="sr-only" for="txtSkill">手机号码</label>
                                <input type="text" class="form-control" v-model="Mobile" placeholder="手机号码"
                                    id="txtMobile" />
                            </div>

                            <button type="submit" class="btn btn-default" v-on:click.prevent="search()">查 询</button>
                        </form>

                        <table class="table table-striped table-bordered table-hover table-advance" style="margin-top: 10px;">
                            <thead>
                                <tr>
                                    <th>选择</th>
                                    <th>ID</th>
                                    <th>头像</th>
                                    <th>姓名</th>
                                    <th>擅长领域</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(index,son) in items">
                                    <td>
                                        <label>
                                            <input v-bind:value="son.UserID" type="checkbox" v-model="item.Lawyers" />选择</label>
                                    </td>
                                    <td>{{son.UserID}}</td>
                                    <td>
                                        <img v-bind:src="son.Face" class="img-face" />
                                    </td>
                                    <td>{{son.Name}}</td>
                                    <td>{{son.Skills}}</td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>


        <div class="modal fade in" id="modal-pics">
            <div class="modal-dialog" style="width: 820px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">选择图片</h4>
                    </div>
                    <div class="modal-body" style="overflow-y: scroll; min-height: 320px; max-height: 640px;" id="picSeletor">

                        <template v-for="pic in pics" track-by="$index">

                             <a href="#" class="thumbnail" v-on:click.prevent="seelcted(pic)">
                                <img v-bind:data-oxlazy-img="pic.Url"/>
                                 <div class="selected_mask" v-show="pic.Sysno==0">
                                     <div class="selected_mask_icon"></div>
                                 </div>
                             </a>  
                        </template>

                    </div>
                    <div class="modal-footer">
                        <p class="pull-left">已选{{selectedCount}}个</p>
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-primary" data-dismiss="modal"
                            v-on:click.prevent="appendPic()">
                            确 定</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>


        <div class="modal fade in" id="modal-artpics">
            <div class="modal-dialog" style="width: 820px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">选择图片</h4>
                    </div>
                    <div class="modal-body" style="overflow-y: scroll; max-height: 640px;">

                        <template v-for="pic in artpics" track-by="$index">

                             <a href="#" class="thumbnail" v-on:click.prevent="selectedArtPic(pic)">
                                <img v-bind:src="pic"/>
                             </a>  
                        </template>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JSContent" runat="server">
    <script src="static/js/plugins/wangeditor/wangEditor.min.js"></script>
    <script src="static/js/page/article-detail.js"></script>
</asp:Content>
