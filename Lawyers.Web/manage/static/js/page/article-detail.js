
function articleDetail() {
    var instance = this;
    instance.debug = true;
    instance.reqData = util.getQueryInfo();
    instance.init = function () {

        instance.actid = util.queryString("id").split("#")[0];

        if (instance.actid == "") {
            instance.actid = 0;
        } else {
            instance.actid = instance.actid - 0;;
        }

        var VUE = new Vue({
            el: '#page-lawyerdetail',
            data: {
                item: { ArtType: "1", Lawyers: [], Title: "", Summary: "", Cover: "", ID: instance.actid, ArtStatus: 1, Link: "", GroupType: 1 },
                showlink: false,
                showLawyer: false,
                Mobile: "",
                items: [],
                pics: [],
                loadMaterial: false,
                picMode: 1,
                artpics: [],
                toPublish: false,
            },
            computed: {
                selectedCount: {
                    get: function () {
                        return this.pics.filter(function (_item) {
                            return _item.Sysno == 0;
                        }).length;
                    }
                }
            },
            methods: {
                clearDefault: function () {
                    //var html = editor1.$txt.html();
                    //if (html == "<h3>这里输入正文内容</h3>") {
                    //    editor1.$txt.html('<p><br></p>');
                    //}
                },
                save: function () {

                    setArticle();
                },
                preview: function () {

                },
                setCover: function () {

                    var html = editor1.$txt.html();
                    var $imgs = $(html).find("img");

                    if ($imgs.length == 0) {
                        util.toast("正文中没有图片");
                        return false;
                    } else if ($imgs.length == 1) {
                        VUE.item.Cover = $imgs.eq(0).attr("src");
                    } else {

                        //for (var i in $imgs) {
                        //    VUE.artpics.push($($imgs).eq(i - 0).attr("src"));
                        //}
                        $imgs.each(function () {

                            VUE.artpics.push($(this).attr("src"));
                        })
                        //artpics

                        $('#modal-artpics').modal();
                    }


                },
                chooseLaws: function () {
                    $('#modal-laws').modal();
                },
                clearLaws: function () {
                    VUE.item.Lawyers = [];
                    VUE.showLawyer =false;
            },
                    search: function () {
                        searchLaws();
            },
                    chooseCover: function () {
                    $('#modal-pics').modal();
                    if (this.loadMaterial == false) {
                        this.loadMaterial = true;
                        setTimeout(queryMaterials, 500);
                    }
                    this.picMode = 1;
            },
                    chooseArticlePic: function () {
                    $('#modal-pics').modal();
                    if (this.loadMaterial == false) {
                        this.loadMaterial = true;
                        setTimeout(queryMaterials, 500);
                    }
                    this.picMode = 2;
            },
                    seelcted: function (_pic) {

                    if (this.picMode == 1) {
                        VUE.item.Cover = _pic.Url;
                        $('#modal-pics').modal('hide');
                    } else {
                        if (_pic.Sysno == 0) {
                            _pic.Sysno = 1;
                        } else {
                            _pic.Sysno = 0;
                    }
                    }
            },
                    selectedArtPic: function (_pic) {
                    VUE.item.Cover = _pic;
                    $('#modal-artpics').modal('hide');
            },
                    appendPic: function () {

                        //var pics = this.pics.filter(function (_item) {
                        //    return _item.Sysno == 0;
                        //})

                    var imgs = "";
                    var innerHtml = '<p><img style="max-width:100%;" src="{src}"\ class=""></p>';
                    this.pics.forEach(function (_item) {
                        if (_item.Sysno == 0) {
                            imgs += innerHtml.replace('{src}', _item.Url);
                    }
                        _item.Sysno = 1;
                    })
                    editor1.$txt.append(imgs);
            }
        }
    });


        var editor1;

        function delayInit() {
            cretaEditor();
            setTimeout(initActicle, 50);

    }



        function cretaEditor() {

            editor1 = new wangEditor('content1');
            editor1.config.uploadImgUrl = 'handler/imgupload.ashx';
            editor1.config.uploadImgFileName = 'imgFile'
            editor1.config.uploadTimeout = 60 * 1000;
            editor1.config.menus =[
        'source',
        '|',
        'bold',
        'underline',
        'italic',
        'strikethrough',
        'eraser',
        'forecolor',
        'bgcolor',
        '|',
        'quote',
        'fontsize',
        'head',
        'unorderlist',
        'orderlist',
        'alignleft',
        'aligncenter',
        'alignright',
        '|',
        'table',
        'img',
        '|',
        'undo',
        'redo',
        'fullscreen'
        ];
            editor1.create();


            //editor1.$txt.html("<h3>这里输入正文内容</h3>");
    }

        function initActicle() {
            if (instance.actid > 0) {
                var ajaxJson = {
            };

                var reqData = util.getUserInfo();
                reqData.Body = instance.actid;
                ajaxJson.data = reqData;
                ajaxJson.callback = function (result) {
                    if (result.ErrCode == 0) {

                        if (result.Body.Link != null && result.Body.Link.length > 0) {
                            VUE.showlink = true;
                    }

                        if (result.Body.Lawyers != null && result.Body.Lawyers.length > 0) {
                            VUE.showLawyer = true;
                    }
                        //showLawyer
                        editor1.$txt.html(result.Body.Contents);


                        VUE.item = result.Body;

                        setTimeout(searchLaws, 100);
                    } else {
                        util.toast(result.Message);
                }

            }
                instance.methods.reqService.detail(ajaxJson);
        }
    }

        function searchLaws() {

            //skillVal
            var auditData = {
        };
            auditData.data = util.getQueryInfo();
            auditData.data.Body = {
                "Mobile": VUE.Mobile, "CustomerType": 2, "AuditStatus": 10
        };
            auditData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.items = result.Body;


                } else {
                    util.toast(result.Message);
            }
        }

            instance.methods.reqService.queryLaws(auditData);
    }

        function queryMaterials() {

            //skillVal
            var auditData = {
        };
            auditData.data = util.getQueryInfo();
            auditData.data.Body = {
                "Type": 1
        };
            auditData.callback = function (result) {
                if (result.ErrCode == 0) {
                    VUE.pics = result.Body;
                    setTimeout(lazyLoad, 100);
                } else {
                    util.toast(result.Message);
            }
        }

            instance.methods.reqService.queryMaterials(auditData);
    }

        function lazyLoad() {
            new oxLazyloader({
                    imgLazyAttr: 'data-oxlazy-img',
                    dynamic: true,
                    autoDestroy: false,
                    container: '#picSeletor'
        });
    }

        var isloading = false;
        function setArticle() {

            if (isloading) {
                util.toast("正在提交,请稍后...");
                return;
        }
            var _item = VUE.item;

            if (_item.Title.length == 0) {
                util.toast("请填写文章标题");
                $("[name=Title]").focus();
                return false;

        }


            if (_item.Cover.length == 0) {
                util.toast("请选择文章封面");
                return false;
        }

            var text = $.trim(editor1.$txt.text().replace(/\r/g, "").replace(/\n/, ""));

            if (text.length == 0) {
                util.toast("请填写正文内容(正文中必须包含文字信息).");
                return false;
        }

            if (_item.Summary.length == 0) {
                _item.Summary = text.substr(0, 45);
        }

            _item.Contents = editor1.$txt.html();

            if (VUE.toPublish && _item.ArtStatus == 1) {
                _item.ArtStatus = 10;
        }

            var artData = {
        };
            artData.data = util.getQueryInfo();
            artData.data.Body = _item;
            artData.callback = function (result) {
                if (result.ErrCode == 0) {

                    util.toast("保存成功", "success");

                    setTimeout(function () {
                        isloading = false;
                    }, 2000)


                } else {
                    util.toast(result.Message);
                    setTimeout(function () {
                        isloading = false;
                    }, 2000)
            }
        }


            if (_item.ID == 0) {
                instance.methods.reqService.addArticle(artData);
            } else {
                instance.methods.reqService.updateArticle(artData);
        }
            isloading = true;
    }

        setTimeout(delayInit);
}

    instance.methods = {

            reqService: {
                    detail: function (data) {
                data.url = "../api/article/getdetail";
                util.reqAjaxHandler(data);
            },
                    queryLaws: function (data) {
                data.url = "../api/customer/lawyers";
                util.reqAjaxHandler(data);
            },
                    addArticle: function (data) {
                data.url = "../api/article/addarticle";
                util.reqAjaxHandler(data);
            },
                    updateArticle: function (data) {
                data.url = "../api/article/updatearticle";
                util.reqAjaxHandler(data);
            },
                    queryMaterials: function (data) {
                data.data.PageInfo.PageSize = "100";
                data.url = "../api/material/items";
                util.reqAjaxHandler(data);
            },
    }
};

};
new articleDetail().init();


