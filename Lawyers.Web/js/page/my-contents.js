
function myConsultation() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {

        var cid = gobal.queryString("cid").split('#')[0];

        if (cid == "") {
            gobal.toast("缺少留言id");
            return false;
        }

        var vm = new Vue({
            el: '#page-my-contents',
            data: {
                item: {},

            },
            computed: {
            },
            methods: {
                getLongDate: function (time) {
                    return time.substr(2, 14).replace("T", " ");
                },
            }
        });



        function events() {


        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser(getReply);
            } else {
                getReply();
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
                        location.href = "jump.ashx?return_url=" + escape('my-content.html?cid=' + cid);
                    }
                }
            };
            instance.api.getUser(reqData);

        }

        function getReply() {

            var req = gobal.QueryJson();
            req.Body = cid;
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
            instance.api.getreplay(reqData);

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
        getreplay: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/consultation/getreplybyid"
            ajaxjson.data = _data.data;
            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new myConsultation().init();
