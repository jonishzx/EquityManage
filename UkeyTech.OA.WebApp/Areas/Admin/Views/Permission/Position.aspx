<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Title="岗位管理" %>

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
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/Permission/Position.js")%>"></script>    
</head>
<body>
    <ld:Loading ID="Loading1" runat="server" />
    <div class="SearchDiv">
        代码或名称：<input type="text" id="txtPositionCode" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadPositionGrid($('#txtPositionCode').val());">
            查询</a>
      
    </div>
    <div class="easyui-layout">
       <%-- <div id="west" region="west" split="true" title="组织架构树" style="width: 10px;">
            <ul id="treePosition">
            </ul>
        </div>--%>
        <div id="center" region="center" title="岗位列表" style="width: auto;">
            <table id="tbPosition">
            </table>
        </div>
        <div id="east" region="east" split="true" title="已关联用户" style="width: 10px;">
            <table id="tbPositionUser">
            </table>
        </div>
    </div>
    <div id="dlgUser" title="未关联用户列表" style="width: 650px; height: 400px; display: none;">
        <div border="false" class="DialogSearchDiv">
            代码或名称：<input type="text" id="txtUserCode" style="width: 120px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadUserGrid($('#txtUserCode').val());">
                查询</a>
        </div>
        <table id="tbUser">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
    <script type="text/javascript" language="javascript">
        $(".easyui-layout").height($(document).height() - 37);
        $("#east").width($(document).width() * 0.23);
        $("#west").width($(document).width() * 0.2);

        var baseurl = '<%=Url.Action("Position","Permission")%>' + "?type=";
        var groupurl = '<%=Url.Action("Group","Permission")%>' + "?type=";
        var GetGroupTree = groupurl + "GetGroupTree";
        var PositionList = baseurl + "PositionList";
        var DeletePositionsUrl= baseurl + "DeletePositions" ;
        var PositionUserList = baseurl + "PositionUserList" ;
        var DeletePositionUserUrl = baseurl + "DeletePositionUser" ;
        var AddPositionUserUrl = baseurl + "AddPositionUser" ;
        var PositionNoUserRef = baseurl + "PositionNoUserRef" ;
    </script>
</body>
</html>