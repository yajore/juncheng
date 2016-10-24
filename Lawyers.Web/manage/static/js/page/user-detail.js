
function userDetail() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.init = function () {

        var userid = util.queryString("uid");
        var userstatus = { 1: "待审核", 10: "审核通过", 11: "冻结" };
        var usersex = { 0: "未知", 1: "男", 2: "女" };

        var VUE = new Vue({
            el: '#page-userlist',
            data: {
                UserID: userid,
                item: {}

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
                isNullEmpty: function (str, nullValue) {
                    return (str == null || (str + "").length == 0) ? nullValue || true : false;
                }

            }
        });



        function delayInit() {
            var queryData = {};
            queryData.data = instance.reqData;
            queryData.data.Body = VUE.UserID;
            queryData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.item = result.Body;

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
        reqAjax: function (ajaxJson) {

            if (instance.debug) {
                console.log(ajaxJson.url, ajaxJson);
            }

            $.ajax({
                url: ajaxJson.url,
                headers: ajaxJson.header ? { "Authorization": "BasicAuth " + instance.ticket } : {},
                data: ajaxJson.data,
                type: ajaxJson.type || "post",
                dataType: "json",
                success: function (msg) {
                    if (instance.debug) {
                        console.log(ajaxJson.url, msg);
                    }
                    ajaxJson.callback && ajaxJson.callback(msg);

                }
            })
        },
        reqAjaxHandler: function (ajaxJson) {

            if (instance.debug) {
                console.log(ajaxJson.url, ajaxJson);
            }

            $.ajax({
                url: ajaxJson.url,
                headers: ajaxJson.header ? { "Authorization": "BasicAuth " + instance.ticket } : {},
                data: { "": JSON.stringify(ajaxJson.data) },
                type: ajaxJson.type || "post",
                dataType: "json",
                success: function (msg) {
                    if (instance.debug) {
                        console.log(ajaxJson.url, msg);
                    }
                    ajaxJson.callback && ajaxJson.callback(msg);

                }
            })
        },
        reqService: {
            getlist: function (data) {
                data.url = "../../api/user/getcustomerbyid";
                instance.methods.reqAjax(data);
            },
            setauditstatus: function (data) {
                data.url = "../../api/user/setcustomerstatus";
                instance.methods.reqAjax(data);
            },
        }
    };

};
new userDetail().init();
