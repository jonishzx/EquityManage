<%@ Page Language="C#" Title="字典信息编辑" Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.Dictionary>" ValidateRequest="false"%>

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
                <td width="100px" align="right">
                    字典代码：
                </td>
                <td>
                    <input name="DictID" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.DictID %>" maxlength="25" />
                    <%= Html.ValidationMessage("DictID")%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    功能标记：
                </td>
                <td>
                    <input name="Tag" type="text" class="form-item-text easyui-validatebox"
                       value="<%=Model.Tag %>" maxlength="25" />
                    <%= Html.ValidationMessage("Tag")%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    名称：
                </td>
                <td>
                    <input name="Name" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.Name %>" maxlength="25" />
                    <%= Html.ValidationMessage("Name")%>
                </td>
            </tr>
             <tr>
                <td align="right">
                    上级字典：
                </td>
                <td>
                  
                    <select id="selParentID" name="selParentID" onchange="$('#ParentId').val($('#selParentID option:selected').val());">
                    <option value='' <%= (string.IsNullOrEmpty(Model.ParentId)) ? "selected=selected" : "" %>>根</option>
                    <%
              foreach (var m in (List<UkeyTech.WebFW.Model.Dictionary>)ViewData["Parentlist"])
                        {
                    %>
                        <option value='<%=m.Id%>' <%=Model.ParentId == m.Id ? "selected=selected" : "" %>><%=m.Name%></option>
                    <%}%>
                    </select>            
                    <input name="ParentId" id="ParentId" type="hidden" value="<%=Model.ParentId%>" />         
                </td>
            </tr>
            <tr>
                <td align="right">
                    脚本语句：
                </td>
                <td>
                    <textarea name="SqlCmd" class="form-item-text textarea w150"><%=Model.SqlCmd%></textarea>
                </td>
            </tr>
            <tr>
                <td align="right">
                    扩展属性：
                </td>
                <td>
                    <textarea name="ExtAttr" class="form-item-text textarea w150"><%=Model.ExtAttr%></textarea>
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