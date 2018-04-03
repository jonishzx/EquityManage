var permission, dictItems, dictItems1, dictItems2, dictItems3;

var common = {
    aryContains : function(ary, obj) {
      if(null==obj || typeof(ary.length) == "undefined"){return false;} 
      for(var i=0;i<ary.length;i++) 
      { 
       if(ary[i]==obj) 
       { 
        return true; 
       } 
      }     
      return false; 
    },
    aryClear : function(ary) {
        ary.length=0;                                              
    },
    aryRemoveAt : function(ary, index) {
        if(typeof(ary.length) == "undefined" || isNaN(index)||index>ary.length){return false;} 
        for(var i=0,n=0;i<ary.length;i++) 
        { 
            if(ary[i]!=ary[index]) 
            { 
                ary[n++]=ary[i] 
            } 
        } 
        ary.length-=1 
    },
    aryCheckAdd : function(ary, obj){
         if(null==obj || typeof(ary.length) == "undefined"){return false;} 
          for(var i=0;i<ary.length;i++) 
          { 
           if(ary[i]==obj) 
           { 
            return false; 
           } 
          }     
          ary.push(obj); 
    }
};

function newguid() {
    var S4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

function getFileSize(val) {
    var tb = 1099511627776;
    var gb = 1073741824;
    var mb = 1048576;
    var kb = 1024;
    if (typeof (val) == "number") {
        if (val > tb) {
            return (val / tb).toFixed(2) + "TB"
        }
        else {
            if (val > gb) {
                return (val / gb).toFixed(2) + "GB"
            } else {
                if (val > mb) {
                    return (val / mb).toFixed(2) + "MB"
                } else {
                    if (val > kb) {
                        return (val / kb).toFixed(2) + "KB"
                    } else { return val + "B" }
                }
            }
        }
    } return ""
}

///datagrid :合并行
function mergeCellsByField(tableID, colList) {
    var ColArray = colList.split(",");
    var tTable = $("#" + tableID);
    var TableRowCnts = tTable.datagrid("getRows").length;
    var tmpA;
    var tmpB;
    var PerTxt = "";
    var CurTxt = "";
    var alertStr = "";
    for (j = ColArray.length - 1; j >= 0; j--) {
        PerTxt = "";
        tmpA = 1;
        tmpB = 0;

        for (i = 0; i <= TableRowCnts; i++) {
            if (i == TableRowCnts) {
                CurTxt = "";
            }
            else {
                CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
            }
            if (PerTxt == CurTxt) {
                tmpA += 1;
            }
            else {
                tmpB += tmpA;

                tTable.datagrid("mergeCells", {
                    index: i - tmpA,
                    field: ColArray[j],　　//合并字段
                    rowspan: tmpA,
                    colspan: null
                });
                tTable.datagrid("mergeCells", { //根据ColArray[j]进行合并
                    index: i - tmpA,
                    field: "Ideparture",
                    rowspan: tmpA,
                    colspan: null
                });

                tmpA = 1;
            }
            PerTxt = CurTxt;
        }
    }
}

//计算两个日期相隔多少天；
function getDaysApart(btime, etime) {
    var days = 0;
    if (btime !== "" && etime !== "") {
        days = parseInt(Math.abs(btime - etime) / 1000 / 60 / 60 / 24);
        days += 1;
    }
    return days;
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
//字符转换为日期；
function stringToDate(strDate) {
    return new Date(strDate.replace(/-/g, "/").replace("T", " "));
}
function getUrlQueryParams() {
    var query = {};
    var inputs = $(".queryarea").find("input,select");
    for (var i = 0; i < inputs.length; i++) {
        var name = $(inputs[i]).attr("name");
        if (name)
            query[name] =  encodeURIComponent($(inputs[i]).val());
    }
    return query;
}
function getQueryParams() {
    var query = {};
    var inputs = $(".queryarea").find("input,select");
    for (var i = 0; i < inputs.length; i++) {
        var name = $(inputs[i]).attr("name");
        if (name)
            query[name] = $(inputs[i]).val();
    }
    return query;
}
function openListParentTab(title, url) {
    window.parent.addTab(title, null, url, "main", true);
}
function printthis() {
    var oldsetting = { h: $("#CenterForm").css("height") };
    $(".SouthForm").hide();
    $(".CenterForm").css("height", "auto");
    $("#A3").hide();
    window.print();
    $("#A3").show();
    $(".CenterForm").css("height", oldsetting.h);
    $(".SouthForm").show();
    return false;
}
function setPrintMode() {

    $("input[type='text']").each(function (e) {
        $(this).parents(".ym-fbox-text,.ym-fbox-select").append("<span class='print'>" + $(this).val() + "</span>");
        if ($(this).parent().is("span"))
            $(this).parent().remove();
        else
            $(this).remove();
    });
}
function getFmtUrl(baseUrl, params) {
    //返回格式化的url
    if (baseUrl.indexOf("?") > 0 && baseUrl.lastIndexOf("&") != 0)
        baseUrl += "&";
    else
        baseUrl += "?";

    for (var m in params) {
        if (params[m])
            baseUrl += m + "=" + params[m] + "&";
    }
    return baseUrl;
}
function post(url, data, callback) {
    if (data)
        data.t = new Date();
    $.ajax({
        url: url,
        data: data,
        type: "POST",

        success: function (msg) {
            parseMessage(msg, callback, '', data);
        }
    });
}
function get(url, data, callback) {
    if (data)
        data.t = new Date();
    $.ajax({
        url: url,
        data: data,
        type: "GET",

        success: function (msg) {
            parseMessage(msg, callback);
        }
    });
}
//权限json获取
function LoadPageModuleFunction(modulecode, callback, chkdatarule) {
    var url = rooturl + "Admin/Permission/GetModuleFunctionJson";

    $.ajax({
        url: url,
        data: { ModuleCode: modulecode, t: new Date().toString(), withdatarule: chkdatarule ? 'true' : 'false' },
        type: "GET",
        dataType: "json",
        success: function (msg) {
            permission = msg;
            callback(msg);
        }
    });
}
//权限json获取
function setEleByPrivilege(permission) {
    for (var p in permission) {
        //检查权限可用性
        if (!permission[p] && /elem\$(\w)+\$(edit|visible)/gi.test(p.toString())) {
            setElemPvgAttr(p);
        }
    }
}

function showPageInput() {
    var result = "", oldval = "", currval = "";
    $("input,select,textarea").each(function() {
        currval = ($(this).attr("name") ? $(this).attr("name") : $(this).attr("id"));
        if (currval && currval != oldval) {
            result += 'elem$' + currval + '$readonly' + "\r\n";
            oldval = currval;
        }
    });
    return result;
}
function setSelectorReadonly(selector) {
    $(selector).each(function(i, elm) {

        if ($(elm).is("select") || $(elm).attr("type") == "checkbox" || $(elm).attr("type") == "radio") {
            setSelectReadonly(elm);
        } else if ($(elm).attr("type") == "text" || $(elm).attr("type") == "hidden" || $(elm).is("textarea")) {
            $(elm).attr("readonly", "readonly").addClass("ym-readonly").removeAttr("onfocus").removeAttr("onclick");
            if ($(elm).parent().hasClass('numberbox')) {
                $(elm).parent().removeClass("textbox numberbox").find(".textbox-text").attr("readonly", "readonly").addClass('ym-readonly');
            }
        } else {
            $(elm).parents("div.ym-fbox-text").hide();
        }
    }
    );
}
//设置控件访问权限
function setElemPvgAttr(functionCode) {
    var vals = functionCode.split('$');
    var notAllowEdit = vals.length > 2 && vals[2] == "edit";
    var notShow = vals.length > 2 && vals[2] == "visible";
    var elm = vals.length > 1 ? $("[name='" +  vals[1] +"']") : null;
    if (!elm)
        return;
    switch(vals[0]) {
        case "elem":
            if (vals.length > 2) {
             
                if (notAllowEdit) {
                    if ($(elm).is("select") || $(elm).attr("type") == "checkbox" || $(elm).attr("type") == "radio") {
                        setSelectReadonly(elm);
                    }
                    else if ($(elm).attr("type") == "text" || $(elm).attr("type") == "hidden" || $(elm).is("textarea")) {
                        $(elm).attr("readonly", "readonly").addClass("ym-readonly").removeAttr("onfocus").removeAttr("onclick");
                        if ($(elm).parent().hasClass('numberbox')) {
                            $(elm).parent().removeClass("textbox numberbox").find(".textbox-text").attr("readonly", "readonly").addClass('ym-readonly');
                        }
                    }
                    else {
                        $(elm).parents("div.ym-fbox-text").hide();
                    }
                } else if(notShow) {
                    $(elm).parents("div.ym-fbox-text").hide();
                }
            }
    }
}

//字典信息获取
function loadDictItems(dictids, callback) {
    var url = rooturl + "Admin/System/GetALLDictItemList";

    $.ajax({
        url: url,
        data: { ParentID: dictids, t: new Date().toString() },
        type: "GET",
        dataType: "json",
        success: function (msg) {
            dictItems = msg.rows;
            callback(msg);
        }
    });
}

function loadDictItems1(dictids, callback) {
    var url = rooturl + "Admin/System/GetALLDictItemList";

    $.ajax({
        url: url,
        data: { ParentID: dictids, t: new Date().toString() },
        type: "GET",
        dataType: "json",
        success: function (msg) {
            dictItems1 = msg.rows;

        }
    });
}

function loadDictItems2(dictids, callback) {
    var url = rooturl + "Admin/System/GetALLDictItemList";

    $.ajax({
        url: url,
        data: { ParentID: dictids, t: new Date().toString() },
        type: "GET",
        dataType: "json",
        success: function (msg) {
            dictItems2 = msg.rows;

        }
    });
}

function loadDictItems3(dictids, callback) {
    var url = rooturl + "Admin/System/GetALLDictItemList";

    $.ajax({
        url: url,
        data: { ParentID: dictids, t: new Date().toString() },
        type: "GET",
        dataType: "json",
        success: function (msg) {
            dictItems3 = msg.rows;
        }
    });
}

//员工信息获取
function loadEmpItems(dictids, callback) {
    var url = rooturl + "Admin/System/GetALLEmpItemList";

    $.ajax({
        url: url,
        data: { ParentID: dictids, t: new Date().toString() },
        type: "GET",
        dataType: "json",
        success: function (msg) {
            dictItems = msg.rows;
            callback(msg);
        }
    });
}

function getDictItemName(dictid, code) {
    if (code)
        for (var i = 0; i < dictItems.length; i++) {
            if (dictItems[i].DictID == dictid && dictItems[i].Code == code)
                return dictItems[i].Name;
        }
    return '';
}
function printArea(e) {
    var container = $(e).attr('rel');
    $(container).printArea();
    return false;
}
function vaildFloatNumber(evnt, obj) {
    evnt = evnt || window.event;
    var keyCode = window.event ? evnt.keyCode : evnt.which;
    var value = $(obj).val();
    if ((value.length == 0 || value.indexOf(".") != -1) && keyCode == 46) return false;
    return keyCode >= 48 && keyCode <= 57 || keyCode == 46 || keyCode == 8;
}
function openNewWinByLink(url) {
    if (!document.all) {
        var a = $("<a href='" + url + "' target='_blank'>go</a>").get(0);
        var e = document.createEvent('MouseEvents');
        e.initEvent('click', true, true);
        a.dispatchEvent(e);
    }
    else {
        window.open(url, '', "resizable=yes,left=0,top=0,width=" + (screen.availWidth - 10) + ",height=" + (screen.availHeight - 30));
    }
}
function openModalWinByLink(url, callback) {
    if (!document.all) {
        var a = $("<a href='javascript:void(0);' onclick='openModalWin( '" + url + ",(screen.availWidth - 10), (screen.availHeight - 30))' target='_blank'>go</a>").get(0);
        var e = document.createEvent('MouseEvents');
        e.initEvent('click', true, true);
        a.dispatchEvent(e);
    }
    else {
        openModalWin(url, (screen.availWidth - 10), (screen.availHeight - 30));
    }
    if (callback)
        callback();
}
function openModalWin(url, width, height) {
    if (window.showModalDialog) {
        window.showModalDialog(url, window,
        "dialogWidth:" + width + "px;dialogHeight:" + height + "px;status:no;help:no;");
    } else {
        window.open(url, window,
        'height=' + width + ',width=' + height + ',toolbar=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no ,modal=yes');
    }
}
function ShowWorkItemsLoading() {
    ShowLoading("正在处理,请稍候...");
}
// 弹出并选择指定框的值
function popupAndSelectItems(url, param, idfield, textfield) {
    if (this.currName == undefined)
        currName = textfield;

    if (this.currId == undefined)
        currId = idfield;

    SetWin(400, 300, url + '?' + param, '数据选择');
}
function newGuid(splitter) {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += (splitter ? "-" : "");
    }
    return guid;
}
function fmtDecimal(value, groupchar) {
    //格式化数值
    if (value == null)
        return "0";
    if (!groupchar)
        groupchar = ',';
    var firCap = "";
    if (typeof (value) == "String") {
        if (value[0] == "-") {
            firCap = value[0];
            value = value.toString().replace("-", "");
        }
    }
    else if (value < 0) {
        firCap = "-";
        value = value.toString().replace("-", "");
    }
    var smallnumber = /(\.)(\d)+/.exec(value);
    var s = value.toString();
    if (smallnumber && smallnumber.length && smallnumber.length > 0)
        s = value.toString().replace(smallnumber[0], "");
    var rst = "", idx = 0;

    for (idx = s.length; idx > 3; idx = idx - 3) {
        rst = groupchar + s.slice(idx - 3, idx) + rst;
    }
    return firCap + s.slice(0, idx) + rst + (smallnumber && smallnumber.length && smallnumber.length > 0 ? smallnumber[0] : "");
}
function newMaxWinlinkbutton(icon, title, url, tip) {

    return "<a href='javascript:void(0)' onclick='SetWinWithMaxSize(\"" + url + "\",\"" + tip + "\")' class='l-btn-text grid-link " + icon + "' title='" + (tip ? tip : "") + "'>" + (title ? title : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "</a>";
}
function newPopWinlinkbutton(icon, title, url, tip, width, height) {

    var pwidth = width ? width : "720";
    var pheight = height ? height : "480";
    return "<a href='javascript:SetWin(" + pwidth + "," + pheight + ",\"" + url + "\",\"" + tip + "\");void(0);' class='l-btn-text grid-link " + icon + "' title='" + (tip ? tip : "") + "'>" + (title ? title : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "</a>";
}
function newlinkbutton(icon, title, func, tip) {
    return "<a href='javascript:" + func + ";void(0);' class='l-btn-text grid-link " + icon + "' title='" + (tip ? tip : "") + "'>" + (title ? title : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "</a>";
}
function newlink(icon, title, url, target, tip) {
    return "<a href='" + url + "' target='" + (target ? target : "_blank") + "' class='l-btn-text grid-link " + icon + "' title='" + (tip ? tip : "") + "'>" + (title ? title : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "</a>";
}
// JScript 文件
function modalPdWin(type, baspath, appendparam) {
    if (window.showModalDialog) {
        window.showModalDialog("../Controls/FileSelector.aspx" + "?sp=1&type=" + type + "&basepath=" + baspath + "&t=" + new Date().getTime() + appendparam, window,
        "dialogWidth:800px;dialogHeight:600px;status:no;help:no;");
    } else {
        window.open('../Controls/FileSelector.aspx' + "?sp=1&type=" + type + "&basepath=" + baspath + "&t=" + new Date().getTime() + appendparam, window,
        'height=800,width=600,toolbar=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no ,modal=yes');
    }
}

function modalDirWin(type, baspath, appendparam) {
    if (window.showModalDialog) {
        window.showModalDialog("../Controls/DirSelector.aspx" + "?st=0&sf=1&dirsel=1&filesel=0&type=" + type + "&basepath=" + baspath + "&t=" + new Date().getTime() + appendparam, window,
        "dialogWidth:800px;dialogHeight:600px;status:no;help:no;");
    } else {
        window.open('../Controls/DirSelector.aspx' + "?st=0&sf=1&dirsel=1&filesel=0&type=" + type + "&basepath=" + baspath + "&t=" + new Date().getTime() + appendparam, window,
        'height=800,width=600,toolbar=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no ,modal=yes');
    }
}

function ShowLoading(text) {
    if ($("#BBQPOPLOADINGWINDOW").length > 0) {
        $("#loadingtext").html(text);
        $("#BBQPOPLOADINGWINDOW").show();
    }
    else {
        $('<div id="BBQPOPLOADINGWINDOW" class="divLoading" style="position: absolute;left: 0px; top: 0px;width: 100%;height: 100%;z-index: 10000;background-color: white;border: 0;"><img src="' + (typeof (rooturl) != "undefined" && rooturl ? rooturl : '/') + 'Content/Images/loading.gif" style="display: block;float: left;margin: 21% 0 0 40%;"/><div id="loadingtext" style="float: left;margin: 22% 0 0 5px;">' + (typeof (text) != "undefined" && typeof (text) == "String" && text ? text : '读取中...') + '</div></div>').appendTo("body").show();
    }
}
function ShowError(text, moretext) {
    if ($("#ERRORMESSAGEBOX").length == 0) {
        $('<div id="ERRORMESSAGEBOX"><div class="messager-icon messager-error"/><div id="errormsgtext"></div><div style="clear:both;"/><div id="pMoreError" style="padding:5px" ><div></div>').appendTo("body").show();
    }
    $("#errormsgtext").html(text);
    if (moretext) {
        $("#pMoreError").panel({
            collapsed: true,
            title: "更多异常信息",
            iconCls: "icon-error"
        });
        $("#pMoreError").show();
        $("#pMoreError").html(moretext);
    }
    else {
        $("pMoreError").hide();
    }
    $('#ERRORMESSAGEBOX').dialog({
        title: '异常提示',
        width: 400,
        height: 200,
        closed: true,
        cache: true,
        modal: true
    });
}
function HideLoading() {
    $("#BBQPOPLOADINGWINDOW").remove();
}

function setSelectReadonly(selElem) {
    if ($(selElem).is("select")) {
        if ($(selElem).parent().find(".seltext").length == 0 && $(selElem).css("display") != "none") {
            $(selElem).hide();
            $(selElem).parent().append("<span class='seltext'>" + $(selElem).find(":selected").text() + "<span>");
        }
    }
    else {
        $(selElem).parent().find("label, input").hide();
        $(selElem).parent().find("[for='" + $(selElem).attr("id") + "']").show();
    }
}

function setreadonly(selector) {
    if (!selector)
        selector = "#busiMainContent"; //default for workflow

    $("#btnSave").remove();

    $("input:radio").each(function () {
        $(this).attr("disabled", "disabled");
    });

    $("input:checkbox").each(function () {$(this).attr("disabled", "disabled");});

    var selects = $(selector).find("select");

    for (var i = 0; i < selects.length; i++) {
        setSelectReadonly(selects[i]);
    }
    $(".easyui-numberspinner").numberspinner('disable');
    $(".form-op-button").remove();
    $(".combotree").combotree('disable');
    $(".combogrid").combogrid('disable');
    $(".ym-required").remove();
    // $(".Wdate").attr("readonly", "readonly").attr("onfocus", "").attr("onclick", "");
    $(".Wdate").attr("readonly", "readonly").removeAttr("onfocus").removeAttr("onclick");
    $(selector).find("textarea,input,select").removeClass("Wdate").addClass("ym-readonly").attr("readonly", "readonly");
    $("span").removeClass("textbox numberbox");
    //$("input[value='...'],input[value='X']").hide();
    var popup = $(selector).find(".popupcontrol");
    $(popup).find(".popuptool").remove();
    $(popup).find("input").addClass("ym-readonly");
    $(popup).removeClass("popupcontrol");
    //$("a[ptype='pop'],a[ptype='clrpop']").hide();
    $("#datafieldstoolbar").remove();
    $("#datafieldstoolbar2").remove();
    $("#toolTravell").remove();
    $("#toolWorkRecord").remove();
    $("#toolQuarterage").remove();
    $("#toolMealfee").remove();
    $("#toolBackCompany").remove();
    $("#tbGetBack").remove();

}

function removeAllWFUI() {
    $("div[for-activity]").remove();
}
function removereadonly() {
    $("#NSTCEndDate").removeAttr("readonly").removeClass("ym-readonly");

    $("#ID").focus(function () {
        WdatePicker({ dateFmt: 'yyyy-MM-dd', onpicked: clacDays() });
    });

    $("#NMInterest").removeAttr("readonly").removeClass("ym-readonly");
    $("#NMServiceCharge").removeAttr("readonly").removeClass("ym-readonly");
    $("#NMPenaltyInterest").removeAttr("readonly").removeClass("ym-readonly");
}
function setWFUIEnabled(currActivity) {
    //专属流程元素才显示
    //$("div[for-activity] + div[for-activity!=" + currActivity + "]").remove();

    selector = "#busiMainContent"; //default for workflow
    $("#btnSave").remove();
    //非专属流程项目不可写
    var divs = $(selector).find("div[for-activity!=" + currActivity + "]");
    for (var i = 0; i < divs.length; i++) {
        if ($(divs[i]).hasClass('popuptool'))
            continue;
        $(divs[i]).find(".easyui-numberspinner").numberspinner('disable');
        $(divs[i]).find(".combotree").combotree('disable');
        $(divs[i]).find(".combogrid").combogrid('disable');
        $(divs[i]).find(".ym-required").remove();
        $(divs[i]).find(".Wdate").attr("onfocus", "");
        $(divs[i]).find("select").attr("disabled", "disabled")
        $(divs[i]).find("textarea,input,select").removeClass("Wdate").addClass("ym-readonly").attr("readonly", "readonly");
        $(divs[i]).find("a[ptype='pop'],a[ptype='clrpop']").hide();
        $(divs[i]).find(".form-op-button").remove();
    }
    $("#datafieldstoolbar").remove();

    $("input:radio").each(function () {
        $(this).attr("disabled", "disabled");
    });
}
function setEditable(selector) {
    $(selector).removeClass("ym-readonly").removeAttr("readonly");
}
function fixCobmboInIE() {
    if ($.browser.msie) {
        var combos = $(".combo-text");
        var numbox = $(".spinner-text");
        if (numbox.length > 0) {
            var w1 = $(numbox).width();
            $(numbox).width(w1 - 5);
        }
        if (combos.length > 0) {
            var w1 = $(combos).width();
            $(combos).width(w1 - 5);
        }
    }
}
function fixCobmboColumnInIE() {
    if ($.browser.msie) {
        var combos = $("td").find("input.combo-text");
        $(combos).width($(combos).width() - 5);
    }
}
function parseMessage(msg, callback, text, data) {
    HideLoading();
    var arry = msg.split(':');
    if (arry[0] == '1') {
        if (callback != undefined) {
            callback(msg, data);
        }
        else {
            if (arry[1])
                setTimeout("MsgShow('系统提示   ','" + arry[1] + "');", 500);
            else
                setTimeout("MsgShow('系统提示   ','" + "操作成功" + "');", 500);
        }
    }
    else if (arry[0] == '0') {
        setTimeout("ShowError('系统提示','" + msg.replace("0:", "") + "');", 1000);
        $.messager.progress('close');
    }
    else if (arry[0] == 'uri') {
        if (arry[1]) {
            SetWinWithMaxSize(arry[1], text ? "提交" + text : "提交任务");
            //window.location.href = arry[1];
            if (callback != undefined)
                setTimeout(callback, 500);
        }
    }
}
function SetWinWithMaxSize(url, title) {
    if (/.*[\u4e00-\u9fa5]+.*$/.test(url)) {
        var tmp = url;
        url = title;
        title = tmp;
    }
    window.top.SetWin($(window.top).width(), $(window.top).height(), url, title);
    window.top.RunBackFunc = window.RunBackFunc;
}

function SetWinTop(width, height, url, title) {
    window.top.SetWin(width, height, url, title);
    window.top.RunBackFunc = window.RunBackFunc;
}

function getQueryStr(str) {
    var LocString = String(window.document.location.href);
    var rs = new RegExp("(^|)" + str + "=([^\&]*)(\&|$)", "gi").exec(LocString), tmp;

    if (tmp = rs) {
        return tmp[2];
    }

    // parameter cannot be found  
    return "";
}

//根据文档宽度获取比例宽度

function ltrim(stringToTrim) {
    return stringToTrim.replace(/^\s+/, "");
}
function rtrim(stringToTrim) {
    return stringToTrim.replace(/\s+$/, "");
}

function trim(stringToTrim) {
    return stringToTrim.replace(/^\s+|\s+$/g, "");
}
function getYMD(days) {
    var y = days > 360 ? Math.round(days / 360) : 0, m = 0, d = 0;
    var mdays = y > 0 ? ((days - y * 360) % 360) : days;

    if (mdays > 0) {

        m = mdays > 30 ? Math.round(mdays / 30) : 0;
        d = m > 0 ? (mdays - m * 30) : mdays;
    }
    return (y > 0 ? y + "年" : "") + (m > 0 ? m + "月" : "") + (d > 0 ? d + "天" : "");
}
function displayage(yr, mon, day, countunit, decimals, rounding, todate) {
    // Starter Variables
    if (todate && todate.indexOf('-') >= 0) {
        todatevars = todate.split('-');
        today = new Date(todatevars[0],
        parseInt(todatevars[1].substring(0, 1) == '0' ? todatevars[1].substring(1) : todatevars[1]) - 1,
        todatevars[2].substring(0, 1) == '0' ? todatevars[2].substring(1) : todatevars[2],
        0, 0, 0, 0);
    }
    else {
        today = todate ? new Date(todate) : new Date();
    }
    yr = parseInt(yr);
    mon = parseInt(mon.substring(0, 1) == '0' ? mon.substring(1) : mon);
    day = parseInt(day.substring(0, 1) == '0' ? day.substring(1) : day);

    var one_day = 1000 * 60 * 60 * 24;
    var one_month = 1000 * 60 * 60 * 24 * 30;
    var one_year = 1000 * 60 * 60 * 24 * 30 * 12;
    var pastdate = new Date(yr, mon - 1, day);
    var return_value = 0;

    finalunit = (countunit == "days") ? one_day : (countunit == "months") ? one_month : one_year;
    decimals = (decimals <= 0) ? 1 : decimals * 10;

    if (countunit != "years") {
        if (rounding == "rounddown")
            return_value = Math.floor((today.getTime() - pastdate.getTime()) / (finalunit) * decimals) / decimals;
        else
            return_value = Math.ceil((today.getTime() - pastdate.getTime()) / (finalunit) * decimals) / decimals;
    } else {
        yearspast = today.getFullYear() - yr - 1;
        tail = (today.getMonth() > mon - 1 || today.getMonth() == mon - 1 && today.getDate() >= day) ? 1 : 0;
        return_value = yearspast + tail;
    }

    return return_value;
}

//与WebCommon中的对应
var JS_EASYUI_SPLIT_CHAT = ";";
var JS_EASYUI_ADV_SPLIT_CHAT = "|";

//web service 调用
/*
$.ajaxWebService(

"WebService1.asmx/TxMsg",

valueStr,
function () { $('#Text2').val("loading..."); },
function (result) { $('#Text2').val(result.d); },
function (result, status) { if (status == 'error') { alert(status); } }

);

*/

$.ajaxWebService = function (url, dataMap, fnBefore, fnSuccess, fnError) {
    $.ajax({
        type: "POST",    //访问WebService使用Post方式请求
        contentType: "application/json",   //WebService 返回Json类型
        url: url,   //调用WebService的地址和方法名称组合 ---- *.asmx/方法名
        data: dataMap,   //要提交传递的参数，格式为 data: "{paraName1:paraValue1 , paraName2:paraValue2 , ......}"
        dataType: "json",  //传输的数据类型
        beforeSend: fnBefore,  //返回结果前的执行的函数， 例如 loading 或 waiting 视图
        success: fnSuccess,  //返回后，页面执行结果的函数
        error: fnError  //返回错误后，页面执行的函数
    });
}
function SubmitFormWithOutValidate(command) {

    if ($("#hiddensubmit").length == 0) {
        if ($('#Target').length == 0) {
            $("form:first").attr("id", "Target");
        }

        $('#Target').append("<span style='display:none;'><input id='hiddensubmit' type='submit' " + (command ? ("value='" + command + "'") : "")
            + "  /></span>");
    }

    if (command) {
        if ($("#Command").length == 0) {
            $('#Target').append("<input type='hidden' id='Command' name='Command' value='' />");
            $("#Command").val(command);
        }
    }

    ShowLoading('提交中,请稍候...');

    $("#hiddensubmit").click();
}
function SubmitForm(command, breforeSubmit, validateInput, allowGobalValidate) {
    if (typeof (breforeSubmit) != "undefined" && breforeSubmit) {
        breforeSubmit();
    }
    if (typeof (window.gobalValidate) != "undefined" && (typeof (allowGobalValidate) == "undefined" || allowGobalValidate == true)) {
        var rst = window.gobalValidate();
        if (rst == false) {
            return;
        }
    }
    if ($("#hiddensubmit").length == 0) {
        if ($('#Target').length == 0) {
            $("form:first").attr("id", "Target");
        }

        $('#Target').append("<span style='display:none;'><input id='hiddensubmit' type='submit' " + (command ? ("value='" + command + "'") : "")
            + "  /></span>");
    }

    if (command) {
        if ($("#Command").length == 0) {
            $('#Target').append("<input type='hidden' id='Command' name='Command' value='' />");
            $("#Command").val(command);
        }
    }

    var flag = true;
    if (typeof (validateInput) == "undefined")
        validateInput = true;

    //自动保存editgrid
    var grids = $("table[id]");
    for (var i = 0; i < grids.length; i++) {
        try {
            $(grids[i]).edatagrid("saveRow");
        } catch (e) {

        }
    }

    if (validateInput) {

        flag = localvalidateBizForm();

        if ($(this).form('validate') && flag) {
            ShowLoading('提交中,请稍候...');
            $("#hiddensubmit").click();
        }
    }
    else {
        $(".easyui-validatebox").each(function () {
            $(this).removeAttr("required");
        });

        ShowLoading('提交中,请稍候...');

        $("#hiddensubmit").click();
    }
}
function validateForm() {
    var vboxes = $("input.easyui-validatebox:visible,select.easyui-validatebox:visible,textarea.easyui-validatebox:visible");
    for (var i = 0; i < vboxes.length; i++) {
        if (!$(vboxes[i]).validatebox("isValid"))
            return false;
    }
}

function localvalidateBizForm() {
    var flag = true;
    var validateBoxs = $(".easyui-validatebox,.easyui-numberbox");
    var scrolldiv;

    for (var i = 0; i < validateBoxs.length; i++) {
        var obj = $(validateBoxs[i]);
        //console.log( obj.attr("id"));
        if (obj.hasClass('easyui-numberbox')){
            try{
              if(!$("#" + obj.attr("id")).numberbox("isValid"))
              {    flag = false;
           
                if (!scrolldiv)
                    scrolldiv = validateBoxs[i];
               }
            }
            catch(e){}
        }
        else if(!obj.validatebox("isValid")) {
            flag = false;
            if (!scrolldiv)
                scrolldiv = validateBoxs[i];
            //$("div").scrollTo(obj);
            //break;
        }
    }

    //滚动到第一个问题出现框
    if (scrolldiv) {
        $(".CenterForm").scrollTop($(scrolldiv).offset().top)
        setTimeout(function () { $(scrolldiv).parent().fadeOut(500).fadeIn(500).fadeOut(500).fadeIn(500); }, 100)
    }

    return flag;
}

//文档加载完成时
$(document).ready(function () {

    if ($('#Target').length == 0) {
        $("form:first").attr("id", "Target");
    }

    if (typeof ($.fn.validatebox) != "undefined") {
        //Email验证
        $.extend($.fn.validatebox.defaults.rules, {
            EmailValid: {
                validator: function (value) {
                    return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(value);
                },
                message: "Email格式错误"
            }
        });

        //日期格式验证
        $.extend($.fn.validatebox.defaults.rules, {
            DataValid: {
                validator: function (value) {
                    if (value == "")
                        return true;
                    else
                        return !isNaN(new Date(value.replace(/-/g, "/")));
                },
                message: "日期格式不正确<br/>应如:2010-12-31"
            }
        });
    }

    //文本框获得焦点时内容全选
    $("input[type='text'], input[type='password']").focus(function () {
        this.select();
    }).blur(function () {
    });

    //处理键盘事件 禁止后退键（Backspace）密码或单行、多行文本框除外 
    function banBackSpace(e) {
        //alert(event.keyCode)
        var ev = e || window.event;//获取event对象   
        var obj = ev.target || ev.srcElement;//获取事件源     
        var t = obj.type || obj.getAttribute('type');//获取事件源类型     
        //获取作为判断条件的事件类型 
        var vReadOnly = obj.readOnly;
        var vDisabled = obj.disabled;
        //处理undefined值情况 
        vReadOnly = (vReadOnly == undefined) ? false : vReadOnly;
        vDisabled = (vDisabled == undefined) ? true : vDisabled;
        if (ev.keyCode == 13) {
            var submitBtn = document.getElementById("btnSubmit");
            if (typeof(submitBtn) != "undefined" && t != "textarea")
                submitBtn.click();
        } else if(ev.keyCode == 8){
            //当敲Backspace键时，事件源类型为密码或单行、多行文本的，  
            //并且readOnly属性为true或disabled属性为true的，则退格键失效  
            var flag1 = (t == "password" || t == "text" || t == "textarea") && (vReadOnly == true || vDisabled == true);
            //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效    
            var flag2 = t != "password" && t != "text" && t != "textarea";
            //判断    
            if (flag2 || flag1) {
                if (typeof(event) != "undefined" && event)
                    event.returnValue = false;//这里如果写 return false 无法实现效果 
                else
                    return false;

            }
        }
    }

    //禁止退格键 作用于Firefox、Opera 
    document.onkeypress = banBackSpace;
    //禁止退格键 作用于IE、Chrome
    document.onkeydown = banBackSpace;

    window.history.forward(1);
})

function queryData() {

}

//re:是否是刷新
function reloadGrid(re) {

    var params = getQueryDataParams();

    //非刷新而是重新查询数据，返回第一页数据
    if (!re) {
        $('#DataGrid').datagrid("load", params); //
    } //刷新数据，返回当前页数据
    else {
        $('#DataGrid').datagrid("reload", params); //
    }
}

function getGridSelections(id, code) {
    var ids = [];
    var rows = $('#' + id.replace(/#/, "") + '').datagrid('getSelections');
    if (rows.length == 0)
        rows = $('#' + id.replace(/#/, "") + '').treegrid('getSelections');

    for (var i = 0; i < rows.length; i++) {
        ids.push(getSelectRowField(rows, code, i));
    }
    return ids.join(',')
}

function getGridSelectRows(id) {
    var rows = $('#' + id.replace(/#/, "") + '').datagrid('getSelections');
    if (rows.length == 0)
        rows = $('#' + id.replace(/#/, "") + '').treegrid('getSelections');
    return rows;
}

function getSelectRowField(rows, code, idx) {
    if (typeof (idx) == "undefined" || idx == null) idx = rows.length - 1;
    var codes = code.split(',');
    var rstAry = [];

    for (var j = 0; j < codes.length; j++) {
        for (var col in rows[idx]) {
            if ($.trim(codes[j]) == $.trim(col)) {

                rstAry.push(rows[idx][col]);
            }
        }
    }
    var rst = rstAry.join(">>");
    return rst;
}
function getGridSelection(id, code) {
    var id = id.replace(/#/, "");
    rows = $('#' + id.replace(/#/, "") + '').datagrid('getSelections');
    if (rows.length == 0)
        rows = $('#' + id.replace(/#/, "") + '').treegrid('getSelections');
    if (!rows.length)
        return "";
    else {
        return getSelectRowField(rows, code);
    }
}

function deleteItems(paras, url, callback) {
    $.ajax({
        url: url,
        data: paras,
        type: "POST",
        success: function (msg) {
            parseMessage(msg, callback);
        }
    });
}

function getTitle() {
    var t_titles = $("title");
    if (t_titles && t_titles.length > 0) {
        return $(t_titles[0]).html().replace(/\s+/g, "");
    } else {
        return "";
    }
}

//JSON返回日期格式 转换
function DateHandler(value) {
    if (value != undefined)
        return value.substring(0, 19).replace('T', '&nbsp;');
    else
        return "";
}
//JSON返回日期格式 转换
function TimeHandler(value) {
    if (value != undefined)
        return value.substring(10, 19).replace('T', '');
    else
        return "";
}
//JSON返回日期格式 转换 去秒
function TimeHHMMHandler(value) {
    if (value != undefined)
        return value.substring(10, 16).replace('T', '');
    else
        return "";
}
//JSON返回日期格式 转换
function ShortDateHandler(value) {
    if (value != undefined)
        return value.substring(0, 10);
    else
        return "";
}

//JSON返回日期格式 转换
function ShortYYDateHandler(value) {
    if (value != undefined)
        return value.substring(2, 10);
    else
        return "";
}

//JSON返回日期格式 转换 
function ShortFrameHandler(value) {
    if (value != undefined)
        return value.substring(2, 10).replace('-', '/').replace('-', '/');
    else
        return "";
}

function RightStr(value) {
    if (value != undefined)
        return value.substring(value.length - 2, value.length);
    else
        return "";
}

function RightStrByIn(value, InL) {
    if (value != undefined)
        return value.substring(value.length - InL, value.length);
    else
        return "";
}

//字符串转json
function strToJson(obj) {
    if (typeof (obj) == "string")
        return obj ? eval('(' + obj.replace(/\\"/g, "") + ')') : null;
    else if (typeof (obj) == "undefined")
        return null;
    else
        return obj;
}

function jsonToString(obj) {
    var rst = "{";
    for (var a in obj) {
        rst += "'" + a + "':" + "'" + obj[a] + "',";
    }

    return rst.substring(0, rst.length - 1) + "}";
}
//json 转 字符串
function jsonToStr(obj) {
    if (!obj)
        return "";

    switch (typeof (obj)) {
        case 'string':
            return '"' + obj.replace(/(["\\])/g, '\\$1') + '"';
        case 'array':
            return '[' + obj.map(jsonToString).join(',') + ']';
        case 'object':
            if (obj instanceof Array) {
                var strArr = [];
                var len = obj.length;
                for (var i = 0; i < len; i++) {
                    strArr.push(JSON.stringify(obj[i]));
                }
                return '[' + strArr.join(',') + ']';
            } else if (obj == null) {
                return 'null';

            } else {
                var string = [];
                for (var property in obj) string.push(jsonToString(property) + ':' + jsonToString(obj[property]));
                return '{' + string.join(',') + '}';
            }
        case 'number':
            return obj;
        case false:
            return obj;
    }
}

//获取EasyUI Tree所选择的ID值
function getTreeSelections(treeId) {
    var ids = "";
    var nodes = $(treeId).tree("getChecked");

    for (var i = 0; i < nodes.length; i++) {
        ids += nodes[i].id.toString() + JS_EASYUI_SPLIT_CHAT;
    }

    return ids;
}

function findGridRecord(dataGridId, keyName, keyValue) {
    var rows = $(dataGridId).datagrid("getRows");
    var row = null;

    for (var i = 0; i < rows.length; i++) {
        if (rows[i][keyName].toString() == keyValue) {
            row = rows[i];
            break;
        }
    }

    return row;
}

function findGridIndex(dataGridId, keyName, keyValue) {
    var rows = $(dataGridId).datagrid("getRows");
    var index = -1;

    for (var i = 0; i < rows.length; i++) {
        if (rows[i][keyName].toString() == keyValue) {
            index = i;
            break;
        }
    }

    return index;
}



function GetText(obj) {
    return (obj.innerText != undefined) ? obj.innerText : obj.textContent;
}

//与首页一致的提示框 主要是考虑控件在无首页时使用
function MsgAlert(title, alertString, alertType, fn, navurl) {
    $.messager.alert(title, alertString, alertType, fn);
}

function MsgAlertAndNav(title, alertString, navurl) {
    $.messager.alert(title, alertString);
    setTimeout(function () { window.location.href = navurl; }, 200);
}


function ShowError(title, alertString, actionRender) {
    if (actionRender)
        alertString += actionRender();
    MsgAlert(title, alertString, 'error')
}

function ShowErrorAndNav(title, alertString, navurl) {
    $.messager.alert(title, alertString, 'error');
    setTimeout(function () { window.location.href = navurl; }, 200);
}


//与首页一致的Show 主要是考虑控件在无首页时使用
function MsgShow(title, msgString, timeOut, msgType) {
    $.messager.show({ title: title, msg: msgString, timeout: timeOut, showType: msgType });
}

//EasyUI日期控件格式化字符串转换为Date日期对象
//$.fn.datebox.defaults.parser = function(dateStr) {
//if (dateStr) {
//return new Date(dateStr.replace(/-/g, "/"))
//} else {
//return new Date();
//}
//};

////EasyUI日期控件Date日期对象转换为格式化字符串
//$.fn.datebox.defaults.formatter = function(date) {
//return TimeFormatter(date);
//};

//设置EasyUI Grid列头居中。内容靠左对齐（如果需设置多列，则用','隔开）
function CellAlignLeft(columnNames) {
    var arr = columnNames.split(",");
    $(".datagrid-header-inner table tr").find("td").each(function () {
        for (var i = 0; i < arr.length; i++) {
            if ($.trim($(this).attr("field")) == $.trim(arr[i])) {
                var child = $(this).find(".datagrid-cell").first();
                child.css("text-align", "center");
            }
        }
    })
}

function SlashFilter(value) {
    return value.replace(/\\/g, "\\\\");
}

//将\替换成\\
String.prototype.SlashFilter = function () {
    return this.replace(/\\/g, "\\\\");
};


//日期格式化
function DateFormatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + "-" + AttachZero(m) + "-" + AttachZero(d);
}

//时间格式化
function TimeFormatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    var h = date.getHours();
    var mm = date.getMinutes();
    var s = date.getSeconds();
    return y + "-" + AttachZero(m) + "-" + AttachZero(d) + " " + AttachZero(h) + ":" + AttachZero(mm) + ":" + AttachZero(s);
}

//日期补0
function AttachZero(intValue) {
    return (intValue < 10 ? "0" + intValue : intValue);
}

//根据文档宽度获取比例宽度
function GetWidth(percent) {
    return Math.round(document.body.clientWidth * percent) + 2;
}

/*-----------------------------分页中英文BEGIN----------------------------------*/
if ($.fn.pagination) {
    $.fn.pagination.defaults.beforePageText = '';
    $.fn.pagination.defaults.afterPageText = '/{pages}';
    $.fn.pagination.defaults.displayMsg = '共{total}记录';
    //$.fn.pagination.defaults.displayMsg = '';
    //$.fn.pagination.defaults.showPageList = false;
    //$.fn.pagination.defaults.showRefresh = true;
}

if ($.fn.datagrid) {
    $.fn.datagrid.defaults.loadMsg = '正在处理，请稍待。。。';
}

if ($.messager) {
    $.messager.defaults.ok = '确定';
    $.messager.defaults.cancel = '取消';
}

if ($.fn.validatebox) {
    $.fn.validatebox.defaults.missingMessage = '该项为必填项';
    $.fn.validatebox.defaults.rules.email.message = '请输入有效的电子邮件地址';
    $.fn.validatebox.defaults.rules.url.message = '请输入有效的URL地址';
    $.fn.validatebox.defaults.rules.length.message = '输入内容长度必须介于{0}和{1}之间';
}
if ($.fn.numberspinner) {
    $.fn.numberspinner.defaults.missingMessage = '该项为必填项';
}
if ($.fn.numberbox) {
    $.fn.numberbox.defaults.missingMessage = '该项为必填项';
}

if ($.fn.combobox) {
    $.fn.combobox.defaults.missingMessage = '该项为必填项';
}

if ($.fn.combotree) {
    $.fn.combotree.defaults.missingMessage = '该项为必填项';
}

if ($.fn.calendar) {
    $.fn.calendar.defaults.weeks = ['日', '一', '二', '三', '四', '五', '六'];
    $.fn.calendar.defaults.months = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
}

if ($.fn.datebox) {
    $.fn.datebox.defaults.currentText = '今天';
    $.fn.datebox.defaults.closeText = '关闭';
    $.fn.datebox.defaults.missingMessage = '该项为必填项';
}
/*-----------------------------中英文END----------------------------------*/

//打开EasyUI模态对话框 obj为jquery对象，是对话框的容器
//function OpenModalDialog(w, h, obj, saveText, saveFunc, cancelText, cancelFunc) {
//    obj.css("display", "block");
//    obj.dialog({
//        closed: false,
//        showType: null,
//        width: w,
//        height: h,
//        modal: true,
//        buttons: [{
//            text: saveText,
//            iconCls: 'icon-save',
//            handler: saveFunc
//        }, {
//            text: cancelText,
//            iconCls: 'icon-cancel',
//            handler: cancelFunc
//        }]
//    });
//}

function CreateFrame(url) {
    var ran = (HasQueryString(url) ? '&r=' : '?r=') + Math.random();
    return '<iframe scrolling="auto" frameborder="0"  src="' + url + ran + '" style="width:100%;height:100%;"></iframe>';
}

function HasQueryString(url) {
    return (url.indexOf('?') > -1);
}

//两种方式解决Frame中弹出新Frame窗口的进度条问题(经测试，Frame中创建新Frame没有问题，问题在创建新Frame同时弹出到窗口)
//1.弹出的新Frame中加上onload="window.status='Finished';"
//2.使用window.setTimeout('',0)弹出框架
function SetWin(w, h, url, title, witht) {
    if (url.indexOf("?") > 0 && url.lastIndexOf("&") != 0)
        url += "&";
    else
        url += "?";

    if (true) {
        if (url.indexOf("t=") < 0)
            url += "t=" + new Date().getTime();
        else
            url.replace(/t=(\d)+/g, new Date().getTime());
    }

    var win = GetNewWin();

    $(win).find("iframe").each(function () {
        clearIframe(this, '读取中...');
    });

    if ($(win).html() == "")
        win.append(CreateFrame(url));
    else {
        $(win).find("iframe").attr("src", url);
    }
    eval("OpenWin(" + w + ", " + h + ", '" + title + "')");
}

function clearIframe(frame, text) {
    frameDoc = frame.contentDocument || frame.contentWindow.document;

    if (text != undefined && text != null && text != "" && frameDoc.documentElement != undefined)
        try {

            frameDoc.body.innerHTML = text;

        } catch (e) { }
    else
        frameDoc.removeChild(frameDoc.documentElement);
}
function GetNewWin() {
    var win = $('#CommonWin');

    //.easyui-layout部分浏览器打不开关不了
    if (win.length == 0) {
        $("body").append("<div id='CommonWin' collapsible='false' minimizable='false' maximizable='false' icon='icon-save' style='padding: 5px; background: #fafafa;'></div>");
        win = $('#CommonWin');
    }

    return win;
}

function OpenWin(w, h, title) {
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var win = $('#CommonWin');
    win.window({
        title: title,
        width: w,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - h) / 4),
        left: Math.max(0, (windowWidth - w) / 2),
        height: h,
        resizable: false
        //onClose: (fn == undefined) ? function () { } : fn
    });
    win.window('open');
}

function showMessage(title, message, url) {
    alert(message);
}


function CloseWin() {
    if ($('#CommonWin').length > 0)
        $('#CommonWin').window('close');
    else
        window.close();
}

function GetRealParent() {
    if (window.opener)
        return window.opener;

    var parent = window.parent;
    var trycount = 0;

    while (window.location.href == parent.location.href.replace(/#$/, "") && trycount <= 3) {
        parent = parent.parent;
        trycount++;
    }
    return parent;
}

function CloseTheWin() {
    if (window.opener) {
        window.close();
    }
    else {
        if (window.parent != null && window.parent.CloseWin != undefined)
            window.parent.CloseWin();
    }
}

function getParamsString(input) {
    var str = "";
    for (var a in input) {
        if (a && input[a])
            str += a + "=" + encodeURIComponent(input[a]) + "&";
    }

    return str.trim("&");
}

function SetBackFunc(fn) {
    window.fn = null;
    window.fn = fn;
}

function RunBackFunc() {
    if (window.fn != undefined) {
        fn();
    }
}
//将带有class=attachmentList的容器赋予附件名称
function InitAttachmentList(action) {

    var list = $("div.attchmentList");
    action = action ? action : "/Admin/Attachment/GetAttachmentList";
    for (var i = 0; i < list.length; i++) {

        $.ajax({
            url: action,
            data: { TargetID: $(list[i]).attr("targetid"), TargetType: $(list[i]).attr("targettype"), page: 1, rows: 100000, t: new Date() },
            type: "POST",
            success: function (msg) {
                var data = strToJson(msg);
                if (data.rows.length > 0) {
                    var dv = $("div.attchmentList[targetid='" + data.rows[0]["TargetID"] + "']");
                    for (var j = 0; j < data.rows.length; j++) {

                        $(dv).append("<a href='/Extenstion/DownloadAttachment.ashx?attachId=" + data.rows[j]["AttachmentID"] + "'>"
                            + data.rows[j].Title + "</a>");
                    }
                }
            }
        });
    }
}

//创建标记
function createAttachmentList(action, type, id) {
    return "<div class='attchmentList' targettype='" + type + "' targetid='" + id + "'></div>";
}
//删除不需要显示的按钮
function RemoveForbidButton() {
    $(".datagrid-toolbar > a").each(function () {
        if ($(this).find(".null").length > 0) {
            $(this).find(" + .datagrid-btn-separator").remove();
            $(this).remove();
        }
    });
}


//获取选择月对应的实际天数（也是本月的最后一天）
function getDates(year, month) {
    var d = new Date(year, month, 0).getDate();
    return d;
}


function numToChineseNum(Num) {
    if (Num == 0) {
        return "零元整";
    }
    Num = Num.toString();
    for (i = Num.length - 1; i >= 0; i--) {
        Num = Num.replace(",", "")//替换tomoney()中的“,”
        Num = Num.replace(" ", "")//替换tomoney()中的空格
    }
    Num = Num.replace("￥", "")//替换掉可能出现的￥字符
    if (isNaN(Num)) { //验证输入的字符是否为数字
        alert("请检查小写金额是否正确");
        return;
    }
    //---字符处理完毕，开始转换，转换采用前后两部分分别转换---//
    part = String(Num).split(".");
    newchar = "";
    //小数点前进行转化
    for (i = part[0].length - 1; i >= 0; i--) {
        if (part[0].length > 10) {
            alert("位数过大，无法计算");
            return "";
        } //若数量超过拾亿单位，提示
        tmpnewchar = ""
        perchar = part[0].charAt(i);
        switch (perchar) {
            case "0": tmpnewchar = "零" + tmpnewchar; break;
            case "1": tmpnewchar = "壹" + tmpnewchar; break;
            case "2": tmpnewchar = "贰" + tmpnewchar; break;
            case "3": tmpnewchar = "叁" + tmpnewchar; break;
            case "4": tmpnewchar = "肆" + tmpnewchar; break;
            case "5": tmpnewchar = "伍" + tmpnewchar; break;
            case "6": tmpnewchar = "陆" + tmpnewchar; break;
            case "7": tmpnewchar = "柒" + tmpnewchar; break;
            case "8": tmpnewchar = "捌" + tmpnewchar; break;
            case "9": tmpnewchar = "玖" + tmpnewchar; break;
        }
        switch (part[0].length - i - 1) {
            case 0: tmpnewchar = tmpnewchar + "元"; break;
            case 1: if (perchar != 0) tmpnewchar = tmpnewchar + "拾"; break;
            case 2: if (perchar != 0) tmpnewchar = tmpnewchar + "佰"; break;
            case 3: if (perchar != 0) tmpnewchar = tmpnewchar + "仟"; break;
            case 4: tmpnewchar = tmpnewchar + "万"; break;
            case 5: if (perchar != 0) tmpnewchar = tmpnewchar + "拾"; break;
            case 6: if (perchar != 0) tmpnewchar = tmpnewchar + "佰"; break;
            case 7: if (perchar != 0) tmpnewchar = tmpnewchar + "仟"; break;
            case 8: tmpnewchar = tmpnewchar + "亿"; break;
            case 9: tmpnewchar = tmpnewchar + "拾"; break;
        }
        newchar = tmpnewchar + newchar;
    } //for
    //小数点之后进行转化
    if (Num.indexOf(".") != -1) {
        if (part[1].length > 2) {
            alert("小数点之后只能保留两位,系统将自动截段");
            part[1] = part[1].substr(0, 2)
        }
        for (i = 0; i < part[1].length; i++) {//for2
            tmpnewchar = ""
            perchar = part[1].charAt(i)
            switch (perchar) {
                case "0": tmpnewchar = "零" + tmpnewchar; break;
                case "1": tmpnewchar = "壹" + tmpnewchar; break;
                case "2": tmpnewchar = "贰" + tmpnewchar; break;
                case "3": tmpnewchar = "叁" + tmpnewchar; break;
                case "4": tmpnewchar = "肆" + tmpnewchar; break;
                case "5": tmpnewchar = "伍" + tmpnewchar; break;
                case "6": tmpnewchar = "陆" + tmpnewchar; break;
                case "7": tmpnewchar = "柒" + tmpnewchar; break;
                case "8": tmpnewchar = "捌" + tmpnewchar; break;
                case "9": tmpnewchar = "玖" + tmpnewchar; break;
            }
            if (i == 0) tmpnewchar = tmpnewchar + "角";
            if (i == 1) tmpnewchar = tmpnewchar + "分";
            newchar = newchar + tmpnewchar;
        } //for2
    }
    //替换所有无用汉字
    while (newchar.search("零零") != -1)
        newchar = newchar.replace("零零", "零");
    newchar = newchar.replace("亿零万", "亿");
    newchar = newchar.replace("零亿", "亿");
    newchar = newchar.replace("亿万", "亿");
    newchar = newchar.replace("零万", "万");
    newchar = newchar.replace("零元", "元");
    newchar = newchar.replace("零角", "");
    newchar = newchar.replace("零分", "");

    if (newchar.charAt(newchar.length - 1) == "元" || newchar.charAt(newchar.length - 1) == "角")
        newchar = newchar + "整"
    return newchar;
}
var ymdhm_regex = /^(?:(?!0000)[0-9]{4}([-/.]?)(?:(?:0?[1-9]|1[0-2])([-/.]?)(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])([-/.]?)(?:29|30)|(?:0?[13578]|1[02])([-/.]?)31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-/.]?)0?2([-/.]?)29)(\s([0-1][0-9]|2[1-3])\:([0-5][0-9]))$/;
var decimal_regex = /^(0|[1-9]|[1-9][0-9]+)(\.[0-9]{2,4})?$/;
if ($.fn.validatebox) {
    // extend the 'equals' rule
    $.extend($.fn.validatebox.defaults.rules, {
        dateimeYMdHm: {
            validator: function (value, param) {
                return ymdhm_regex.test(value);
            },
            message: '格式必须符合yyyy-MM-dd HH:mm(年-月-日 时:分).'
        }
    });

    // extend the 'decimal' rule
    $.extend($.fn.validatebox.defaults.rules, {
        posDecimal: {
            validator: function (value, param) {
                return decimal_regex.test(value);
            },
            message: '格式必须符合yyyy-MM-dd HH:mm(年-月-日 时:分).'
        }
    });
}
if ($.fn.pagination) {
    $.fn.pagination.defaults.beforePageText = '第';
    $.fn.pagination.defaults.afterPageText = '共{pages}页';
    $.fn.pagination.defaults.displayMsg = '显示{from}到{to},共{total}记录';
}
if ($.fn.datagrid) {
    $.fn.datagrid.defaults.loadMsg = '正在处理，请稍待。。。';
}
if ($.fn.treegrid && $.fn.datagrid) {
    $.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager) {
    $.messager.defaults.ok = '确定';
    $.messager.defaults.cancel = '取消';
}
if ($.fn.validatebox) {
    $.fn.validatebox.defaults.missingMessage = '该项为必填项';
    $.fn.validatebox.defaults.rules.email.message = '请输入有效的电子邮件地址';
    $.fn.validatebox.defaults.rules.url.message = '请输入有效的URL地址';
    $.fn.validatebox.defaults.rules.length.message = '输入内容长度必须介于{0}和{1}之间';
    $.fn.validatebox.defaults.rules.remote.message = '请修正该字段';
}
if ($.fn.numberbox) {
    $.fn.numberbox.defaults.missingMessage = '该项为必填项';
}
if ($.fn.combobox) {
    $.fn.combobox.defaults.missingMessage = '该项为必填项';
}
if ($.fn.combotree) {
    $.fn.combotree.defaults.missingMessage = '该项为必填项';
}
if ($.fn.combogrid) {
    $.fn.combogrid.defaults.missingMessage = '该项为必填项';
}
if ($.fn.calendar) {
    $.fn.calendar.defaults.weeks = ['日', '一', '二', '三', '四', '五', '六'];
    $.fn.calendar.defaults.months = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
}
if ($.fn.datebox) {
    $.fn.datebox.defaults.currentText = '今天';
    $.fn.datebox.defaults.closeText = '关闭';
    $.fn.datebox.defaults.okText = '确定';
    $.fn.datebox.defaults.missingMessage = '该项为必填项';
    $.fn.datebox.defaults.formatter = function (date) {
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
    };
    $.fn.datebox.defaults.parser = function (s) {
        if (!s) return new Date();
        var ss = s.split('-');
        var y = parseInt(ss[0], 10);
        var m = parseInt(ss[1], 10);
        var d = parseInt(ss[2], 10);
        if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
            return new Date(y, m - 1, d);
        } else {
            return new Date();
        }
    };
}
if ($.fn.datetimebox && $.fn.datebox) {
    $.extend($.fn.datetimebox.defaults, {
        currentText: $.fn.datebox.defaults.currentText,
        closeText: $.fn.datebox.defaults.closeText,
        okText: $.fn.datebox.defaults.okText,
        missingMessage: $.fn.datebox.defaults.missingMessage
    });
}

/**
* Cookie plugin
*
* Copyright (c) 2006 Klaus Hartl (stilbuero.de)
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
*/
jQuery.cookie = function (B, I, L) { if (typeof I != "undefined") { L = L || {}; if (I === null) { I = ""; L.expires = -1 } var E = ""; if (L.expires && (typeof L.expires == "number" || L.expires.toUTCString)) { var F; if (typeof L.expires == "number") { F = new Date(); F.setTime(F.getTime() + (L.expires * 24 * 60 * 60 * 1000)) } else { F = L.expires } E = "; expires=" + F.toUTCString() } var K = L.path ? "; path=" + (L.path) : ""; var G = L.domain ? "; domain=" + (L.domain) : ""; var A = L.secure ? "; secure" : ""; document.cookie = [B, "=", encodeURIComponent(I), E, K, G, A].join("") } else { var D = null; if (document.cookie && document.cookie != "") { var J = document.cookie.split(";"); for (var H = 0; H < J.length; H++) { var C = jQuery.trim(J[H]); if (C.substring(0, B.length + 1) == (B + "=")) { D = decodeURIComponent(C.substring(B.length + 1)); break } } } return D } };

(function ($) { $.fn.wresize = function (f) { version = "1.1"; wresize = { fired: false, width: 0 }; function resizeOnce() { if ($.browser.msie) { if (!wresize.fired) { wresize.fired = true } else { var version = parseInt($.browser.version, 10); wresize.fired = false; if (version < 7) { return false } else { if (version == 7) { var width = $(window).width(); if (width != wresize.width) { wresize.width = width; return false } } } } } return true } function handleWResize(e) { if (resizeOnce()) { return f.apply(this, [e]) } } this.each(function () { if (this == window) { $(this).resize(handleWResize) } else { $(this).resize(f) } }); return this } })(jQuery);

/*
* jQuery.ScrollTo
* Copyright (c) 2007-2009 Ariel Flesler - aflesler(at)gmail(dot)com | http://flesler.blogspot.com
* Dual licensed under MIT and GPL.
* Date: 06/05/2009
*
* @projectDescription Easy element scrolling using jQuery.
* http://flesler.blogspot.com/2007/10/jqueryscrollto.html
* Works with jQuery +1.2.6. Tested on FF 2/3, IE 6/7/8, Opera 9.5/6, Safari 3, Chrome 1 on WinXP.
*
* @author Ariel Flesler
* @version 1.4.2
*
* @id jQuery.scrollTo
* @id jQuery.fn.scrollTo
* @param {String, Number, DOMElement, jQuery, Object} target Where to scroll the matched elements.
*        The different options for target are:
*              - A number position (will be applied to all axes).
*              - A string position ('44', '100px', '+=90', etc ) will be applied to all axes
*              - A jQuery/DOM element ( logically, child of the element to scroll )
*              - A string selector, that will be relative to the element to scroll ( 'li:eq(2)', etc )
*              - A hash { top:x, left:y }, x and y can be any kind of number/string like above.
*              - A percentage of the container's dimension/s, for example: 50% to go to the middle.
*              - The string 'max' for go-to-end.
* @param {Number, Function} duration The OVERALL length of the animation, this argument can be the settings object instead.
* @param {Object,Function} settings Optional set of settings or the onAfter callback.
*       @option {String} axis Which axis must be scrolled, use 'x', 'y', 'xy' or 'yx'.
*       @option {Number, Function} duration The OVERALL length of the animation.
*       @option {String} easing The easing method for the animation.
*       @option {Boolean} margin If true, the margin of the target element will be deducted from the final position.
*       @option {Object, Number} offset Add/deduct from the end position. One number for both axes or { top:x, left:y }.
*       @option {Object, Number} over Add/deduct the height/width multiplied by 'over', can be { top:x, left:y } when using both axes.
*       @option {Boolean} queue If true, and both axis are given, the 2nd axis will only be animated after the first one ends.
*       @option {Function} onAfter Function to be called after the scrolling ends.
*       @option {Function} onAfterFirst If queuing is activated, this function will be called after the first scrolling ends.
* @return {jQuery} Returns the same jQuery object, for chaining.
*
* @desc Scroll to a fixed position
* @example $('div').scrollTo( 340 );
*
* @desc Scroll relatively to the actual position
* @example $('div').scrollTo( '+=340px', { axis:'y' } );
*
* @desc Scroll using a selector (relative to the scrolled element)
* @example $('div').scrollTo( 'p.paragraph:eq(2)', 500, { easing:'swing', queue:true, axis:'xy' } );
*
* @desc Scroll to a DOM element (same for jQuery object)
* @example var second_child = document.getElementById('container').firstChild.nextSibling;
*                      $('#container').scrollTo( second_child, { duration:500, axis:'x', onAfter:function(){
*                              alert('scrolled!!');                                                                                                                              
*                      }});
*
* @desc Scroll on both axes, to different values
* @example $('div').scrollTo( { top: 300, left:'+=200' }, { axis:'xy', offset:-20 } );
*/
(function ($) { var $scrollTo = $.scrollTo = function (target, duration, settings) { $(window).scrollTo(target, duration, settings) }; $scrollTo.defaults = { axis: "xy", duration: parseFloat($.fn.jquery) >= 1.3 ? 0 : 1, limit: true }; $scrollTo.window = function (scope) { return $(window)._scrollable() }; $.fn._scrollable = function () { return this.map(function () { var elem = this, isWin = !elem.nodeName || $.inArray(elem.nodeName.toLowerCase(), ["iframe", "#document", "html", "body"]) != -1; if (!isWin) { return elem } var doc = (elem.contentWindow || elem).document || elem.ownerDocument || elem; return $.browser.safari || doc.compatMode == "BackCompat" ? doc.body : doc.documentElement }) }; $.fn.scrollTo = function (target, duration, settings) { if (typeof duration == "object") { settings = duration; duration = 0 } if (typeof settings == "function") { settings = { onAfter: settings } } if (target == "max") { target = 9000000000 } settings = $.extend({}, $scrollTo.defaults, settings); duration = duration || settings.duration; settings.queue = settings.queue && settings.axis.length > 1; if (settings.queue) { duration /= 2 } settings.offset = both(settings.offset); settings.over = both(settings.over); return this._scrollable().each(function () { var elem = this, $elem = $(elem), targ = target, toff, attr = {}, win = $elem.is("html,body"); switch (typeof targ) { case "number": case "string": if (/^([+-]=)?\d+(\.\d+)?(px|%)?$/.test(targ)) { targ = both(targ); break } targ = $(targ, this); case "object": if (targ.is || targ.style) { toff = (targ = $(targ)).offset() } } $.each(settings.axis.split(""), function (i, axis) { var Pos = axis == "x" ? "Left" : "Top", pos = Pos.toLowerCase(), key = "scroll" + Pos, old = elem[key], max = $scrollTo.max(elem, axis); if (toff) { attr[key] = toff[pos] + (win ? 0 : old - $elem.offset()[pos]); if (settings.margin) { attr[key] -= parseInt(targ.css("margin" + Pos)) || 0; attr[key] -= parseInt(targ.css("border" + Pos + "Width")) || 0 } attr[key] += settings.offset[pos] || 0; if (settings.over[pos]) { attr[key] += targ[axis == "x" ? "width" : "height"]() * settings.over[pos] } } else { var val = targ[pos]; attr[key] = val.slice && val.slice(-1) == "%" ? parseFloat(val) / 100 * max : val } if (settings.limit && /^\d+$/.test(attr[key])) { attr[key] = attr[key] <= 0 ? 0 : Math.min(attr[key], max) } if (!i && settings.queue) { if (old != attr[key]) { animate(settings.onAfterFirst) } delete attr[key] } }); animate(settings.onAfter); function animate(callback) { $elem.animate(attr, duration, settings.easing, callback && function () { callback.call(this, target, settings) }) } }).end() }; $scrollTo.max = function (elem, axis) { var Dim = axis == "x" ? "Width" : "Height", scroll = "scroll" + Dim; if (!$(elem).is("html,body")) { return elem[scroll] - $(elem)[Dim.toLowerCase()]() } var size = "client" + Dim, html = elem.ownerDocument.documentElement, body = elem.ownerDocument.body; return Math.max(html[scroll], body[scroll]) - Math.min(html[size], body[size]) }; function both(val) { return typeof val == "object" ? val : { top: val, left: val } } })(jQuery);
// Simple Set Clipboard System
// Author: Joseph Huckaby

var ZeroClipboard = { version: "1.0.6", clients: {}, moviePath: "/Scripts/ZeroClipboard.swf", nextId: 1, $: function (thingy) { if (typeof (thingy) == "string") { thingy = document.getElementById(thingy) } if (!thingy.addClass) { thingy.hide = function () { this.style.display = "none" }; thingy.show = function () { this.style.display = "" }; thingy.addClass = function (name) { this.removeClass(name); this.className += " " + name }; thingy.removeClass = function (name) { var classes = this.className.split(/\s+/); var idx = -1; for (var k = 0; k < classes.length; k++) { if (classes[k] == name) { idx = k; k = classes.length } } if (idx > -1) { classes.splice(idx, 1); this.className = classes.join(" ") } return this }; thingy.hasClass = function (name) { return !!this.className.match(new RegExp("\\s*" + name + "\\s*")) } } return thingy }, setMoviePath: function (path) { this.moviePath = path }, dispatch: function (id, eventName, args) { var client = this.clients[id]; if (client) { client.receiveEvent(eventName, args) } }, register: function (id, client) { this.clients[id] = client }, getDOMObjectPosition: function (obj, stopObj) { var info = { left: 0, top: 0, width: obj.width ? obj.width : obj.offsetWidth, height: obj.height ? obj.height : obj.offsetHeight }; while (obj && (obj != stopObj)) { info.left += obj.offsetLeft; info.top += obj.offsetTop; obj = obj.offsetParent } return info }, Client: function (elem) { this.handlers = {}; this.id = ZeroClipboard.nextId++; this.movieId = "ZeroClipboardMovie_" + this.id; ZeroClipboard.register(this.id, this); if (elem) { this.glue(elem) } } }; ZeroClipboard.Client.prototype = { id: 0, ready: false, movie: null, clipText: "", handCursorEnabled: true, cssEffects: true, handlers: null, glue: function (elem, appendElem, stylesToAdd) { this.domElement = ZeroClipboard.$(elem); var zIndex = 99; if (this.domElement.style.zIndex) { zIndex = parseInt(this.domElement.style.zIndex, 10) + 1 } if (typeof (appendElem) == "string") { appendElem = ZeroClipboard.$(appendElem) } else { if (typeof (appendElem) == "undefined") { appendElem = document.getElementsByTagName("body")[0] } } var box = ZeroClipboard.getDOMObjectPosition(this.domElement, appendElem); this.div = document.createElement("div"); var style = this.div.style; style.position = "absolute"; style.left = "" + box.left + "px"; style.top = "" + box.top + "px"; style.width = "" + box.width + "px"; style.height = "" + box.height + "px"; style.zIndex = zIndex; if (typeof (stylesToAdd) == "object") { for (addedStyle in stylesToAdd) { style[addedStyle] = stylesToAdd[addedStyle] } } appendElem.appendChild(this.div); this.div.innerHTML = this.getHTML(box.width, box.height) }, getHTML: function (width, height) { var html = ""; var flashvars = "id=" + this.id + "&width=" + width + "&height=" + height; if (navigator.userAgent.match(/MSIE/)) { var protocol = location.href.match(/^https/i) ? "https://" : "http://"; html += '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="' + protocol + 'download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" width="' + width + '" height="' + height + '" id="' + this.movieId + '" align="middle"><param name="allowScriptAccess" value="always" /><param name="allowFullScreen" value="false" /><param name="movie" value="' + ZeroClipboard.moviePath + '" /><param name="loop" value="false" /><param name="menu" value="false" /><param name="quality" value="best" /><param name="bgcolor" value="#ffffff" /><param name="flashvars" value="' + flashvars + '"/><param name="wmode" value="transparent"/></object>' } else { html += '<embed id="' + this.movieId + '" src="' + ZeroClipboard.moviePath + '" loop="false" menu="false" quality="best" bgcolor="#ffffff" width="' + width + '" height="' + height + '" name="' + this.movieId + '" align="middle" allowScriptAccess="always" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="' + flashvars + '" wmode="transparent" />' } return html }, hide: function () { if (this.div) { this.div.style.left = "-2000px" } }, show: function () { this.reposition() }, destroy: function () { if (this.domElement && this.div) { this.hide(); this.div.innerHTML = ""; var body = document.getElementsByTagName("body")[0]; try { body.removeChild(this.div) } catch (e) { } this.domElement = null; this.div = null } }, reposition: function (elem) { if (elem) { this.domElement = ZeroClipboard.$(elem); if (!this.domElement) { this.hide() } } if (this.domElement && this.div) { var box = ZeroClipboard.getDOMObjectPosition(this.domElement); var style = this.div.style; style.left = "" + box.left + "px"; style.top = "" + box.top + "px" } }, setText: function (newText) { this.clipText = newText; if (this.ready) { this.movie.setText(newText) } }, addEventListener: function (eventName, func) { eventName = eventName.toString().toLowerCase().replace(/^on/, ""); if (!this.handlers[eventName]) { this.handlers[eventName] = [] } this.handlers[eventName].push(func) }, setHandCursor: function (enabled) { this.handCursorEnabled = enabled; if (this.ready) { this.movie.setHandCursor(enabled) } }, setCSSEffects: function (enabled) { this.cssEffects = !!enabled }, receiveEvent: function (eventName, args) { eventName = eventName.toString().toLowerCase().replace(/^on/, ""); switch (eventName) { case "load": this.movie = document.getElementById(this.movieId); if (!this.movie) { var self = this; setTimeout(function () { self.receiveEvent("load", null) }, 1); return } if (!this.ready && navigator.userAgent.match(/Firefox/) && navigator.userAgent.match(/Windows/)) { var self = this; setTimeout(function () { self.receiveEvent("load", null) }, 100); this.ready = true; return } this.ready = true; this.movie.setText(this.clipText); this.movie.setHandCursor(this.handCursorEnabled); break; case "mouseover": if (this.domElement && this.cssEffects) { this.domElement.addClass("hover"); if (this.recoverActive) { this.domElement.addClass("active") } } break; case "mouseout": if (this.domElement && this.cssEffects) { this.recoverActive = false; if (this.domElement.hasClass("active")) { this.domElement.removeClass("active"); this.recoverActive = true } this.domElement.removeClass("hover") } break; case "mousedown": if (this.domElement && this.cssEffects) { this.domElement.addClass("active") } break; case "mouseup": if (this.domElement && this.cssEffects) { this.domElement.removeClass("active"); this.recoverActive = false } break } if (this.handlers[eventName]) { for (var idx = 0, len = this.handlers[eventName].length; idx < len; idx++) { var func = this.handlers[eventName][idx]; if (typeof (func) == "function") { func(this, args) } else { if ((typeof (func) == "object") && (func.length == 2)) { func[0][func[1]](this, args) } else { if (typeof (func) == "string") { window[func](this, args) } } } } } } };

var clip = null;

function initGobalClip() {
    $("body").append("<div id='d_clip_button' style='display:none;'/>")
    clip = new ZeroClipboard.Client();
    clip.setHandCursor(true);
    clip.glue("d_clip_button");
    clip.show();
    $("input,textarea").bind({
        copy: function () {
            clip.setText($(this).val());
        }
    });
}
function clipData() {
    clip.setText($(this).val());
}

//json2 lib
var JSON;if(!JSON){JSON={}}(function(){function f(n){return n<10?"0"+n:n}if(typeof Date.prototype.toJSON!=="function"){Date.prototype.toJSON=function(key){return isFinite(this.valueOf())?this.getUTCFullYear()+"-"+f(this.getUTCMonth()+1)+"-"+f(this.getUTCDate())+"T"+f(this.getUTCHours())+":"+f(this.getUTCMinutes())+":"+f(this.getUTCSeconds())+"Z":null};String.prototype.toJSON=Number.prototype.toJSON=Boolean.prototype.toJSON=function(key){return this.valueOf()}}var cx=/[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,escapable=/[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,gap,indent,meta={"\b":"\\b","\t":"\\t","\n":"\\n","\f":"\\f","\r":"\\r",'"':'\\"',"\\":"\\\\"},rep;function quote(string){escapable.lastIndex=0;return escapable.test(string)?'"'+string.replace(escapable,function(a){var c=meta[a];return typeof c==="string"?c:"\\u"+("0000"+a.charCodeAt(0).toString(16)).slice(-4)})+'"':'"'+string+'"'}function str(key,holder){var i,k,v,length,mind=gap,partial,value=holder[key];if(value&&typeof value==="object"&&typeof value.toJSON==="function"){value=value.toJSON(key)}if(typeof rep==="function"){value=rep.call(holder,key,value)}switch(typeof value){case"string":return quote(value);case"number":return isFinite(value)?String(value):"null";case"boolean":case"null":return String(value);case"object":if(!value){return"null"}gap+=indent;partial=[];if(Object.prototype.toString.apply(value)==="[object Array]"){length=value.length;for(i=0;i<length;i+=1){partial[i]=str(i,value)||"null"}v=partial.length===0?"[]":gap?"[\n"+gap+partial.join(",\n"+gap)+"\n"+mind+"]":"["+partial.join(",")+"]";gap=mind;return v}if(rep&&typeof rep==="object"){length=rep.length;for(i=0;i<length;i+=1){if(typeof rep[i]==="string"){k=rep[i];v=str(k,value);if(v){partial.push(quote(k)+(gap?": ":":")+v)}}}}else{for(k in value){if(Object.prototype.hasOwnProperty.call(value,k)){v=str(k,value);if(v){partial.push(quote(k)+(gap?": ":":")+v)}}}}v=partial.length===0?"{}":gap?"{\n"+gap+partial.join(",\n"+gap)+"\n"+mind+"}":"{"+partial.join(",")+"}";gap=mind;return v}}if(typeof JSON.stringify!=="function"){JSON.stringify=function(value,replacer,space){var i;gap="";indent="";if(typeof space==="number"){for(i=0;i<space;i+=1){indent+=" "}}else{if(typeof space==="string"){indent=space}}rep=replacer;if(replacer&&typeof replacer!=="function"&&(typeof replacer!=="object"||typeof replacer.length!=="number")){throw new Error("JSON.stringify")}return str("",{"":value})}}if(typeof JSON.parse!=="function"){JSON.parse=function(text,reviver){var j;function walk(holder,key){var k,v,value=holder[key];if(value&&typeof value==="object"){for(k in value){if(Object.prototype.hasOwnProperty.call(value,k)){v=walk(value,k);if(v!==undefined){value[k]=v}else{delete value[k]}}}}return reviver.call(holder,key,value)}text=String(text);cx.lastIndex=0;if(cx.test(text)){text=text.replace(cx,function(a){return"\\u"+("0000"+a.charCodeAt(0).toString(16)).slice(-4)})}if(/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g,"@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,"]").replace(/(?:^|:|,)(?:\s*\[)+/g,""))){j=eval("("+text+")");return typeof reviver==="function"?walk({"":j},""):j}throw new SyntaxError("JSON.parse")}}}());var JSON;if(!JSON){JSON={}}(function(){function f(n){return n<10?"0"+n:n}if(typeof Date.prototype.toJSON!=="function"){Date.prototype.toJSON=function(key){return isFinite(this.valueOf())?this.getUTCFullYear()+"-"+f(this.getUTCMonth()+1)+"-"+f(this.getUTCDate())+"T"+f(this.getUTCHours())+":"+f(this.getUTCMinutes())+":"+f(this.getUTCSeconds())+"Z":null};String.prototype.toJSON=Number.prototype.toJSON=Boolean.prototype.toJSON=function(key){return this.valueOf()}}var cx=/[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,escapable=/[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,gap,indent,meta={"\b":"\\b","\t":"\\t","\n":"\\n","\f":"\\f","\r":"\\r",'"':'\\"',"\\":"\\\\"},rep;function quote(string){escapable.lastIndex=0;return escapable.test(string)?'"'+string.replace(escapable,function(a){var c=meta[a];return typeof c==="string"?c:"\\u"+("0000"+a.charCodeAt(0).toString(16)).slice(-4)})+'"':'"'+string+'"'}function str(key,holder){var i,k,v,length,mind=gap,partial,value=holder[key];if(value&&typeof value==="object"&&typeof value.toJSON==="function"){value=value.toJSON(key)}if(typeof rep==="function"){value=rep.call(holder,key,value)}switch(typeof value){case"string":return quote(value);case"number":return isFinite(value)?String(value):"null";case"boolean":case"null":return String(value);case"object":if(!value){return"null"}gap+=indent;partial=[];if(Object.prototype.toString.apply(value)==="[object Array]"){length=value.length;for(i=0;i<length;i+=1){partial[i]=str(i,value)||"null"}v=partial.length===0?"[]":gap?"[\n"+gap+partial.join(",\n"+gap)+"\n"+mind+"]":"["+partial.join(",")+"]";gap=mind;return v}if(rep&&typeof rep==="object"){length=rep.length;for(i=0;i<length;i+=1){if(typeof rep[i]==="string"){k=rep[i];v=str(k,value);if(v){partial.push(quote(k)+(gap?": ":":")+v)}}}}else{for(k in value){if(Object.prototype.hasOwnProperty.call(value,k)){v=str(k,value);if(v){partial.push(quote(k)+(gap?": ":":")+v)}}}}v=partial.length===0?"{}":gap?"{\n"+gap+partial.join(",\n"+gap)+"\n"+mind+"}":"{"+partial.join(",")+"}";gap=mind;return v}}if(typeof JSON.stringify!=="function"){JSON.stringify=function(value,replacer,space){var i;gap="";indent="";if(typeof space==="number"){for(i=0;i<space;i+=1){indent+=" "}}else{if(typeof space==="string"){indent=space}}rep=replacer;if(replacer&&typeof replacer!=="function"&&(typeof replacer!=="object"||typeof replacer.length!=="number")){throw new Error("JSON.stringify")}return str("",{"":value})}}if(typeof JSON.parse!=="function"){JSON.parse=function(text,reviver){var j;function walk(holder,key){var k,v,value=holder[key];if(value&&typeof value==="object"){for(k in value){if(Object.prototype.hasOwnProperty.call(value,k)){v=walk(value,k);if(v!==undefined){value[k]=v}else{delete value[k]}}}}return reviver.call(holder,key,value)}text=String(text);cx.lastIndex=0;if(cx.test(text)){text=text.replace(cx,function(a){return"\\u"+("0000"+a.charCodeAt(0).toString(16)).slice(-4)})}if(/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g,"@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,"]").replace(/(?:^|:|,)(?:\s*\[)+/g,""))){j=eval("("+text+")");return typeof reviver==="function"?walk({"":j},""):j}throw new SyntaxError("JSON.parse")}}}());