
function find() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {



        var vm = new Vue({
            el: '#page-find',
            data: {
                items: [],
                items1: [],
                items2: [],
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

        function getNews() {

            var req = gobal.QueryJson();
            req.Body = "1,2,3";
            var reqData = {
                data: req,
                callback: function (result) {
                    if (result.ErrCode == 0) {

                        var _item = result.Body;

                        if (_item) {
                            for (var i = 0; i < _item.length; i++) {
                                if (_item[i].ArtType == 1) {
                                    vm.items.push(_item[i]);
                                } else if (_item[i].ArtType == 2) {
                                    vm.items1.push(_item[i]);
                                } else if (_item[i].ArtType == 3) {
                                    vm.items2.push(_item[i]);
                                }
                            }
                        }
                    } else {
                        gobal.toast(result.Message, 2);
                    }
                }
            };
            instance.api.getNews(reqData);

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
        getNews: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/article/getshowitems"
            ajaxjson.data = _data.data;
            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },

    };

};
new find().init();
