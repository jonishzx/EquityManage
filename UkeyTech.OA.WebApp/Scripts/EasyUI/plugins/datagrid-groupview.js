var groupview = $.extend({}, $.fn.datagrid.defaults.view, {
    render: function (target, container, frozen) {
        var table = [];
        var groups = this.groups;
        for (var i = 0; i < groups.length; i++) {
            table.push(this.renderGroup.call(this, target, i, groups[i], frozen));
        }
        $(container).html(table.join(''));
    },

    renderGroup: function (target, groupIndex, group, frozen) {
        var state = $.data(target, 'datagrid');
        var opts = state.options;
        var fields = $(target).datagrid('getColumnFields', frozen);

        var table = [];
        table.push('<div class="datagrid-group" group-index=' + groupIndex + '>');
        table.push('<table cellspacing="0" cellpadding="0" border="0" style="height:100%"><tbody>');
        table.push('<tr>');
        if ((frozen && (opts.rownumbers || opts.frozenColumns.length)) ||
				(!frozen && !(opts.rownumbers || opts.frozenColumns.length))) {
            table.push('<td style="border:0;text-align:center;width:25px"><span class="datagrid-row-expander datagrid-row-collapse" style="display:inline-block;width:16px;height:16px;cursor:pointer">&nbsp;</span></td>');
        }
        table.push('<td style="border:0;">');
        if (!frozen) {
            table.push('<span class="datagrid-group-title">');
            table.push(opts.groupFormatter.call(target, group.value, group.rows));
            table.push('</span>');
        }
        table.push('</td>');
        table.push('</tr>');
        table.push('</tbody></table>');
        table.push('</div>');

        table.push('<table class="datagrid-btable" cellspacing="0" cellpadding="0" border="0"><tbody>');
        var index = group.startIndex;
        for (var j = 0; j < group.rows.length; j++) {
            var css = opts.rowStyler ? opts.rowStyler.call(target, index, group.rows[j]) : '';
            var classValue = '';
            var styleValue = '';
            if (typeof css == 'string') {
                styleValue = css;
            } else if (css) {
                classValue = css['class'] || '';
                styleValue = css['style'] || '';
            }

            var cls = 'class="datagrid-row ' + (index % 2 && opts.striped ? 'datagrid-row-alt ' : ' ') + classValue + '"';
            var style = styleValue ? 'style="' + styleValue + '"' : '';
            var rowId = state.rowIdPrefix + '-' + (frozen ? 1 : 2) + '-' + index;
            table.push('<tr id="' + rowId + '" datagrid-row-index="' + index + '" ' + cls + ' ' + style + '>');
            table.push(this.renderRow.call(this, target, fields, frozen, index, group.rows[j]));
            table.push('</tr>');
            index++;
        }
        table.push('</tbody></table>');
        return table.join('');
    },

    bindEvents: function (target) {
        var state = $.data(target, 'datagrid');
        var dc = state.dc;
        var body = dc.body1.add(dc.body2);
        var bodyevents = ($.data(body[0], 'events') || $._data(body[0], 'events'));
        var clickHandler = bodyevents ? bodyevents.click[0].handler : null;
        body.unbind('click').bind('click', function (e) {
            var tt = $(e.target);
            var expander = tt.closest('span.datagrid-row-expander');
            if (expander.length) {
                var gindex = expander.closest('div.datagrid-group').attr('group-index');
                if (expander.hasClass('datagrid-row-collapse')) {
                    $(target).datagrid('collapseGroup', gindex);
                } else {
                    $(target).datagrid('expandGroup', gindex);
                }
            } else {
                if(clickHandler)
                    clickHandler(e);
            }
            e.stopPropagation();
        });
    },

    onBeforeRender: function (target, rows) {
        var state = $.data(target, 'datagrid');
        var opts = state.options;

        initCss();

        var groups = [];
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            var group = getGroup(row[opts.groupField]);
            if (!group) {
                group = {
                    value: row[opts.groupField],
                    rows: [row]
                };
                groups.push(group);
            } else {
                group.rows.push(row);
            }
        }

        var index = 0;
        var newRows = [];
        for (var i = 0; i < groups.length; i++) {
            var group = groups[i];
            group.startIndex = index;
            index += group.rows.length;
            newRows = newRows.concat(group.rows);
        }

        state.data.rows = newRows;
        this.groups = groups;

        var that = this;
        setTimeout(function () {
            that.bindEvents(target);
        }, 0);

        function getGroup(value) {
            for (var i = 0; i < groups.length; i++) {
                var group = groups[i];
                if (group.value == value) {
                    return group;
                }
            }
            return null;
        }
        function initCss() {
            if (!$('#datagrid-group-style').length) {
                $('head').append(
					'<style id="datagrid-group-style">' +
					'.datagrid-group{height:25px;overflow:hidden;font-weight:bold;border-bottom:1px solid #ccc;}' +
					'</style>'
				);
            }
        }
    }
});

