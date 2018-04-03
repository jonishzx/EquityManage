<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="我的已办任务" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
     <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/WorkItems.js")%>"></script>
     <script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>

     <script type="text/javascript">
         var baseurl = '<%=Url.Action("","WorkFlow")%>';
         var hiddenGroupId = null;

         function init() {
             window.queryData = function () {
                 var stext = getQueryDataParams().searchtext;
                 if (stext != "")
                     LoadHaveDoneWorkItems(stext);
                 else
                     LoadHaveDoneWorkItems();
             }

             queryData();
         }

    
         function parseResult(json) {
             if (json == '') {
                 HideLoading();
                 LoadHaveDoneWorkItems();
                 MsgShow('系统提示', '操作成功。');
             }
             if (json.indexOf('uri:') >= 0) {
                 var url = json.replace("uri:", "");
                 window.location.href = url;
             }
             else if (json != '') {
                 HideLoading();
                 MsgShow('系统提示', json);
             }
         }

         function withdrawWorkItem(id) {
             if (confirm("你确定需要取回任务吗?"))
             //完成任务
                 $.ajax({
                     type: "POST",
                     url: baseurl + "/WithDrawWorkItem",
                     data: { workItemId: id },
                     dataType: "html",
                     beforeSend: ShowWorkItemsLoading,
                     error: HideLoading,
                     success: function (json) {
                         parseResult(json);
                     }
                 });
         }
         
         function LoadHaveDoneWorkItems(code) {
             if (permission.Browse)
                 $('#DataGrid').datagrid({
                     nowrap: false,
                     striped: true,
                     fit: true,
                     border: false,
                     url: baseurl + "/GetHaveDoneWorkItemList",
                     queryParams: getQueryDataParams(),
                     columns: [[
                    { field: 'ProcessDisplayName', title: '所属流程', width: 80 },
					{ field: 'DisplayName', title: '环节说明', width: 120 },
                    { field: 'ProcessInstanceBizInfo', title: '业务信息', width: 150 },
                    { field: 'Comments', title: '办理信息', width: 120 },
                    { field: 'ActorName', title: '领取人', width: 80 },
                    { field: 'CreatorName', title: '发起人', width: 80 },
                    { field: 'CompleteActorName', title: '完成人', width: 80 },
					{ field: 'CreatedTime', title: '开始时间', width: 120,
					    formatter: function (value, rec) {
					        return DateHandler(value);
					    }
					},
                    { field: 'ClaimedTime', title: '签收时间', width: 120,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                    { field: 'EndTime', title: '完成时间', width: 120,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    }
					
			]],
                     pagination: true,
                     pageSize: 15,
                     pageList: [10, 15, 20, 30],
                     rownumbers: true,
                     pageNumber: 1,
                    // toolbar: [
                    //{
                    //    text: permission.Edit ? '取回' : '',
                    //    iconCls: permission.Edit ? 'icon-withdraw' : "null",
                    //    handler: function () {
                    //        var workitemid = getGridSelection('#DataGrid', 'Id');
                    //        withdrawWorkItem(workitemid)
                    //    }
                    //}],
                     singleSelect: true,
                     onBeforeLoad: function () {
                         RemoveForbidButton();
                     }
                 });
         }

          $(document).ready(function () {
             //权限获取
             LoadPageModuleFunction("MyHaveDoneWorkItem", init);
                currName = $("#iNames");
            currId = $("#iIds");
         });

        function getQueryDataParams() {
            return { actorid:$("#iIds").val(),
                     startdate:$("#StartDate").val(), 
                     enddate:$("#EndDate").val(), 
                     processid:$("#PROCESS_ID").val()};
        }

        var returnValue;
        var popupbaseurl = '<%=Url.Action("PreViewSelectType","CustomForm")%>';
        var currName,currId;
        function popupActor() {
           
            SetWin(510, 410, popupbaseurl + '?SelectTypeId=' + '14', '数据选择');
        }
        function clearPopupId(){
            $(currId).val("");
            $(currName).val("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="SearchDiv" region="north">
        <label class="ybtext">
            发起人：</label>
        <%Html.RenderPartial(Helper.PopupControlPath,
                                new ViewDataDictionary(new
                                {
                                    IDControlName = "iIds",
                                    TextControlName = "iNamesText",
                                    DictID = "ValidUser",
                                    AddEmptyItem = true
                                }));%>  
        <label class="ybtext">
            日期范围：</label>
        <input id="StartDate" name="StartDate" type="text" class="form-item-text Wdate" onclick="WdatePicker()"
            value="<%=TempData["StartDate"]%>" style="width: 90px" maxlength="50" />-
        <input id="EndDate" name="EndDate" type="text" class="form-item-text Wdate" onclick="WdatePicker()"
            value="<%=TempData["EndDate"]%>" style="width: 90px" maxlength="50" />
        <label class="ybtext">
            所属流程：</label>
        <select id="PROCESS_ID" name="PROCESS_ID" class="form-item-text easyui-validatebox">
            <option value="">全部</option>
            <%
                foreach (var m in (List<FireWorkflow.Net.Engine.Definition.WorkflowDefinition>)ViewData["WorkFlowList"])
                {
            %>
            <option value='<%=m.ProcessId%>'>
                <%=m.DisplayName%></option>
            <%}%>
        </select>
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="queryData();">
            查询</a>
    </div>
    <div id="center" region="center" title="我的已办任务" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
