


var util = {};
util.queryString = function () {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return "";
}


toastr.options = {
    "closeButton": true,
    "debug": false,
    "positionClass": "toast-bottom-center",
    "onclick": null,
    "showDuration": "1000",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}
util.toast = function (msg, type, title) {
    //type= success info  warning  error
    toastr[type || "error"](msg, title || "")
}
util.drawOnCanvas = function (file_input, op, callback) {
    if (file_input == null || file_input.files.length == 0) {
        util.toast("请先上传图片");
        return false;
    }
    var file = file_input.files[0];
    var filedata = { name: file.name, size: file.size }
    var reader = new FileReader();
    reader.onload = function (e) {

        var dataURL = e.target.result,
            canvas = document.querySelector('canvas'),
            ctx = canvas.getContext('2d'),
            img = new Image();

        img.onload = function () {

            if (op.width == "auto" && op.height == "auto") {
                if (this.width > 500) {
                    op.width = 500;
                } else {
                    op.width = this.width;
                }

                if (this.height > 500) {
                    op.height = 500;
                } else {
                    op.height = this.height;
                }

            }
            if (this.width < op.width || this.height < op.height) {

                util.toast("上传的图片分辨率须大于" + op.width + "*" + op.height + "(当前图片"
                     + this.width + "*" + this.height + ").");

                $(file_input).val("");
                return false;
            }
            var square = op.width > op.height ? op.width : op.height;
            canvas.width = op.width;
            canvas.height = op.height;
            var context = canvas.getContext('2d');
            context.clearRect(0, 0, canvas.width, canvas.height);
            //alert("img onload clearRect")
            var imageWidth;
            var imageHeight;
            var offsetX = 0;
            var offsetY = 0;
            if (this.width > op.width) {
                //imageWidth = Math.round(square * this.width / this.height);
                imageWidth = this.width;
                offsetX = Math.round((imageWidth - op.width) / 2);
            }
            if (this.height > op.height) {
                imageHeight = this.height;
                offsetY = Math.round((imageHeight - op.height) / 2);
            }
            context.drawImage(this, offsetX, offsetY, op.width, op.height, 0, 0, op.width, op.height);
            //alert("img onload drawImage")
            var base64 = canvas.toDataURL('image/jpeg', op.quality || 0.7);
            //alert("img callback")
            callback && callback(base64, filedata);
        };
        img.src = dataURL;

        reader.abort();
    };
    reader.readAsDataURL(file);
    //alert("reader end")
}


util.getUserInfo = function () {
    var uinfodata = JSON.parse(uinfo);
    var data = {};
    data.Header = {
        DisplayName: uinfodata.NickName || uinfodata.Name,
        UserID: uinfodata.UserID,
        DeviceID: 5
    };
    data.Body = {};
    return data;
}



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

