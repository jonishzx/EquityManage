
function init() {
    LoadBillNoGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("WebSiteMgmt", init);
});

var currBillNoRow;
var currBillNo;
function FixBillNo() {

    if (currBillNo && currBillNoRow) {
        //注意，只有在单选时可以采用删除前判断
        var cdata = currBillNoRow;
        cdata.format = $("#format").val();
        cdata.datetimeformat = $("#datetimeformat").val();
        cdata.keyfield = $("#keyfield").val();
        cdata.sortfield = $("#sortfield").val();
        cdata.BillNo = currBillNo;
        $.messager.confirm('Question', '确定要修复错误的单据编码?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: baseurl + '/FixBillNo',
                    data: cdata,
                    dataType: "json",
                    success: function (json) {

                        setTimeout("LoadExpectBillNoGrid();", 500);
                        setTimeout("MsgShow('系统提示','操作成功。');", 1000);
                        currBillNo = null;
                    }
                });
            }
        });
    }
}

function LoadBillNoGrid(code, where) {
    $('#DataGrid').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/GetBillNoList",
        columns: [[
            { field: 'document', title: '编号代码', width: 90, align: 'left' },
            { field: 'number', title: '目前编号', width: 90, align: 'left' },
            { field: 'date', title: '日期', width: 120, align: 'left' },
            { field: 'TargetTable', title: '目标业务表', width: 180, align: 'left' },
            { field: 'TargetColumn', title: '列', width: 120, align: 'left' }
        ]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        onBeforeLoad: function() {
            RemoveForbidButton();
        },
        onClickRow: function (index, data) {
            if (data.document) {
                currBillNoRow = data;
                LoadExpectBillNoGrid();
            }
        }
    });
}

function LoadExpectBillNoGrid() {
    $('#ExceptGrid').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/GetExceptBillNoList",
        queryParams: { document: currBillNoRow.document },
        columns: [[
            { field: currBillNoRow.TargetColumn, title: '编号', width: 90, align: 'left' }
        ]],
        toolbar: [{
            text: '修正',
            iconCls: 'icon-edit',
            handler: function () {
                FixBillNo();
            }
        }],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        onBeforeLoad: function () {
            RemoveForbidButton();
        },
        onClickRow: function (index, data) {
            currBillNo = data[currBillNoRow.TargetColumn];
            
        }
    });
}