<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Title="模块管理" %>

<%@ Register Assembly="UkeyTech.OA.FrameWork" Namespace="RepeaterInMvc.Codes"
    TagPrefix="MVC" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
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
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/Permission/Module.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>

</head>
<body>
    <ld:Loading ID="Loading1" runat="server" />
    <div class="SearchDiv">
        代码或名称：<input type="text" id="txtModuleCode" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadModuleGrid($('#txtModuleCode').val());">
            查询</a>
    </div>
    <div class="easyui-layout">
        <div id="west" region="west" split="true" title="模块树" style="width:120px;">
            <MVC:MvcRepeater ID="rpTreeMenu" Name="PMSystemList" runat="server">
                <ItemTemplate>
                    <div class="divbox">
                        <%--<strong>
                            <%# Eval("SystemName")%></strong>--%>
                        <ul id="treeModule_<%#Eval("SystemID")%>" class="treeModule">                           
                        </ul>
                    </div>
                </ItemTemplate>
            </MVC:MvcRepeater>
            <ul id="treeModule">
            </ul>
        </div>
        <div id="center" region="center" title="模块列表" style="width: auto;">
            <table id="tbModule">
            </table>
        </div>
        <div id="east" region="east" split="true" title="已关联" style="width: 10px;">
            <div id="tabs" class="easyui-tabs" fit="true" border="false">
                <div title="功能" cache="false">
                    <div class="easyui-layout" fit="true">
                        <div region="center" border="false">
                            <table id="tbModuleFunction">
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="dlgFunction" title="未关联功能列表" style="width: 650px; height: 400px; display: none;">
        <div border="false" class="DialogSearchDiv">
            代码或名称：<input type="text" id="txtFunctionCode" style="width: 120px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadFunctionGrid($('#txtFunctionCode').val());">
                查询</a>
        </div>
        <table id="tbFunction">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
    <script type="text/javascript" language="javascript">
        $(".easyui-layout").height($(document).height() - 27);
        $("#east").width($(document).width() * 0.25);
        $("#west").width($(document).width() * 0.2);
        
        var baseurl = '<%=Url.Action("Module","Permission")%>'  + "?type=";
        var GetAllSystemModuleTree = baseurl + "GetAllSystemModuleTree" ;
        var ModuleTree = baseurl + "GetModuleTree" ;
        var ModuleList = baseurl + "ModuleList" ;
        var DeleteModuleUrl= baseurl + "DeleteModule" ;
        var ModuleFunctionListUrl = baseurl + "ModuleFunctionList" ;
        var DeleteModuleFunctionUrl = baseurl + "DeleteModuleFunction" ;
        var AddModuleFunctionUrl = baseurl + "AddModuleFunction" ;
        var NotJoinFunctionByModuleUrl = baseurl + "NotJoinFunctionByModule" ;
    </script>

</body>
</html>
