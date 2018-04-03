var gobalkeyvalues = [];
var gridId = "DataGrid";
var callback = "";
var valueId = "";

function findColumnValByName(row, text) {
    for (var col in row) {
        if (col.indexOf(text) >= 0) {
            return row[col];
        }
    }
    return "";
}
function delById(id) {
    for (var i = 0; i < gobalkeyvalues.length; i++) {
        if (gobalkeyvalues[i].id == id) {
            gobalkeyvalues.splice(i, 1);
            if (gobalkeyvalues.length > 0)
                setTextValue();
            else
                clearvalue();
            break;
        }
    }
}
function find(id) {
    for (var i = 0; i < gobalkeyvalues.length; i++) {
        if (gobalkeyvalues[i].id == id) {
            return i;
        }
    }
    return -1;
}
function findByName(name) {
    for (var i = 0; i < gobalkeyvalues.length; i++) {
        if (gobalkeyvalues[i].name == name) {
            return i;
        }
    }
    return -1;
}

function setTextValue() {
    var ids = '';
    var names = '';


    for (var i = 0; i < gobalkeyvalues.length; i++) {

        ids += gobalkeyvalues[i].id;
        names += gobalkeyvalues[i].name;

        if ((i + 1) < gobalkeyvalues.length) {
            ids += ',';
            names += ',';
        }

    }
    $("#text").val(names);
    $("#idvalue").val(ids);
}

function addValues(ids, names) {
    if (ids == "" || ids === undefined) {
        ids = getGridSelections(gridId, "ID");
        names = getGridSelections(gridId, "名称");
    }

    if (ids == "" || ids === undefined) {
        return;
    }
    var idsary = ids.split(',');
    var namesary = names.split(',');
    var idx = -1;
    for (var i = 0; i < idsary.length; i++) {
        idx = find(idsary[i]);
        idx = -1;
        if (idx >= 0) {
            continue;
        } else {
            gobalkeyvalues.push({ id: idsary[i], name: namesary[i] });
        }
    }
    setTextValue();
}

function getOpener() {
    var theopener;
    if (window.dialogArguments != null) {
        theopener = window.dialogArguments;
    } else if (window.opener != null) {
        theopener = window.opener;
    } else {
        theopener = window.parent;
    }
    return theopener;
}

function clearvalue() {
    gobalkeyvalues.length = 0;
    $("#" + gridId).datagrid('unselectAll');
    $("#idvalue").val('')
    $("#text").val('');
}

function getValueAndQuit() {
    var theopener = getOpener();

    var ids = $("#idvalue").val();
    var names = $("#text").val();

    //call back
    if (typeof (theopener[callback]) != "undefined") {
        theopener[callback](ids, names, getGridSelectRows(gridId));
    }
    CloseTheWin();
}

function initValue() {
    var theopener = getOpener();
    if (valueId) {
        var ids = $(theopener.document).find('#' + valueId).val();
        $("#idvalue").val(ids);
    }
}