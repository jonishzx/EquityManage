<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    操作日志查看
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SearchDiv" region="north">
        <label class="ybtext">
            操作查询：</label>
        <input id="SearchText" name="SearchText" type="text" class="form-item-text" maxlength="50" />
        <label class="ybtext">
            日期范围：</label>
        <input id="StartDate" name="StartDate" type="text" class="form-item-text Wdate" onClick="WdatePicker()"
            value="<%=TempData["StartDate"]%>" style="width: 90px" maxlength="50" />-
        <input id="EndDate" name="EndDate" type="text" class="form-item-text Wdate" onClick="WdatePicker()"
            value="<%=TempData["EndDate"]%>" style="width: 90px" maxlength="50" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="queryData();">
            查询</a>
    </div>
    <div id="center" region="center" title="操作日志查看" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <div id="logdetailbox" style="width: 500px; height: 400px;">
        <div id="logcontent">
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        var permission = <%=UkeyTech.OA.WebApp.Helper.GetPermissionJson("OPLog","Delete")%>
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>
    <script type="text/javascript">
        window.queryData = function () {
            $('#DataGrid').datagrid("load", getQueryDataParams());
        }

        function init() {
            $('#DataGrid').datagrid({
                iconCls: 'icon-save',
                nowrap: true,
                striped: true,
                fit: true,
                url: '<%=Url.Action("GetLogList","System")%>',
                loadMsg: '数据加载中,请稍候……',
                pageSize: 10,
                columns: [[
                    { field: 'ck', checkbox: true },
					{ field: 'LoginName', title: '登录ID', width:100 },
					{ field: 'UserName', title: '姓名', width: GetWidth(0.1) },
					{ field: 'LogOPName', title: '操作', width: GetWidth(0.2) },
//					{ field: 'LogMessage', title: '操作', width: GetWidth(0.1) },
                    { field: 'UserIP', title: '登录IP', width: GetWidth(0.1) },
					{ field: 'UpdateTime', title: '记录时间', width: GetWidth(0.15),
					    formatter: function (value, rec) {
					        return DateHandler(value);
					    }
					},
					{ field: 'id', title: '操作', width: GetWidth(0.07), formatter: function (val, rec) {
					    var o = newlinkbutton("", "查看", 'viewlog("' + escape(val) + '")', "查看")

					    if (permission.Delete)
					        o = o + " | " + newlinkbutton("", "删除", 'del("' + escape(val) + '")', "删除");
					    return o;
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

        $(function () {
            //权限获取
            LoadPageModuleFunction("OPLog", init);
        });

        function getQueryDataParams() {
            return { searchtext: $("#SearchText").val(), StartDate: $("#StartDate").val(), EndDate: $("#EndDate").val() };
        }
        function closedialog() {

            $("#logdetailbox").dialog("close");
        }
        function viewlog(paras) {
            $('#logcontent').html("Loading...");

            $("#logdetailbox").dialog({ modal: true,
                title: '日志详细信息查看',
                buttons: [{
                    text: '关闭',
                    iconCls: 'icon-ok',

                    handler: function () {
                        $('#logcontent').html("");
                        $("#logdetailbox").dialog("close");

                    }
                }]
            });

            $('#logcontent').load('<%=Url.Action("LogDetail","System") %>' + '/' + paras);
        }

        function del(paras) {

            if (confirm('你确定删除该日志?'))
                $.ajax({
                    url: '<%=Url.Action("DelLog","System") %>',
                    data: { id: paras },
                    type: "POST",
                    success: function (msg) {
                        var arry = msg.split(':');
                        if (arry[0] == '1') {
                            showMessage("提示", "删除成功", "");
                            queryData("re"); //刷新当前页
                        } else {
                            alert(arry[1]);
                        }
                    }
                });
        }

    </script>
</asp:Content>
