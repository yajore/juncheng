
function laws() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {


        var vm = new Vue({
            el: '#page-my-news',
            data: {
                items: [],

            },
            computed: {
            },
            methods: {
                go: function (_id) {
                    location.href = "lawdetail.html?id=" + _id;
                },
                getLongDate: function (time) {
                    return time.substr(2, 8).replace("T", " ");
                },
            }
        });



        function events() {

            var wscroll = function () {

               
                $(window).scroll(function () {

                    var w_height = $(window).height();

                    var tops = $(this).scrollTop();
                    if ($(document).height() - tops - w_height == 0) {


                        getNews(true);

                    }

                });
            };
            setTimeout(wscroll);

        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser(getNews);
            } else {
                getNews();
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

        var req = gobal.QueryJson();

        function getNews(append) {
            if (instance.load) {
                gobal.toast("没有更多内容了.");
                return false;
            }

            req.Body.ArtType = 4;

            var reqData = {
                data: req,
                callback: function (result) {
                    if (result.ErrCode == 0) {

                        if (result.Body.length < req.PageInfo.PageSize) {
                            instance.load = true;
                        }

                        if (append) {
                            vm.items = vm.items.concat(result.Body);

                        } else {
                            vm.items = result.Body;
                        }

                      
                        req.PageInfo.PageIndex++;
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
            ajaxjson.url = "api/article/getdailynews"
            ajaxjson.data = _data.data;
            ajaxjson.data.PageInfo.PageSize = 8;

            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new laws().init();
