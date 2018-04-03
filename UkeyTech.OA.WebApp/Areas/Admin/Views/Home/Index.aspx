<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register Assembly="UkeyTech.OA.FrameWork" Namespace="RepeaterInMvc.Codes" TagPrefix="MVC"  %>

<%@ Import Namespace="Clover.Web.Core" %>
<%@ Import Namespace="Clover.Config" %>
<%@ Import Namespace="Clover.Web.HTMLRender" %>
<%@ Import Namespace="StackExchange.Profiling" %>
<%@ Register src="../Shared/PopupWin.ascx" tagname="PopupWin" tagprefix="uc2" %>
<%@ Register src="../Shared/Loading.ascx" tagname="Loading" tagprefix="uc1" %>
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
        <%{ var context = ((IWebContext)ViewData["PWebContext"]); %>
        var currPos = '<%= context.CurrentUser.CurrPositionName%>';
        var currGroup = '<%= context.CurrentUser.CurrGroupName%>';
        var oldsessionuserid = '<%= context.CurrentUser.UniqueId%>';
		var currRole = '<%= context.CurrentUser.CurrRoleName%>';
        <%} %>
	     

    </script>
   <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.portal.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Common.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>	
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/dust-full.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/home.min.js")%>"></script>   
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/ForBidBackSpace.js")%>"></script>
</head>
<body class="easyui-layout">
<uc1:Loading ID="Loading1" runat="server" />
<div region="north" border="false">
  <div class="header" style="height:80px;padding:0;overflow:hidden;">
      <div style="float:left;height:80px;width:360px; margin-top:0px; background: url('/Scripts/EasyUI/themes/logo.png') no-repeat;"></div>
      <div class="sysTitle" style="height:60px;"><%=WebSiteConfig.Config.WebAppName%></div>
      <div class="sysInfo">
      <div><strong><%=((IWebContext)ViewData["PWebContext"]).Username%><span id="currPos"></span></strong> </div>
      </div>
      <div class="sysLink">
        <div style="width:150px !important;">
            <%--<a href="<%= Url.Action("Index","Home") %>">首页</a> |--%>
            <%--<a href="#">系统帮助</a> | --%>
            <a href="javascript:void(0)" id="mb1" class="easyui-menubutton" menu="#myconosle" <%--iconCls="icon-tip"--%>>控制台</a>
            <div id="myconosle" style="width:150px;display: none">
            <div id="achangeuser"  iconCls="icon-swithuser" style="width:150px;display: none"><a href="javascript:void(0)">切换用户</a></div>
              <%if (Clover.Config.CPM.PermissionConfig.Config.EnableChangePosition){ %>
                <div id="acChangeCurrPosition"  iconCls="icon-swithuser"><a  href="javascript:void(0)">切换兼职</a></div>
                <%} %>
                <div id="achangepwd"  iconCls="icon-password"><a href="javascript:void(0)">修改密码</a></div>
                <div id="amyInfo"  iconCls="icon-config"><a  href="javascript:void(0)">我的信息</a></div>
                <div  iconCls="icon-undo" onclick="if( confirm('确定要退出后台吗？')) top.window.location.href='<%= Url.Action("Logout","Account") %>';"><a href="javascript:void(0);">[退出系统]</a> </div>
            </div>
            | <a href="javascript:void(0);" onclick="if( confirm('确定要退出后台吗？')) top.window.location.href='<%= Url.Action("Logout","Account") %>';">退出</a>
        </div>
       <%-- <span style="width:60px; float:left; line-height:22px;">系统换肤</span>
        <ul id="skin">
          <li id="skin_0" title="0" <%=UkeyTech.OA.WebApp.Helper.GetSkin()=="skin_0" ? "class='selected'" : "" %>>0</li>
          <li id="skin_1" title="1" <%=UkeyTech.OA.WebApp.Helper.GetSkin()=="skin_1" ? "class='selected'" : "" %>>1</li>
          <li id="skin_2" title="2" <%=UkeyTech.OA.WebApp.Helper.GetSkin()=="skin_2" ? "class='selected'" : "" %>>2</li>
        </ul>--%>
     <!-- <input class="easyui-validatebox" type="text" name="name" ></input>
        <a href="#" class="easyui-linkbutton" >搜索</a>--></div>
      <ul class="mainNav">        
        <% foreach(var m in (List<UkeyTech.WebFW.Model.UserConfig>)ViewData["PMemoItems"]) {%>
            <li onclick="addTab('<%=m.Title %>',null,'<%=m.ConfigValue %>','main')"><%=m.Title %></li>         
        <%}%>      
        <li  class="const" onclick="addTab('站内消息',null,'/Admin/Message','main')"><span class="memo-icon icon icon-message"></span>站内消息</li>       
        <li id="tool" class="customize const"><span class="memo-icon icon icon-tool"></span>自定义</li>
      </ul>
    </div> 
