var messageBeforeEvent = [];
var messageAfterEvent = [];

//easy-ui生成菜单时绑定方法
function openLink(link) {
    var tabTitle = $(link).text();
    var url = $(link).attr("href");
    var target = $(link).attr("target");
    var icon = $(link).find(".icon").attr("class");
    if (target != "_blank" && url != "")
        addTab(tabTitle, icon, url, target);
    $(link).parent().parent().addClass("selected tree-node-selected");
}

function InitLeftMenu() {
    $('.tree-title a').click(function () {
        $("div.tree-node-selected").removeClass("tree-node-selected");
        openLink(this);
        return false;
    });

    $(".tree-node").click(function () {
        $("div.tree-node-selected").removeClass("tree-node-selected");
        var link = $(this).find("a:first");
        openLink(link);
        return false;
    });

    $(".easyui-accordion").accordion();
}
 
//绑定右键菜单事件
function tabCloseEvent() {

    /*双击关闭TAB选项卡(第一个不处理)*/
    $(".tabs-inner:gt(0)").dblclick(function() {
        var subtitle = $(this).children("span").text();
        $('#tabs').tabs('close', subtitle);
    });
    
    //关闭当前
    $('#tabMenu-tabclose').click(function () {
        var currtab_title = $('#tabMenu').data("currtab");
        $('#tabs').tabs('close', currtab_title);
    })
    //全部关闭
    $('#tabMenu-tabcloseall').click(function () {
        $('.tabs-inner:gt(0) span').each(function (i, n) {
            var t = $(n).text();
            $('#tabs').tabs('close', t);
        });
    });
    //关闭除当前之外的TAB
    $('#tabMenu-tabcloseother').click(function () {
        var currtab_title = $('#tabMenu').data("currtab");
        $('.tabs-inner:gt(0) span').each(function (i, n) {
            var t = $(n).text();
            if (t != currtab_title)
                $('#tabs').tabs('close', t);
        });
    });
    //关闭当前右侧的TAB
    $('#tabMenu-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            return false;
        }
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tabs').tabs('close', t);
        });
        return false;
    });
    //关闭当前左侧的TAB
    $('#tabMenu-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll("li:gt(0)");
        if (prevall.length == 0) {
            return false;
        }
        prevall.each(function (i, n) {
            if (i != 0) {
                var t = $('a:eq(0) span', $(n)).text();
                $('#tabs').tabs('close', t);
            }
        });
        return false;
    });

    //退出
    $("#tabMenu-exit").click(function () {
        $('#tabMenu').menu('hide');
    });
}

//自定义工具栏
function initCustomizeTab() {
    $("#customize").click(function () {

    });
}

//增加选项卡
function refreshTab() {
    var tab = $('#tabs').tabs('getSelected');
    var index = $('#tabs').tabs('getTabIndex', tab);
    var window = $(tab).find("iframe")[0].contentWindow;
    window.location.reload(true);
}

//增加选项卡
function addTab(subtitle, icon, url, target, refresh) {
    if (!$('#tabs').tabs('exists', subtitle)) {
        $('#tabs').tabs('add', {
            title: subtitle,
            icon: icon,
            content: createFrame(target, url),
            closable: true,
            width: $('#center').width() - 10,
            height: $('#center').height() - 26,
            tools: [{
                iconCls: 'icon-mini-refresh',
                handler: function () {
                    refreshTab();
                }
            }]
        });
        var tab = $('#tabs').tabs('getSelected');
        $(tab).dblclick(function () {
            var index = $('#tabs').tabs('getTabIndex', tab);
            $('#tabs').tabs('close', index);
        });
    
    } else {
        $('#tabs').tabs('select', subtitle);
        if (refresh) {
            refreshTab();
        }
    }

    showMenu();
}

function createFrame(name, url) {
    var s = '<iframe name="' + name + '" scrolling="no" frameborder="0"  src="' + url + '" style="width:100%;height:99.9%;" marginheight="0" marginwidth="0"></iframe>';
    return s;
}

function showMenu() {
    $(".tabs-inner:gt(0)").bind('contextmenu', function (e) {
        $('#tabMenu').menu('show', {
            left: e.pageX,
            top: e.pageY
        });

        var subtitle = $(this).children("span").text();
        $('#tabMenu').data("currtab", subtitle);
        return false;
    });
}

