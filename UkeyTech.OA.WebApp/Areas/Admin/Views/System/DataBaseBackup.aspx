<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.DBBackup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    数据库备份列表
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        var permission = <%=UkeyTech.OA.WebApp.Helper.GetPermissionJson("DataBaseBackup","Browse","Edit")%>
    </script>
    <div class="SearchDiv" region="north" style="overflow:hidden;">
        <%using (Html.BeginForm())
          { %>
        备份文件名称：
        <input id="SearchText" name="FileName" type="text" class="form-item-text" maxlength="12" />
        <%= Html.ValidationMessage("FileName")%>
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-save" onclick="SubmitForm();">
            备份</a>
        <%} %>
    </div>
    <div id="center" region="center" title="数据库备份列表" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        $(function () {
            //权限获取
            LoadPageModuleFunction("DataBaseBackup", init);
        });

        function init() {
            if (!permission.Browse)
                $(".SearchDiv").html("");

            window.queryData = function () {
                $('#DataGrid').datagrid("load", null);
            }

            $('#DataGrid').datagrid({
                iconCls: 'icon-save',
                nowrap: false,
                striped: true,
                fit: true,
                url: '<%=Url.Action("GetBackupList","System")%>',
                pageSize: 15,
                pageList: [10, 15, 20, 30],
                columns: [[
                    { field: 'ck', checkbox: true },
					{ field: 'UpdateTime', title: '备份时间', width: 200,
					    formatter: function (value, rec) {
					        return DateHandler(value);
					    }
					},
					{ field: 'FileName', title: '文件名', width: 200 },
					{ field: 'DirName', title: '存放路径', width: 60 },
					{ field: 'FullFileName', title: '操作', width: 80, formatter: function (val, rec) {
					    if (permission.Edit) {
					        return newlinkbutton("", "还原", 'restoreDB("' + escape(val) + '")', "点击还原数据库") + "|" +
                             newlinkbutton("", "删除", 'delDB("' + escape(val) + '")', "点击删除数据库");

					    }
					}
					}
				]],
                onClickRow: function (rowIndex, rowData) {
                    $('#DataGrid').datagrid('unselectAll');
                    $('#DataGrid').datagrid('selectRow', rowIndex);
                },

                pagination: true,
                rownumbers: true,
                singleSelect: false

            });
        }

        function getQueryDataParams() {
            return { searchtext: $("#SearchText").val() };
        }

        function restoreDB(paras) {
            if (confirm('你确定还原该备份文件?'))
                $.ajax({
                    url: '<%=Url.Action("ResotreDBBackup","System") %>',
                    data: { file: paras },
                    type: "POST",
                    success: function (msg) {
                        var arry = msg.split(':');
                        if (arry[0] == '1') {
                            showMessage("提示", "还原成功", "");
                        } else {
                            alert(arry[1]);
                        }
                    }
                });
        }

        function delDB(paras) {

            if (confirm('你确定删除该备份?'))
                $.ajax({
                    url: '<%=Url.Action("DeleteDBBackup","System") %>',
                    data: { file: paras },
                    type: "POST",
                    success: function (msg) {
                        var arry = msg.split(':');
                        if (arry[0] == '1') {
                            showMessage("提示", "删除成功", "");
                            queryData(); //刷新当前页
                        } else {
                            alert(arry[1]);
                        }
                    }
                });
        }

    </script>
</asp:Content>
