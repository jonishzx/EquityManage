<%@ Page Language="C#" Title="字典项目编辑" Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.DictItem>" %>

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
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>

</head>
<body class="easyui-layout" fit="true">
    <ld:Loading ID="Loading1" runat="server" />
    <div region="center" border="false" class="CenterForm">
        <%using (Html.BeginForm())
          {%>
        <table cellpadding="2">
            <tr>
                <td align="right" width="20%">
                    代码：
                </td>
                <td>
                    <input name="Code" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.Code %>" <%=ViewData["EDIT"]!=null && (bool)ViewData["EDIT"]?"readonly=readonly":"" %> maxlength="25" />
                    <%= Html.ValidationMessage("Code")%>
                </td>
            </tr>           
            <tr>
                <td align="right">
                    名称：
                </td>
                <td>
                    <input name="Name" type="text" class="form-item-text easyui-validatebox" missingmessage="必填" 
                        required="true" value="<%=Model.Name %>" maxlength="256" />
                    <%= Html.ValidationMessage("Name")%>
                </td>
            </tr>
             <tr>
                <td align="right">
                    值：
                </td>
                <td>
                    <input name="Value" type="text" class="form-item-text easyui-validatebox" 
                       value="<%=Model.Value %>" maxlength="25" />
                    <%= Html.ValidationMessage("Value")%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    排序：
                </td>
                <td>
                   <input name="ViewOrder" type="text" class="form-item-text easyui-numberspinner" min="1"
                        max="1000" required="true" value="<%=Model.ViewOrder %>" maxlength="250" maxlength="300" />
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