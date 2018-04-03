<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Clover.Permission.Model.Module>"
    Title="模块编辑" %>

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
        
    <script type="text/javascript">
       function LoadModuleTree(systemid) {
           $('#ParentID').combotree({
               url: '<%=Url.Action("Module","Permission")%>' + "?Type=GetModuleTree&SystemID=" + systemid +  "&mid=" + "<%=Model.ModuleID %>",
               onClick: function(node) {
                   $("#ParentID").val(node.id);
               },
               onLoadSuccess: function () {
                   if (!$("#selRoot").attr("checked") && $("#ParentID").val()=="-1")
                        $("#selRoot").parent().find(".combo-text").val("");
               }
           });
       }

       function LoadRoot() {
           if ($("#selRoot").attr("checked")) {
               //选择用根             
               $("#selRoot").parent().find(".combo-text").val("根");
               $("#selRoot").parent().find(".combo").hide();
               $("#ParentID").val(-1);
           } else {
               $("#ParentID").val('');
               $("#selRoot").parent().find(".combo-text").val("");
               $("#selRoot").parent().find(".combo").show();
               LoadModuleTree($('#SystemID').val());
           }
       }
       
       $(document).ready(function() {

           if ($("#selRoot").attr("checked")) {
               //选择用根
               $("#selRoot").parent().find(".combo-text").val("根");
               $("#selRoot").parent().find(".combo").hide();
               $("#ParentID").val(-1);
           } else {
             
               LoadModuleTree($('#SystemID').val());
           }

           $("#selRoot").click(function () {
               LoadRoot();
           });

           if ($("#ParentID").val() != -1)
               LoadModuleTree($('#SystemID').val());
       });

    </script>
</head>
<body class="easyui-layout" fit="true">
   
    <ld:Loading ID="Loading1" runat="server" />
    
    <div region="center" border="false" class="CenterForm">
        <%using (Html.BeginForm())
          {%>
        <table cellpadding="2">
            <tr>
                <td>
                    模块代码：
                </td>
                <td>
                    <input name="ModuleCode" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.ModuleCode %>" maxlength="25" />
                    <%= Html.ValidationMessage("ModuleCode")%>
                </td>
            </tr>
            <tr>
                <td>
                    模块名称：
                </td>
                <td>
                    <input name="ModuleName" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.ModuleName %>" maxlength="25" />
                    <%= Html.ValidationMessage("ModuleName")%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    模块标记：
                </td>
                <td>
                    <input name="ModuleTag" type="text" class="form-item-text" value="<%=Model.ModuleTag %>"
                        maxlength="250" maxlength="300" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    所属系统：
                </td>
                <td>
                    <select id="SystemID" name="SystemID" onchange="LoadModuleTree($('#SystemID').val())" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true">
                        <%
                        foreach (var m in (List<Clover.Permission.Model.PMSystem>)ViewData["PMSystemList"])
                            {
                        %>
                        <option value="<%=m.SystemID%>" <%=Model.SystemID == m.SystemID ? "selected=selected" : "" %>>
                            <%=m.SystemName%></option>
                        <%}%>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">
                    所属模块：
                </td>
                <td>
                    <input name="ParentID" id="ParentID" class="easyui-combotree" required="true" value="<%=Model.ParentID.HasValue ? Model.ParentID.ToString() : "" %>" style="width:200px;">                    
                    <label for="selRoot">根</label><input id="selRoot" name="selRoot" type=checkbox <%= Model.ParentID.HasValue || Model.ParentID.Value == 0 || Model.ParentID == -1 ? "" : "checked=checked" %> />
                </td>
            </tr>
            <tr style="display:none;">
                <td>
                    菜单路径：
                </td>
                <td>
                    <input name="ParentTitle" type="text" class="form-item-text" value="<%=Model.ParentTitle %>"
                        maxlength="250" maxlength="300" />（用'|'竖线分隔）
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
            <tr>
                <td align="right">
                    状态：
                </td>
                <td>
                    <%
                        var radioButtonList = new SelectList(new List<ListItem> {
                                    new ListItem { Text = "启用", Value="1",},
                                    new ListItem { Text = "停用", Value="0",}}, "Value", "Text", Model.Status);
                        var htmlAttributes = new Dictionary<string, object> {
                                    { "class", "radioButtonList" },                           
                                };
                        foreach (var radiobutton in radioButtonList)
                        { %>
                    <%=Html.RadioButton("Status", radiobutton.Value, radiobutton.Selected, htmlAttributes)%>
                    <label>
                        <%=radiobutton.Text%></label>
                    <% } %>
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
</body>
<uc1:ScriptBlock ID="ScriptBlockA" runat="server" />
<uc2:PopupWin ID="PopupWin1" runat="server" />
</html>
