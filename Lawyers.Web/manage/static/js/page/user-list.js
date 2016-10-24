
function userList() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.init = function () {

        var userstatus = { 1: "待审核", 10: "审核通过", 11: "冻结" };
        var usersex = { 0: "未知", 1: "男", 2: "女" };

        var VUE = new Vue({
            el: '#page-userlist',
            data: {
                query: { NickName: '', AuditStatus: 0, CustomerType: 1, Mobile: '' },
                items: []

            },
            computed: {
            },
            methods: {

                search: function () {
                    delayInit();
                },
                getUserStatus: function (status) {
                    return userstatus[status];
                },
                getSex: function (status) {
                    return usersex[status];
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
                    auditData.data.Body = { UserID: item.UserID, UserStatus: auditStatus };
                    auditData.callback = function (result) {
                        if (result.ErrCode == 0) {
                            item.AuditStatus = auditStatus;
                            util.toast("设置成功.", "success");
                        } else {
                            util.toast(result.Message);
                        }
                    }

                    if (auditStatus == 11) {
                        if (confirm("确定要“冻结”此账号吗？")) {
                            instance.methods.reqService.setauditstatus(auditData);
                        }
                    } else {
                        instance.methods.reqService.setauditstatus(auditData);
                    }
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

        setTimeout(delayInit, 200);
    }

    instance.methods = {

        reqService: {
            getlist: function (data) {
                data.url = "../api/user/getcustomers";
                util.reqAjax(data);
            },
            setauditstatus: function (data) {
                data.url = "../api/user/setcustomerstatus";
                util.reqAjax(data);
            },
        }
    };

};
new userList().init();
