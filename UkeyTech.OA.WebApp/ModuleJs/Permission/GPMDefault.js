$(function () {
    $('.easyui-accordion a').click(function () {
        var tabTitle = $(this).text();
        var url = $(this).attr("href");
        var target = $(this).attr("target");
        AddTab(tabTitle, url, target);
        $('.easyui-accordion li div').removeClass("selected");
        $(this).parent().addClass("selected");
        return false;
    }).hover(function () {
        $(this).parent().addClass("hover");
    }, function () {
        $(this).parent().removeClass("hover");
    });

    $('#loginOut').click(function () {
        var logoutUrl = $(this).attr("href");
        $.messager.confirm('系统提示', '您确定要退出系统吗？', function (r) {
            if (r) {
                location.href = logoutUrl;
            }
        });
        return false;
    });

    if ($.messager) {
        $.messager.defaults.ok = '确定';
        $.messager.defaults.cancel = '取消';
    }
})

$(document).ready(function () {
    $(".divLoading").fadeOut("slow");
});

function AddTab(subtitle, url, target) {
    if (!$('#tabs').tabs('exists', subtitle)) {
        $('#tabs').tabs('add', {
            title: subtitle,
            content: CreateFrame(url, target),
            closable: true,
            width: $('#mainPanle').width() - 10,
            height: $('#mainPanle').height() - 26
        });
    } else {
        $('#tabs').tabs('select', subtitle);
    }
}

function CreateFrame(url, target) {
    if (target == undefined || target == '') {
        return '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
    }
    else {
        return '<iframe name="' + target + '" id="' + target + '" scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
    }
}

function RefreshFrame(frameName) {
    //兼容IE FireFox
    var fr = document.getElementById(frameName);

    if (fr != undefined) {
        fr.contentWindow.location.href = fr.contentWindow.location.href;
    }
}