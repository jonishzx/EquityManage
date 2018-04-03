<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Title="WorkWeekCalendar" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
     <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div class="SearchDiv queryarea" region="north">
            <div class="ym-fbox-text SearchField">
                  <label for="YearNum">年</label>
                  <input id="YearNum" name="YearNum" type="text" class="form-item-text" value="" maxlength="70" />
            </div>
            <div class="ym-fbox-text SearchField">
                  <label for="WeekNum">周</label>
                  <input id="WeekNum" name="WeekNum" type="text" class="form-item-text" value="" maxlength="70" /> 
            </div>
            <div style="float:left">
              <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadWorkWeekCalendarGrid();">
                查询</a>
            </div>
            </div>
     
        <div id="center" region="center" title="" style="width: auto;">
            <table id="tbGrid">
            </table>
        </div>       
   
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","Calendar")%>';     
        var listurl = baseurl + "/GetWorkWeekCalendarList";      
    </script>
    <script type="text/javascript">
        var hiddenId = null;

        function init() {            
            LoadWorkWeekCalendarGrid();
        }
       
        $(document).ready(function () {
            //权限获取
            LoadPageModuleFunction("Calendar",
                function () {
                    loadDictItems("", init); 
            });
        });            
        
        function LoadWorkWeekCalendarGrid(code, where) {
            $('#tbGrid').datagrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: false,
                url: listurl,
                idField: 'Id',
                queryParams: getQueryParams(),
                columns: [[
                     { field: 'YearNum', title: '年', width: 80, align: 'center' },
                     { field: 'WeekNum', title: '周', width: 80, align: 'center' },
                     { field: 'StartDate', title: '本周开始时间', width: 80, align: 'center', formatter: function (value, rec) {
                         return ShortDateHandler(value);
                     }
                     },
                     { field: 'EndDate', title: '本周结束时间', width: 80, align: 'center', formatter: function (value, rec) {
                         return ShortDateHandler(value);
                     }
                     },
                     { field: 'IsWorkWeek', title: '是否工作周', width: 80, align: 'center',
                         formatter: function (value, rec) {
                             return value == 0 ? '否' : '是';
                         }
                     },
                     { field: 'WeekRange', title: '日期范围', width: 200, align: 'center' }
                    ]],

                onBeforeLoad: function (row, param) {
                },
                pagination: true,
                pageSize: 15,
                pageList: [10, 15, 20, 30],
                rownumbers: true,
                pageNumber: 1,
                singleSelect: true,
                toolbar: [{
                    text: permission.Create ? '添加' : '',
                    iconCls: permission.Create ? 'icon-add' : "null",
                    handler: function () {
                        SetBackFunc(SaveWorkWeekCalendarSuccess);
                        SetWin(640, 430, 'CreateWorkWeekCalendar', '添加资料');
                    }
                },
                    '-',
                    {
                        text: permission.Edit ? '修改' : '',
                        iconCls: permission.Edit ? 'icon-edit' : "null",
                        handler: function () {
                            var id;
                            id = getGridSelection('#tbGrid', 'WeekCalendarId');
                            if (id) {
                                SetBackFunc(SaveWorkWeekCalendarSuccess);
                                SetWin(640, 430, 'EditWorkWeekCalendar/?WeekCalendarId=' + id, '修改资料');
                            }
                        }
                    },
                    '-',
                    {
                        text: permission.Delete ? '删除' : '',
                        iconCls: permission.Delete ? 'icon-cut' : "null",
                        handler: function () {
                            DeleteWorkWeekCalendar();
                        }
                    }],
                onClickRow: function (idx, data) {

                },
                onBeforeLoad: function () {
                    RemoveForbidButton();
                }
            });
        }
        
        function SaveWorkWeekCalendarSuccess() {
            setTimeout("LoadWorkWeekCalendarGrid();", 500);
            setTimeout("MsgShow('系统提示','保存成功。');", 1000);
        }
        
        function DeleteWorkWeekCalendar() {
            
            var deleteId;
            deleteId = getGridSelection('#tbGrid', 'WeekCalendarId');
            if (deleteId) {
        
                $.messager.confirm('Question', '确定要删除?', function (r) {
                    if (r) {
                        $.ajax({
                            type: "POST",
                            url: 'DeleteWorkWeekCalendar',
                            data: { delids: deleteId },
                            dataType: "text",
                            success: function (json) {
                                setTimeout("LoadWorkWeekCalendarGrid();", 500);
                                setTimeout("MsgShow('系统提示','删除成功。');", 1000);
                            }
                        });
                    }
                });
            }
        }

    </script>
</asp:Content>