$.extend($.fn.datagrid.methods, {
    expandGroup:function(jq, groupIndex){
        return jq.each(function(){
            var view = $.data(this, 'datagrid').dc.view;
            var group = view.find(groupIndex!=undefined ? 'div.datagrid-group[group-index="'+groupIndex+'"]' : 'div.datagrid-group');
            var expander = group.find('span.datagrid-row-expander');
            if (expander.hasClass('datagrid-row-expand')){
                expander.removeClass('datagrid-row-expand').addClass('datagrid-row-collapse');
                group.next('table').show();
            }
            $(this).datagrid('fixRowHeight');
        });
    },
    collapseGroup:function(jq, groupIndex){
        return jq.each(function(){
            var view = $.data(this, 'datagrid').dc.view;
            var group = view.find(groupIndex!=undefined ? 'div.datagrid-group[group-index="'+groupIndex+'"]' : 'div.datagrid-group');
            var expander = group.find('span.datagrid-row-expander');
            if (expander.hasClass('datagrid-row-collapse')){
                expander.removeClass('datagrid-row-collapse').addClass('datagrid-row-expand');
                group.next('table').hide();
            }
            $(this).datagrid('fixRowHeight');
        });
    }
});

$.extend(groupview, {
	refreshGroupTitle: function(target, groupIndex){
		var state = $.data(target, 'datagrid');
		var opts = state.options;
		var dc = state.dc;
		var group = this.groups[groupIndex];
		var span = dc.body2.children('div.datagrid-group[group-index=' + groupIndex + ']').find('span.datagrid-group-title');
		span.html(opts.groupFormatter.call(target, group.value, group.rows));
	},
	
	insertRow: function(target, index, row){
		var state = $.data(target, 'datagrid');
		var opts = state.options;
		var dc = state.dc;
		var group = null;
		var groupIndex;
		
		for(var i=0; i<this.groups.length; i++){
			if (this.groups[i].value == row[opts.groupField]){
				group = this.groups[i];
				groupIndex = i;
				break;
			}
		}
		if (group){
			if (index == undefined || index == null){
				index = state.data.rows.length;
			}
			if (index < group.startIndex){
				index = group.startIndex;
			} else if (index > group.startIndex + group.rows.length){
				index = group.startIndex + group.rows.length;
			}
			$.fn.datagrid.defaults.view.insertRow.call(this, target, index, row);
			
			if (index >= group.startIndex + group.rows.length){
				_moveTr(index, true);
				_moveTr(index, false);
			}
			group.rows.splice(index - group.startIndex, 0, row);
		} else {
			group = {
				value: row[opts.groupField],
				rows: [row],
				startIndex: state.data.rows.length
			}
			groupIndex = this.groups.length;
			dc.body1.append(this.renderGroup.call(this, target, groupIndex, group, true));
			dc.body2.append(this.renderGroup.call(this, target, groupIndex, group, false));
			this.groups.push(group);
			state.data.rows.push(row);
		}
		
		this.refreshGroupTitle(target, groupIndex);
		
		function _moveTr(index,frozen){
			var serno = frozen?1:2;
			var prevTr = opts.finder.getTr(target, index-1, 'body', serno);
			var tr = opts.finder.getTr(target, index, 'body', serno);
			tr.insertAfter(prevTr);
		}
	},
	
	updateRow: function(target, index, row){
		var opts = $.data(target, 'datagrid').options;
		$.fn.datagrid.defaults.view.updateRow.call(this, target, index, row);
		var tb = opts.finder.getTr(target, index, 'body', 2).closest('table.datagrid-btable');
		var groupIndex = parseInt(tb.prev().attr('group-index'));
		this.refreshGroupTitle(target, groupIndex);
	},
	
	deleteRow: function(target, index){
		var state = $.data(target, 'datagrid');
		var opts = state.options;
		var dc = state.dc;
		var body = dc.body1.add(dc.body2);
		
		var tb = opts.finder.getTr(target, index, 'body', 2).closest('table.datagrid-btable');
		var groupIndex = parseInt(tb.prev().attr('group-index'));
		
		$.fn.datagrid.defaults.view.deleteRow.call(this, target, index);
		
		var group = this.groups[groupIndex];
		if (group.rows.length > 1){
			group.rows.splice(index-group.startIndex, 1);
			this.refreshGroupTitle(target, groupIndex);
		} else {
			body.children('div.datagrid-group[group-index='+groupIndex+']').remove();
			for(var i=groupIndex+1; i<this.groups.length; i++){
				body.children('div.datagrid-group[group-index='+i+']').attr('group-index', i-1);
			}
			this.groups.splice(groupIndex, 1);
		}
		
		var index = 0;
		for(var i=0; i<this.groups.length; i++){
			var group = this.groups[i];
			group.startIndex = index;
			index += group.rows.length;
		}
	}
});

