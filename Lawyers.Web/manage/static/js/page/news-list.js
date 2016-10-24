
function newsList() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.init = function () {

        var newsstatus = { 1: "待审核", 10: "上架", 11: "下架" };

        var arttype = { 1: "新闻", 2: "每日焦点", 3: "家事国事天下事", 4: "每日普法" };

        var VUE = new Vue({
            el: '#page-newslist',
            data: {
                query: { Title: '', ArtStatus: 0, ArtType: 0 },
                items: []

            },
            computed: {
            },
            methods: {

                search: function () {
                    queryNews();
                },
                getNewsStatus: function (status) {
                    return newsstatus[status];
                },
                getArtType: function (status) {
                    return arttype[status];
                },
                getShortDate: function (time) {
                    return time.split('T')[0];
                },
                getLongDate: function (time) {
                    return time.substr(0, 16).replace("T", " ");
                },
                audit: function (item, auditStatus) {
                    if (item.AuditStatus == auditStatus) {
                        util.toast("当前用户状态无需此操作");
                        return false;
                    }
                    var auditData = {};
                    auditData.data = instance.reqData;
                    auditData.data.Body = { ID: item.ID, ArtStatus: auditStatus };
                    auditData.callback = function (result) {
                        if (result.ErrCode == 0) {
                            item.ArtStatus = auditStatus;
                            util.toast("设置成功.", "success");
                        } else {
                            util.toast(result.Message);
                        }
                    }

                    if (auditStatus == 11) {
                        if (confirm("确定要“下架”当前新闻吗？")) {
                            instance.methods.reqService.setarticlestatus(auditData);
                        }
                    } else {
                        instance.methods.reqService.setarticlestatus(auditData);
                    }
                },
                savesortno: function (_item) {
                    if (_item.SortNo > 9999) {
                        util.toast("最大序号为9999");
                        return false;
                    }
                    setSortNo(_item);
                }
            }
        });



        function delayInit() {

            $('.datetimepicker').datetimepicker({
                format: 'yyyy-mm-dd hh:ii',
                todayHighlight: true,
                language: "zh-CN",
                autoclose: true,
            });


            queryNews();
        }

        setTimeout(delayInit);

        function queryNews() {
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
        }

        function setSortNo(_item) {

            //skillVal
            var auditData = {};
            auditData.data = util.getQueryInfo();
            auditData.data.Body = { "SortNo": _item.SortNo, "ID": _item.ID };
            auditData.callback = function (result) {
                if (result.ErrCode == 0) {
                    util.toast("保存成功.", "success");
                } else {
                    util.toast(result.Message);
                }
            }

            instance.methods.reqService.setarticlesortno(auditData);
        }

    }

    instance.methods = {

        reqService: {
            getlist: function (data) {
                data.url = "../api/article/getitems";
                util.reqAjaxHandler(data);
            },
            setarticlestatus: function (data) {
                data.url = "../api/article/setarticlestatus";
                util.reqAjaxHandler(data);
            },
            setarticlesortno: function (data) {
                data.url = "../api/article/setarticlesortno";
                util.reqAjaxHandler(data);
            },
        }
    };

};

new newsList().init();
