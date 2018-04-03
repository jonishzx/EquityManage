<%@ Page Title="关联公司信息" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register Src="~/Areas/Admin/Views/Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="~/Areas/Admin/Views/Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","BankAccount")%>';       
    </script>

    <script type="text/javascript" language="javascript">
            var hiddenGroupId = null;

            function getQueryDataParams() {
                return { searchtext: $("#SearchText").val() };
            }

            function init() {
                window.queryData = function () {        
                    var stext = getQueryDataParams().searchtext;
                    if (stext != "")
                        LoadMyRelateCompanyListGrid(stext);
                    else
                        LoadMyRelateCompanyListGrid();
                }
    
                queryData();
            }

            $(document).ready(function () {
                //权限获取
                LoadPageModuleFunction("MyRelateCompany", init);
            });


            function LoadMyRelateCompanyListGrid(code) {
                if (permission.Browse)                    
                $('#DataGrid').datagrid({
                    nowrap: false,
                    striped: true,
                    fit: true,
                    border: false,
                    url: baseurl + "/GetMyRelateCompanyList",
                    queryParams: { CodeOrName: code },
                    columns: [[
                                { field: 'ck', checkbox: true },
					            { field: 'MyCompanyName', title: '企业名称', width: 200 },  
                                { field: 'ShortName', title: '简称', width: 100 }   
                                				
			            ]],
                    pagination: true,
                    pageSize: 30,
                    pageList: [10, 15, 20, 30],
                    rownumbers: true,
                    pageNumber: 1,
                    singleSelect: true,
                    toolbar: [{
                        text: permission.Create ? '添加' : '',
                        iconCls: permission.Create ? 'icon-add' : "null",
                        handler: function () {
                            SetBackFunc(AddSuccess);
                            SetWin(740, 350, baseurl + '/CreateMyRelateCompany', '添加' + getTitle());
                        }
                    },
                        '-',
                        {
                            text: permission.Edit ? '修改' : '',
                            iconCls: permission.Edit ? 'icon-edit' : "null",
                            handler: function () {
                                var selId = getGridSelection('#DataGrid', 'MyCompanyId');

                                if (selId != "") {
                                    SetBackFunc(EditSuccess);
                                    SetWin(750, 350, baseurl + '/EditMyRelateCompany/' + selId, '修改' + getTitle());
                                }
                            }
                        },
                        '-',
                        {
                            text: permission.Delete ? '删除' : '',
                            iconCls: permission.Delete ? 'icon-cut' : "null",
                            handler: function () {
                                var ids = getGridSelections("DataGrid", "MyCompanyId");
                                if (ids != null && ids != '' && confirm('你确定删除选中的记录?'))
                                    deleteItems({ delids: ids }, baseurl + '/DeleteMyRelateCompany', DeleteSuccess);
                            }
                        }],
                    onBeforeLoad: function () {
                        RemoveForbidButton();
                    }
                });
            }


            function AddSuccess() {
                LoadMyRelateCompanyListGrid();
                setTimeout("MsgShow('系统提示','添加成功。');", 500);
            }

            function EditSuccess(func) {
                LoadMyRelateCompanyListGrid();
                setTimeout("MsgShow('系统提示','修改成功。');", 500);
            }

            function DeleteSuccess(func) {
                LoadMyRelateCompanyListGrid();
                setTimeout("MsgShow('系统提示','删除成功。');", 500);
            }
     </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SearchDiv" region="north">
    代码或名称：<input type="text" id="SearchText" style="width: 120px;" />
    <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="queryData();">
        查询</a>
    </div>
    <div id="center" region="center" title="关联公司列表" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
