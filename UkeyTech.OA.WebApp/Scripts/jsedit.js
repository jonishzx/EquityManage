﻿/*
* jQuery plugin that makes elements editable
*
* @author Victor Jonsson (http://victorjonsson.se/)
* @website https://github.com/victorjonsson/jquery-editable/
* @license GPLv2 http://www.gnu.org/licenses/gpl-2.0.html
* @version 1.3.1
* @donations http://victorjonsson.se/donations/
*/
(function ($, window) {
    "use strict";
    var $win = $(document),
	$textArea = false,
	$currentlyEdited = false,
	EVENT_ATTR = "data-edit-event",
	IS_EDITING_ATTR = "data-is-editing",
	DBL_TAP_EVENT = "dbltap",
	SUPPORTS_TOUCH = "ontouchend" in window,
	TINYMCE_INSTALLED = "tinyMCE" in window && typeof window.tinyMCE.init == "function",
	oldjQueryIs = $.fn.is,
	lastTap = 0,
	tapper = function () {
	    var now = (new Date).getTime();
	    if (now - lastTap < 250) {
	        $(this).trigger(DBL_TAP_EVENT)
	    }
	    lastTap = now
	},
	keyHandler = function (e) {
	    if (typeof e.data.mykeydown == "function")
	        return e.data.mykeydown(e, $currentlyEdited, $textArea);
	    else {
	        if (e.keyCode == 13 && e.data.closeOnEnter) {
	            $currentlyEdited.editable("close")
	        } else if (e.data.toggleFontSize && e.metaKey && (e.keyCode == 38 || e.keyCode == 40)) {
	            var fontSize = parseInt($textArea.css("font-size"), 10);
	            fontSize += e.keyCode == 40 ? -1 : 1;
	            $textArea.css("font-size", fontSize + "px");
	            return false
	        }
	    }
	},
	adjustTextAreaHeight = function () {
	    if ($textArea[0].scrollHeight !== parseInt($textArea.attr("data-scroll"), 10)) {
	        $textArea.css("height", $textArea[0].scrollHeight + "px");
	        $textArea.attr("data-scroll", $textArea[0].scrollHeight)
	    }
	},
	resetElement = function ($el, newText, opts) {
	    if (typeof opts.validate == "function" && !opts.validate($el))
	        return false;
	    $el.removeAttr("data-is-editing");
	    $el.html(newText);
	    $textArea.remove()
	},
	elementEditor = function ($el, opts) {
	    if ($el.is(":editing"))
	        return;
	    $currentlyEdited = $el;
	    $el.attr("data-is-editing", "1");
	    var defaultText = $.trim(typeof opts.opValue == "function" ? opts.opValue($el.html()) : $el.html()),
		defaultFontSize = $el.css("font-size"),
		elementHeight = $el.height(),
		textareaStyle = "width: 96%; padding:0; margin:0; border:0; background:none;" + "font-family: " + $el.css("font-family") + "; font-size: " + $el.css("font-size") + ";" + "font-weight: " + $el.css("font-weight") + ";";
	    if (opts.lineBreaks) {
	        defaultText = defaultText.replace(/<br( |)(|\/)>/g, "\n")
	    }
	    $textArea = $("<textarea></textarea>");
	    $el.text("");
	    if (navigator.userAgent.match(/webkit/i) !== null) {
	        textareaStyle = document.defaultView.getComputedStyle($el.get(0), "").cssText
	    }
	    if (opts.tinyMCE !== false) {
	        var id = "editable-area-" + (new Date).getTime();
	        $textArea.val(defaultText).appendTo($el).attr("id", id);
	        if (typeof opts.tinyMCE != "object")
	            opts.tinyMCE = {};
	        opts.tinyMCE.mode = "exact";
	        opts.tinyMCE.elements = id;
	        opts.tinyMCE.width = $el.innerWidth();
	        opts.tinyMCE.height = $el.height() + 200;
	        opts.tinyMCE.theme_advanced_resize_vertical = true;
	        opts.tinyMCE.setup = function (ed) {
	            ed.onInit.add(function (editor, evt) {
	                var editorWindow = editor.getWin();
	                var hasPressedKey = false;
	                var editorBlur = function () {
	                    var newText = $(editor.getDoc()).find("body").html();
	                    if ($(newText).get(0).nodeName == $el.get(0).nodeName) {
	                        newText = $(newText).html()
	                    }
	                    resetElement($el, newText, opts);
	                    editor.remove();
	                    $textArea = false;
	                    $win.unbind("click", editorBlur);
	                    $currentlyEdited = false;
	                    if (typeof opts.callback == "function") {
	                        opts.callback({
	                            content: newText == defaultText || !hasPressedKey ? false : newText,
	                            fontSize: false,
	                            $el: $el
	                        })
	                    }
	                };
	                setTimeout(function () {
	                    $win.bind("click", editorBlur)
	                }, 500);
	                $textArea = $("<textarea></textarea>");
	                $textArea.bind("blur", editorBlur);
	                editorWindow.onkeydown = function () {
	                    hasPressedKey = true
	                };
	                editorWindow.focus()
	            })
	        };
	        tinyMCE.init(opts.tinyMCE)
	    } else {
	        if (opts.toggleFontSize || opts.closeOnEnter) {
	            $win.bind("keydown", opts, keyHandler)
	        }
	        $win.bind("keyup", adjustTextAreaHeight);

	        if (typeof opts.mykeypress == "function")
	        	$textArea.keypress(function (e) {
	        	    return opts.mykeypress(e, this);
	        	});
	        	if (typeof opts.mykeyup == "function")
	        	    $textArea.keyup(function (e) {
	        	        return opts.mykeyup(e, this);
	        	    });
	        $textArea.val(defaultText).blur(function () {
	            $currentlyEdited = false;
	            var newText = $.trim($textArea.val()),
				newFontSize = $textArea.css("font-size");
	            if (opts.lineBreaks) {
	                newText = newText.replace(new RegExp("\n", "g"), "<br />")
	            }
	            resetElement($el, newText, opts);
	            if (newFontSize != defaultFontSize) {
	                $el.css("font-size", newFontSize)
	            }

	            $win.unbind("keydown", keyHandler);
	            if (typeof opts.mykeypress == "function")
	                $textArea.unbind("keypress", opts.mykeypress, $textArea);
	            if (typeof opts.mykeyup == "function")
	                $textArea.unbind("keyup", opts.mykeyup, $textArea);
	            $win.unbind("keyup", adjustTextAreaHeight);
	            if (typeof opts.callback == "function") {
	                opts.callback({
	                    content: newText == defaultText ? false : newText,
	                    fontSize: newFontSize == defaultFontSize ? false : newFontSize,
	                    $el: $el
	                })
	            }
	        }).attr("style", textareaStyle).appendTo($el).css({
	            margin: 0,
	            padding: 0,
	            height: elementHeight + "px",
	            overflow: "hidden"
	        }).focus().get(0).select();
	        adjustTextAreaHeight()
	    }
	    $el.trigger("edit", [$textArea])
	},
	editEvent = function (event) {
	    if ($currentlyEdited !== false) {
	        $currentlyEdited.editable("close");
	        var $this = $(this);
	        elementEditor($this, event.data)
	    } else {
	        elementEditor($(this), event.data)
	    }
	    return false
	};
    $.fn.editable = function (opts) {
        if (typeof opts == "string") {
            if (this.is(":editable")) {
                switch (opts) {
                    case "open":
                        if (!this.is(":editing")) {
                            this.trigger(this.attr(EVENT_ATTR))
                        }
                        break;
                    case "close":
                        if (this.is(":editing")) {
                            $textArea.trigger("blur")
                        }
                        break;
                    case "destroy":
                        if (this.is(":editing")) {
                            $textArea.trigger("blur")
                        }
                        this.unbind(this.attr(EVENT_ATTR));
                        this.removeAttr(EVENT_ATTR);
                        break;
                    default:
                        console.warn('Unknown command "' + opts + '" for jquery.editable')
                }
            } else {
                console.error("Calling .editable() on an element that is not editable, call .editable() first")
            }
        } else {
            if (this.is(":editable")) {
                console.warn('Making an already editable element editable, call .editable("destroy") first');
                this.editable("destroy")
            }
            opts = $.extend({
                event: "dblclick",
                touch: true,
                lineBreaks: true,
                toggleFontSize: true,
                closeOnEnter: false,
                tinyMCE: false
            }, opts);
            if (opts.tinyMCE !== false && !TINYMCE_INSTALLED) {
                console.warn("Trying to use tinyMCE as editor but id does not seem to be installed");
                opts.tinyMCE = false
            }
            if (SUPPORTS_TOUCH && opts.touch) {
                opts.event = DBL_TAP_EVENT;
                this.unbind("touchend", tapper);
                this.bind("touchend", tapper)
            } else {
                opts.event += ".textEditor"
            }
            this.bind(opts.event, opts, editEvent);
            this.attr(EVENT_ATTR, opts.event)
        }
        return this
    };
    $.fn.is = function (statement) {
        if (typeof statement == "string" && statement.indexOf(":") === 0) {
            if (statement == ":editable") {
                return this.attr(EVENT_ATTR) !== undefined
            } else if (statement == ":editing") {
                return this.attr(IS_EDITING_ATTR) !== undefined
            }
        }
        return oldjQueryIs.apply(this, arguments)
    }
})(jQuery, window);