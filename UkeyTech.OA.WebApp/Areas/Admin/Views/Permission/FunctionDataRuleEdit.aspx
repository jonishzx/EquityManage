<%@ Page Language="C#" Title="数据权限编辑" Inherits="System.Web.Mvc.ViewPage<Clover.Permission.Model.FunctionDataRule>" %>

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
   
</head>
<body class="easyui-layout" fit="true">
    <ld:Loading ID="Loading1" runat="server" />
    <div region="center" border="false" class="CenterForm">
        <%using (Html.BeginForm())
          {%>
        <table cellpadding="2">
            <tr>
                <td width="100px" align="right">
                    数据权限代码：
                </td>
                <td>
                    <input name="Code" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.Code %>" maxlength="25" />
                    <%= Html.ValidationMessage("Code")%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    数据权限名称：
                </td>
                <td>
                    <input name="Name" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.Name %>" maxlength="25" />
                    <%= Html.ValidationMessage("Name")%>
                </td>
            </tr>
           
            <tr>
                <td align="right">
                    优先级：
                </td>
                <td>
                   <input name="Priority" type="text" class="form-item-text easyui-numberspinner" min="1"
                        max="1000" required="true" value="<%=Model.Priority %>" maxlength="250" maxlength="300" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    数据规则：
                </td>
                <td>
                    <textarea name="DataRule" class="form-item-text textarea w150"  maxlength="2000" style="height:200px;width:300px;"><%=Model.DataRule%></textarea>
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