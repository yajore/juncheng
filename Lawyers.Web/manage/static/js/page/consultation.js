
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
                items: []

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
                audit: function (item, auditStatus) {
                    if (item.AuditStatus == auditStatus) {
                        util.toast("当前用户状态无需此操作");
                        return false;
                    }
                    var auditData = {};
                    auditData.data = instance.reqData;
                    auditData.data.Body = { Sysno: item.Sysno, ConStatus: auditStatus };
                    auditData.callback = function (result) {
                        if (result.ErrCode == 0) {
                            item.ConStatus = auditStatus;
                            util.toast("处理成功.", "success");
                        } else {
                            util.toast(result.Message);
                        }
                    }

                    instance.methods.reqService.setconsultationstatus(auditData);
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
        }
    };

};
new consultation().init();