</div>

<div region="west" split="true"  border="true" title="系统菜单" style="width:190px;" class="sidebarbox_bg">  
	<div class="sidebarbox">
		 <ul class="lable">
            <MVC:MvcRepeater ID="rpParentMenu" Name="PMenusItems" runat="server" >
                <ItemTemplate>
                    <li mid="<%# Eval("ModuleID") %>"><%# Eval("Name")%></li>
                </ItemTemplate>
            </MVC:MvcRepeater>
	      </ul>	
          <div class="content">
              <MVC:MvcRepeater ID="rpTreeMenu" Name="PMenusItems" runat="server" >
                <ItemTemplate>
                    <div class="divbox" mid="<%# Eval("ModuleID") %>">
                        <div class="stree"><%# UkeyTech.OA.WebApp.Helper.RenderChildrenNodesVisible((IWebContext)ViewData["PWebContext"],(List<int>)ViewData["ModuleItems"],Eval("Id").ToString())%></div>
                     </div>  
                </ItemTemplate>
              </MVC:MvcRepeater>            
           </div>
    </div>
</div>
  
<div id="content" region="center" split="true" border="true">
    <div id="tabs" class="easyui-tabs" fit="true" border="false">
        <div id="desktop" title="我的桌面[<span id='dstool' class='icon icon-tool' style='padding:0 0 0 15px;'>&nbsp;</span>]" icon="icon icon-home"  closable="false">
            <div id="desktopSetting" style="display:none;z-index:999;position:absolute; background-color:#fff" class="panel-body ">
                <a id="portalsetting" href="javascript:void(0);"><span class="memo-icon icon icon-plugin" style="padding:0;margin:0;"></span>桌面设置</a>
            </div>
		    <div id="pp" style="position:relative;">
			    <div style="width:25%;float:left"><span style="height:2px;width:10px;display:block;">&nbsp;</span>
			    </div>
			    <div style="width:75%;float:left;"><span style="height:2px;width:10px;display:block;">&nbsp;</span>
			    </div>
			    
		    </div>
        </div>
    </div>    
</div>

<uc2:PopupWin ID="PopupWin1" runat="server" />
<div region="south" border="false" style="overflow:hidden;" >
  <div class="footer" style="height:30px;">
    <div class="left"><%--<a href="#">提醒中心</a> |  <a href="#">每日提示</a>--%></div>

    <div class="right">copyright all rights </div>
    <div style="clear:both;"></div>
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

<div id="changeuser" style="display:none;" class="">
    <div class="ym-form linearize-form ym-columnar">
    <div class="ym-form-fields">
        <div class="ym-fbox-text">    
            <label for="UserId">用户名:</label>
            <input id="UserId" name="UserId" maxlength="20" class="easyui-validatebox" required="true"  value="<%=ViewData["PUserId"] %>" type="text" />
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
    <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="ChangeUser();" id="A1">
        确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
            onclick="closeLoginWin()" id="btnCancel">取消</a>
    </div>
</div>
<!--<%if(Request.IsLocal){%><%=StackExchange.Profiling.MiniProfiler.RenderIncludes(RenderPosition.Right)%><%} %>-->
</body>
</html>