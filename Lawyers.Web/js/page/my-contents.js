
function myConsultation() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {



        var vm = new Vue({
            el: '#page-my-contents',
            data: {
                item: [],

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

            var content = sessionStorage.getItem("juncheng_user_consultation");
            if (content) {

                content = JSON.parse(content);
                content.Mobile = content.Mobile.substr(0, 3) + "****" + content.Mobile.substr(7);
                vm.item = content;
            } else {
                gobal.toast("留言内容缺失", 2);
            }
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
