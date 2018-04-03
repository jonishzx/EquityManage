<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<List<Node>>" %>

<%@ Import Namespace="FireWorkflow.Net.Model.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    下一步业务活动选择
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        var activityId   = null;
        $(function () {
            $("#dvSubmitToActivity > ul >li").click(function () {
                activityId = $(this).attr("activityId");
               if(activityId.indexOf("EndNode")>=0) 
               {
                  //终结节点无需选择操作员
                  $("#searchbar").hide();
               }
               else{
                   $("#searchbar").show();
               } 

                $(this).siblings().removeClass('selected');
                $(this).addClass("selected");
                <%if (Request.QueryString["allowselectuser"] != null && Request.QueryString["allowselectuser"].ToLower() == "true")
                {%>
                loadUserGrid();
                <%} %>
            }).hover(function () {
                $(this).addClass("hover");
            }, function () {
                $(this).removeClass("hover");
            });
          
            //load first activity
            if($("#dvSubmitToActivity > ul >li").length > 0){
                var first = $("#dvSubmitToActivity > ul >li:eq(0)");
                activityId = $(first).attr("activityId")
                $(first).addClass('selected');
                loadUserGrid();
            }
            //check all box change
            $("#cbAll").change(loadUserGrid);

            loadUserGrid();
        });
        function submit(){
           /*
           if(activityId) 
           {
               if(activityId.indexOf("EndNode")<0){
                   //终结节点无需选择最后一个节点
                   var selActors = getGridSelections("usergrid","AdminId");
                   if(!activityId || !selActors){
                       alert("请选择下一个步骤及受理人");
                       return;
                   }
               }
           }
           else{
                alert("请选择下一个步骤");
           }
           */
           var selActors = getGridSelections("usergrid","AdminId");

           if( !selActors){
                alert("请选择下一个步骤及受理人");
                return;
            }
           if (window.parent.submitCallback != undefined) {
                var obj = { NextActivityId: activityId,  NextActivityActors: selActors };
                if(window.parent.submitCallback(obj)){
                    CloseTheWin();    
                }
           }
        }
        function loadUserGrid() {
            $('#usergrid').datagrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: false,
                url: "<%= !string.IsNullOrEmpty(Request.QueryString["userselaction"])? Request.QueryString["userselaction"] : "/Admin/WorkFlow/WFActivityActorSelector" %>",
                queryParams: {showall:$("#cbAll").is(":checked"), processid: '<%=Request.QueryString["processid"] %>', activityId: activityId, workItemId: '<%=Request.QueryString["workItemId"]%>', codeorname:$("#CodeOrName").val() },
                columns: [[
                                { field: 'ck', checkbox: true },
                                { field: 'LoginName', title: '工号', width: 80, align: 'center' },
                                { field: 'AdminName', title: '用户名', width: 80, align: 'center' },
                                { field: 'GroupNames', title: '部门', width: 120, align: 'left' },
                                { field: 'PositionNames', title: '岗位', width: 120, align: 'left' }
			            ]],
                pagination: true,
                pageSize: 30,
                pageList: [10, 15, 20, 30],
                rownumbers: true,
                pageNumber: 1,
                singleSelect: false,
                onBeforeLoad: function () {
                    RemoveForbidButton();
                },
                onLoadSuccess: function () {
                    var data = $('#usergrid').datagrid("getData");
                   
                    if(data.rows.length == 1){
                        $('#usergrid').datagrid("checkRow", 0);
                    }
                }
            });
        }
    </script>
    <style type="text/css">
    .selected{background-color:#F2F5F9;border:solid 1px #B9C6DA !important;}
    #dvSubmitToActivity{margin:2px;}
    #dvSubmitToActivity ul{font-size:12px}
    #dvSubmitToActivity li{color:#1D96E8;border:1px solid #F0F0F0;margin:2px 0 0;text-align:center;padding:1px;cursor:pointer}
    #dvSubmitToActivity div{font-size:12px;color:gray}
    .hover{background-color:#EFEFEF}
   
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%if(true) {%>
    <div class="editpage" region="west" class="CenterForm" title="下一个操作步骤" style="width:150px;">
        <div id="dvSubmitToActivity">
            <ul>
                <%foreach (Node act in Model)
                  {%><li activityid='<%=act.Id%>'>
                      <%=act.DisplayName%><div>
                          <%=act.Description%></div>
                  </li>
                <%}%>
            </ul>
        </div>
    </div>
    <%} %>
    <%if (Request.QueryString["allowselectuser"] != null && Request.QueryString["allowselectuser"].ToLower() == "true")
      {%>
    <div region="center" class="CenterForm" title="下一流程受理人">
        <div class="easyui-layout" fit="true">
            <div id="searchbar" region="north" style="height: 30px;">
                <label>姓名或账号：</label><input type=text id="CodeOrName" />
		        <a href="javascript:void(0);" class="easyui-linkbutton" iconCls="icon-search" plain="true" onclick="javascript:loadUserGrid()">查询</a>		
                <span style="<%=!string.IsNullOrEmpty(Request["workitemid"])? "":"display:none;" %>">
                    <label for="cbAll">检索全部用户：</label><input type=checkbox id="cbAll"  />
                </span>
            </div>
            <div region="center">
                <table id="usergrid">
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="javascript:void(0)" onclick="window.parent.submitCallback(null);CloseTheWin();"
            id="A1">根据原流程选择步骤</a>
              <a class="easyui-linkbutton" icon="icon-ok" href="javascript:void(0)" onclick="submit();"
            id="A2">确定</a>  <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
