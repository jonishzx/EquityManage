/**
* custcolgrid - jQuery EasyUI
* 
* Licensed under the GPL:
*   http://www.gnu.org/licenses/gpl.txt
*
* Copyright (c) 2014 www.jeasyui.com
* 
* Dependencies:
*   datagrid
*   menu
*   dialog
*   layout
* 
*/
(function ($) {
    function create(target) {
        var opts = $.data(target, 'custcolgrid').options;

        //load user setting
        if (opts.useUserSetting && !opts.hasLoadUserSetting) {
            var urlsp = opts.url.split('/');
            opts.settingCode = urlsp[urlsp.length - 1];
            $.ajax({
                type: "GET",
                url: opts.getSettingUrl + opts.settingCode,
                dataType: "text",
                success: function (data) {
                    if (data) {
                        var columnsetting = strToJson(data);
                        if (columnsetting.filters)
                            opts.colsetting.filters = columnsetting.filters.split(',');
                        if (columnsetting.columns)
                            opts.colsetting.columns = columnsetting.columns.split(',');
                        opts.hasLoadUserSetting = true;
                    }
                }
            });
        }

        opts.colsetting.filters = opts.colsetting.filters || [];
        opts.colsetting.filterRules = opts.colsetting.filterRules || [];

        var filterRules = {};
        $.map(opts.colsetting.filters, function (field) {
            filterRules[field] = opts.colsetting.filterRules[field] || [];
        });
        opts.colsetting.filterRules = filterRules;

        clearFilterBar(target);

        $(target).datagrid($.extend({}, opts, {
            onBeforeSortColumn: function (field) {
                var f = function (data) { return data };
                $(this).datagrid('options').loadFilter = f;
            },
            onSortColumn: function () {
                $(this).datagrid('options').loadFilter = opts.loadFilter;
            },
            loadFilter: function (data, parentId) {
                var state = $(this).data('custcolgrid');
                state.data = data;
                var opts = state.options;
                var originalData = opts.data;
                var originalUrl = opts.url;
                var filteredData = getFilteredData(target, data);

                opts.colsetting.fields = getFields(data[0]);

                $(this).datagrid({
                    data: null,
                    url: null,
                    frozenColumns: opts.frozenColumns,
                    columns: getColumns(this, filteredData)
                });

                buildFilterBar(this, data);

                setTimeout(function () {
                    opts.data = originalData;
                    opts.url = originalUrl;
                }, 0);

                return {
                    total: filteredData.length,
                    rows: filteredData
                };

                function getFields(row) {

                    var fields = [];

                    if (opts.colsetting.optioncolumns) {
                        for (var field in opts.colsetting.optioncolumns) {
                            fields.push(opts.colsetting.optioncolumns[field].field);
                        }
                    } else {
                        for (var field in row) {
                            fields.push(field);
                        }
                    }
                    subtract(opts.colsetting.filters);
                    subtract(opts.colsetting.columns);
                    return fields;

                    function subtract(aa) {
                        $.map(aa || [], function (a) {
                            var index = $.inArray(typeof a == 'string' ? a : a.field, fields);
                            if (index >= 0) {
                                fields.splice(index, 1);
                            }
                        });
                    }
                }
            }
        }));
    }

    function getFilteredData(target, data) {
        var state = $.data(target, 'custcolgrid');
        var opts = state.options;
        var rows = [];
        $.map(data || [], function (row) {
            if (isMatch(row)) {
                rows.push(row);
            }
        });
        return rows;

        function isMatch(row) {
            for (var field in opts.colsetting.filterRules) {
                var values = opts.colsetting.filterRules[field] || [];
                if (values.length) {
                    if ($.inArray(String(row[field]), values) == -1) {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    function clearFilterBar(target) {
        if ($(target).data('datagrid')) {
            var panel = $(target).datagrid('getPanel');
            var fbar = panel.children('div.datagrid-toolbar');
            fbar.find('.combo-f').combo('destroy');
            fbar.find('.pg-fbar').remove();
        }
    }
    function buildFilterBar(target, rows) {
        var opts = $.data(target, 'custcolgrid').options;
        if (!opts.colsetting.filters.length) {
            return;
        }
        var panel = $(target).datagrid('getPanel');
        var tb = panel.children('div.datagrid-toolbar');
        if (tb.length) {
            var bar = $('<div class="pg-fbar"></div>').appendTo(tb);
            bar.css('margin-top', '5px');
        } else {
            tb = $('<div class="datagrid-toolbar"></div>').prependTo(panel);
            var bar = $('<div class="pg-fbar"></div>').appendTo(tb);
        }

        $.map(opts.colsetting.filters, function (field) {
            var fieldsetting = getColumnByField(target, field);
            $('<span class="pg-flabel"></span>').html(fieldsetting.title).appendTo(bar);
            var f = $('<input>').attr('name', field).appendTo(bar);
            f.combobox({
                multiple: true,
                prompt: '请选择',
                data: getValues(field),
                icons: [{
                    iconCls: 'icon-ok',
                    handler: function (e) {
                        var t = $(e.data.target);
                        var field = t.attr('comboName');
                        opts.colsetting.filterRules[field] = t.combobox('getValues');
                        t.combobox('hidePanel');
                        $(target).custcolgrid();
                    }
                }],

                onSelect: handler1,
                onUnselect: handler1,
                onShowPanel: handler1,
                onLoadSuccess: handler2,
                onHidePanel: handler2
            });
            function handler1() {
                $(this).combobox('setText', '');
            };
            function handler2() {
                var field = $(this).attr('comboName');
                var values = opts.colsetting.filterRules[field] || [];
                $(this).combobox('setValues', values);
                //$(this).combobox('setText', values.length ? (values.length == 1 ? values[0] : 'multiple items') : '');
            }
        });

        function getValues(field) {
            var result = {};
            $.map(rows, function (row) {
                result[row[field]] = 1;
            });
            var values = [];
            for (var v in result) {
                values.push({ value: v, text: v });
            }
            return values;
        }
    }

    function getColumns(target, data) {
        var opts = $.data(target, 'custcolgrid').options;
        if (!opts.colsetting.columns) { return null; }
        var columns = [];
        for (var col in opts.colsetting.columns) {
            columns.push(getColumnByField(target, opts.colsetting.columns[col]));
        }
        return [columns];
    }

    function getColumnByField(target, name) {
        var opts = $.data(target, 'custcolgrid').options;
        if (opts.colsetting.optioncolumns) {
            for (var col in opts.colsetting.optioncolumns) {
                if (opts.colsetting.optioncolumns[col].field == name) {
                    return opts.colsetting.optioncolumns[col];
                }
            }
        }

        return { field: name };
    }
    function layout(target) {
        var state = $.data(target, 'custcolgrid');
        var opts = state.options;

        if (!state.layoutDialog) {
            var content = '<div class="easyui-layout" fit="true">' +
					'<div region="west" class="pg-fields" title="' + opts.i18n.fields + '" style="width:140px"></div>' +
					'<div region="center" border="false">' +
					'<div style="height:100%;">' +
					'<div class="easyui-panel pg-filters" title="' + opts.i18n.filters + '" data-options="style:{float:\'left\'}" style="width:152px;height:250px"></div>' +
					'<div class="easyui-panel pg-columns" title="' + opts.i18n.columns + '" data-options="style:{float:\'right\'}" style="width:152px;height:250px"></div>' +
					'</div>' +

					'</div>' +
					'</div>';
            state.layoutDialog = $('<div style="border:0"></div>').appendTo('body');
            state.layoutDialog.dialog({
                noheader: true,
                width: 460,
                height: 300,
                resizable: true,
                content: content,
                buttons: [{
                    text: opts.i18n.ok,
                    width: 80,
                    handler: function () {
                        opts.colsetting.filters = getFields('filters');
                        opts.colsetting.columns = getFields('columns');
                        state.layoutDialog.dialog('close');
                        $(target).custcolgrid();

                        //发送本地保存信息到本地
                        //save user setting 
                        if (opts.useUserSetting) {
                            $.ajax({
                                type: "POST",
                                url: opts.saveSettingUrl + opts.settingCode,
                                data: { val: jsonToString({ columns: opts.colsetting.columns, filters: opts.colsetting.filters }) },
                                dataType: "json",
                                success: function (data) {
                                    //ok!

                                }
                            });
                        }
                    }
                }, {
                    text: opts.i18n.cancel,
                    width: 80,
                    handler: function () {
                        state.layoutDialog.dialog('close');
                    }
                }]
            });
            $.parser.parse(state.layoutDialog);
        }
        state.layoutDialog.dialog('open');

        fill(state.layoutDialog.find('div.pg-filters'), opts.colsetting.filters);
        fill(state.layoutDialog.find('div.pg-columns'), opts.colsetting.columns);
        fill(state.layoutDialog.find('div.pg-fields'), opts.colsetting.fields);

        dnd();

        function fill(p, d) {
            p.empty();
            $.map(d, function (name) {

                var fieldopts = typeof name == 'object' ? name : getColumnByField(target, name);
                var text = typeof name == 'object' ? (name.title) : (fieldopts ? fieldopts.title : name);
                var item = $('<a class="custcolgrid-item" style="width:115px;" href="javascript:void(0)"></a>').appendTo(p);
                item.linkbutton($.extend({}, fieldopts, {
                    text: text,
                    plain: true,
                    width: '133px'
                }));
            });
        }
        function dnd() {
            state.layoutDialog.find('.custcolgrid-item').draggable({
                revert: true,
                proxy: function () {
                    var a = $(this).clone().appendTo('body');
                    a.removeClass('l-btn-plain').css('zIndex', '999999');
                    return a;
                },
                onBeforeDrag: function (e) {
                    if (e.which != 1) { return false; }
                }
            }).droppable({
                accept: '.custcolgrid-item',
                onDragEnter: function (e, source) {
                    $(this).addClass('custcolgrid-item-ins');
                },
                onDragLeave: function (e, source) {
                    $(this).removeClass('custcolgrid-item-ins');
                }
            });
            state.layoutDialog.find('.pg-fields,.pg-filters,.pg-columns').droppable({
                accept: '.custcolgrid-item',
                onDrop: function (e, source) {
                    var btn = $(this).find('.custcolgrid-item-ins');
                    if (btn.length) {
                        btn.removeClass('custcolgrid-item-ins');
                        $(source).insertBefore(btn);
                    } else {
                        $(source).appendTo(this);
                    }
                    var opts = $(source).linkbutton('options');
                    var text = opts.text;
                    $(source).linkbutton({
                        text: text
                    });
                }
            });
        }
        function getFields(type) {
            var fields = [];
            state.layoutDialog.find('.pg-' + type + ' .l-btn').each(function () {
                var bopts = $(this).linkbutton('options');
                if (type == 'values') {
                    fields.push($.extend({}, bopts, {
                        width: opts.valueFieldWidth,
                        align: 'right'
                    }));
                } else {
                    fields.push(bopts.field);
                }
            });
            return fields;
        }
    }

    function initCss() {
        if (!$('#custcolgrid-style').length) {
            $('head').append(
				'<style id="custcolgrid-style">' +
				'a.custcolgrid-item,a.custcolgrid-item:hover{text-align:left;-moz-border-radius:0;-webkit-border-radius:0;border-radius:0;}' +
				'a.custcolgrid-item-ins{border-top:1px solid red;}' +
				'.pg-fbar{padding:0;}' +
				'.pg-flabel{display:inline-block;height:22px;line-height:22px;vertical-align:middle;margin:0 5px;}' +
				'</style>'
			);
        }
    }

    $.fn.custcolgrid = function (options, param) {
        if (typeof options == 'string') {
            var method = $.fn.custcolgrid.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.datagrid(options, param);
            }
        }

        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'custcolgrid');
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, 'custcolgrid', {
                    options: $.extend({}, $.fn.custcolgrid.defaults, $.fn.custcolgrid.parseOptions(this), options)
                });
            }
            initCss();
            create(this);
        });
    };

    $.fn.custcolgrid.methods = {
        options: function (jq) {
            return $.data(jq[0], 'custcolgrid').options;
        },
        getData: function (jq) {
            return $.data(jq[0], 'custcolgrid').data;
        },
        layout: function (jq) {
            return jq.each(function () {
                layout(this);
            });
        }
    };

    $.fn.custcolgrid.parseOptions = function (target) {
        return $.extend({}, $.fn.datagrid.parseOptions(target), $.parser.parseOptions(target, [
		]));
    };

    $.fn.custcolgrid.defaults = $.extend({}, $.fn.datagrid.defaults, {

        autoRowHeight: false,
        remoteSort: false,
        useUserSetting: true, //auto save user column setting
        getSettingUrl: '/Admin/Account/GetUserSetting?code=',  //get user setting url
        saveSettingUrl: '/Admin/Account/SaveUserSetting?code=', //set user setting url
        settingCode: '',
        valueStyler: function () { },
        valueFormatter: function (value) { return value; },
        i18n: {
            fields: '可选数据列',
            filters: '过滤',
            columns: '数据列',
            ok: '确定',
            cancel: '取消'
        }
    });

    $.parser.plugins.push('custcolgrid');
})(jQuery);
