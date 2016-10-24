
function lawdetail() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {

        var id = gobal.queryString("id").split('#')[0];

        if (id == "") {
            gobal.toast("缺少律师信息");
            return false;
        }
        var vm = new Vue({
            el: '#page-lawdetail',
            data: {
                item: {
                    Company: "公司",
                    Name: "某某",
                    WexinNo: "微信号",
                    Email: "邮件地址",
                    QQNo: "QQ号码",
                    Address: "地址",
                    Subscribe: "还没有填写关注信息",
                    Resume: "该律师还没填写履历",
                    WexinQrcode: "",
                },

            },
            computed: {
            },
            methods: {
                go: function () {
                    location.href = 'message.html?tolaw=' + this.item.UserID + '&law=' + escape(this.item.Name);
                },

            }
        });



        function events() {


        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser(getLaw);
            } else {
                getLaw();
            }


            events();


        }

        setTimeout(Init);

        function getUser(cb) {

            var reqData = {
                data: "",
                callback: function (result) {
                    if (result.ErrCode == 0) {
                        gobal.setUser(result.Body);
                        cb && cb();

                    } else {
                        gobal.toast(result.Message, 2);
                    }
                }
            };
            instance.api.getUser(reqData);

        }

        function getLaw() {

            var req = gobal.QueryJson();
            req.Body = id;
            var reqData = {
                data: req,
                callback: function (result) {
                    if (result.ErrCode == 0) {

                        vm.item = result.Body;
                    } else {
                        gobal.toast(result.Message, 2);
                    }
                }
            };
            instance.api.getLawyer(reqData);

        }


    }
    instance.api = {
        getUser: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/customerhander.ashx"
            ajaxjson.data = _data.data;
            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },
        getLawyer: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/customer/getcustomerbyid"
            ajaxjson.data = _data.data;

            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new lawdetail().init();