var loginwin, pwdchangewin, usereditwin;

//初始化控件
$(function () {

    //////////////////////////////换肤切换
    var $li = $("#skin li");
    $li.click(function () {
        $("#" + this.id).addClass("selected").siblings().removeClass("selected");
        $("link[href*='/styles/theme/skin_']").attr("href", cssbaseurl + (this.id) + "/easyui.css");
        //add cookie
        $.cookie('sysadmin_skin', this.id, { expires: 365 * 1000 * 1000 });
        window.location.href = window.location.href;
        //除了用户桌面外，更改所有框架页里面的skin
        //$("#tabs > .tabs-panels > .panel:gt(0) iframe").contents().find("link[href*='/styles/theme/skin_']").attr("href", cssbaseurl + (this.id) + "/easyui.css");

    })
    ////////////////////////////// input 焦点触发
    $(":input").focus(function () { $(this).addClass("focus"); })
				.blur(function () { $(this).removeClass("focus"); });


    ///////////////////////////////////////我的桌面 版块折叠
    $(".homebox .divbox .box h3 span").click(function () {
        $(this).parent().parent(".box").toggleClass("close");
        //alert("11");
    })

    //////////////////////////////我的桌面 li触发
    $(".homebox > .divbox >.box > ul > li").hover(
			function () { $(this).addClass("hover"); },
			function () { $(this).removeClass("hover"); }
	);
    //////////////////////////////////////////////////
    var $div_li = $(".sidebarbox .lable li");
    var $div_box = $(".sidebarbox .content .divbox");
    for (var i = 0; i < $div_li.length; i++) {
        var treedv = $($div_box).parent().find("[mid='" + $($div_li[i]).attr("mid") + "']");
        if (!$(treedv).find(".stree").text().replace(/\n/g, "") || $(treedv).find("a[href!='']").length ==0) {
            $(treedv).hide();
            $($div_li[i]).hide();
        }
    }
    $div_li.click(function () {
        $(this).addClass("current")                  //当前<li>元素高亮
				.siblings().removeClass("current");  //去掉其它同辈<li>元素的高亮
        var index = $div_li.index(this);    // 获取当前点击的<li>元素 在 全部li元素中的索引。
        //选取子节点。不选取子节点的话，会引起错误。如果里面还有div 
        $div_box.eq(index).show().siblings().hide(); //隐藏其它几个同辈的<div>元素
    }).hover(
			function () {
			    $(this).addClass("hover");
			},
			function () {
			    $(this).removeClass("hover");
			})
    ////////////////////////
    if ($($div_li).find())
        $("ul.lable > li:first").addClass("current");

    //show desktop setting menu
    $("#dstool").mouseover(
        function () {
            $("#desktopSetting").fadeIn(500);
        }
    );

    $("#desktopSetting").mouseleave(function () {
        $(this).fadeOut(500);
    });


    $("div.divbox:first").show();

    InitLeftMenu();
    tabCloseEvent();
    //var ppwidth = $("#pp").css("width") - 20;

    //$("#pp").css("width", ppwidth);
    $(".divLoading").fadeOut("slow", function () {
        $(this).remove();
    });

    initMemo();
    initPortal();

    //切换用户功能
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var h = 150, w = 320;

    loginwin = $("#changeuser");
    $("#achangeuser").click(function () {
        $(loginwin).dialog("open");
    });
    $("#changeuser").css("display", "block");
    loginwin.dialog({
        title: '切换用户',
        width: w,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - h) / 2),
        left: Math.max(0, (windowWidth - w) / 2),
        height: h,
        resizable: false
        //onClose: (fn == undefined) ? function () { } : fn
    });

    //修改密码
    $("#achangepwd").click(function () {
        SetWin(480, 250, '/Admin/Account/ChangePassword/', '修改密码');
        return false;
    });

    //修改用户信息
    $("#amyInfo").click(function () {
        SetWin(480, 350, '/Admin/Account/MyInfoEdit/', '修改个人信息');
        return false;
    });

    //切换用户组织岗位信息
    $("#acChangeCurrPosition").click(function () {
        SetWin(480, 550, '/Admin/Account/ChangeDefaultGroupPosition/', '切换兼职信息');
        return false;
    });

    //大小变化影响部件布局
    $(window).wresize(function () {
        setTimeout(function () { $('#pp').portal('resize') }, 500);
    });
    //messageAfterEvent.push(popupMessager);
    //setInterval(showMessageAlert, 600000); //1min 刷新

    loadCurrPost();

    
});
var messageBeforeEvent = [];
var messageAfterEvent = [];

