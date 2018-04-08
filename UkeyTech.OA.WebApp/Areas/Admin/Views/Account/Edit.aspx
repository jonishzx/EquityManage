<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="UkeyTech.OA.WebApp.Extenstion.EnchanceViewPage<UkeyTech.WebFW.Model.Admin>" %>

<%@ Register Src="~/Areas/Admin/Views/Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Register Src="GroupPositionEdit.ascx" TagName="GroupPositionEdit" TagPrefix="uc1" %>
<%@ Import Namespace="UkeyTech.OA.WebApp.Extenstion" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    管理员信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
<%if (IsEdit)
  {%>
        var GroupPositionUrl = '<%=Url.Action("UserGroupPositionListWithAdminId","Account")%>';
        var GroupPositionPostUrl = '<%=Url.Action("SetDefaultUserGroupPositionWithAdminId","Account")%>';
        var hidAdminId = '<%=Model.AdminId%>';

        function loadGroupPosition() {
            $("#GroupPosition").datagrid({
                nowrap: false,
                striped: true,
                width: 450,
                height: 200,
                border: true,
                url: GroupPositionUrl,
                queryParams: { adminid: hidAdminId },
                columns: [[
                        { field: 'GroupName', title: '部门', width: 80 },
                        { field: 'PositionName', title: '岗位', width: 140 },
						{ field: 'RoleName', title: '角色', width: 100 },
                        {
                            field: 'Status', title: '默认', width: 60, align: "center",
                            formatter: function (value, rec) {
                                var isSelected = rec.CurrGroupId == (rec.GroupId ? rec.GroupId : "") && rec.CurrPositionId == (rec.PositionId ? rec.PositionId : "");
                                return '<a groupid="'
                                + (rec.GroupId ? rec.GroupId : "")
                                + '" posid="' + (rec.PositionId ? rec.PositionId : "")
                               + '" groupname="' + (rec.GroupName ? rec.GroupName : "")
                               + '" posname="' + (rec.PositionName ? rec.PositionName : "")
							   + '" roleid="' + (rec.RoleId ? rec.RoleId : "")
                               + '" rolename="' + (rec.RoleName ? rec.RoleName : "")
                               + '" class="setDefaultGroupPos icon ' + (isSelected ? "icon-light" : "icon-unlight") + '" href="#">' + "&nbsp;" + '</a>';
                            }
                        }
                ]],
                pageSize: 100,
                pageList: [100, 200, 300],
                rownumbers: true,
                pageNumber: 1,
                singleSelect: true,
                toolbar: [
                    {
                        text: '添加',
                        iconCls: 'icon-add',
                        handler: function () {
                            $("#CAdminName").val('<%=Model.AdminName %>');
							window.isDefaultGroupRole = false;
							showAdminGroupPosition();
                }
                },
 {
                text: '修改',
                iconCls: 'icon-edit',
                handler: function () {
                        $("#CAdminName").val('<%=Model.AdminName %>');
                        var posId = getGridSelection('#GroupPosition', 'PositionId');
                         var groupId = getGridSelection('#GroupPosition', 'GroupId');
                         var roleId = getGridSelection('#GroupPosition', 'RoleId');
                         var posName = getGridSelection('#GroupPosition', 'PositionName');
                         var groupName = getGridSelection('#GroupPosition', 'GroupName');
                         var roleName = getGridSelection('#GroupPosition', 'RoleName');
                         var currGroupId =  getGridSelection('#GroupPosition', 'CurrGroupId');
                         var currPositionId =  getGridSelection('#GroupPosition', 'CurrPositionId');
                         window.isDefaultGroupRole = currGroupId == (groupId ? groupId : "") &&  currPositionId == (posId ?posId : "");

                         $("#AdminPosIds").val(posId);
                         $("#pAdminGroupIds").val(groupId);
                         $("#pAdminRoleIds").val(roleId);
                         $("#pAdminGroupName").val(groupName);
                         $("#AdminPosName").val(posName);
                         $("#pAdminRoleName").val(roleName);
                    if (hidAdminId) {
                         showAdminGroupPosition(true);
                    }
                }},
            {
                text: '删除',
                iconCls: 'icon-cut',
                handler: function () {
                    if (hidAdminId && confirm('你确定删除选中的记录?')) {
                        var posId = getGridSelection('#GroupPosition', 'PositionId');
                        var groupId = getGridSelection('#GroupPosition', 'GroupId');
						var roleId = getGridSelection('#GroupPosition', 'RoleId');

                        removeAminGroupPosition(groupId, posId, roleId);
                    }
                }
            }],
            onLoadSuccess: function () {
                $(".setDefaultGroupPos").click(function () {
                    post(GroupPositionPostUrl,
                        {
                            adminId: hidAdminId,
                            groupId: $(this).attr("groupid"),
                            positionId: $(this).attr("posid"),
                            groupName: $(this).attr("groupname"),
                            positionName: $(this).attr("posname"),
							roleId: $(this).attr("roleid"),
                            changedefault: true
                        },
                         function (text, data) {

                             $('#GroupPosition').datagrid("reload");
                         }
                    );
                });
            }
        });
            };
            $(function () {
                //loadGroupPosition();
            });
        <%} %>
    </script>
    <style type="text/css">
        .w110 {
            width: 110px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center" class="CenterForm">
        <%using (Html.BeginForm())
          { %>
        <div class="ym-form linearize-form ym-columnar zcolumn">
            <div class="ym-form-fields">
                <div class="form-message">
                    <%=ViewData["StateText"]%>
                </div>
                <div class="ym-fbox-text">
                    <label class=" required ">
                        <sup class="ym-required">*</sup> 用户名：</label>
                    <div class="form-element">
                        <input id="AdminName" name="AdminName" type="text" class="form-item-text easyui-validatebox" value="<%=Model.AdminName %>"
                            maxlength="25" required="true" />
                        <%= Html.ValidationMessage("AdminName")%>
                    </div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        <sup class="ym-required">*</sup> 登录名：</label>
                    <div class="form-element">
                        <input name="LoginName" <%= IsEdit ? "readonly=\"readonly\"" : "" %> type="text" class="form-item-text  easyui-validatebox <%= IsEdit ? "ym-readonly" : "" %>"
                            value="<%=Model.LoginName %>" maxlength="25" required="true" />
                        <%= Html.ValidationMessage("LoginName")%>
                    </div>
                    <div class="form-clear-left">
                    </div>
                </div>
           <%--     <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        邮件地址：</label>
                    <div class="form-element">
                        <input name="Email" type="text" class="form-item-text easyui-validatebox" validtype="email"
                            value="<%=Model.Email %>" maxlength="50" />
                        <%= Html.ValidationMessage("Email")%>
                    </div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        邮件密码：</label>
                    <div class="form-element">
                        <input name="EmailPwd" type="password" class="form-item-text easyui-validatebox"
                            value="<%=Model.EmailPwd %>" maxlength="64" />
                        <%= Html.ValidationMessage("EmailPwd")%>
                    </div>
                    <div class="form-clear-left">
                    </div>
                </div>--%>
                <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        联系电话：</label>
                    <div class="form-element">
                        <input name="MobilePhone" type="text" class="form-item-text easyui-validatebox"
                            value="<%=Model.MobilePhone %>" maxlength="25" />
                        <%= Html.ValidationMessage("MobilePhone")%>
                    </div>
                    <div class="form-clear-left">
                    </div>
                </div>

                <div class="ym-fbox-text" style="<%= UkeyTech.OA.WebApp.Helper.ShowUIElement(IsEdit)%>">
                    <label class=" w110 required ">
                        <sup class="ym-required">*</sup>密码(最长16位)：</label>
                    <div class="form-element">
                        <input name="Password" type="password" class="form-item-text" maxlength="16" />
                        <%= Html.ValidationMessage("Password")%>
                    </div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="ym-fbox-text" style="<%= UkeyTech.OA.WebApp.Helper.ShowUIElement(IsEdit)%>">
                    <label class=" w110 required ">
                        <sup class="ym-required">*</sup>新密码确认：</label>
                    <div class="form-element">
                        <input name="ConfirmPwd" type="password" class="form-item-text" maxlength="16" />
                        <%= Html.ValidationMessage("ConfirmPwd")%>
                    </div>
                    <div class="form-clear-left">
                    </div>
                </div>
   <%--             <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        用户部门：</label>
                    <div class="form-element">
                        <%
                  Html.RenderPartial(Helper.PopupControlPath,
                              new ViewDataDictionary(new
                              {
                                  IDControlName = "AdminGroupIds",
                                  TextControlName = "AdminGroupName",
                                  DictID = "AllEnabledGroup",
                                  Value = ViewData["AdminGroupIds"],
                                  Width = "300"
                              }));%>
                        <div class="form-clear-left">
                        </div>
                    </div>
                </div>--%>
                <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        用户角色：</label>
                    <div class="form-element">
                        <%Html.RenderPartial(Helper.PopupControlPath,
                                new ViewDataDictionary(new
                                {
                                    IDControlName = "AdminRoleIds",
                                    TextControlName = "AdminRoleName",
                                    DictID = "AllEnabledRole",
                                    Value = ViewData["AdminRoleIds"],
                                    MutilSelect = true,
                                    Width = "300"
                                }));%>
                        <div class="form-clear-left">
                        </div>
                    </div>
                </div>               
                <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        状 态：</label>
                    <div class="radioButtonList">
                        <%
                  var radioButtonList = new SelectList(new List<ListItem> {
                                            new ListItem { Text = "启用", Value="1",},
                                            new ListItem { Text = "停用", Value="0",}}, "Value", "Text", Model.Status);
                  var htmlAttributes = new Dictionary<string, object> {
                                            { "class", "radioButtonList" },                           
                                            { "style", "width:30px !important" }
                                        };
                  foreach (var radiobutton in radioButtonList)
                  { %>
                        <label>
                            <%=radiobutton.Text%></label>
                        <%=Html.RadioButton("Status", radiobutton.Value, radiobutton.Selected, htmlAttributes)%>

                        <% } %>
                        <div class="form-clear-left">
                        </div>
                    </div>
                </div>
                <div class="ym-fbox-text">
                    <label class=" w110 required ">
                        备 注：</label>
                    <div class="form-element">
                        <textarea name="Descn" class="form-item-text textarea w300"><%=Model.Descn%></textarea>
                        <div class="form-clear-left">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%} %>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
            onclick="CloseTheWin();" id="btnCancel">取消</a>
    </div>
    <uc1:GroupPositionEdit ID="GroupPositionEdit1" runat="server" />

</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
