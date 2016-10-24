
function index() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {

        var id = gobal.queryString("id").split('#')[0];
        if (id == "") {
            gobal.toast("缺少参数", 2);
            return false;
        }
        var vm = new Vue({
            el: '#page-detail',
            data: {
                item: {},

            },
            computed: {
            },
            methods: {
                go: function (_id) {
                    location.href = "lawdetail.html?id=" + _id;
                }, getLongDate: function (time) {
                    return time.substr(2, 14).replace("T", " ");
                },
            }
        });



        function events() {


        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser(getdetail);
            } else {
                getdetail();
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

        function getdetail() {

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
            instance.api.getdetail(reqData);

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
        getdetail: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/article/getdetail"
            ajaxjson.data = _data.data;


            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new index().init();
