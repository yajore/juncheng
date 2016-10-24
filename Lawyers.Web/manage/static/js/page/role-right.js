
function roleright() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.menuData = [];
    instance.menuExtendData = [];
    instance.roleid = 0;
    instance.init = function () {


        var VUE = new Vue({
            el: '#page-role',
            data: {
                query: { ActivityStatus: 0 },
                roles: [],
                role: { RoleID: 0, Status: "A" }

            },
            computed: {
            },
            methods: {

                search: function () {
                    delayInit();
                },
                create: function () {
                    var newrole = { RoleID: 0, Status: "A", Remark: "", RoleName: "" };
                    this.role = newrole;
                    $('#modal-edit').modal();
                },
                edit: function (item) {
                    item.Status = "A";
                    this.role = item;
                    $('#modal-edit').modal();
                },
                save: function () {
                    var set_role_data = VUE.role;

                    var roles = VUE.roles;
                    if (set_role_data.RoleID == 0) {
                        for (var i in roles) {
                            if (roles[i].RoleName == set_role_data.RoleName) {
                                util.toast("已存在此角色名.");
                                return false;
                            }
                        }
                    } else {
                        for (var i in roles) {
                            if (roles[i].RoleID == set_role_data.RoleID) {
                                continue;
                            }
                            if (roles[i].RoleName == set_role_data.RoleName) {
                                util.toast("已存在此角色名.");
                                return false;
                            }
                        }
                    }

                    var queryData = {};
                    queryData.data = instance.reqData;
                    queryData.data.Body = set_role_data;
                    queryData.callback = function (result) {
                        if (result.ErrCode == 0) {

                            localStorage.setItem("user-roles", JSON.stringify(result.Body))
                            VUE.roles = result.Body;
                            //if (set_role_data.RoleID > 0 && set_role_data.Status == "D") {
                            //    for (var i in roles) {

                            //        if (roles[i].RoleID == set_role_data.RoleID)
                            //            roles.splice(i - 0, 1);
                            //    }

                            //} else if (set_role_data.RoleID == 0) {
                            //    set_role_data.RoleID=
                            //    roles.unshift(set_role_data);
                            //}

                            
                            $('#modal-edit').modal("hide");

                            util.toast("设置成功.", "success");
                        } else {
                            util.toast(result.Message);
                        }

                    }

                    instance.methods.reqService.setrole(queryData);
                },
                getusertole: function (roleid, event) {
                    instance.roleid = roleid;
                    var $tr = $(event.target).closest("tr");
                    $tr.addClass("warning").siblings().removeClass("warning");
                    var $tree = $('#tree_1').jstree(true);
                    $tree.deselect_all();
                    var $tree2 = $('#tree_2').jstree(true);
                    $tree2.deselect_all();
                    var queryData = {};
                    queryData.data = instance.reqData;
                    queryData.data.Body = roleid;
                    queryData.callback = function (result) {
                        if (result.ErrCode == 0) {
                            if (result.Body.length > 0) {

                                for (var i in result.Body) {
                                    if (result.Body[i].MID < 0) {
                                        $('#tree_2').jstree('select_node', result.Body[i].MID);
                                    } else {
                                        $('#tree_1').jstree('select_node', result.Body[i].MID);
                                    }

                                }
                            }
                        } else {
                            util.toast(result.Message);
                        }

                    }
                    instance.methods.reqService.getrolelist(queryData);

                },
                saveroleright: function () {
                    if (instance.roleid == 0) {
                        util.toast("请先【双击】左侧角色信息行");
                        return false;
                    }
                    if (instance.menuData.length == 0) {
                        util.toast("请先选择当前角色权限树");
                        return false;
                    }
                    if (instance.menuExtendData.length > 0) {
                        for (var i in instance.menuExtendData) {
                            instance.menuData.push(instance.menuExtendData[i]);
                        }
                    }

                    var queryData = {};
                    queryData.data = instance.reqData;
                    queryData.data.Body = {};
                    queryData.data.Body.RoleID = instance.roleid;
                    queryData.data.Body.RightIDs = instance.menuData;
                    queryData.callback = function (result) {
                        if (result.ErrCode == 0) {
                            util.toast("设置角色成功", "success");
                        } else {
                            util.toast(result.Message);
                        }

                    }
                    instance.methods.reqService.setrolelist(queryData);


                }
            }
        });



        function delayInit() {
            var queryData = {};
            queryData.data = instance.reqData;
            queryData.data.Body = "A";
            queryData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.roles = result.Body;


                } else {
                    util.toast(result.Message);
                }

            }
            instance.methods.reqService.getlist(queryData);
            //util.pageNumDraw({ pageIndex: 0, pageSize: 10, totalPage: 100 });

            var handleSample1 = function (result) {


                $('#tree_1').jstree({
                    'plugins': ["wholerow", "checkbox", "types"],
                    'core': {
                        "themes": {
                            "responsive": false
                        },
                        'data': result
                    },
                    "types": {
                        "default": {
                            "icon": "fa fa-folder"
                        }
                    }
                });


            }

            var handleSample2 = function (result) {

                $('#tree_2').jstree({
                    'plugins': ["wholerow", "checkbox", "types"],
                    'core': {
                        "themes": {
                            "responsive": false
                        },
                        'data': result
                    },
                    "types": {
                        "default": {
                            "icon": "fa fa-folder"
                        }
                    }
                });

                //redraw
                // $('#tree_2').jstree(true).destroy(true);
            }

            $('#tree_1').on("changed.jstree", function (e, treedata) {
                instance.menuData = [];
                var selected = treedata.instance.get_selected(true);
                for (var i in selected) {
                    //id
                    var mid = selected[i].id;
                    if (mid != "#" && instance.menuData.indexOf(mid) == -1) {
                        instance.menuData.push(mid);
                    }


                    for (var j in selected[i].parents) {

                        var mid_item = selected[i].parents[j];
                        if (mid_item != "#" && instance.menuData.indexOf(mid_item) == -1) {
                            instance.menuData.push(mid_item);
                        }
                    }
                    //instance.menuData.push(selected[i].parents);
                }
                console.log(instance.menuData);
                //console.log(treedata.instance.get_selected(true));
                //console.log(treedata.instance.get_node(treedata.selected[0]));
            });

            $('#tree_2').on("changed.jstree", function (e, treedata) {
                instance.menuExtendData = [];
                var selected = treedata.instance.get_selected(true);
                for (var i in selected) {
                    //id
                    var mid = selected[i].id;
                    if (selected[i].parent != "#") {
                        instance.menuExtendData.push(mid);
                    }

                }
                console.log(instance.menuExtendData);
            });


            //$('#tree_1').jstree(true).select_node('-1');

            //setTimeout(function () {
            //    $('#tree_1').jstree('select_node', '-1');

            //    $('#tree_1').jstree('select_node', '1001');
            //}, 1000)


            var queryTree = {};
            queryTree.data = instance.reqData;
            queryTree.callback = function (result) {
                if (result.ErrCode == 0) {
                    handleSample1(result.Body.PageRight);

                    handleSample2(result.Body.PageRightExtend);

                } else {
                    util.toast(result.Message);
                }

            };
            instance.methods.reqService.gettree(queryTree);
        }

        setTimeout(delayInit, 200);
    }

    instance.methods = {
        reqService: {
            getlist: function (data) {
                data.url = "../api/roleright/getroles";
                util.reqAjax(data);
            },
            setrole: function (data) {
                data.url = "../api/roleright/setrole";
                util.reqAjax(data);
            },
            gettree: function (data) {
                data.url = "../api/roleright/gettree";
                util.reqAjax(data);
            },
            getrolelist: function (data) {
                data.url = "../api/roleright/getuserrolerightlist";
                util.reqAjax(data);
            },
            setrolelist: function (data) {
                data.url = "../api/roleright/setuserrolerightlist";
                util.reqAjax(data);
            },
        }
    };

};
new roleright().init();
