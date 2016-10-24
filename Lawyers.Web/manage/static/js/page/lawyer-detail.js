
function lawyerDetail() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.init = function () {

        var defaultFace = "data:image/svg+xml;charset=UTF-8,%3Csvg%20width%3D%22353%22%20height%3D%22444%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20viewBox%3D%220%200%20353%20444%22%20preserveAspectRatio%3D%22none%22%3E%3Cdefs%3E%3Cstyle%20type%3D%22text%2Fcss%22%3E%23holder_157e01d9df4%20text%20%7B%20fill%3A%23AAAAAA%3Bfont-weight%3Abold%3Bfont-family%3AArial%2C%20Helvetica%2C%20Open%20Sans%2C%20sans-serif%2C%20monospace%3Bfont-size%3A22pt%20%7D%20%3C%2Fstyle%3E%3C%2Fdefs%3E%3Cg%20id%3D%22holder_157e01d9df4%22%3E%3Crect%20width%3D%22353%22%20height%3D%22444%22%20fill%3D%22%23EEEEEE%22%3E%3C%2Frect%3E%3Cg%3E%3Ctext%20x%3D%22119.4140625%22%20y%3D%22231.9%22%3E353x444%3C%2Ftext%3E%3C%2Fg%3E%3C%2Fg%3E%3C%2Fsvg%3E";

        instance.uid = util.queryString("uid").split("#")[0];
        if (instance.uid == "") {
            instance.uid = 0;
        }
        instance.Status = { "A": "启用", "D": "禁用" }
        var VUE = new Vue({
            el: '#page-lawyerdetail',
            data: {
                item: {
                    Face: defaultFace, Name: "",
                    WexinNo: "", Mobile: "",
                    WexinQrcode: defaultFace,
                    Email: "",
                    UserID: instance.uid,
                    Skills: "",
                    Sex: 0,
                    CustomerType: 2,
                    AuditStatus: 1,
                },
                showQrcode: false,
                items: [],
                skillName: "",
                skills: [],
            },
            computed: {
                skillVal: {
                    get: function () {
                        return this.skills.join(',');
                    }
                }
            },
            methods: {
                displayQrcode: function () {
                    this.showQrcode = !this.showQrcode;
                },
                addfile: function (event) {
                    util.drawOnCanvas(event.target, { width: 'auto', height: 'auto', quality: 1 },
                        function (base64, file) {

                            var reqJson = {};
                            reqJson.data = { id: instance.uid, imgdata: base64, type: "user" }
                            reqJson.callback = function (result) {
                                if (result.ErrCode == 0) {
                                    VUE.item.Face = result.Fields;
                                } else {
                                    util.toast(result.Message);
                                }
                            }
                            instance.methods.reqService.saveImg(reqJson);

                        })

                },
                addqr: function (event) {
                    util.drawOnCanvas(event.target, { width: 300, height: 300, quality: 1 },
                        function (base64, file) {

                            var reqJson = {};
                            reqJson.data = { id: instance.uid, imgdata: base64, type: "qrcode" }
                            reqJson.callback = function (result) {
                                if (result.ErrCode == 0) {
                                    VUE.item.WexinQrcode = result.Fields;

                                } else {
                                    util.toast(result.Message);
                                }
                            }
                            instance.methods.reqService.saveImg(reqJson);

                        })

                },
                showModal: function () {
                    $('#modal-skill').modal();
                },
                save: function () {
                    saveUserInfo();
                },
                getStatus: function (stat) {
                    return instance.Status[stat];
                },
                delStatus: function (_item, _index) {
                    _item.Status = "D";
                    setSkill(_item, _index);
                },
                addSkill: function () {

                    if (this.skillName.length == 0) {
                        util.toast("请填写领域名称");
                        $("#txtSkillName").focus();
                        return false;
                    }
                    var _item = { Sysno: 0, Status: 'A', Skill: this.skillName };
                    setSkill(_item);
                },
                isCheck: function (_skill) {

                    if (this.skillVal.length == 0) {
                        return false;
                    }
                    if (_skill.indexOf(this.skillVal) > -1) {
                        return true;
                    }
                    return false;
                }
            }
        });



        function delayInit() {

            getSkills(getLawyers);


            //instance.methods.reqService.getlist(queryData);
            //util.pageNumDraw({ pageIndex: 0, pageSize: 10, totalPage: 100 });
        }

        setTimeout(delayInit);

        function getLawyers() {

            if (instance.uid > 0) {
                var queryData = {};
                queryData.data = instance.reqData;
                queryData.data.Body = instance.uid;
                queryData.callback = function (result) {
                    if (result.ErrCode == 0) {
                        if (result.Body.Skills) {
                            VUE.skills = result.Body.Skills.split(',');
                        } else {
                            VUE.skills = [];
                        }
                        if (result.Body.BirthDay == "0001-01-01T00:00:00") {
                            result.Body.BirthDay = "";
                        }
                        if (result.Body.Face == null || result.Body.Face == "") {
                            result.Body.Face = defaultFace;
                        }

                        if (result.Body.WexinQrcode == null || result.Body.WexinQrcode == "") {
                            result.Body.WexinQrcode = defaultFace;
                        }

                        VUE.item = result.Body;
                    } else {
                        util.toast(result.Message);
                    }

                }
                instance.methods.reqService.getitem(queryData);
            }

        }

        function saveUserInfo() {
            var _item = VUE.item;

            if (_item.Face == defaultFace) {
                _item.Face = "";
            }

            if (_item.WexinQrcode == defaultFace) {
                _item.WexinQrcode = "";
            }
            if (_item.Name == "") {
                util.toast("请填写姓名");
                return false;
            }
            if (_item.Mobile == "") {
                util.toast("请填写手机");
                return false;
            }
            if (VUE.skills.length == 0) {
                util.toast("请选择领域");
                return false;
            }
            _item.Skills = VUE.skillVal;

            //skillVal
            var auditData = {};
            auditData.data = util.getQueryInfo();
            auditData.data.Body = _item;
            auditData.callback = function (result) {
                if (result.ErrCode == 0) {
                    util.toast("保存成功.", "success");
                } else {
                    util.toast(result.Message);
                }
            }

            instance.methods.reqService.saveinfo(auditData);
        }

        function getSkills(cb) {
            var reqData = {};
            reqData.data = util.getQueryInfo();
            reqData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.items = result.Body;
                    cb && cb();
                } else {
                    util.toast(result.Message);
                }
            }

            instance.methods.reqService.getskills(reqData);
        }

        function setSkill(_item, _index) {
            var reqData = {};
            reqData.data = util.getQueryInfo();
            reqData.data.Body = _item;
            reqData.callback = function (result) {
                if (result.ErrCode == 0) {
                    if (_item.Sysno == 0) {
                        util.toast("添加成功", "success");
                        VUE.skillName = "";
                        getSkills();
                    } else {

                        VUE.items.splice(_index - 0, 1);
                        for (var i in VUE.skills) {
                            if (VUE.skills[i] == _item.Skill) {
                                VUE.skills.splice(i - 0, 1);
                            }
                        }

                        util.toast("删除成功", "success");
                    }
                } else {
                    util.toast(result.Message);
                }
            }

            instance.methods.reqService.setSkill(reqData);
        }

    }

    instance.methods = {

        reqService: {
            getitem: function (data) {
                data.url = "../api/user/getcustomerbyid";
                util.reqAjax(data);
            },
            saveinfo: function (data) {
                data.url = "../api/user/setcustomer";
                util.reqAjaxHandler(data);
            },
            saveImg: function (data) {
                data.url = "handler/fileupload.ashx";
                util.reqAjax(data);
            },
            getskills: function (data) {
                data.url = "../api/user/getcustomerskill";
                util.reqAjax(data);
            },
            setSkill: function (data) {
                data.url = "../api/user/setcustomerskill";
                util.reqAjax(data);
            },
        }
    };

};
new lawyerDetail().init();