function popupMessager(data, totalcount, unreadcount) {
    if (unreadcount == 0)
        return;
    var link = "<a href='javascript:void(0);' onclick='addTab(\"站内消息\",null,\"/Admin/Message\",\"main\")'>你有" + unreadcount + "条消息尚未阅读。</a>";
    $.messager.show({
        title: '你有新的消息',
        msg: link,
        timeout: 5000,
        showType: 'slide'
    });
}
function showMessageAlert() {
    if (messageBeforeEvent.length > 0) {
        for (var bm in messageBeforeEvent) {
            messageBeforeEvent[bm]();
        }
    }
    $.getJSON(
            "/Admin/Message/MessageWidget?t" + new Date().toString(),
            function (data) {
                var totalcount = 0;
                var unreadcount = 0;
                if (!data)
                    return;
                for (var i = 0; i < data.length; i++) {
                    for (var j = 0; j < data[i].Items.length; j++) {
                        data[i].Items[j].ReceiveTime = TimeHandler(data[i].Items[j].ReceiveTime);
                        totalcount++;
                        if (!data[i].Items[j].ReadTime || data[i].Items[j].ReadTime == '0001-01-01T00:00:00')
                            unreadcount++;
                    }
                }
                if (messageAfterEvent.length > 0) {
                    for (var bm in messageAfterEvent) {
                        messageAfterEvent[bm](data, totalcount, unreadcount);
                    }
                }
            }
        );
}
function closeLoginWin() {
    $(loginwin).dialog("close");
}

function ChangeUser() {
    var flag = true;
    $(".easyui-validatebox").each(function () {
        if (!$(this).validatebox("isValid")) {
            flag = false;
        }
    });
    if (flag)
        $.ajax({
            type: "POST",
            url: changeuserurl,
            data: { UserId: $("#UserId").val(), Password: $("#Password").val() },
            dataType: 'html',
            success: function (data) {
                if (data) {
                    alert(data);
                }
                else {
                    window.location.reload();
                }
            }
        });
}

function loadLastPage() {
    //打开上次的url功能
    var url = decodeURIComponent(getQueryStr("url"));
    if (url && url.indexOf("Home") < 0)
        addTab("上次访问", null, url, "main");
}

//用户自定义桌面
var panels;
var layout;

function initPortal() {
    $("#portalsetting").click(function () {
        //打开菜单
        SetBackFunc(saveUserPortalSuccess);
        SetWin(800, 520, layoutsetting, '用户桌面设置');
    });

    var homeTab = $("#tabs").find("li:eq(0)");
    $(homeTab).attr("title", "点击刷新桌面").click(function () { refreshPortal(); });
    $(homeTab).find("a").css("cursor", "pointer");

    //loadPortal();
    BindPortal();
}

function refreshPortal() {
    $('#pp').portal('clear');
    loadPortal();
}

function saveUserPortalSuccess() {
    refreshPortal();
    setTimeout("MsgShow('系统提示','保存部件成功。');", 500);
}

function loadPortal() {

    $.ajax({
        type: "GET",
        url: loaduserportal + "?t" + new Date().toString(),
        datatype: 'text',
        success: function (data) {
            panels = strToJson(data);
            loadPortalLayout();
        }
    });
}

function loadPortalLayout() {

    $.ajax({
        type: "GET",
        url: getuserlayout + "?t=" + new Date().toLocaleString(),
        datatype: 'text',
        success: function (data) {
            layout = data;
            BindPortal();
        }
    });
}

