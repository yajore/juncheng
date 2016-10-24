
$('.login-bg').backstretch([
           "http://juncheng.oss-cn-shanghai.aliyuncs.com/images/banner/bg1.jpg",
           "http://juncheng.oss-cn-shanghai.aliyuncs.com/images/banner/bg2.jpg",
           "http://juncheng.oss-cn-shanghai.aliyuncs.com/images/banner/bg3.jpg"
], {
    fade: 1000,
    duration: 5000
});

$("#back-btn").click(function () {
    $(".forget-form").addClass("display-hide");
    $(".login-form").removeClass("display-hide");
});

$("#forget-password").click(function () {
    $(".forget-form").removeClass("display-hide");
    $(".login-form").addClass("display-hide");
});

var $uname = $("#txt_account");
var $upwd = $("#txt_password");
$("#btn-login").click(function () {
    var uname = $.trim($uname.val());

    if (uname.length == 0) {
        $uname.focus();
        showMessageBox("请输入账号");
        return false;
    }
    var upwd = $.trim($upwd.val());

    if (upwd.length == 0) {
        $upwd.focus();
        showMessageBox("请输入密码");
        return false;
    }
    var user = util.getFrom();
    user.Body = {
        Account: uname,
        Password: upwd,
        IsRemeber: $("#cb_remeber").prop("checked"),
        "ReferUrl": util.queryString("referUrl")
    };
    var ajaxJson = {};
    ajaxJson.url = "handler/login.ashx";
    ajaxJson.data = JSON.stringify(user);
    ajaxJson.callback = function (result) {
        if (result.ErrCode == 0) {
            location.href = result.Message;
        } else {
            showMessageBox(result.Message);
        }
    }
    reqAjax(ajaxJson);
    return false;
})

var $box = $("#alert-box");
function showMessageBox(msg) {
    $box.html(msg).removeClass("display-hide");
}

function hideMessageBox() {
    $box.addClass("display-hide");
}

function reqAjax(ajaxJson) {
    $.ajax({
        type: "post",
        url: ajaxJson.url,
        dataType: "json",
        data: { "queryString": ajaxJson.data },
        success: function (result) {
            ajaxJson.callback && ajaxJson.callback(result);
        }
    });
}
