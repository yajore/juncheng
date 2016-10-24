<%@ Page Title="" Language="C#" MasterPageFile="~/manage/admin.Master" AutoEventWireup="true" CodeBehind="LawyersDetail.aspx.cs" Inherits="Lawyers.Web.manage.LawyersDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .page-content .form-horizontal .control-label { text-align: left; }
        .editor { overflow: scroll; max-height: 300px; }
        div.divider { border-top: solid 1px #ccc; margin: 10px 0; }
        .well { background-color: #fff !important; }
        .act-btns a { margin-left: 10px; }
        .tab-content { height: 330px; }
        .file { position: absolute; width: 100%; height: 100%; left: 0; top: 0; bottom: 0; opacity: 0.01; }
        .popover-content { width: 260px; }
            .popover-content img { width: 100%; }
        .fade.in { display: block; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHContent" runat="server">

    <div id="page-lawyerdetail">
        <div class="row">
            <div class="col-md-12">
                <!-- BEGIN VALIDATION STATES-->
                <div class="portlet light portlet-fit portlet-form">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class=" icon-layers font-green"></i>
                            <span class="caption-subject font-green sbold uppercase">基本信息</span>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <!-- BEGIN FORM-->
                        <div class="form-horizontal" id="form_basic">
                            <div class="form-body">
                                <div class="form-group form-md-line-input">
                                    <div class="col-md-2">
                                        <div class="thumbnail">
                                            <img v-bind:src="item.Face" />
                                            <div class="caption">
                                                <p style="position: relative;">
                                                    <a href="#" class="btn btn-primary btn-block" role="button">上传头像</a>
                                                    <input class="file form-control" type="file" title="上传头像"
                                                        v-on:change.prevent="addfile" />
                                                </p>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-10">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <label class="control-label">
                                                    姓名<span class="required">*</span>
                                                </label>
                                                <input type="text" class="form-control" placeholder="" name="ActivityTitle"
                                                    v-model="item.Name" maxlength="50" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <label class="control-label">
                                                    性别
                                                </label>
                                                <select class="form-control" v-model="item.Sex">
                                                    <option value="0">请选择</option>
                                                    <option value="1">男</option>
                                                    <option value="2">女</option>
                                                </select>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <label class="control-label">
                                                    生日
                                                </label>
                                                <input type="text" class="form-control" placeholder="" name="ActivityTitle"
                                                    v-model="item.BirthDay" />
                                            </div>
                                        </div>

                                    </div>

                                </div>

                                <div class="form-group">

                                    <div class="portlet light">
                                        <div class="portlet-title">
                                            <div class="caption">
                                                <i class="fa fa-sms"></i>联系方式
                                            </div>
                                        </div>
                                        <div class="portlet-body ">

                                            <div class="form-group form-md-line-input">
                                                <div class="col-md-2">
                                                    <label class="control-label">
                                                        微信号<span class="required">*</span>
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="WexinNo"
                                                        v-model="item.WexinNo" maxlength="50" />
                                                </div>
                                                <div class="col-md-1">

                                                    <button class="btn btn-link"
                                                        v-on:click="displayQrcode()">
                                                        <i class="fa fa-search"></i>查看二维码</button>
                                                    <div class="popover fade right in" role="tooltip" v-show="showQrcode"
                                                        style="top: -152.5px; left: 120.156px; display: block;">
                                                        <div class="arrow" style="top: 50%;"></div>
                                                        <h3 class="popover-title">微信二维码</h3>
                                                        <div class="popover-content">
                                                            <img v-bind:src="item.WexinQrcode" />
                                                        </div>
                                                    </div>

                                                    <p style="margin-bottom: 0; position: relative;">
                                                        <button class="btn btn-link">
                                                            <i class="fa fa-upload"></i>
                                                            上传二维码</button>
                                                        <input class="file form-control" type="file" title="上传二维码"
                                                            v-on:change.prevent="addqr" />
                                                    </p>

                                                </div>
                                                <div class="col-md-3">
                                                    <label class="control-label">
                                                        手机号码<span class="required">*</span>
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="Mobile"
                                                        v-model="item.Mobile" maxlength="50" />
                                                </div>
                                            </div>
                                            <div class="form-group form-md-line-input">
                                                <div class="col-md-3">
                                                    <label class="control-label">
                                                        QQ号码<span class="required">*</span>
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="QQNo"
                                                        v-model="item.QQNo" maxlength="14" />
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="control-label">
                                                        邮箱<span class="required">*</span>
                                                    </label>
                                                    <input type="email" class="form-control" placeholder="" name="Email"
                                                        v-model="item.Email" maxlength="50" />
                                                </div>

                                            </div>
                                            <div class="form-group form-md-line-input">
                                                <div class="col-md-6">
                                                    <label class="control-label">
                                                        地址<span class="required">*</span>
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="Address"
                                                        v-model="item.Address" maxlength="50" />
                                                </div>
                                            </div>
                                            <div class="form-group form-md-line-input">
                                                <div class="col-md-6">
                                                    <label class="control-label">
                                                        公司<span class="required">*</span>
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="Company"
                                                        v-model="item.Company" maxlength="50" />
                                                </div>
                                            </div>
                                            <div class="form-group form-md-line-input">
                                                <div class="col-md-6">
                                                    <label class="control-label">
                                                        我的关注
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="Subscribe"
                                                        v-model="item.Subscribe" maxlength="100" />
                                                </div>
                                            </div>
                                            <div class="form-group form-md-line-input">
                                                <div class="col-md-6">
                                                    <label class="control-label">
                                                        我的履历
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="Resume"
                                                        v-model="item.Resume" maxlength="100" />
                                                </div>

                                            </div>
                                            <div class="form-group form-md-line-input">
                                                <div class="col-md-6">
                                                    <label class="control-label">
                                                        擅长领域<span class="required">*</span>
                                                    </label>
                                                    <input type="text" class="form-control" placeholder="" name="Skills"
                                                        v-model="skillVal" maxlength="200" readonly="readonly" />
                                                    <p>
                                                        <template v-for="tm in items">
                                                           <label class="checkbox-inline">
                                                            <input type="checkbox" v-bind:checked="isCheck(tm.Skill)"
                                                                v-model="skills" v-bind:value="tm.Skill">
                                                           {{tm.Skill}}
                                                        </label>
                                                       </template>

                                                    </p>
                                                    <button class="btn btn-defalut"
                                                        v-on:click="showModal()">
                                                        <i class="fa fa-search"></i>添加擅长领域</button>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>




                            </div>

                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-offset-2 col-md-2">
                                        <button type="submit" class="btn btn-primary btn-lg" v-on:click="save()" id="btn-publish">
                                            <i class="fa fa-save"></i>
                                            保存信息</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- END FORM-->
                    </div>
                </div>
                <!-- END VALIDATION STATES-->
            </div>

        </div>

        <canvas id="canvas" class="display-hide"></canvas>


        <div class="modal fade" id="modal-skill">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">设置律师领域</h4>
                    </div>
                    <div class="modal-body">
                        <form class="form-inline" role="form">
                            <div class="form-group">
                                <label class="sr-only" for="txtSkill">领域名称</label>
                                <input type="text" class="form-control" v-model="skillName" placeholder="领域名称"
                                    id="txtSkillName" />
                            </div>

                            <button type="submit" class="btn btn-default" v-on:click.prevent="addSkill()">新 增</button>
                        </form>

                        <table class="table table-striped table-bordered table-hover table-advance" style="margin-top: 10px;">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>领域</th>
                                    <th>状态</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(index,sonItem) in items">
                                    <td>{{sonItem.Sysno}}</td>
                                    <td>{{sonItem.Skill}}</td>
                                    <td>{{getStatus(sonItem.Status)}}</td>
                                    <td>
                                        <button type="button" class="btn btn-default btn-xs"
                                            v-on:click.prevent="delStatus(sonItem,index)">
                                            删除
                                        </button>
                                    </td>
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
        <!-- /.modal -->


    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JSContent" runat="server">
    <script src="static/js/page/lawyer-detail.js"></script>
</asp:Content>
