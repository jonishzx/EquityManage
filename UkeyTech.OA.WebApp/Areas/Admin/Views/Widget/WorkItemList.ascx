<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UkeyTech.WebFW.Model.Widget>" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
 
    <table id="tbDataGrid" toolbar="#datafieldstoolbar">
    </table>
    <div id="datafieldstoolbar">
          <label class="ybtext">
            发起人：</label>
      <%Html.RenderPartial(Helper.PopupControlPath,
                                new ViewDataDictionary(new
                                {
                                    IDControlName = "iIds",
                                    TextControlName = "iNamesText",
                                    DictID = "ValidUser",
                                    AddEmptyItem = true,
                                    Required = false,
                                    Width = "80"
                                }));%>  
       
        <label class="ybtext">
            日期范围：</label>
        <input id="StartDate" name="StartDate" type="text" class="form-item-text Wdate" onclick="WdatePicker()"
            value="" style="width: 90px" maxlength="50" />-
        <input id="EndDate" name="EndDate" type="text" class="form-item-text Wdate" onclick="WdatePicker()"
            value="<%=DateTime.Now.ToString("yyyy-MM-dd")%>" style="width: 90px" maxlength="50" />
		<a href="javascript:void(0);" class="easyui-linkbutton" iconCls="icon-search" plain="true" onclick="javascript:LoadDataGrid()">查询</a>		
	</div>
    <script type="text/javascript">
     var baseurl = '<%=Url.Action("","WorkFlow")%>';
     function refresh(){
         $('#tbDataGrid').datagrid("reload");
     }
    SetBackFunc(refresh);
    function LoadDataGrid() {
        $('#tbDataGrid').datagrid({
            nowrap: false,
            striped: true,
            fit:true,
            border: false,
            queryParams: getQueryDataParams(),
            url: '<%=Model.Target %>',           
            columns:[
                <%=Model.Parameters %>]
            ,
            singleSelect: true,
            pagination: true,
            pageSize: 15,
            pageList: [10, 15, 20, 30],
            rownumbers: true

        });
    }
     function getQueryDataParams() {
            return { actorid:$("#iIds").val(),
                     startdate:$("#StartDate").val(), 
                     enddate:$("#EndDate").val(), 
                     processid:$("#PROCESS_ID").val()};
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
                        
                        SetBackFunc(refresh);
                    }
                });
        }
        function claimWorkItem(id, text) {
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
                        parseMessage(json, refresh, text);
                        LoadDataGrid();
                        SetBackFunc(refresh);
                    }
                });
        }
      
        function completeWorkItem(id, text) {
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
                    parseMessage(json, refresh, text);
                    SetBackFunc(refresh);
                }
            });
        }

    LoadDataGrid();
    </script>

