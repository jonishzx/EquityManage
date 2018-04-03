<%@ Page Language="C#" Title="功能管理"  Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register src="../Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>
<%@ Register src="../Shared/PopupWin.ascx" tagname="PopupWin" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">  
    <script type="text/javascript">
    var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>';
    </script>  
     <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
	<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/EasyUI/locale/easyui-lang-zh_CN.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/Permission/Function.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
</head>
<body>
    <div class="SearchDiv">
        代码或名称：<input type="text" id="txtFunctionCode" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadFunctionGrid($('#txtFunctionCode').val());">
            查询</a>
    </div>
    <div class="easyui-layout">
        <div id="center" region="center" title="功能列表" style="width: auto;">
            <table id="tbFunction">
            </table>
        </div>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
    <script type="text/javascript" language="javascript">
        $(".easyui-layout").height($(document).height() - 37);

        var baseurl = '<%=Url.Action("Function","Permission")%>'  + "?type=";
        var FunctionList = baseurl + "FunctionList" ;
        var DeleteFunctionsUrl= baseurl + "DeleteFunctions" ;

    </script>
 
</body>
</html>