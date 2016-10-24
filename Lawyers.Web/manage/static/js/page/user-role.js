
function userRole() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.init = function () {
        instance.customerType = { 1: "注册用户", 2: "律师", 0: "未设置" };

        var VUE = new Vue({
            el: '#page-userlist',
            data: {
                query: { UserRoleID: 0, Accout: "" },
                items: [],
                roles: [],
                user: { UserID: 0, RoleID: 0 },
                userpwd: { UserID: 0, Password: "" },
            },
            computed: {
            },
            methods: {

                search: function () {
                    delayInit();
                },
                getrolebyid: function (rid) {
                    return instance.role[rid];
                },
                gettypebyid: function (tid) {
                    return instance.customerType[tid];
                },
                setuserrole: function (item) {
                    VUE.user = { UserID: item.UserID, RoleID: item.UserRoleID };
                    $('#modal-rolesend').modal();
                },
                saveuserrole: function () {
                    var saveUser = this.user;
                    if (saveUser.CustomerType != 2 && saveUser.RoleID == 12) {
                        if (confirm("当前用户还不是领队，是否设置领队权限？")) {
                            updaterole();
                        }

                    } else {
                        updaterole();
                    }

                    function updaterole() {
                        var updRole = {};
                        updRole.data = instance.reqData;
                        updRole.data.Body = saveUser;
                        updRole.callback = function (result) {
                            if (result.ErrCode == 0) {
                                util.toast("设置成功，权限在下次登录生效", "success");
                            } else {
                                util.toast(result.Message);
                            }

                        }
                        instance.methods.reqService.updaterole(updRole);
                    }

                },
                showsetpwdmodal: function (item) {
                    VUE.userpwd = { UserID: item.UserID, Password: "", };
                    $('#modal-setpwd').modal();
                },
                setpassword: function () {
                    var setUserPwd = this.userpwd;
                    if (setUserPwd.Password.length == 0 || setUserPwd.MgrPassword == null || setUserPwd.MgrPassword == '') {
                        util.toast("请设置密码");
                        return false;
                    }
                    function updatePwd() {
                        var updPwd = {};
                        updPwd.data = instance.reqData;
                        updPwd.data.Body = setUserPwd;
                        updPwd.callback = function (result) {
                            if (result.ErrCode == 0) {
                                util.toast("设置成功,需要重新登录生效", "success");
                            } else {
                                util.toast(result.Message);
                            }

                        }
                        instance.methods.reqService.updatepwd(updPwd);
                    }
                    updatePwd();
                },
                getDefaultFace: function (face) {
                    return (face == null || (face + "").length == 0) ? defaultFace : face;
                },
            }
        });



        function delayInit() {

            instance.role = {};
            var queryRole = {};
            queryRole.data = instance.reqData;
            queryRole.data.Body = "";
            queryRole.callback = function (result) {
                if (result.ErrCode == 0) {

                    for (var i in result.Body) {
                        instance.role[result.Body[i].RoleID] = result.Body[i].RoleName;
                    }
                    VUE.roles = result.Body;

                    result.Body.unshift({ "RoleID": 0, "RoleName": "请选择", "Status": "A" });
                    instance.methods.reqService.getusers(queryData);

                } else {
                    util.toast(result.Message);
                }

            }
            instance.methods.reqService.getroles(queryRole);


            var queryData = {};
            queryData.data = instance.reqData;
            queryData.data.Body = {};
            queryData.data.Body = VUE.query;
            queryData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.items = result.Body;

                    util.pageNumDraw({
                        pageIndex: instance.reqData.PageInfo.PageIndex,
                        pageSize: instance.reqData.PageInfo.PageSize,
                        totalPage: result.TotalCount,
                        requery: function (pageIndex, pageSize) {
                            instance.reqData.PageInfo.PageIndex = pageIndex;
                            instance.reqData.PageInfo.PageSize = pageSize;
                            instance.methods.reqService.getusers(queryData);
                        }
                    });
                } else {
                    util.toast(result.Message);
                }

            }

            //util.pageNumDraw({ pageIndex: 0, pageSize: 10, totalPage: 100 });
        }

        setTimeout(delayInit, 200);
    }

    instance.methods = {
        reqService: {
            getusers: function (data) {
                data.url = "../api/roleright/getuserroles";
                util.reqAjax(data);
            },
            getroles: function (data) {
                data.url = "../api/roleright/getroles";
                util.reqAjax(data);
            },
            updaterole: function (data) {
                data.url = "../api/roleright/updateuserrole";
                util.reqAjax(data);
            },
            updatepwd: function (data) {
                data.url = "../api/roleright/updatepwd";
                util.reqAjax(data);
            },
        }
    };

};
new userRole().init();
