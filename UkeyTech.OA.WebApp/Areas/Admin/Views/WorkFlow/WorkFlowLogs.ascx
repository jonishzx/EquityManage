<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (ViewData.Eval("ProcessInstanceId") != null && ViewData.Eval("ProcessInstanceId") != null)
   {%>
<table id="tbWorkflowLogGrid<%=ViewData.Eval("ID") %>" title="日程日志" >
</table>

<script type="text/javascript">
    function getStausName(state) {
        switch (state) {
            case 0:
                return '<span class="initialized">' + '初始化中<span>' + "</span>";
            case 1:
                return '<span class="running">' + '运行中' + "</span>";
            case 7:
                return '<span class="completed">' + '完成' + "</span>";
            case 9:
                return '<span class="canceled">' + '被撤销' + "</span>";

        }
    }
    function initWorkflowLogGrid<%=ViewData.Eval("ID") %>() {
        $("#tbWorkflowLogGrid<%=ViewData.Eval("ID") %>").css("width", $(document).width() - 35);
        $('#tbWorkflowLogGrid<%=ViewData.Eval("ID") %>').datagrid({
            nowrap: false,
            striped: true,
            fit: false,
            border: false,
            url: '<%=Url.Action("","WorkFlow", new{Area = "Admin"})%>/<%=ViewData.Eval("GetWorkItemListAction")%>',
            queryParams: { ProcessInstanceId: '<%=ViewData.Eval("ProcessInstanceId") %>', allActorWI:true},
            idField: 'ID',
            columns: [[
                    { field: 'ProcessDisplayName', title: '流程', width: 120 },
					{ field: 'DisplayName', title: '环节', width: 120 },
                    { field: 'ActorName', title: '处理人', width: 70,  formatter: function (value, rec) {
                            if(rec.CompleteActorId && rec.CompleteActorId != rec.ActorId)
                                return rec.CompleteActorName + "(代)";
                            else
                                return value;
                        }  },
                    { field: 'AssignActors', title: '被委托人', width: 80},
					{ field: 'Comments', title: '处理情况', width: GetWidth(0.29) },
                    { field: 'ClaimedTime', title: '签收时间', width: 130,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        } 
                    },
                    { field: 'EndTime', title: '完成时间', width: 130, 
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        } 
                    },
                    { field: 'State', title: '状态', width: 80,
                        formatter: function (value, rec) {
                            return getStausName(value);
                        } 
                    }
                    ]],
            onBeforeLoad: function (row, param) {
            },
            pagination: false,
            pageSize: 15,
            pageList: [10, 15, 20, 30],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,

            onBeforeLoad: function () {
                RemoveForbidButton();
            }
        });
        }
     
   
</script>
<%} %>