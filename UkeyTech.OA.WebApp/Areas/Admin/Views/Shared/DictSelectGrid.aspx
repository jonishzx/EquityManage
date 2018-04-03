<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="选择字段内容查看" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","Utility")%>';
        var returnValue = true;
        function LoadSelectItems() {
                $('#DataGrid').datagrid({
                    nowrap: false,
                    striped: true,
                    fit: true,
                    border: false,
                    url: baseurl + "/GetPopupSelectDataList",
                    queryParams: { selecttypeid: <%= ViewData["SelectTypeId"] %> },
                    columns: [[
                        <% foreach(var m in (List<UkeyTech.WebFW.DAO.SYSDataColumn>)ViewData["Columns"]) {%>
                             { field: '<%=m.ColumnName %>', title:'<%=m.Caption %>', width: 100},
                        <%} %>
                        { field: 'ANC', title:'', width: 1}
			        ]],
                    pagination: true,
                    pageSize: 100,
                    pageList: [100, 150, 200, 300],
                    rownumbers: true,
                    pageNumber: 1,
                    singleSelect: true,
                    onClickRow: function (index, data) {
                        if (index > -1 && returnValue) {    
                            window.parent.returnValue = data.ID + ":" + data.名称;               
                            
                            if(window.parent.currId){
                                $(window.parent.currId).val(data.ID );
                                $(window.parent.currName).val(data.名称 );                              
                            }
                            else{
                                $(window.parent.currName).val(window.parent.returnValue);
                            }

                            if(window.parent.popupCallback){
                                window.parent.popupCallback(data.ID, data.名称);
                            }
                            CloseTheWin();
                        }
                    }
                 
                });
     }

     $(function(){
        LoadSelectItems();
     });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   
     <ld:Loading ID="Loading1" runat="server" />
    <div id="center" region="center">
        <table id="DataGrid">
        </table>
    </div>    
    <script type="text/javascript" language="javascript">
        var permission = <%=UkeyTech.OA.WebApp.Helper.GetPermissionJson(
        "CustomForm","View")%>
    </script>
</asp:Content>
