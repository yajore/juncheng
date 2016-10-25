
function consultation() {
    var instance = this;
    instance.user = gobal.getUser();
    instance.load = false;
    instance.init = function () {

        var tolaw = gobal.queryString("tolaw").split('#')[0];
        var law = unescape(gobal.queryString("law").split('#')[0]);




        if (tolaw == "") {
            gobal.toast("缺少律师信息");
            return false;
        }

        var vm = new Vue({
            el: '#page-message',
            data: {
                item: { ToLawyer: tolaw, Lawyer: law, Contents: "", Mobile: "", Code: "" },
                codeText: "获取验证码",
                stoping: false,
            },
            computed: {
            },
            methods: {
                getCode: function () {
                    if (this.stoping == false) {
                        getCode();
                    }
                },
                add: function () {
                    setMessage();
                }
            }
        });



        function events() {


        }
        function Init() {


            if (gobal.isLogin == false) {
                getUser();
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

        function setMessage() {

            var _item = vm.item;

            if (_item.Contents.length <= 5) {
                gobal.toast("留言内容在5-1000字之间");
                $("#txtContents").focus();
                return false;
            }

            if (_item.Mobile.length == 0) {
                gobal.toast("请填写手机号码");
                $("#txtMobile").focus();
                return false;
            }

            if (_item.Code.length == 0) {
                gobal.toast("请填写短信验证码");
                $("#txtCode").focus();
                return false;

            }

            _item.CustomerFace = instance.user.Face;

            var req = gobal.QueryJson();
            req.Body = _item;
            var reqData = {
                data: req,
                callback: function (result) {
                    if (result.ErrCode == 0) {
                        gobal.toast("留言成功,请耐心等待回复");
                        setTimeout(function () {
                            history.go(-1);
                        }, 2000)
                    } else {
                        gobal.toast(result.Message, 2);
                    }
                }
            };
            instance.api.addConsultation(reqData);

        }

        function getCode() {
            var mobile = vm.item.Mobile;
            if (mobile.length == 0) {
                gobal.toast("请填写手机号码");
                $("#txtMobile").focus();
                return false;
            }
            var req = gobal.QueryJson();
            req.Body = { MsgType: 1, Mobile: mobile };
            var reqData = {
                data: req,
                callback: function (result) {
                    if (result.ErrCode == 0) {

                        var s = 60;
                        var clearid = setInterval(function () {
                            vm.stoping = true;
                            if (s <= 0) {

                                clearInterval(clearid);
                                vm.stoping = false;
                                vm.codeText = "获取验证码";
                            } else {

                                vm.codeText = s + "秒";
                                s--;
                            }
                        }, 1000);

                    } else {
                        gobal.toast(result.Message, 2);
                    }
                }
            };
            instance.api.sendSms(reqData);

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
        sendSms: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/sms/send2"
            ajaxjson.data = _data.data;
            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },
        addConsultation: function (_data) {
            var ajaxjson = {};
            ajaxjson.url = "api/consultation/addconsultation"
            ajaxjson.data = _data.data;
            ajaxjson.callback = _data.callback;
            new gobal.reqAjaxHandler(ajaxjson);
        },
    };

};
new consultation().init();
