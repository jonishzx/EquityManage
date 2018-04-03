<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="我的待办任务" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","WorkFlow")%>';
        var hiddenGroupId = null;

        function RunBackFunc(){
            queryData(); 
        }
        function init() {
            window.queryData = function () {
                var stext = getQueryDataParams().searchtext;
                if (stext != "")
                    LoadWorkItems(stext);
                else
                    LoadWorkItems();
            }

            queryData();

            $("#Dialog").css("display","inline");

            $("#Dialog").dialog({
                title: "提示",
                modal: false,
                shadow: false,
                closed: true,
                width:500,
                height:520
            });
        }
        
        function viewWorkItem(id) {
            //查看任务
                $.ajax({
                    type: "POST",
                    url: baseurl + "/ViewWorkItem",
                    data: { workItemId: id },
                    dataType: "html",
                    beforeSend: ShowWorkItemsLoading,
                    error: HideLoading,
                    success: function (json) {
                        parseMessage(json + "&mode=view", refresh);
                    }
                });
        }
        function claimWorkItem(id) {
            if (confirm("你确定需要签收任务吗?"))
            //领取任务
                $.ajax({
                    type: "POST",
                    url: baseurl + "/ClaimWorkItem",
                    data: { workItemId: id },
                    dataType: "html",
                    beforeSend: ShowWorkItemsLoading,
                    error: HideLoading,
                    success: function (json) {
                        parseMessage(json, refresh);
                        LoadWorkItems();
                    }
                });
        }

        function completeWorkItem(id) {
            //if (confirm("你确定需要完成任务吗?"))
            //完成任务
            $.ajax({
                type: "POST",
                url: baseurl + "/CompleteWorkItem",
                data: { workItemId: id },
                dataType: "html",
                beforeSend: ShowWorkItemsLoading,
                error: HideLoading,
                success: function (json) {
                    parseMessage(json, refresh);
                }
            });
        }

        function loadUserGrid(code) {
            $('#UserGrid').datagrid({
                nowrap: false,
                striped: true,
                width:'auto',
                height:310,
                border: false,
                url: "<%=Url.Action("","Account")%>/GetNoCurrentAccountList",
                queryParams: { CodeOrName: code },
                columns: [[
					{ field: 'AdminId', title: '编号', width: 280 },
					{ field: 'AdminName', title: '用户名称', width: 120 }
				
			]],
                pagination: true,
                pageSize: 10,
                pageList: [10, 15, 20, 30],
                rownumbers: true,
                pageNumber: 1,
                singleSelect: true,
                onBeforeLoad: function () {
                    RemoveForbidButton();
                }
            });
        }

        function queryUserData(){
           loadUserGrid($("#SearchText").val());
        }

        function refresh(){
            $('#DataGrid').datagrid("reload");
        }

       
        function commissionWorkItem(id) {
                                 
                //弹出用户选择框                
                $("#userlist").show();
                $("#comments").val('');
                $("#Dialog").dialog("open");
                loadUserGrid('');

                $("#submit").unbind("click");
                $("#submit").click(function () {
                 
                    var comment = $("#comments").val();
                    var actorid = getGridSelection('#UserGrid', 'AdminId');

                    if(!actorid)
                    {
                        alert('请选择接受委托的用户');
                        return;
                    }
                    if (!confirm("你确定需要将任务委托吗?"))
                        return;

                    $.ajax({
                        type: "POST",
                        url: baseurl + "/CommissionWorkItem",
                        data: { workItemId: id, actorId: actorid, comments: comment },
                        dataType: "html",
                        beforeSend: ShowLoading,
                        error: HideLoading,
                        success: function (json) {
                            parseMessage(json, refresh);
                            CloseDialog();
                        }
                    });
                });
        }

        function rejectWorkItem(id) {
          
                //弹出用户选择框
                $("#userlist").hide();
                $("#comments").val('');
                $("#Dialog").dialog("open");
                $("#submit").unbind("click");
                $("#submit").click(function () {
                    if (!confirm("你确定需要将任务退回吗?"))
                      return;
                    var comment = $("#comments").val();
                    $.ajax({
                        type: "POST",
                        url: baseurl + "/RejectWorkItem",
                        data: { workItemId: id, comments: comment },
                        dataType: "html",
                        beforeSend: ShowLoading,
                        error: HideLoading,
                        success: function (json) {
                            parseMessage(json, refresh);
                            CloseDialog();
                        }
                    });
                });
        }

        function LoadWorkItems(code) {
            if (permission.Browse)
                $('#DataGrid').datagrid({
                    nowrap: false,
                    striped: true,
                    fit: true,
                    border: false,
                    url: baseurl + "/GetWorkItemList",
                    queryParams: getQueryDataParams(),
                    columns: [[					
                    { field: 'ProcessDisplayName', title: '所属流程', width: 120 },
					{ field: 'DisplayName', title: '环节说明', width: 100 },
                    { field: 'ActorName', title: '领取人', width: 60 },
					{ field: 'ProcessInstanceBizInfo', title: '业务信息', width: 350 },
                    { field: 'CreatorName', title: '发起人', width: 60 },					
                    { field: 'State', title: '状态', width: 50,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 0:
                                    return '初始化';
                                case 1:
                                    return '进行中';
                                case 7:
                                    return '已经结束';
                                case 9:
                                    return '已被撤销';
                            }
                        }
                    },
					{ field: 'AAA', title: '任务操作', width: 100, formatter: function (val, rec) {
					    var showWithDraw = false, showAgency=false;
                        var link = "";
					    if (rec.State == 0) {
                            link = newlinkbutton("", "签收任务", 'claimWorkItem("' + rec.Id + '")', "点击查看流程进度");
					    }
					    else if (rec.State == 1) {
                            link = newlinkbutton("", "完成任务", 'completeWorkItem("' + rec.Id + '")', "点击完成任务");
					    }
					    
					    return link + " | " +  newlinkbutton("", "查看", 'viewWorkItem("' + rec.Id + '")', "点击查看任务内容");
					}
					},
                    { field: 'CreatedTime', title: '开始时间', width: 130,
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
//                    toolbar: [
//                    {
//                        text: permission.Edit ? '拒收' : '',
//                        iconCls: permission.Edit ? 'icon-reject' : "null",
//                        handler: function () {
//                            var workitemid = getGridSelection('#DataGrid', 'Id');
//                            rejectWorkItem(workitemid)
//                        }
//                    },'-'
//                    ,{
//                        text: permission.Edit ? '委托' : '',
//                        iconCls: permission.Edit ? 'icon-commission' : "null",
//                        handler: function () {
//                            var workitemid = getGridSelection('#DataGrid', 'Id');
//                            commissionWorkItem(workitemid)

//                      }
//                    }],
                    singleSelect: true,
                    onBeforeLoad: function () {
                        RemoveForbidButton();
                    }
                });
        }


        function AddSuccess() {
            LoadWorkItems();
            setTimeout("MsgShow('系统提示','添加成功。');", 500);
        }

        function EditSuccess(func) {
            LoadWorkItems();
            setTimeout("MsgShow('系统提示','修改成功。');", 500);
        }

        function DeleteSuccess(func) {
            LoadWorkItems();
            setTimeout("MsgShow('系统提示','删除成功。');", 500);
        }

        function openParentTab() {
            window.parent.addTab("已办任务", null, '<%=Url.Action("MyHaveDoneWorkItem","WorkFlow")%>', "main");
        }

        function CloseDialog() {
            $("#Dialog").dialog("close");
        }

        
        $(document).ready(function () {
            //权限获取
            LoadPageModuleFunction("MyWorkItem", init);

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
    <div id="center" region="center" title="我的待办任务管理" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <div id="Dialog" style="display: none;">
        <div class="easyui-layout" fit="true">
            <div region="center" id="userlist">
                <div class="SearchDiv">
                    代码或名称：<input type="text" id="SearchText" style="width: 120px;" />
                    <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="queryUserData();">
                        查询</a>
                </div>
                <table id="UserGrid">
                </table>
            </div>
            <div region="south" border="false" style="height: 130px">
                <div class="ym-form linearize-form ym-columnar">
                    <div class="ym-form-fields">
                        <div class="ym-fbox-text">
                            <label for="comments">
                                附加信息</label>
                            <textarea id="comments" name="comments" rows="3"></textarea>
                        </div>
                    </div>
                </div>
                <div class="SouthForm form-action">
                    <a class="easyui-linkbutton" icon="icon-ok" href="#" id="submit">确定</a> <a class="easyui-linkbutton"
                        icon="icon-cancel" href="javascript:void(0)" onclick="CloseDialog();" id="btnCancel">
                        取消</a>
                </div>
            </div>
        </div>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
