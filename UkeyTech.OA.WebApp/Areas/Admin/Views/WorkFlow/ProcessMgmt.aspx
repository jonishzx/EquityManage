﻿<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="工作流程进度管理(管理员)" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>    
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/jquery.edatagrid.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/ProcessMgmt.js")%>"></script>    
    
    <script type="text/javascript">
         var baseurl = '<%=Url.Action("","WorkFlow")%>';
    </script>
    <style type="text/css">
         #workitems
         {
            height: 210px;
            overflow: auto;
         }
         
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="SearchDiv" region="north">
        <label class="ybtext">
            发起人：</label>
      <%Html.RenderPartial(Helper.PopupControlPath,
                                new ViewDataDictionary(new
                                {
                                    IDControlName = "iNames",
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
    <div region="center"  style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <div region="south" split="true" style="width: auto;height:220px;">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
            <div title="任务实例">
                <div class="easyui-layout" fit="true">
                    <div region="center">
                        <table id="DGTaskInstance">
                        </table>
                    </div>
                    <div region="east" style="width:300px">
                        <div id="workitems">
                        </div>
                    </div>
                </div>
            </div>
             <div title="流程变量">
                <table id="DGTaskVar" toolbar="#toolbar" fitColumns="true" singleSelect="true">
                </table>
                <div id="toolbar">
		            <a href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="javascript:$('#DGTaskVar').edatagrid('addRow')">新建</a>
		            <a href="#" class="easyui-linkbutton" iconCls="icon-save" plain="true" onclick="javascript:$('#DGTaskVar').edatagrid('saveRow')">保存</a>
		            <a href="#" class="easyui-linkbutton" iconCls="icon-undo" plain="true" onclick="javascript:$('#DGTaskVar').edatagrid('cancelRow')">取消</a>
	            </div>   
            </div>     
        </div>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
