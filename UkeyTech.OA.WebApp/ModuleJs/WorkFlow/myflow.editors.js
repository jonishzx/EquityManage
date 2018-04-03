(function ($) {
    var myflow = $.myflow;

    $.extend(true, myflow.editors, {
        inputEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<input style=""/>').val(props[_k].value).change(function () {
                    if (_k == 'DisplayName' && _src.setText) {
                        _src.setText($(this).val());
                        $(this).parents('table').find("#ptext").find('input').val($(this).val());
                    }
                    props[_k].value = $(this).val();
                }).appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            }
        },
        readonlyEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<label>' + props[_k].value + "</label>").appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {

            }
        },
        hiddenEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                //$('<label>' + props[_k].value + "</label>").appendTo('#' + _div);
                $('#' + _div).parent().parent().hide();
                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {

            }
        },
        taskEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<input  type="button" value="设置"/>').change(function () {
                    props[_k].value = $(this).val();
                }).click(function () {
                    showTaskDialog(props.DisplayName.value, props.Id.value);
                }).appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            }
        },
        selectEditor: function (arg) {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                var sle = $('<select/>').val(props[_k].value).change(function () {
                    props[_k].value = $(this).val();
                }).appendTo('#' + _div);

                if (typeof arg === 'string') {
                    $.ajax({
                        type: "GET",
                        url: arg,
                        success: function (data) {
                            var opts = eval(data);
                            if (opts && opts.length) {
                                for (var idx = 0; idx < opts.length; idx++) {
                                    sle.append('<option value="' + opts[idx].value + '">' + opts[idx].name + '</option>');
                                }
                                sle.val(_props[_k].value);
                            }
                        }
                    });
                } else {
                    for (var idx = 0; idx < arg.length; idx++) {
                        sle.append('<option value="' + arg[idx].value + '">' + arg[idx].name + '</option>');
                    }
                    sle.val(_props[_k].value);
                }

                $('#' + _div).data('editor', this);
            };
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            };
        }
    });

})(jQuery);

