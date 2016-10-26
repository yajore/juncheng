
function index() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {



        var vm = new Vue({
            el: '#page-index',
            data: {
                items: [],

            },
            computed: {
            },
            methods: {
                go: function (_id) {
                    location.href = "lawdetail.html?id=" + _id;
                }
            }
        });



        function events() {


        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser(getLaws);
            } else {
                getLaws();
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

        function getLaws() {

            var req = gobal.QueryJson();
            req.Body.CustomerType = 2;
            req.Body.AuditStatus = 10;
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
            instance.api.getLawyers(reqData);

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
        getLawyers: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/customer/lawyers"
            ajaxjson.data = _data.data;
            ajaxjson.data.PageInfo.PageSize = 8;
            ajaxjson.data.PageInfo.SortField = "B.SortNo"
            ajaxjson.data.PageInfo.SortType = "ASC"

            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new index().init();
