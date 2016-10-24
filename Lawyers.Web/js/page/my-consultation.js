
function myConsultation() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {



        var vm = new Vue({
            el: '#page-my-consultation',
            data: {
                items: [],

            },
            computed: {
            },
            methods: {
                go: function (_item) {
                    sessionStorage.setItem("juncheng_user_consultation", JSON.stringify(_item));
                    location.href = "my-content.html";
                },
                getLongDate: function (time) {
                    return time.substr(2, 14).replace("T", " ");
                },
            }
        });



        function events() {


        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser(getcon);
            } else {
                getcon();
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

        function getcon() {

            var req = gobal.QueryJson();
            req.Body = gobal.user.UserID;
            var reqData = {
                data: req,
                callback: function (result) {
                    if (result.ErrCode == 0) {

                        vm.items = result.Body;
                    } else {
                        gobal.toast(result.Message, 2);
                    }
                }
            };
            instance.api.getcon(reqData);

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
        getcon: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/consultation/my"
            ajaxjson.data = _data.data;
            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new myConsultation().init();
