<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="切换兼职" Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.Admin>" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        var currGroupId = '<%= Model.CurrGroupId %>';
        var currPosId = '<%= Model.CurrPositionId %>';

        function Redirect() {
            //转到自定义页面asdasd
             window.location.href = '<%=!string.IsNullOrEmpty(Request["NavUrl"]) ? Request["NavUrl"].Replace("__","&") : "#" %>' + "&checked=1";
        }

        $(function () {
            $('#DataGrid').datagrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: false,
                url: '<%=Url.Action("UserGroupPositionList","Account")%>',
                columns: [[
					{ field: 'GroupName', title: '部门', width: 150 },
					{ field: 'PositionName', title: '岗位', width: 120 },
                    { field: 'RoleName', title: '角色', width: 70 },
                    { field: 'Status', title: '当前岗位', width: 70, align: "center",
                        formatter: function (value, rec) {
                            var isSelected = currGroupId == (rec.GroupId ? rec.GroupId:"") && currPosId == (rec.PositionId ? rec.PositionId : "");

                            return '<a groupid="'
                            + (rec.GroupId ? rec.GroupId : "")
                            + '" posid="' + (rec.PositionId ? rec.PositionId : "")
                           + '" groupname="' + (rec.GroupName ? rec.GroupName :"")
                           + '" posname="' + (rec.PositionName ? rec.PositionName : "")
                           + '" roleid="' + (rec.RoleId ? rec.RoleId : "")
                           + '" rolename="' + (rec.RoleName ? rec.RoleName : "")
                           + '" class="setDefaultGroupPos icon ' + (isSelected ? "icon-light" : "icon-unlight") + '" href="#">' + "&nbsp;" + '</a>';
                        }
                    }
			]],
                pagination: true,
                pageSize: 15,
                pageList: [10, 15, 20, 30],
                rownumbers: false,
                pageNumber: 1,
                singleSelect: true,
                onLoadSuccess: function () {
                    $(".setDefaultGroupPos").click(function () {
                        post('<%=Url.Action("SetDefaultUserGroupPosition","Account")%>',
                        {
                            groupId: $(this).attr("groupid"),
                            positionId: $(this).attr("posid"),
                            groupName: $(this).attr("groupname"),
                            positionName: $(this).attr("posname"),
                            roleId: $(this).attr("roleid"),
                            roleName: $(this).attr("rolename"),
                            changedefault : <%= Request["NavUrl"] == null ? "true" : "false" %>
                        },
                         function (text, data) {
                             
                             currGroupId = data.groupId;
                             currPosId = data.positionId;
                           
                             window.parent.currGroup = data.groupName;
                             window.parent.currPos = data.positionName;
                             window.parent.loadCurrPost();
                             $('#DataGrid').datagrid("reload");

                             <% if (Request["NavUrl"] != null && !string.IsNullOrEmpty(Request["NavUrl"].ToString()))
                                { %>
                             Redirect();
                             return;
                             <% }
                                else
                                { %>
                                 window.parent.location.reload();
                             <% } %>
                            
                         }
                    );
                    });
                }
            });
        });   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <% if (Request["NavMessage"] != null)
       { %>
    <div id="Div1" region="north" style="height:30px;">
         <%= Request["NavMessage"]%>
    </div>
    <% } %>
    <div id="center" region="center" title="兼职列表" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <% if (Request["NavUrl"] != null)
       { %>
        <div region="south" border="false" class="SouthForm form-action">
          <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="Redirect();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" id="btnCancel">取消</a>
    </div>
    <% } %>
</asp:Content>
