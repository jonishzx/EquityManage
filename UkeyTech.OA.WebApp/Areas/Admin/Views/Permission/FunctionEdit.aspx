<%@ Page Language="C#" Title="功能编辑" Inherits="System.Web.Mvc.ViewPage<Clover.Permission.Model.Function>" %>

<%@ Register Src="../Shared/ScriptBlock.ascx" TagName="ScriptBlock" TagPrefix="uc1" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <script type="text/javascript">
         if (typeof (Object) === "undefined") {
             window.location.reload();
         }
         var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>';
    </script>
    <title></title>
     <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>

    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/EasyUI/locale/easyui-lang-zh_CN.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/Permission/FunctionEdit.js")%>"></script>
        
    <script type="text/javascript">
        function loadFunctionType() {
            $('#FunctionTag').combobox({
                onChange: function (val1, val2) {
                    changeUI(val1);
                }
            });
        }

        function changeUI(val1) {
            if (val1 == "Special") {
                $("#ModuleSelector").show();
                LoadModuleTree($("#SystemID").val());
            } else {
                $("#ModuleSelector").hide();
                $("#ModuleID").val("");
            }
        }
        function LoadModuleTree(systemid) {
            $('#ModuleID').combotree({
                url: '<%=Url.Action("Module","Permission")%>' + "?Type=GetModuleTree&SystemID=" + systemid,
                onClick: function (node) {
                    $("#ModuleID").val(node.id);
                },
                onLoadSuccess: function () {

                }
            });
        }

        $(document).ready(function () {

            loadFunctionType();
            changeUI($("#FunctionTag").val());
        });

        var baseurl = '<%=Url.Action("Function","Permission")%>' + "?type=";
        var FunctionList = baseurl + "FunctionList";
    </script>
</head>
<body class="easyui-layout" fit="true">
    <ld:Loading ID="Loading1" runat="server" />
    <div region="center" border="false" class="CenterForm">
        <%using (Html.BeginForm())
          {%>
        <table cellpadding="2">
            <tr>
                <td width="100px" align="right">
                    功能代码：
                </td>
                <td>
                    <input name="FunctionCode" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.FunctionCode %>" maxlength="25" />
                    <%= Html.ValidationMessage("FunctionCode")%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    功能名称：
                </td>
                <td>
                    <input name="FunctionName" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.FunctionName %>" maxlength="25" />
                    <%= Html.ValidationMessage("FunctionName")%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    功能类型：
                </td>
                <td>
                    <select id="FunctionTag" name="FunctionTag" style="width:200px;">
		                <option value="Common" <%= Model.FunctionTag == "Common" ? "selected=selected" : "" %>>常用功能</option>
		                <option value="Special" <%= Model.FunctionTag == "Special" ? "selected=selected" : "" %>>专用功能</option>
                    </select>
                    <div id="ModuleSelector" style="display:none;">
                    <select id="SystemID" name="SystemID" onchange="LoadModuleTree($('#SystemID').val())" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true">
                        <%
                        foreach (var m in (List<Clover.Permission.Model.PMSystem>)ViewData["PMSystemList"])
                            {
                        %>
                        <option value="<%=m.SystemID%>" <%= (ViewData["SystemID"]!=null && ViewData["SystemID"].ToString() == m.SystemID.ToString()) ? "selected=selected":""  %>>
                            <%=m.SystemName%></option>
                        <%}%>
                    </select>
                    <br />
                    <input name="ModuleID" id="ModuleID" class="easyui-combotree" value="<%=ViewData["ModuleID"] %>" style="width:200px;" />
                    </div>                   
                </td>
            </tr>
            <tr>
                <td align="right">
                    排序：
                </td>
                <td>
                   <input name="ViewOrd" type="text" class="form-item-text easyui-numberspinner" min="1"
                        max="1000" required="true" value="<%=Model.ViewOrd %>" maxlength="250" maxlength="300" />
                </td>
            </tr>
            <tr style="display:none;">
                <td align="right" >
                    关联功能：
                </td>
                <td>
                    <input id="RelationFunctionID" name="RelationFunctionID" type="text" class="form-item-text" required="true" value="<%=Model.RelationFunctionID %>" maxlength="250" maxlength="300" />
                    <a href="#" id="btnAddAttribute" class="easyui-linkbutton" plain="true" icon="icon-search"
                        onclick="OpenFunctionDialog();LoadFunctionGrid();"></a>
                </td>
            </tr>
            <tr>
                <td align="right">
                    说明：
                </td>
                <td>
                    <textarea name="Descn" class="form-item-text textarea w150"><%=Model.Descn%></textarea>
                </td>
            </tr>
        </table>
        <%} %>
    </div>
    <div region="south" border="false" class="SouthForm">
          <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" runat="server" id="btnCancel">取消</a>
    </div>
    <div id="dlgFunction" title="需要关联的功能列表" style="width: 350px; height: 290px; display: none;">
        <div border="false" class="DialogSearchDiv">
            功能名称或代码：<input type="text" id="txtCode" style="width: 120px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadFunctionGrid($('#txtCode').val());">
                查询</a>
        </div>
        <table id="tbFunction">
        </table>
    </div>
</body>
<uc1:ScriptBlock ID="ScriptBlockA" runat="server" />
<uc2:PopupWin ID="PopupWin1" runat="server" />
</html>