
function consultation() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.init = function () {

        var constatus = { 1: "待处理", 10: "已处理" };
        var usersex = { 0: "未知", 1: "男", 2: "女" };

        var VUE = new Vue({
            el: '#page-lawyerlist',
            data: {
                query: { LawyerName: '', ConStatus: 0, Mobile: '' },
                items: [],
                reply: { ReplyContent: "", _index: 0 },
                liveMessage: '',

            },
            computed: {
            },
            methods: {

                search: function () {

                    delayInit();
                },
                getStatus: function (status) {
                    return constatus[status];
                },
                getShortDate: function (time) {
                    return time.split('T')[0];
                },
                getLongDate: function (time) {
                    return time.substr(0, 16).replace("T", " ");
                },
                getDefaultFace: function (face) {
                    return (face == null || (face + "").length == 0) ? defaultFace : face;
                },
                showModal: function (item, _index) {
                    $('#modal-reply').modal();
                    setTimeout(function () { $("#txtReply").focus() }, 500);
                    VUE.reply._index = _index;
                    VUE.reply.ConsultationID = item.Sysno;
                    this.liveMessage = item.Contents;
                },
                save: function () {
                    if (confirm('确定要回复用户此内容吗?')) {
                        reply();
                    }

                },
                getRows: function () {
                    var rows = parseInt(this.liveMessage.length / 60) + this.liveMessage.split('\n').length;
                    if (rows > 15) {
                        rows = 15;
                    }
                    return rows;
                }
            }
        });



        function delayInit() {
            var queryData = {};
            queryData.data = instance.reqData;
            queryData.data.Body = VUE.query;
            queryData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.items = result.Body;

                    util.pageNumDraw({
                        pageIndex: instance.reqData.PageInfo.PageIndex,
                        pageSize: instance.reqData.PageInfo.PageSize,
                        totalPage: result.TotalCount,
                        requery: function (pageIndex, pageSize) {
                            instance.reqData.PageInfo.PageIndex = pageIndex;
                            instance.reqData.PageInfo.PageSize = pageSize;
                            instance.methods.reqService.getlist(queryData);
                        }
                    });
                } else {
                    util.toast(result.Message);
                }

            }
            instance.methods.reqService.getlist(queryData);
            //util.pageNumDraw({ pageIndex: 0, pageSize: 10, totalPage: 100 });
        }

        setTimeout(delayInit);

        function reply() {
            var replyData = {};
            replyData.data = instance.reqData;
            replyData.data.Body = VUE.reply;
            replyData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.items[VUE.reply._index].ConStatus = 10;
                    VUE.items[VUE.reply._index].Reply = VUE.reply.ReplyContent.substr(0, 50);
                    util.toast("处理成功.", "success");

                    $('#modal-reply').modal('hide');
                } else {
                    util.toast(result.Message);
                }
            }

            instance.methods.reqService.setreply(replyData);
        }
    }

    instance.methods = {

        reqService: {
            getlist: function (data) {
                data.url = "../api/consultation/getitems";
                util.reqAjaxHandler(data);
            },
            setconsultationstatus: function (data) {
                data.url = "../api/consultation/setconsultationstatus";
                util.reqAjaxHandler(data);
            },
            setreply: function (data) {
                data.url = "../api/consultation/setreply";
                util.reqAjaxHandler(data);
            },
        }
    };

};
new consultation().init();
