<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Clover.Permission.Model.Group>"
    Title="组织架构管理" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Register src="../Shared/ScriptBlock.ascx" tagname="ScriptBlock" tagprefix="uc1" %>
<%@ Register src="../Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>
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
                <td>
                    组织架构代码：
                </td>
                <td>
                    <input name="GroupCode" type="text" class="form-item-text easyui-validatebox" style="width: 250px;" missingmessage="必填"
                        required="true" value="<%=Model.GroupCode %>" maxlength="50" />
                    <%= Html.ValidationMessage("GroupCode")%>
                </td>
            </tr>
            <tr>
                <td>
                    组织架构名称：
                </td>
                <td>
                    <input name="GroupName" type="text" class="form-item-text easyui-validatebox" style="width: 250px;" missingmessage="必填"
                        required="true" value="<%=Model.GroupName %>" maxlength="50" />
                     <%= Html.ValidationMessage("GroupName")%>
                </td>
            </tr>
               <tr>
                <td>
                    全名：
                </td>
                <td>
                    <input name="FullName" type="text" class="form-item-text easyui-validatebox" style="width: 250px;" 
                        required="true" value="<%=Model.FullName %>" maxlength="100" />
                     <%= Html.ValidationMessage("FullName")%>
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
                        foreach (var m in (List<Clover.Permission.Model.Group>)ViewData["Parentlist"])
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
                    部门负责人：
                </td>
                
                <td>
                   <div>
                     <uc2:PopupWin ID="PopupWin1" runat="server" />
                      <%Html.RenderPartial(Helper.PopupControlPath, 
                          new ViewDataDictionary(new {
                              IDControlName = "Modifitor", 
                              Required = false,
                              TextControlName = "EmpName", 
                              DictID = "STCManager",
                              TextValue = Model.EmpName,
                              Value = Model.Modifitor, 
                              CallBack = "nothinghappen",
                              width = "120"
                          }));%>
                      </div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    状态：
                </td>
                <td>
                   <div>
                    <%Html.RenderPartial(Helper.DictDropDownListPath,
                new ViewDataDictionary(new
                {
                    ID = "Status",
                    DictID = "YesOrNo5",
                    width = "160",
                    AddEmptyItem = true,
                    Default = "1",
                    Value = Model.Status
                }));%>
                      </div>
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