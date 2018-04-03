<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Clover.Permission.Model.Position>"
    Title="岗位管理" %>

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
     <script type="text/javascript">
         function LoadGroupTree() {
             $('#GroupId').combotree({
                 url: '<%=Url.Action("Group","Permission")%>' + "?Type=GetGroupTree" + "&mid=" + "<%=Model.PositionID %>",
                 checkbox: false,
                 onClick: function (node) {
                     $("#GroupId").val(node.id);

                     loadGroupPosition(node.id);

                     $("#ParentID").val("");                
                 }
             });
         }

         function loadGroupPosition(groupid) {
             //动态加载对应的父级岗位信息(暂时不过滤)
             $('#ParentID').combotree({
                 checkbox: false,
                 url: '<%=Url.Action("GetPositionTree","Permission")%>' + "?GroupID=&exceptPosID=<%=Model.PositionID %>&t=" + new Date().toLocaleString(),
                 onClick: function (node) {
                     $("#ParentID").val(node.id);                  
                 }
             });
         }

         $(document).ready(function () {
            LoadGroupTree();

            loadGroupPosition($("#GroupId").val());
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
                    岗位代码：
                </td>
                <td>
                    <input name="PositionCode" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.PositionCode %>" maxlength="25" />
                    <%= Html.ValidationMessage("PositionCode")%>
                </td>
            </tr>
            <tr>
                <td>
                    岗位名称：
                </td>
                <td>
                    <input name="PositionName" type="text" class="form-item-text easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.PositionName %>" maxlength="25" />
                     <%= Html.ValidationMessage("PositionName")%>
                </td>
            </tr>
            <tr>
                <td>
                    岗位等级：
                </td>
                <td>
                    <input name="PositionLevel" type="text" class="easyui-numberbox easyui-validatebox" missingmessage="必填"
                        required="true" value="<%=Model.PositionLevel %>" maxlength="25" />
                     <%= Html.ValidationMessage("PositionLevel")%>
                </td>
            </tr>
            <%--<tr>
                <td align="right">
                    所属组织：
                </td>
                <td>
                    <input name="GroupId" id="GroupId" class="easyui-combotree" required="true" value="<%=Model.GroupId.HasValue ? Model.GroupId.ToString() : "" %>" style="width:200px;">                    
                </td>
            </tr>
             <tr>
                <td align="right">
                    主管岗位：
                </td>
                <td>                 
                    <input name="ParentID" id="ParentID" class="easyui-combotree" value="<%=Model.ParentID.HasValue ? Model.ParentID.ToString() : "" %>" style="width:200px;">                                                                              
                </td>
            </tr>--%>
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
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();"
            id="A1">确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)" onclick="CloseTheWin();"
                runat="server" id="btnCancel">取消</a>
    </div>
</body>
<uc1:ScriptBlock ID="ScriptBlockA" runat="server" />
</html>