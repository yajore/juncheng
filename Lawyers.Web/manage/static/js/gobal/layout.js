/**
Core script to handle the entire theme and core functions
**/
var resizeHandlers = [];
var App = {
    getResponsiveBreakpoint: function (size) {
        // bootstrap responsive breakpoints
        var sizes = {
            'xs': 480,     // extra small
            'sm': 768,     // small
            'md': 992,     // medium
            'lg': 1200     // large
        };

        return sizes[size] ? sizes[size] : 0;
    },
    getViewPort: function () {
        var e = window,
            a = 'inner';
        if (!('innerWidth' in window)) {
            a = 'client';
            e = document.documentElement || document.body;
        }

        return {
            width: e[a + 'Width'],
            height: e[a + 'Height']
        };
    },
    isAngularJsApp: function () {
        return (typeof angular == 'undefined') ? false : true;
    },
    destroySlimScroll: function (el) {
        $(el).each(function () {
            if ($(this).attr("data-initialized") === "1") { // destroy existing instance before updating the height
                $(this).removeAttr("data-initialized");
                $(this).removeAttr("style");

                var attrList = {};

                // store the custom attribures so later we will reassign.
                if ($(this).attr("data-handle-color")) {
                    attrList["data-handle-color"] = $(this).attr("data-handle-color");
                }
                if ($(this).attr("data-wrapper-class")) {
                    attrList["data-wrapper-class"] = $(this).attr("data-wrapper-class");
                }
                if ($(this).attr("data-rail-color")) {
                    attrList["data-rail-color"] = $(this).attr("data-rail-color");
                }
                if ($(this).attr("data-always-visible")) {
                    attrList["data-always-visible"] = $(this).attr("data-always-visible");
                }
                if ($(this).attr("data-rail-visible")) {
                    attrList["data-rail-visible"] = $(this).attr("data-rail-visible");
                }

                $(this).slimScroll({
                    wrapperClass: ($(this).attr("data-wrapper-class") ? $(this).attr("data-wrapper-class") : 'slimScrollDiv'),
                    destroy: true
                });

                var the = $(this);

                // reassign custom attributes
                $.each(attrList, function (key, value) {
                    the.attr(key, value);
                });

            }
        });
    },
    addResizeHandler: function (func) {
        resizeHandlers.push(func);
    },
    scrollTo: function (el, offeset) {
        var pos = (el && el.size() > 0) ? el.offset().top : 0;

        if (el) {
            if ($('body').hasClass('page-header-fixed')) {
                pos = pos - $('.page-header').height();
            } else if ($('body').hasClass('page-header-top-fixed')) {
                pos = pos - $('.page-header-top').height();
            } else if ($('body').hasClass('page-header-menu-fixed')) {
                pos = pos - $('.page-header-menu').height();
            }
            pos = pos + (offeset ? offeset : -1 * el.height());
        }

        $('html,body').animate({
            scrollTop: pos
        }, 'slow');
    },
}
var Layout = function () {


    var resBreakpointMd = App.getResponsiveBreakpoint('md');

    //* BEGIN:CORE HANDLERS *//
    // this function handles responsive layout on screen size resize or mobile device rotate.

    // Set proper height for sidebar and content. The content and sidebar height must be synced always.
    var handleSidebarAndContentHeight = function () {
        var content = $('.page-content');
        var sidebar = $('.page-sidebar');
        var body = $('body');
        var height;

        if (body.hasClass("page-footer-fixed") === true && body.hasClass("page-sidebar-fixed") === false) {
            var available_height = App.getViewPort().height - $('.page-footer').outerHeight() - $('.page-header').outerHeight();
            if (content.height() < available_height) {
                content.attr('style', 'min-height:' + available_height + 'px');
            }
        } else {
            if (body.hasClass('page-sidebar-fixed')) {
                height = _calculateFixedSidebarViewportHeight();
                if (body.hasClass('page-footer-fixed') === false) {
                    height = height - $('.page-footer').outerHeight();
                }
            } else {
                var headerHeight = $('.page-header').outerHeight();
                var footerHeight = $('.page-footer').outerHeight();

                if (App.getViewPort().width < resBreakpointMd) {
                    height = App.getViewPort().height - headerHeight - footerHeight;
                } else {
                    height = sidebar.height() + 20;
                }

                if ((height + headerHeight + footerHeight) <= App.getViewPort().height) {
                    height = App.getViewPort().height - headerHeight - footerHeight;
                }
            }
            content.attr('style', 'min-height:' + height + 'px');
        }
    };

    // Handle sidebar menu links
    var handleSidebarMenuActiveLink = function (mode, el) {
        var url = location.hash.toLowerCase();

        var menu = $('.page-sidebar-menu');

        if (mode === 'click' || mode === 'set') {
            el = $(el);
        } else if (mode === 'match') {
            menu.find("li > a").each(function () {
                var path = $(this).attr("href").toLowerCase();
                // url match condition         
                if (path.length > 1 && url.substr(1, path.length - 1) == path.substr(1)) {
                    el = $(this);
                    return;
                }
            });
        }

        if (!el || el.size() == 0) {
            return;
        }

        if (el.attr('href').toLowerCase() === 'javascript:;' || el.attr('href').toLowerCase() === '#') {
            return;
        }

        var slideSpeed = parseInt(menu.data("slide-speed"));
        var keepExpand = menu.data("keep-expanded");

        // disable active states
        menu.find('li.active').removeClass('active');
        menu.find('li > a > .selected').remove();

        if (menu.hasClass('page-sidebar-menu-hover-submenu') === false) {
            menu.find('li.open').each(function () {
                if ($(this).children('.sub-menu').size() === 0) {
                    $(this).removeClass('open');
                    $(this).find('> a > .arrow.open').removeClass('open');
                }
            });
        } else {
            menu.find('li.open').removeClass('open');
        }

        el.parents('li').each(function () {
            $(this).addClass('active');
            $(this).find('> a > span.arrow').addClass('open');

            if ($(this).parent('ul.page-sidebar-menu').size() === 1) {
                $(this).find('> a').append('<span class="selected"></span>');
            }

            if ($(this).children('ul.sub-menu').size() === 1) {
                $(this).addClass('open');
            }
        });

        if (mode === 'click') {
            if (App.getViewPort().width < resBreakpointMd && $('.page-sidebar').hasClass("in")) { // close the menu on mobile view while laoding a page 
                $('.page-header .responsive-toggler').click();
            }
        }
    };

    // Handle sidebar menu
    var handleSidebarMenu = function () {
        // handle sidebar link click
        $('.page-sidebar-menu').on('click', 'li > a.nav-toggle, li > a > span.nav-toggle', function (e) {
            var that = $(this).closest('.nav-item').children('.nav-link');

            if (App.getViewPort().width >= resBreakpointMd && !$('.page-sidebar-menu').attr("data-initialized") && $('body').hasClass('page-sidebar-closed') && that.parent('li').parent('.page-sidebar-menu').size() === 1) {
                return;
            }

            var hasSubMenu = that.next().hasClass('sub-menu');

            if (App.getViewPort().width >= resBreakpointMd && that.parents('.page-sidebar-menu-hover-submenu').size() === 1) { // exit of hover sidebar menu
                return;
            }

            if (hasSubMenu === false) {
                if (App.getViewPort().width < resBreakpointMd && $('.page-sidebar').hasClass("in")) { // close the menu on mobile view while laoding a page 
                    $('.page-header .responsive-toggler').click();
                }
                return;
            }

            if (that.next().hasClass('sub-menu always-open')) {
                return;
            }

            var parent = that.parent().parent();
            var the = that;
            var menu = $('.page-sidebar-menu');
            var sub = that.next();

            var autoScroll = menu.data("auto-scroll");
            var slideSpeed = parseInt(menu.data("slide-speed"));
            var keepExpand = menu.data("keep-expanded");

            if (!keepExpand) {
                parent.children('li.open').children('a').children('.arrow').removeClass('open');
                parent.children('li.open').children('.sub-menu:not(.always-open)').slideUp(slideSpeed);
                parent.children('li.open').removeClass('open');
            }

            var slideOffeset = -200;

            if (sub.is(":visible")) {
                $('.arrow', the).removeClass("open");
                the.parent().removeClass("open");
                sub.slideUp(slideSpeed, function () {
                    if (autoScroll === true && $('body').hasClass('page-sidebar-closed') === false) {
                        if ($('body').hasClass('page-sidebar-fixed')) {
                            menu.slimScroll({
                                'scrollTo': (the.position()).top
                            });
                        } else {
                            App.scrollTo(the, slideOffeset);
                        }
                    }
                    handleSidebarAndContentHeight();
                });
            } else if (hasSubMenu) {
                $('.arrow', the).addClass("open");
                the.parent().addClass("open");
                sub.slideDown(slideSpeed, function () {
                    if (autoScroll === true && $('body').hasClass('page-sidebar-closed') === false) {
                        if ($('body').hasClass('page-sidebar-fixed')) {
                            menu.slimScroll({
                                'scrollTo': (the.position()).top
                            });
                        } else {
                            App.scrollTo(the, slideOffeset);
                        }
                    }
                    handleSidebarAndContentHeight();
                });
            }

            e.preventDefault();
        });



        // handle scrolling to top on responsive menu toggler click when header is fixed for mobile view
        $(document).on('click', '.page-header-fixed-mobile .page-header .responsive-toggler', function () {
            App.scrollTop();
        });

        // handle sidebar hover effect        
        handleFixedSidebarHoverEffect();



    };

    // Helper function to calculate sidebar height for fixed sidebar layout.
    var _calculateFixedSidebarViewportHeight = function () {
        var sidebarHeight = App.getViewPort().height - $('.page-header').outerHeight(true);
        if ($('body').hasClass("page-footer-fixed")) {
            sidebarHeight = sidebarHeight - $('.page-footer').outerHeight();
        }

        return sidebarHeight;
    };

    // Handles fixed sidebar
    var handleFixedSidebar = function () {
        var menu = $('.page-sidebar-menu');

        App.destroySlimScroll(menu);

        if ($('.page-sidebar-fixed').size() === 0) {
            handleSidebarAndContentHeight();
            return;
        }

        if (App.getViewPort().width >= resBreakpointMd) {
            menu.attr("data-height", _calculateFixedSidebarViewportHeight());
            App.initSlimScroll(menu);
            handleSidebarAndContentHeight();
        }
    };

    // Handles sidebar toggler to close/hide the sidebar.
    var handleFixedSidebarHoverEffect = function () {
        var body = $('body');
        if (body.hasClass('page-sidebar-fixed')) {
            $('.page-sidebar').on('mouseenter', function () {
                if (body.hasClass('page-sidebar-closed')) {
                    $(this).find('.page-sidebar-menu').removeClass('page-sidebar-menu-closed');
                }
            }).on('mouseleave', function () {
                if (body.hasClass('page-sidebar-closed')) {
                    $(this).find('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
                }
            });
        }
    };

    // Hanles sidebar toggler
    var handleSidebarToggler = function () {
        var body = $('body');
        if ($.cookie && $.cookie('sidebar_closed') === '1' && App.getViewPort().width >= resBreakpointMd) {
            $('body').addClass('page-sidebar-closed');
            $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        }

        // handle sidebar show/hide
        $('body').on('click', '.sidebar-toggler', function (e) {
            var sidebar = $('.page-sidebar');
            var sidebarMenu = $('.page-sidebar-menu');
            $(".sidebar-search", sidebar).removeClass("open");

            if (body.hasClass("page-sidebar-closed")) {
                body.removeClass("page-sidebar-closed");
                sidebarMenu.removeClass("page-sidebar-menu-closed");
                if ($.cookie) {
                    $.cookie('sidebar_closed', '0');
                }
            } else {
                body.addClass("page-sidebar-closed");
                sidebarMenu.addClass("page-sidebar-menu-closed");
                if (body.hasClass("page-sidebar-fixed")) {
                    sidebarMenu.trigger("mouseleave");
                }
                if ($.cookie) {
                    $.cookie('sidebar_closed', '1');
                }
            }

            $(window).trigger('resize');
        });
    };

    // Handles the horizontal menu
    var handleHorizontalMenu = function () {
        //handle tab click
        $('.page-header').on('click', '.hor-menu a[data-toggle="tab"]', function (e) {
            e.preventDefault();
            var nav = $(".hor-menu .nav");
            var active_link = nav.find('li.current');
            $('li.active', active_link).removeClass("active");
            $('.selected', active_link).remove();
            var new_link = $(this).parents('li').last();
            new_link.addClass("current");
            new_link.find("a:first").append('<span class="selected"></span>');
        });

        // handle search box expand/collapse        
        $('.page-header').on('click', '.search-form', function (e) {
            $(this).addClass("open");
            $(this).find('.form-control').focus();

            $('.page-header .search-form .form-control').on('blur', function (e) {
                $(this).closest('.search-form').removeClass("open");
                $(this).unbind("blur");
            });
        });


        // handle hover dropdown menu for desktop devices only
        $('[data-hover="megamenu-dropdown"]').not('.hover-initialized').each(function () {
            $(this).dropdownHover();
            $(this).addClass('hover-initialized');
        });

        $(document).on('click', '.mega-menu-dropdown .dropdown-menu', function (e) {
            e.stopPropagation();
        });
    };

    // Handles Bootstrap Tabs.
    var handleTabs = function () {
        // fix content height on tab click
        $('body').on('shown.bs.tab', 'a[data-toggle="tab"]', function () {
            handleSidebarAndContentHeight();
        });
    };

    // Handles the go to top button at the footer
    var handleGoTop = function () {
        var offset = 300;
        var duration = 500;

        if (navigator.userAgent.match(/iPhone|iPad|iPod/i)) {  // ios supported
            $(window).bind("touchend touchcancel touchleave", function (e) {
                if ($(this).scrollTop() > offset) {
                    $('.scroll-to-top').fadeIn(duration);
                } else {
                    $('.scroll-to-top').fadeOut(duration);
                }
            });
        } else {  // general 
            $(window).scroll(function () {
                if ($(this).scrollTop() > offset) {
                    $('.scroll-to-top').fadeIn(duration);
                } else {
                    $('.scroll-to-top').fadeOut(duration);
                }
            });
        }

        $('.scroll-to-top').click(function (e) {
            e.preventDefault();
            $('html, body').animate({ scrollTop: 0 }, duration);
            return false;
        });
    };

    // Hanlde 100% height elements(block, portlet, etc)
    var handle100HeightContent = function () {

        $('.full-height-content').each(function () {
            var target = $(this);
            var height;

            height = App.getViewPort().height -
                $('.page-header').outerHeight(true) -
                $('.page-footer').outerHeight(true) -
                $('.page-title').outerHeight(true) -
                $('.page-bar').outerHeight(true);

            if (target.hasClass('portlet')) {
                var portletBody = target.find('.portlet-body');

                App.destroySlimScroll(portletBody.find('.full-height-content-body')); // destroy slimscroll 

                height = height -
                    target.find('.portlet-title').outerHeight(true) -
                    parseInt(target.find('.portlet-body').css('padding-top')) -
                    parseInt(target.find('.portlet-body').css('padding-bottom')) - 5;

                if (App.getViewPort().width >= resBreakpointMd && target.hasClass("full-height-content-scrollable")) {
                    height = height - 35;
                    portletBody.find('.full-height-content-body').css('height', height);
                    App.initSlimScroll(portletBody.find('.full-height-content-body'));
                } else {
                    portletBody.css('min-height', height);
                }
            } else {
                App.destroySlimScroll(target.find('.full-height-content-body')); // destroy slimscroll 

                if (App.getViewPort().width >= resBreakpointMd && target.hasClass("full-height-content-scrollable")) {
                    height = height - 35;
                    target.find('.full-height-content-body').css('height', height);
                    App.initSlimScroll(target.find('.full-height-content-body'));
                } else {
                    target.css('min-height', height);
                }
            }
        });
    };
    //* END:CORE HANDLERS *//

    return {
        // Main init methods to initialize the layout
        //IMPORTANT!!!: Do not modify the core handlers call order.

        initHeader: function () {
            handleHorizontalMenu(); // handles horizontal menu    
        },

        setSidebarMenuActiveLink: function (mode, el) {
            handleSidebarMenuActiveLink(mode, el);
        },

        initSidebar: function () {

            handleSidebarMenu(); 
            handleSidebarToggler(); 

        },

        initContent: function () {
            handle100HeightContent(); 
            handleTabs(); 

            App.addResizeHandler(handleSidebarAndContentHeight); 
            App.addResizeHandler(handle100HeightContent); 
        },

        initFooter: function () {
            handleGoTop(); 
        },
        handInput: function () {
            var _handleInput = function (el) {
                if (el.val() != "") {
                    el.addClass('edited');
                } else {
                    el.removeClass('edited');
                }
            }

            $('body').on('blur', '.form-md-floating-label .form-control', function (e) {
                _handleInput($(this));
            });

        },
        init: function () {
            this.initHeader();
            this.initSidebar();
            this.initContent();
            this.initFooter();
            this.handInput();
        },

        //public function to fix the sidebar and content height accordingly
        fixContentHeight: function () {
            handleSidebarAndContentHeight();
        },

        initFixedSidebarHoverEffect: function () {
            handleFixedSidebarHoverEffect();
        },

        initFixedSidebar: function () {
            handleFixedSidebar();
        },

    };

}();


; (function (define) {
    define(['jquery'], function ($) {
        return (function () {
            var $container;
            var listener;
            var toastId = 0;
            var toastType = {
                error: 'error',
                info: 'info',
                success: 'success',
                warning: 'warning'
            };

            var toastr = {
                clear: clear,
                remove: remove,
                error: error,
                getContainer: getContainer,
                info: info,
                options: {},
                subscribe: subscribe,
                success: success,
                version: '2.1.0',
                warning: warning
            };

            var previousToast;

            return toastr;

            //#region Accessible Methods
            function error(message, title, optionsOverride) {
                return notify({
                    type: toastType.error,
                    iconClass: getOptions().iconClasses.error,
                    message: message,
                    optionsOverride: optionsOverride,
                    title: title
                });
            }

            function getContainer(options, create) {
                if (!options) { options = getOptions(); }
                $container = $('#' + options.containerId);
                if ($container.length) {
                    return $container;
                }
                if (create) {
                    $container = createContainer(options);
                }
                return $container;
            }

            function info(message, title, optionsOverride) {
                return notify({
                    type: toastType.info,
                    iconClass: getOptions().iconClasses.info,
                    message: message,
                    optionsOverride: optionsOverride,
                    title: title
                });
            }

            function subscribe(callback) {
                listener = callback;
            }

            function success(message, title, optionsOverride) {
                return notify({
                    type: toastType.success,
                    iconClass: getOptions().iconClasses.success,
                    message: message,
                    optionsOverride: optionsOverride,
                    title: title
                });
            }

            function warning(message, title, optionsOverride) {
                return notify({
                    type: toastType.warning,
                    iconClass: getOptions().iconClasses.warning,
                    message: message,
                    optionsOverride: optionsOverride,
                    title: title
                });
            }

            function clear($toastElement) {
                var options = getOptions();
                if (!$container) { getContainer(options); }
                if (!clearToast($toastElement, options)) {
                    clearContainer(options);
                }
            }

            function remove($toastElement) {
                var options = getOptions();
                if (!$container) { getContainer(options); }
                if ($toastElement && $(':focus', $toastElement).length === 0) {
                    removeToast($toastElement);
                    return;
                }
                if ($container.children().length) {
                    $container.remove();
                }
            }
            //#endregion

            //#region Internal Methods

            function clearContainer(options) {
                var toastsToClear = $container.children();
                for (var i = toastsToClear.length - 1; i >= 0; i--) {
                    clearToast($(toastsToClear[i]), options);
                };
            }

            function clearToast($toastElement, options) {
                if ($toastElement && $(':focus', $toastElement).length === 0) {
                    $toastElement[options.hideMethod]({
                        duration: options.hideDuration,
                        easing: options.hideEasing,
                        complete: function () { removeToast($toastElement); }
                    });
                    return true;
                }
                return false;
            }

            function createContainer(options) {
                $container = $('<div/>')
                    .attr('id', options.containerId)
                    .addClass(options.positionClass)
                    .attr('aria-live', 'polite')
                    .attr('role', 'alert');

                $container.appendTo($(options.target));
                return $container;
            }

            function getDefaults() {
                return {
                    tapToDismiss: true,
                    toastClass: 'toast',
                    containerId: 'toast-container',
                    debug: false,

                    showMethod: 'fadeIn', //fadeIn, slideDown, and show are built into jQuery
                    showDuration: 300,
                    showEasing: 'swing', //swing and linear are built into jQuery
                    onShown: undefined,
                    hideMethod: 'fadeOut',
                    hideDuration: 1000,
                    hideEasing: 'swing',
                    onHidden: undefined,

                    extendedTimeOut: 1000,
                    iconClasses: {
                        error: 'toast-error',
                        info: 'toast-info',
                        success: 'toast-success',
                        warning: 'toast-warning'
                    },
                    iconClass: 'toast-info',
                    positionClass: 'toast-top-right',
                    timeOut: 5000,
                    titleClass: 'toast-title',
                    messageClass: 'toast-message',
                    target: 'body',
                    closeHtml: '<button>&times;</button>',
                    newestOnTop: true,
                    preventDuplicates: false
                };
            }

            function publish(args) {
                if (!listener) { return; }
                listener(args);
            }

            function notify(map) {
                var options = getOptions(),
                    iconClass = map.iconClass || options.iconClass;

                if (options.preventDuplicates) {
                    if (map.message === previousToast) {
                        return;
                    }
                    else {
                        previousToast = map.message;
                    }
                }

                if (typeof (map.optionsOverride) !== 'undefined') {
                    options = $.extend(options, map.optionsOverride);
                    iconClass = map.optionsOverride.iconClass || iconClass;
                }

                toastId++;

                $container = getContainer(options, true);
                var intervalId = null,
                    $toastElement = $('<div/>'),
                    $titleElement = $('<div/>'),
                    $messageElement = $('<div/>'),
                    $closeElement = $(options.closeHtml),
                    response = {
                        toastId: toastId,
                        state: 'visible',
                        startTime: new Date(),
                        options: options,
                        map: map
                    };

                if (map.iconClass) {
                    $toastElement.addClass(options.toastClass).addClass(iconClass);
                }

                if (map.title) {
                    $titleElement.append(map.title).addClass(options.titleClass);
                    $toastElement.append($titleElement);
                }

                if (map.message) {
                    $messageElement.append(map.message).addClass(options.messageClass);
                    $toastElement.append($messageElement);
                }

                if (options.closeButton) {
                    $closeElement.addClass('toast-close-button').attr("role", "button");
                    $toastElement.prepend($closeElement);
                }

                $toastElement.hide();
                if (options.newestOnTop) {
                    $container.prepend($toastElement);
                } else {
                    $container.append($toastElement);
                }


                $toastElement[options.showMethod](
                    { duration: options.showDuration, easing: options.showEasing, complete: options.onShown }
                );

                if (options.timeOut > 0) {
                    intervalId = setTimeout(hideToast, options.timeOut);
                }

                $toastElement.hover(stickAround, delayedHideToast);
                if (!options.onclick && options.tapToDismiss) {
                    $toastElement.click(hideToast);
                }

                if (options.closeButton && $closeElement) {
                    $closeElement.click(function (event) {
                        if (event.stopPropagation) {
                            event.stopPropagation();
                        } else if (event.cancelBubble !== undefined && event.cancelBubble !== true) {
                            event.cancelBubble = true;
                        }
                        hideToast(true);
                    });
                }

                if (options.onclick) {
                    $toastElement.click(function () {
                        options.onclick();
                        hideToast();
                    });
                }

                publish(response);

                if (options.debug && console) {
                    console.log(response);
                }

                return $toastElement;

                function hideToast(override) {
                    if ($(':focus', $toastElement).length && !override) {
                        return;
                    }
                    return $toastElement[options.hideMethod]({
                        duration: options.hideDuration,
                        easing: options.hideEasing,
                        complete: function () {
                            removeToast($toastElement);
                            if (options.onHidden && response.state !== 'hidden') {
                                options.onHidden();
                            }
                            response.state = 'hidden';
                            response.endTime = new Date();
                            publish(response);
                        }
                    });
                }

                function delayedHideToast() {
                    if (options.timeOut > 0 || options.extendedTimeOut > 0) {
                        intervalId = setTimeout(hideToast, options.extendedTimeOut);
                    }
                }

                function stickAround() {
                    clearTimeout(intervalId);
                    $toastElement.stop(true, true)[options.showMethod](
                        { duration: options.showDuration, easing: options.showEasing }
                    );
                }
            }

            function getOptions() {
                return $.extend({}, getDefaults(), toastr.options);
            }

            function removeToast($toastElement) {
                if (!$container) { $container = getContainer(); }
                if ($toastElement.is(':visible')) {
                    return;
                }
                $toastElement.remove();
                $toastElement = null;
                if ($container.children().length === 0) {
                    $container.remove();
                }
            }
            //#endregion

        })();
    });
}(typeof define === 'function' && define.amd ? define : function (deps, factory) {
    if (typeof module !== 'undefined' && module.exports) { //Node
        module.exports = factory(require('jquery'));
    } else {
        window['toastr'] = factory(window['jQuery']);
    }
}));




jQuery(document).ready(function () {
    Layout.init(); // init metronic core componets
});



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




var util = {};
util.debug = true;
util.queryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return "";
}


