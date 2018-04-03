var functionCodes = new Array();
var functionName
var gridInit = false;

function OpenFunctionDialog() {
    var dlg = $("#dlgFunction");
    dlg.css("display", "block");
    dlg.dialog({
        closed: false,
        showType: null,
        modal: true,
        top: 3,
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                AddCodes();
                var codes = functionCodes.join(')(');
               
                if (codes != "") {
                    codes = "(" + codes + ")";
                }
                alert(codes);
                $('#RelationFunctionID').val(codes);
                CloseFunctionDialog();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                CloseFunctionDialog();
            }
        }]
    });
}

function CloseFunctionDialog() {
    $('#dlgFunction').dialog({ closed: true });
    common.aryClear(functionCodes);
}

function AddCodes() {
    var rows = $('#tbFunction').datagrid("getSelections");

    for (var i = 0; i < rows.length; i++) {
        var code = rows[i]["FunctionID"].toString();

        common.aryCheckAdd(functionCodes, code);
    }
}

function LoadFunctionGrid(code) {
    gridInit = true;

    $('#tbFunction').datagrid({
        height: 185,
        width: 335,
        nowrap: false,
        striped: true,
        border: false,
        url: FunctionList,
        queryParams: {CodeOrName: code },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
			]],
        columns: [[
                    { field: 'FunctionCode', title: '功能代码', width: 80, align: 'left' },
                    { field: 'FunctionName', title: '功能名称', width: 80, align: 'left' },
                    { field: 'FunctionType', title: '功能类型', width: 60, align: 'center' }
			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        onBeforeLoad: function (param) {
            if (!gridInit) {
                AddCodes();
            }
        }
    });

    var p = $('#tbFunction').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }

    gridInit = false;
}