function getPanelOptions(id) {
    for (var i = 0; i < panels.length; i++) {
        if (panels[i].id == id) {
            return panels[i];
        }
    }
    return undefined;
}
function getPortalState() {
    var aa = [];
    for (var columnIndex = 0; columnIndex < 3; columnIndex++) {
        var cc = [];
        var panels = $('#pp').portal('getPanels', columnIndex);
        for (var i = 0; i < panels.length; i++) {
            cc.push(panels[i].attr('id'));
        }
        aa.push(cc.join(','));
    }
    return aa.join(':');
}

function savePortalLayout() {
    var state = getPortalState();
    UpdateUserConfigs(state);
}

function addPanels(portalState) {
    var columns = portalState.split(':');
    for (var columnIndex = 0; columnIndex < columns.length; columnIndex++) {
        var cc = columns[columnIndex].split(',');
        for (var j = 0; j < cc.length; j++) {
            var options = getPanelOptions(cc[j]);
            if (options) {
                var p = $('<div/>').attr('id', options.id).appendTo('body');

                //attach change state
                if (options.closable) {
                    options.onClose = function () {
                        $('#pp').portal('remove', this);
                        savePortalLayout();
                    };
                }
                p.panel(options);

                $('#pp').portal('add', {
                    panel: p,
                    columnIndex: columnIndex
                });
            }
        }
    }
}

//我的部件
function BindPortal() {

    $('#pp').portal({
        fit: true,
        border: false,
        onStateChange: function () {
            savePortalLayout();
        }
    });

    //var state = $.cookie('portal-state');
    if (!layout) {
        layout = 'p5,p6:p3,p4:p1,p2'; // the default portal state
    }

    if (layout) {
        addPanels(layout);
        $('#pp').portal('resize');
    }

    setTimeout(loadLastPage, 1000);
}

function UpdateUserConfigs(state) {

    $.ajax({
        type: "POST",
        url: savelayout,
        data: { layout: state },
        datatype: "text",
        success: function (msg) {
            parseMessage(msg);
        }
    });
}

//快捷工具栏
function initMemo() {

    $("#tool").click(function () {
        //打开菜单
        SetBackFunc(saveUserConfigSuccess);
        SetWin(640, 520, memowin, '用户快捷菜单设置');
    });
}

function bindMemo() {
    var count = $(".mainNav li").length - 1;
    $(".mainNav li:lt(" + count + ")").click(function () { $(this).addClass("current").siblings("li").removeClass("current"); });
    $(".mainNav li").hover(function () { $(this).addClass("hover") }, function () { $(this).removeClass("hover") });
}

function refreshUserConfig() {
    var first = $(".mainNav .first");
    var tool = $("#tool");

    $.ajax({
        type: "GET",
        url: memourl,
        datatype: 'json',
        contentType: "json",
        success: function (data) {
            var le = $(".mainNav li").length;
            while (le > 2) {
                $(".mainNav li:eq(1)").remove();
                le = $(".mainNav li").length;
            }

            var datas = strToJson(data);
            var content;
            var len = 1;
            $(datas.rows).each(function () {
                content = '<li onclick="addTab(\'{0}\',null,\'{1}\',\'main\')">{0}</li>'.replace(/\{0\}/g, this.Title).replace(/\{1\}/g, this.ConfigValue);
                $(content).insertAfter($(".mainNav li:eq(" + (len - 1) + ")"));
                len++;
            });

            bindMemo();
        }
    });
}

function saveUserConfigSuccess() {
    refreshUserConfig();
    setTimeout("MsgShow('系统提示','保存成功。');", 500);
}

function CheckCurrUser() {
    $.ajax({
        type: "GET",
        url: iCheckUserUrl + "?t=" + new Date().toString(),
        datatype: 'text',
        success: function (currsessionid) {
            if (oldsessionuserid != currsessionid) {
                clearInterval(timerid);
                delete timerid;
                alert("当前用户已经被注销或重新登录");
                window.location.href = loginurl;
            }
        }
    });
}
function loadCurrPost() {
    if (currGroup)
          $("#currPos").html("[" + currGroup + (currPos ? ("->" + currPos) : "") + (currRole ? ("(" + currRole) + ")": "") + "]");
}

//检查一下用户的登录是否已经覆盖
var timerid = setInterval(CheckCurrUser, 30 * 1000);     