toastr.options = {
    "closeButton": true,
    "debug": false,
    "positionClass": "toast-top-center",
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
    //限制最多5个
    var len = file_input.files.length;
    if (len > 5) {
        len = 5;
    }
    for (var i = 0 ; i < len; i++) {

        setTimeout(function (j) {
            var file = file_input.files[j];
            var filedata = { name: file.name, size: file.size }
            var reader = new FileReader();
            reader.onload = function (e) {

                var dataURL = e.target.result,
                    canvas = document.querySelector('canvas'),
                    ctx = canvas.getContext('2d'),
                    img = new Image();

                img.onload = function () {

                    if (op.width == "auto" && op.height == "auto") {
                        op.width = this.width;

                        op.height = this.height;

                    }
                    if (this.width < op.width || this.height < op.height) {

                        op.width = this.width;

                        op.height = this.height;
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
        }, i * 500, i);

    }

    //alert("reader end")
}


util.getUInfo = function () {
    return JSON.parse(uinfo);
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

util.getQueryInfo = function () {
    var uinfodata = JSON.parse(uinfo);

    var data = {};
    data.Header = {
        DisplayName: uinfodata.NickName || uinfodata.Name,
        UserID: uinfodata.UserID,
        DeviceID: 5
    };
    data.PageInfo = {
        PageIndex: 0,
        PageSize: 20
    }
    data.Body = {};
    return data;
}

util.getFrom = function () {

    var data = {};
    data.Header = {
        DisplayName: "customer",
        UserID: 0,
        DeviceID: 5
    };
    data.Body = {};
    return data;
}


util.pageNumDraw = function (options) {

    var option = $.extend({}, util.pageNumDraw.default, options);
    var htmls = [];

    var pageNum = option.getPage();

    var forButs = option.showBtns;

    var pageIndex = option.pageIndex;

    if (pageNum >= 0) {
        //5 
        if (pageIndex >= 1) {
            for (var i = pageIndex - 1; i >= 0; i--) {

                if (forButs <= 0) {
                    break;
                }
                htmls.unshift(option.numBtn.replace("{pageindex}", i).replace("{num}", i + 1));
                forButs--;
            }
        }

        if (pageIndex == 0) {
            htmls.unshift(option.firstBtn.replace("first-btn", "first-btn disabled"));
            htmls.unshift(option.prevBtn.replace("prev-btn", "prev-btn disabled"));
        } else {
            htmls.unshift(option.firstBtn);
            htmls.unshift(option.prevBtn);
        }
        forButs = option.showBtns;
        htmls.push(option.numBtn.replace("{pageindex}", pageIndex).replace("{num}", pageIndex + 1).replace("pageNum-btn", "pageNum-btn disabled"));
        pageIndex = pageIndex + 1;
        for (var i = pageIndex; i < pageIndex + forButs; i++) {

            if (i >= pageNum) {
                break;
            }
            htmls.push(option.numBtn.replace("{pageindex}", pageIndex).replace("{num}", i + 1));
        }

        if (pageIndex == pageNum) {
            htmls.push(option.nextBtn.replace("next-btn", "next-btn disabled"));
            htmls.push(option.lastBtn.replace("last-btn", "last-btn disabled"));
        } else {
            htmls.push(option.nextBtn);
            htmls.push(option.lastBtn);
        }

        htmls.push(option.renderMenuBtn());

        $(".pagination .btn-group").html(htmls.join(''));


        var initEvent = function () {

            $(".pagination .first-btn").on('click', function () {
                option.requery(0, option.pageSize);
            })
            $(".pagination .prev-btn").on('click', function () {
                option.requery(option.pageIndex - 1, option.pageSize);
            })
            $(".pagination .pageNum-btn").on('click', function () {
                option.requery($(this).data("pageindex"), option.pageSize);
            })
            $(".pagination .next-btn").on('click', function () {
                option.requery(option.pageIndex + 1, option.pageSize);
            })
            $(".pagination .last-btn").on('click', function () {
                option.requery(pageNum, option.pageSize);
            })
            $(".pagination .dropdown-menu a").on('click', function () {
                var _pagesize = $(this).data("pagesize");
                option.pageSize = _pagesize;
                option.requery(0, _pagesize);
            })
        }
        initEvent();

    }

}

util.pageNumDraw.default = {
    firstBtn: '<button type="button" class="btn btn-default first-btn">\
                                    <i class="fa fa-angle-double-left"></i>\
                                </button>',
    prevBtn: ' <button type="button" class="btn btn-default prev-btn">\
                                    <i class="fa  fa-angle-left"></i>\
                                </button>',
    numBtn: '<button type="button" class="btn btn-default pageNum-btn" data-pageindex="{pageindex}">{num}</button>',

    nextBtn: '<button type="button" class="btn btn-default next-btn">\
                                    <i class="fa fa-angle-right"></i>\
                                </button>',
    lastBtn: '<button type="button" class="btn btn-default last-btn">\
                                    <i class="fa fa-angle-double-right"></i>\
                                </button>',
    lenMenuBtn: '<div class="btn-group btn-group-sm dropup" role="group">\
                     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">\
                                    每页{pagesize}条<span class="caret" ></span>\
                      </button>\
                      <ul class="dropdown-menu">{list}\
                      </ul>\
                 </div>',
    lenList: '<li class="dropdown-menu-list"><a href="javascript:;" data-pagesize="{count}" >每页{count}条</a></li>',
    pageIndex: 0,
    pageSize: 10,
    totalPage: 0,
    showBtns: 5,
    lengthMenu: [10, 20, 50, 100],
    getPage: function () {
        return this.pageSize % this.totalPage == 0 ?
          parseInt(this.totalPage / this.pageSize) : parseInt(this.totalPage / this.pageSize) + 1;
    },
    renderMenuBtn: function () {
        var _html = "";
        //class="disabled"
        for (var i in this.lengthMenu) {
            if (this.lengthMenu[i] == this.pageSize) {
                _html += this.lenList.replace(/{count}/g, this.lengthMenu[i]).replace("dropdown-menu-list", "disabled");
            } else {
                _html += this.lenList.replace(/{count}/g, this.lengthMenu[i]);
            }

        }
        return this.lenMenuBtn.replace("{pagesize}", this.pageSize).replace("{list}", _html);
    },
    complelt: function () { }
}


util.reqAjax = function (ajaxJson) {

    if (util.debug) {
        console.log(ajaxJson.url, ajaxJson);
    }

    $.ajax({
        url: ajaxJson.url,
        data: ajaxJson.data,
        type: ajaxJson.type || "post",
        dataType: "json",
        success: function (msg) {
            if (util.debug) {
                console.log(ajaxJson.url, msg);
            }
            ajaxJson.callback && ajaxJson.callback(msg);

        }
    })
};

util.reqAjaxHandler = function (ajaxJson) {

    if (util.debug) {
        console.log(ajaxJson.url, ajaxJson);
    }

    $.ajax({
        url: ajaxJson.url,
        data: { "": JSON.stringify(ajaxJson.data) },
        type: ajaxJson.type || "post",
        dataType: "json",
        success: function (msg) {
            if (util.debug) {
                console.log(ajaxJson.url, msg);
            }
            ajaxJson.callback && ajaxJson.callback(msg);

        }
    })
};