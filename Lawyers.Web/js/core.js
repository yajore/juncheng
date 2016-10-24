var gobal = {
    debug: true,

    isLogin: false,
    user: {},
    getUser: function () {
        var userstr = localStorage.getItem("juncheng_h5_user");
        if (userstr) {
            this.isLogin = true;
            this.user = JSON.parse(userstr);
            return this.user;
        }

        return null;
    },
    setUser: function (_data) {
        if (_data) {
            this.user = _data;
            this.isLogin = true;
            localStorage.setItem("juncheng_h5_user", JSON.stringify(_data));
        }
    },
    reqJson: function () {
        var data = {};
        data.Header = {
            DisplayName: this.user.NickName || "customer",
            UserID: this.user.UserID || 1,
            DeviceID: 5
        };
        data.Body = {};
        return data;
    },
    QueryJson: function () {
        var data = {};
        data.Header = {
            DisplayName: this.user.NickName || "customer",
            UserID: this.user.UserID || 1,
            DeviceID: 5
        };
        data.PageInfo = {
            PageSize: 20,
            PageIndex: 0,
        };
        data.Body = {};
        return data;
    },
    queryString: function (name) {
        var url = location.href;
        var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
        var paraObj = {}
        for (i = 0; j = paraString[i]; i++) {
            paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
        }
        var returnValue = paraObj[name.toLowerCase()];
        if (typeof (returnValue) == "undefined") {
            return "";
        }
        else {
            return returnValue;
        };
    },
    lazy: function () {

        var oxLazyloader = function (options) {
            this.options = $.extend({}, oxLazyloader.defaults, options);
            this._init();
        };


        oxLazyloader.prototype = {
            constructor: 'oxLazyloader',

            _init: function () {
                var opts = this.options;
                this.container = opts.container && $(opts.container).length !== 0 ? $(opts.container) : $(document);
                /*若无动态插入的内容,缓存待加载的内容*/
                if (!opts.dynamic) {
                    this._filterItems();
                    if (this.imgLazyItems.length === 0 && this.moduleLazyItems.length === 0 && this.bgLazyItems.length === 0) {
                        return; /*无加载内容 return*/
                    }
                }
                this._bindEvent();
                this._loadItems();/*初始化时，load一次*/
            },

            /*
             * 获取需lazyload的图片 和模块
            */
            _filterItems: function () {
                var opts = this.options;
                this.bgLazyItems = this.container.find('[' + opts.bgLazyAttr + ']');
                this.imgLazyItems = this.container.find('[' + opts.imgLazyAttr + ']');
                this.moduleLazyItems = this.container.find('[' + opts.moduleLazyAttr + ']');
            },

            _bindEvent: function () {
                var self = this;
                this._loadFn = function () {
                    self._loadItems();
                }
                $(window).bind('resize.' + this.constructor + ' scroll.' + this.constructor, this._loadFn);
            },

            /*
             * 加载lazy内容
            */
            _loadItems: function () {
                /*dynamic为true(存在动态内容)时，每次都取一次待加载内容*/
                var opts = this.options, self = this;
                if (opts.dynamic) {
                    this._filterItems();
                }
                this._loadBgItems();
                this._loadImgItems();
                this._loadModuleItems();
                /*无动态插入时，更新待加载内容*/
                if (!opts.dynamic) {
                    this._filterItems();
                    /*完全加载时，若autoDestroy为true，执行destroy*/
                    if (this.bgLazyItems === 0 && this.imgLazyItems.length === 0 && this.moduleLazyItems.length === 0 && opts.autoDestroy) {
                        self.destroy();
                    }
                }
            },

            /*
             * 加载背景图
            */
            _loadBgItems: function () {
                var opts = this.options, self = this, attr = opts.bgLazyAttr, items = this.bgLazyItems;
                $.each(items, function () {
                    var item = $(this);
                    if (self._isInViewport(item)) {
                        var src = item.attr(attr);
                        item.css("background-image", "url(" + src + ")").removeAttr(attr);
                    }
                });
            },

            /*
             * 加载图片
            */
            _loadImgItems: function () {
                var opts = this.options, self = this, attr = opts.imgLazyAttr, items = this.imgLazyItems;
                $.each(items, function () {
                    var item = $(this);
                    if (self._isInViewport(item)) {
                        var src = item.attr(attr);
                        item.attr('src', src).removeAttr(attr);
                    }
                });
            },

            /*
             * 加载模块
            */
            _loadModuleItems: function (targetId) {
                var opts = this.options, self = this, attr = opts.moduleLazyAttr, callback = opts.moduleLoadCallback, items = this.moduleLazyItems;

                if (targetId) {
                    $.each(items, function () {
                        var item = $(this);
                        var id = item.attr(attr);

                        if (id == targetId) {
                            if (self._isInViewport(item)) {
                                item.removeAttr(attr);
                                if (typeof callback == 'function') {
                                    callback(id, item);
                                }
                            }
                        }
                    });
                } else {
                    $.each(items, function () {
                        var item = $(this);
                        if (self._isInViewport(item)) {
                            var id = item.attr(attr);
                            item.removeAttr(attr);
                            if (typeof callback == 'function') {
                                callback(id, item);
                            }
                        }
                    });
                }

            },


            /*
             * 是否在视窗中 暂只考虑垂直方向
            */
            _isInViewport: function (item) {
                var win = $(window),
                    scrollTop = win.scrollTop(),
                    threshold = !isNaN(item.data('threshold')) ? item.data('threshold') : this.options.threshold,
                    maxTop = scrollTop + win.height() + threshold,
                    minTop = scrollTop - threshold,
                    itemTop = item.offset().top,
                    itemBottom = itemTop + item.outerHeight();
                if (itemTop > maxTop || itemBottom < minTop) {
                    return false;
                }
                return true;
            },

            /*
             * 停止监听
            */
            destroy: function () {
                $(window).unbind('resize.' + this.constructor + ' scroll.' + this.constructor, this._loadFn);
            }
        };

        /*
         * static function
         * 替换Html代码中src
        */
        oxLazyloader.toLazyloadHtml = function (html, imgLazyAttr, placeholder) {
            var reg;
            imgLazyAttr = imgLazyAttr || this.defaults.imgLazyAttr;
            placeholder = placeholder || this.defaults.placeholder;
            reg = /(<img.*?)src=["']([^"']+)["'](.*?>)/gi;
            return html.replace(reg, '$1src=\"' + placeholder + '\" ' + imgLazyAttr + '=\"$2\" $3');
        }

        /*
         * 默认配置
        */
        oxLazyloader.defaults = {
            container: 'document',
            placeholder: 'http://wsstatic.mbaobao.com/v5/images/blank.png', /*占位图地址*/
            bgLazyAttr: 'data-oxlazy-bg', /*bg lazyload属性名，值为图片真实地址*/
            imgLazyAttr: 'data-oxlazy-img', /*img lazyload属性名，值为图片真实地址*/
            moduleLazyAttr: 'data-oxlazy-module', /*module lazyload属性名，值为module ID, ID作为回调事件的参数*/
            autoDestroy: true, /*容器内的图片都加载完成时，是否自动停止监听*/
            dynamic: false, /*是否有动态的内容插入*/
            threshold: 0, /*预加载的距离*/
            moduleLoadCallback: null
        }

        window.oxLazyloader = oxLazyloader;
    },
    getNowDate: function (nowDate, fmt) {
        var o = {
            "M+": nowDate.getMonth() + 1,
            "d+": nowDate.getDate(),
            "h+": nowDate.getHours() % 12 == 0 ? 12 : nowDate.getHours() % 12,
            "H+": nowDate.getHours(),
            "m+": nowDate.getMinutes(),
            "s+": nowDate.getSeconds(),
            "q+": Math.floor((nowDate.getMonth() + 3) / 3),
            "S": nowDate.getMilliseconds()
        };
        var week = {
            "0": "/u65e5",
            "1": "/u4e00",
            "2": "/u4e8c",
            "3": "/u4e09",
            "4": "/u56db",
            "5": "/u4e94",
            "6": "/u516d"
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (nowDate.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        if (/(E+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[nowDate.getDay() + ""]);
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    },
    initAjax: function () {


        //if ($(document).ajaxStart) {
        //    $(document).ajaxStart(function () {
        //        $('#loadingToast').show();
        //    }).ajaxComplete(function () {
        //        $('#loadingToast').hide();
        //    }).ajaxError(function (alt) {
        //        $('#loadingToast').hide();
        //        console && console.log(JSON.stringify(alt));
        //    })
        //}
    },
    lazyLoad: function () {

        var oxLazyloader = function (options) {
            this.options = $.extend({}, oxLazyloader.defaults, options);
            this._init();
        };


        oxLazyloader.prototype = {
            constructor: 'oxLazyloader',

            _init: function () {
                var opts = this.options;
                this.container = opts.container && $(opts.container).length !== 0 ? $(opts.container) : $(document);
                /*若无动态插入的内容,缓存待加载的内容*/
                if (!opts.dynamic) {
                    this._filterItems();
                    if (this.imgLazyItems.length === 0 && this.moduleLazyItems.length === 0 && this.bgLazyItems.length === 0) {
                        return; /*无加载内容 return*/
                    }
                }
                this._bindEvent();
                this._loadItems();/*初始化时，load一次*/
            },

            /*
             * 获取需lazyload的图片 和模块
            */
            _filterItems: function () {
                var opts = this.options;
                this.bgLazyItems = this.container.find('[' + opts.bgLazyAttr + ']');
                this.imgLazyItems = this.container.find('[' + opts.imgLazyAttr + ']');
                this.moduleLazyItems = this.container.find('[' + opts.moduleLazyAttr + ']');
            },

            _bindEvent: function () {
                var self = this;
                this._loadFn = function () {
                    self._loadItems();
                }
                $(window).bind('resize.' + this.constructor + ' scroll.' + this.constructor, this._loadFn);
            },

            /*
             * 加载lazy内容
            */
            _loadItems: function () {
                /*dynamic为true(存在动态内容)时，每次都取一次待加载内容*/
                var opts = this.options, self = this;
                if (opts.dynamic) {
                    this._filterItems();
                }
                this._loadBgItems();
                this._loadImgItems();
                this._loadModuleItems();
                /*无动态插入时，更新待加载内容*/
                if (!opts.dynamic) {
                    this._filterItems();
                    /*完全加载时，若autoDestroy为true，执行destroy*/
                    if (this.bgLazyItems === 0 && this.imgLazyItems.length === 0 && this.moduleLazyItems.length === 0 && opts.autoDestroy) {
                        self.destroy();
                    }
                }
            },

            /*
             * 加载背景图
            */
            _loadBgItems: function () {
                var opts = this.options, self = this, attr = opts.bgLazyAttr, items = this.bgLazyItems;
                $.each(items, function () {
                    var item = $(this);
                    if (self._isInViewport(item)) {
                        var src = item.attr(attr);
                        item.css("background-image", "url(" + src + ")").removeAttr(attr);
                    }
                });
            },

            /*
             * 加载图片
            */
            _loadImgItems: function () {
                var opts = this.options, self = this, attr = opts.imgLazyAttr, items = this.imgLazyItems;
                $.each(items, function () {
                    var item = $(this);
                    if (self._isInViewport(item)) {
                        var src = item.attr(attr);
                        item.attr('src', src).removeAttr(attr);
                    }
                });
            },

            /*
             * 加载模块
            */
            _loadModuleItems: function (targetId) {
                var opts = this.options, self = this, attr = opts.moduleLazyAttr, callback = opts.moduleLoadCallback, items = this.moduleLazyItems;

                if (targetId) {
                    $.each(items, function () {
                        var item = $(this);
                        var id = item.attr(attr);

                        if (id == targetId) {
                            if (self._isInViewport(item)) {
                                item.removeAttr(attr);
                                if (typeof callback == 'function') {
                                    callback(id, item);
                                }
                            }
                        }
                    });
                } else {
                    $.each(items, function () {
                        var item = $(this);
                        if (self._isInViewport(item)) {
                            var id = item.attr(attr);
                            item.removeAttr(attr);
                            if (typeof callback == 'function') {
                                callback(id, item);
                            }
                        }
                    });
                }

            },


            /*
             * 是否在视窗中 暂只考虑垂直方向
            */
            _isInViewport: function (item) {
                var win = $(window),
                    scrollTop = win.scrollTop(),
                    threshold = !isNaN(item.data('threshold')) ? item.data('threshold') : this.options.threshold,
                    maxTop = scrollTop + win.height() + threshold,
                    minTop = scrollTop - threshold,
                    itemTop = item.offset().top,
                    itemBottom = itemTop + item.outerHeight();
                if (itemTop > maxTop || itemBottom < minTop) {
                    return false;
                }
                return true;
            },

            /*
             * 停止监听
            */
            destroy: function () {
                $(window).unbind('resize.' + this.constructor + ' scroll.' + this.constructor, this._loadFn);
            }
        };

        /*
         * static function
         * 替换Html代码中src
        */
        oxLazyloader.toLazyloadHtml = function (html, imgLazyAttr, placeholder) {
            var reg;
            imgLazyAttr = imgLazyAttr || this.defaults.imgLazyAttr;
            placeholder = placeholder || this.defaults.placeholder;
            reg = /(<img.*?)src=["']([^"']+)["'](.*?>)/gi;
            return html.replace(reg, '$1src=\"' + placeholder + '\" ' + imgLazyAttr + '=\"$2\" $3');
        }

        /*
         * 默认配置
        */
        oxLazyloader.defaults = {
            container: 'document',
            placeholder: 'http://wsstatic.mbaobao.com/v5/images/blank.png', /*占位图地址*/
            bgLazyAttr: 'data-oxlazy-bg', /*bg lazyload属性名，值为图片真实地址*/
            imgLazyAttr: 'data-oxlazy-img', /*img lazyload属性名，值为图片真实地址*/
            moduleLazyAttr: 'data-oxlazy-module', /*module lazyload属性名，值为module ID, ID作为回调事件的参数*/
            autoDestroy: true, /*容器内的图片都加载完成时，是否自动停止监听*/
            dynamic: false, /*是否有动态的内容插入*/
            threshold: 0, /*预加载的距离*/
            moduleLoadCallback: null
        }

        window.oxLazyloader = oxLazyloader;

    },
    init: function () {
        this.initAjax();
        this.lazyLoad();
    },
    reqAjax: function (ajaxJson) {

        if (ajaxJson.header) {
            var header = gobal.reqJson();
            header.Body = {};
            header.Body = ajaxJson.data;
            ajaxJson.data = header;
        }

        if (gobal.debug) {
            console.group("请求参数");
            console.info(JSON.stringify(ajaxJson.data));
            console.groupEnd();
        }

        $.ajax({
            url: ajaxJson.url,
            data: ajaxJson.data,
            type: ajaxJson.type || "post",
            dataType: "json",
            success: function (msg) {
                ajaxJson.callback && ajaxJson.callback(msg);

            }
        })
    },
    reqAjaxHandler: function (ajaxJson) {

        if (ajaxJson.header) {
            var header = gobal.reqJson();
            header.Body = {};
            header.Body = ajaxJson.data;
            ajaxJson.data = header;
        }

        if (gobal.debug) {
            console.group("请求参数");
            console.info(JSON.stringify(ajaxJson.data));
            console.groupEnd();
        }

        $.ajax({
            url: ajaxJson.url,
            data: { "": JSON.stringify(ajaxJson.data) },
            type: ajaxJson.type || "post",
            dataType: "json",
            success: function (msg) {
                ajaxJson.callback && ajaxJson.callback(msg);

            }
        })
    },
    toast: function (msg, ses) {
        var _html = $('<div class="mint-toast is-placebottom">\
       <span class="mint-toast-text">' + msg + '</span>\
                  </div>');
        $("body").append(_html);
        _html.show();
        setTimeout(function () {
            _html.remove();
        }, ses * 1000 || 2500);

    },
    brower: function () {
        var _browser = {
            versions: function () {
                var u = navigator.userAgent, app = navigator.appVersion;
                return {         //移动终端浏览器版本信息
                    trident: u.indexOf('Trident') > -1, //IE内核
                    presto: u.indexOf('Presto') > -1, //opera内核
                    webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                    gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
                    mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                    ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                    android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或uc浏览器
                    iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器
                    iPad: u.indexOf('iPad') > -1, //是否iPad
                    webApp: u.indexOf('Safari') == -1 //是否web应该程序，没有头部与底部
                };
            }(),
            language: (navigator.browserLanguage || navigator.language).toLowerCase()
        };

        this.ismobile = _browser.versions.mobile;
        var ua = navigator.userAgent.toLowerCase();//获取判断用的对象
        this.inweixin = function () {
            return this.ismobile && ua.match(/MicroMessenger/i) == "micromessenger"
        }
        this.inweibo = function () {
            return this.ismobile && ua.match(/WeiBo/i) == "weibo"
        }
        this.inqq = function () {
            return this.ismobile && ua.match(/QQ/i) == "qq"
        }
        this.inios = function () {
            return this.ismobile && _browser.versions.ios
        }
        this.inandroid = function () {
            return this.ismobile && _browser.versions.android
        }


    }
};

gobal.init();