<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Clover.Web.Core" %>
<%@ Import Namespace="Clover.Config" %>

<%@ Import Namespace="StackExchange.Profiling" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title><%=WebSiteConfig.Config.WebAppName%></title>
    <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
    <script type="text/javascript">
        var cssbaseurl = '<%=Url.Content("~/Scripts/EasyUI/themes/")%>';

        var memowin = '<%=Url.Action("MemoSetting","Home")%>';
        var memourl = '<%=Url.Action("MemoSettingList","Home")%>';
        var getuserlayout = '<%=Url.Action("GetUserWidgetLayout","Widget")%>';
        var loaduserportal = '<%=Url.Action("GetUserSelectdWidgetList","Widget")%>';
        var layoutsetting = '<%=Url.Action("WidgetSetting","Widget")%>';
        var savelayout = '<%=Url.Action("SaveUserWidget","Widget")%>';

        var changeuserurl = '<%=Url.Action("ChangeUser","Account")%>';

        var panels = <%=ViewData["Panels"]%>;
        var layout = '<%=ViewData["Layouts"]%>';
        var iCheckUserUrl = '<%=Url.Action("GetCurrSessionUserId","Utility")%>';

        var loginurl = '<%= Url.Action("Login","Account") %>';
        <%{
            var context = ((IWebContext)ViewData["PWebContext"]); %>
        var currPos = '<%= context.CurrentUser.CurrPositionName%>';
        var currGroup = '<%= context.CurrentUser.CurrGroupName%>';
        var oldsessionuserid = '<%= context.CurrentUser.UniqueId%>';
        var currRole = '<%= context.CurrentUser.CurrRoleName%>';
        <%} %>
</script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Common.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/home.js?t=1.3")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/ForBidBackSpace.js")%>"></script>
</head>
<body class="easyui-layout">
    <uc1:Loading ID="Loading1" runat="server" />
    <div region="north" border="false">
        <div class="header" style="height: 60px; padding: 0; overflow: hidden;">
            <div style="float: left; height: 60px; width: 250px; margin-top: 0px;">
                <div class="sysTitle" ><%=WebSiteConfig.Config.WebAppName%></div>
            </div>
            <div class="sysInfo">
                <div><strong><%=((IWebContext)ViewData["PWebContext"]).Username%><span id="currPos"></span></strong> </div>
            </div>
            <div class="sysLink">
                <div style="width: 150px !important;">
                    <a href="javascript:void(0)" id="mb1" class="easyui-menubutton" menu="#myconosle">控制台</a>
                    <div id="myconosle" style="width: 150px; display: none">
                        <div id="achangeuser" iconcls="icon-swithuser" style="width: 150px; display: none"><a href="javascript:void(0)">切换用户</a></div>
                        <%if (Clover.Config.CPM.PermissionConfig.Config.EnableChangePosition)
                            { %>
                        <div id="acChangeCurrPosition" iconcls="icon-swithuser"><a href="javascript:void(0)">切换兼职</a></div>
                        <%} %>
                        <div id="achangepwd" iconcls="icon-password"><a href="javascript:void(0)">修改密码</a></div>
                        <div id="amyInfo" iconcls="icon-config"><a href="javascript:void(0)">我的信息</a></div>
                        <div iconcls="icon-undo" onclick="if( confirm('确定要退出后台吗？')) top.window.location.href='<%= Url.Action("Logout","Account") %>';"><a href="javascript:void(0);">[退出系统]</a> </div>
                    </div>
                    | <a href="javascript:void(0);" onclick="if( confirm('确定要退出后台吗？')) top.window.location.href='<%= Url.Action("Logout","Account") %>';">退出</a>
                </div>
            </div>
        </div>
    </div>

    <div region="west" split="true" border="true" title="系统菜单" style="width: 190px;" class="sidebarbox_bg">
        <div class="sidebarbox">
            <ul class="lable">
                <%foreach (var m in (dynamic)ViewData["PMenusItems"]) {%>
                    <li mid="<%:m.ModuleID %>"><%:m.Name%></li>
                <%} %>
            </ul>
            <div class="content">
                <%foreach (var m in (dynamic)ViewData["PMenusItems"]) {%>
                    <div class="divbox" mid="<%:m.ModuleID %>">
                            <div class="stree"><%= UkeyTech.OA.WebApp.Helper.RenderChildrenNodesVisible((IWebContext)ViewData["PWebContext"],
    (List<int>)ViewData["ModuleItems"],m.Id.ToString())%></div>
                        </div>
                <%} %>
            </div>
        </div>
    </div>

    <div id="content" region="center" split="true" border="true">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
            <div id="desktop" title="我的桌面" icon="icon icon-home" closable="false">
                <%--<div id="desktopSetting" style="display: none; z-index: 999; position: absolute; background-color: #fff" class="panel-body ">
                    <a id="portalsetting" href="javascript:void(0);"><span class="memo-icon icon icon-plugin" style="padding: 0; margin: 0;"></span>桌面设置</a>
                </div>--%>
             <%--   <div id="pp" style="position: relative;">
                    <div style="width: 25%; float: left">
                        <span style="height: 2px; width: 10px; display: block;">&nbsp;</span>
                    </div>
                    <div style="width: 75%; float: left;">
                        <span style="height: 2px; width: 10px; display: block;">&nbsp;</span>
                    </div>
                </div>--%>
                <div style="width:500px;height:500px;line-height:400px; margin:0 auto">
                    <h1>欢迎使用区块链股权管理系统</h1>
                </div>
            </div>
        </div>
    </div>

    <uc2:PopupWin ID="PopupWin1" runat="server" />
    <div region="south" border="false" style="overflow: hidden;">
        <div class="footer" style="line-height: 30px;">
            <div class="left"></div>
            <div class="right">copyright all rights </div>
            <div style="clear: both;"></div>
        </div>
    </div>

    <div id="tabMenu" class="easyui-menu" style="width: 150px;">
        <div id="tabMenu-tabclose">关闭</div>
        <div id="tabMenu-tabcloseall">全部关闭</div>
        <div id="tabMenu-tabcloseother">除此之外全部关闭</div>
        <div class="menu-sep"></div>
        <div id="tabMenu-tabcloseleft">当前页左侧全部关闭</div>
        <div id="tabMenu-tabcloseright">当前页右侧全部关闭</div>
        <div class="menu-sep"></div>
        <div id="tabMenu-exit">退出</div>
    </div>

    <div id="changeuser" style="display: none;" class="">
        <div class="ym-form linearize-form ym-columnar">
            <div class="ym-form-fields">
                <div class="ym-fbox-text">
                    <label for="UserId">用户名:</label>
                    <input id="UserId" name="UserId" maxlength="20" class="easyui-validatebox" required="true" value="<%=ViewData["PUserId"] %>" type="text" />
                    <%= Html.ValidationMessage("UserId")%>
                </div>
                <div class="ym-fbox-text">
                    <label for="Password">密&nbsp;&nbsp;&nbsp;码:</label>
                    <input id="Password" name="Password" type="password" class="easyui-validatebox" required="true" maxlength="20" />
                    <%= Html.ValidationMessage("Password")%>
                </div>
            </div>
        </div>
        <div region="south" border="false" class="SouthForm form-action">
            <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="ChangeUser();" id="A1">确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="closeLoginWin()" id="btnCancel">取消</a>
        </div>
    </div>
</body>
</html>
