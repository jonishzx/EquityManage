<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单进度查看-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
    <style type="text/css">
        #formframe{width:100%;overflow:auto;}
    </style>
    <script type="text/javascript">
        var model = <%=ViewData["current"]%>;
        function setJsonToForm() {
            if(model) {
                for(var p in model){
                    $("[name='" + p + "']").val(model[p]).attr("disabled","disabled");                    
                }
            }

           
            $("#CustomForm").click(function(){return false;});
            $(".easyui-combobox").combobox("disable");
        }
       
        $(function () {
            setJsonToForm();
            init();
          
        });

        function resize(){          
            var height = $("#processview").parent().height();
            $("#processview").height(height);
            $("#processview").parent().css("overflow","hidden");
            //业务表单
            if( $("#formframe").length > 0)
            {
                height = $("#formframe").parent().parent().height();
                $("#formframe").height(height);
                $("#formframe").parent().parent().css("overflow","hidden");
             }
        }

        function init(){
            $('#DataGrid').datagrid({
                    nowrap: false,
                    striped: true,
                    fit: true,
                    border: false,
                    url: "<%=Url.Action("LoadApproveInfoList","CustomForm") %>",
                    queryParams: {targetid:'<%= ViewData["bizid"] %>', targetobject:'<%= ViewData["targetobject"] %>'},
                    columns: [[
					{ field: 'StaffID', title: '审核人', width: 150 },
					{ field: 'AuditingInfo', title: '审核信息', width: 250 },
					{ field: 'AuditingState', title: '审核结果', width: 70 },
					{ field: 'AuditingTime', title: '审核时间', width: 150,
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
                    onBeforeLoad: function () {
                        RemoveForbidButton();
                    }
                });

              $(window).resize(resize);
              setTimeout(resize, 2000);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="west" style="width:500px" title="表单信息">
        <div id="CustomForm">
            <%=TempData["ParsedFormContent"]%>
            <% TempData.Keep("ParsedFormContent"); %>
        </div>
    </div>
    <div region="center" title="流程进度" fit="false">    
       <iframe id="processview" style="overflow:auto;" src='<%= Url.Action("ProcessTraceView","WorkFlow") + "?id=" + ViewData["id"].ToString() + "&workflowprocessid=" + ViewData["workflowprocessid"].ToString() %>' 
            width="100%" height="400px">
            
       </iframe>
    </div>
    <div region="south" title="审核信息" border="false" class="" style="text-align:left;height: 200px">
        <table id="DataGrid">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
