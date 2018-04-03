<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Title="字典信息管理" %>

<%@ Register src="~/Areas/Admin/Views/Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>

<%@ Register src="~/Areas/Admin/Views/Shared/PopupWin.ascx" tagname="PopupWin" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">  
    <script type="text/javascript">
        var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>';
    </script>  
    <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
	<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/System/Dict.js")%>"></script>    
</head>
<body>
    <ld:Loading ID="Loading1" runat="server" />
    <div class="SearchDiv">
        <%-- 搜索内容： <input type="text" id="txtDictCode" style="width: 60px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadDictTree($('#txtDictCode').val());">
            字典查询</a>--%>
        <span>
            <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-add" onclick="AddDict();">添加字典</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-edit" onclick="EditDict();">修改字典</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-remove" onclick="DeleteDict();">删除字典</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-reload" onclick="RefreshDict();">刷新缓存</a>
        </span>
       <%-- 搜索内容：<input type="text" id="txtCode" style="width: 60px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadDictItemsGrid($('#txtCode').val());">
             字典项目查询</a>--%>
      
    </div>
    <div class="easyui-layout">
        <div id="west" region="west" split="true" title="参数字典" style="width: 10px;">        
           
            <ul id="treeDictionary">
            </ul>
        </div>
        <div id="center" region="center" title="字典明细" style="width: auto;">
            <table id="tbDictionary">
            </table>
        </div>       
    </div>  
    <uc2:PopupWin ID="PopupWin1" runat="server" />
    <script type="text/javascript" language="javascript">
        $(".easyui-layout").height($(document).height() - 37);
        $("#east").width($(document).width() * 0.25);
        $("#west").width($(document).width() * 0.25);

        var baseurl = '<%=Url.Action("","System")%>';
        var GetDictTree = baseurl + "/GetDictionaryTree";
        var DictItemsUrl = baseurl + "/GetDictItemList";
        var DeleteDictUrl = baseurl + "/DeleteDict";
        var RefreshDictUrl = baseurl + "/RefreshDict";
        var DeleteDictItemUrl = baseurl + "/DeleteDictItem" ;
    </script>
</body>
</html>