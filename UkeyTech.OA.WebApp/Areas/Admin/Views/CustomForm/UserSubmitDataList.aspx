<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="业务信息列表" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","CustomForm")%>';
        var wfbaseurl = '<%=Url.Action("","WorkFlow")%>';
        var hiddenGroupId = null;

        function getQueryDataParams() {
            return { searchtext: $("#SearchText").val() };
        }

        function init() {
            window.queryData = function () {
                var stext = getQueryDataParams().searchtext;
                if (stext != "")
                    LoadUserSubmitDataList(stext);
                else
                    LoadUserSubmitDataList();
            }

            queryData();
        }

        $(document).ready(function () {
            //权限获取
            LoadPageModuleFunction("AddWorkItemList", init);
        });

        function LoadUserSubmitDataList(code) {
            if (permission.Browse)
                $('#DataGrid').datagrid({
                    nowrap: false,
                    striped: true,
                    fit: true,
                    border: false,
                    url: baseurl + "/LoadUserSubmitDataList?formid=<%=ViewData["FormId"] %>",
                    queryParams: {},
                    columns: [[
					<%=ViewData["Columns"] %>
			]],
                    pagination: true,
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
        function ViewProcess(bizid, id, workflowprocessid) {
             window.open(baseurl + "/UserSubmitDataView?" + "formid=<%=ViewData["FormId"] %>" + "&bizid=" + bizid + "&id=" + id + "&workflowprocessid=" + workflowprocessid + "&t=" + new Date().toString(), "progressview");             
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <div id="center" region="center" title="<%=ViewData["Title"] %>信息列表" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
