
function search() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {



        var vm = new Vue({
            el: '#page-search',
            data: {
                articles: [],
                lawyers: [],
                keyword: "",
            },
            computed: {
            },
            methods: {
                go: function (_id) {
                    location.href = "lawdetail.html?id=" + _id;
                },
                search: function () {
                    search();
                }
            }
        });



        function events() {


        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser(search);
            } else {
                search();
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

        function search() {

            var req = gobal.QueryJson();

            if (vm.keyword.length == 0) {
                req.Body = "推荐";
            } else {
                req.Body = vm.keyword;
            }

            var reqData = {
                data: req,
                callback: function (result) {
                    if (result.ErrCode == 0) {

                        if (result.Body.Articles) {
                            vm.articles = result.Body.Articles;
                        }
                        if (result.Body.Lawyers) {
                            vm.lawyers = result.Body.Lawyers;
                        }
                    } else {
                        gobal.toast(result.Message, 2);
                    }
                }
            };
            instance.api.search(reqData);

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
        search: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/article/search"
            ajaxjson.data = _data.data;
            ajaxjson.data.PageInfo.PageSize = 4;
            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new search().init();
