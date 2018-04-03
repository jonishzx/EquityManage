<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Clover.Permission.Model.Role>" Title="角色管理" %>
<%@ Register src="../Shared/ScriptBlock.ascx" tagname="ScriptBlock" tagprefix="uc1" %>
<%@ Register src="../Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>
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
</head>
<body class="easyui-layout" fit="true" >
    <ld:Loading ID="Loading1" runat="server" />
    <div region="center" border="false" class="CenterForm">
        <%using (Html.BeginForm())
          {%>
        <table cellpadding="2">
            <tr>
                <td>
                    角色代码：
                </td>
                <td>
                    <input name="RoleCode" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.RoleCode %>" maxlength="25" />
                    <%= Html.ValidationMessage("RoleCode")%>
                </td>
            </tr>
            <tr>
                <td>
                    角色名称：
                </td>
                <td>
                    <input name="RoleName" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.RoleName %>" maxlength="25" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    角色类型：
                </td>
                <td>
                   <select name="RoleTag" id="RoleTag">
                        <option value="Custom" <%=Model.RoleTag =="Custom" ? "selected=selected" :"" %>>自定义角色</option>
                        <option value="System" <%=Model.RoleTag =="System" ? "selected=selected" :"" %>>系统角色</option>
                    </select>
                </td>
            </tr>
           <tr>
                <td align="right">
                    父级：
                </td>
                <td>
                      <select id="selParentID" name="selParentID" onchange="$('#ParentID').val($('#selParentID option:selected').val());">
                       <option value='' <%= (!Model.ParentID.HasValue || Model.ParentID.Value == -1) ? "selected=selected" : "" %>>根</option>
                       <%
              foreach (var m in (List<Clover.Permission.Model.Role>)ViewData["Parentlist"])
                         {
                        %>
                            <option value='<%=m.Id%>' <%=Model.ParentId == m.Id ? "selected=selected" : "" %>><%=m.Name%></option>
                       <%}%>
                       </select>                                         
                    <input name="ParentID" id="ParentID" type="hidden" value="<%=Model.ParentID%>" />
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
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();"
            id="A1">确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)" onclick="CloseTheWin();"
                runat="server" id="btnCancel">取消</a>
    </div>
</body>
<uc1:ScriptBlock ID="ScriptBlockA" runat="server" />
</html>
