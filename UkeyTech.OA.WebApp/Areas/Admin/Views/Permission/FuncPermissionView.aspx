<%@ Page Language="C#"  Inherits="System.Web.Mvc.ViewPage" Title="授权明细表" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">    
     <script type="text/javascript">
         if (typeof (Object) === "undefined") {
             window.location.reload();
         }
         var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>';
    </script>
     <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
	<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/locale/easyui-lang-zh_CN.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
</head>
<script type="text/javascript">
$(document).ready(function () {
    //初始化列表
    LoadFuncPermissionGrid();
});

function CreatCellInfo(value, denyEnabled) {
    if (value != null && value.length > 0) {
        var firstStr = value.split(';');

        if (denyEnabled == 'False') {
          
            if (firstStr[0] == "True") {
                return '<img src=<%=Url.Content("~/Scripts/EasyUI/themes/icons/ok.png")%> />';
            }
        }
        else {
            if (firstStr[0] == "True" && firstStr[1] == "False") {
                return '<img src=<%=Url.Content("~/Scripts/EasyUI/themes/icons/ok.png")%> />';
            }
           
        }
    }    
};

function LoadFuncPermissionGrid() {
    $('#tableFuncPermissionlist').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        width: $(document).width(),
        height: $(document).height()-35,
        url:  '<%=Url.Action("FuncPermissionViewList","Permission")%>',
        queryParams: { OwnerCode: '<%=HttpContext.Current.Request["OwnerCode"] %>',OwnerValue: '<%=HttpContext.Current.Request["OwnerValue"].Replace(@"\", @"\\") %>',ScopeCode:'<%= HttpContext.Current.Request["ScopeCode"] %>',ScopeValue:'<%=HttpContext.Current.Request["ScopeValue"] %>' }, //参数(Post传递)
        frozenColumns: [[
            { field: 'ModuleName', title: '模块名称', width: 120, align: 'left' }
			]],
        columns: [[
                 <%=ViewData["Columns"] %>   
			]],
        pagination: true, //页码条
        pageSize: 20, //每页显示多少行
        pageList: [10, 20, 30], //可选行数
        rownumbers: true, //行号
        singleSelect: true, //单选
        pageNumber: 1, //初始显示页号
        onLoadSuccess: function () {
            _w_table_rowspan("#tableFuncPermissionlist", 2);
        }
    });
    
    var p = $('#tableFuncPermissionlist').datagrid('getPager');

    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}

function _w_table_rowspan(_w_table_id, _w_table_colnum) {
    _w_table_firsttd = "";
    _w_table_currenttd = "";
    _w_table_SpanNum = 0;
    _w_table_Obj = $(_w_table_id).parent().find("> .datagrid-view1 > .datagrid-body table tr td:nth-child(" + _w_table_colnum + ")");
    _w_table_Obj.each(function (i) {
        if (i == 0) {
            _w_table_firsttd = $(this);
            _w_table_SpanNum = 1;
        } else {
            _w_table_currenttd = $(this);
            if (_w_table_firsttd.text() == _w_table_currenttd.text()) {
                _w_table_SpanNum++;
                _w_table_currenttd.hide(); //remove();  
                _w_table_firsttd.attr("rowSpan", _w_table_SpanNum);
            } else {
                _w_table_firsttd = $(this);
                _w_table_SpanNum = 1;
            }
        }
    });
}
</script>
<body class="easyui-layout" fit="true">
    <div id="center" region="center">
        <table id="tableFuncPermissionlist" style="background: #fafafa;">
        </table>
    </div>
</body>
</html>