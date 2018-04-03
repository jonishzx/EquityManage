<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="UkeyTech.OA.FrameWork" Namespace="RepeaterInMvc.Codes"
    TagPrefix="MVC" %>
<%@ Import Namespace="Clover.Web.Core" %>
<%@ Import Namespace="Clover.Config" %>
<%@ Import Namespace="Clover.Web.HTMLRender" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Scripts" runat="server">    
    <script type="text/javascript">
        var indicator;
        var template = "<li tag='{0}' target='{1}' class='drag-item'><span class='drag-item-text'>{0}</span><span class='drag-item-icon icon icon-delete' onclick='$(this).parent().remove()'></span></li>";

        $(function () {

            var $div_li = $(".sidebarbox .lable li");
            var $div_box = $(".sidebarbox .content .divbox");
            $div_li.click(function () {
                $(this).addClass("current")                  //当前<li>元素高亮
						.siblings().removeClass("current");  //去掉其它同辈<li>元素的高亮
                var index = $div_li.index(this);    // 获取当前点击的<li>元素 在 全部li元素中的索引。
                //选取子节点。不选取子节点的话，会引起错误。如果里面还有div 
                $div_box.eq(index).show().siblings().hide(); //隐藏其它几个同辈的<div>元素
            }).hover(
					function () {
					    $(this).addClass("hover");
					},
					function () {
					    $(this).removeClass("hover");
					})
            ////////////////////////

            $("ul.lable > li:first").addClass("current");
            $("div.divbox:first").show();


            //拖放
            indicator = $('<div class="indicator">>></div>').appendTo('body');
            $('.tree-node').draggable({
                revert: true,
                deltaX: 0,
                deltaY: 0,
                proxy: function (source) {
                    var n = $('<div class="proxy"></div>');
                    n.html($(source).html()).appendTo('body');
                    return n;
                }
            });

            var treenodes = $('.tree-title >a');
            $(treenodes).each(function () {
                $(this).attr("target", $(this).attr("href"))
                $(this).attr("href", "javascript:void(0);")
                $(this).click(function () { return false; });
                $(this).parent().dblclick(function () {
                    addItemToList($(this));
                });
            });

            $('#targetlist').droppable({
                onDragEnter: function (e, source) {
                    $("#message").html('进入')
                    $(this).addClass('over');
                },
                onDragLeave: function (e, source) {
                    $("#message").html('离开')
                    $(this).removeClass('over');
                },
                onDrop: function (e, source) {

                    $("#message").html('放入');
                    if ($(source).html().indexOf("<li") < 0) {
                        addItemToList(source);
                    }

                    $(this).removeClass('over');
                }
            });

            bindMenuDrag("#targetlist li");
        })

        function addItemToList(source) {
            var a = $(source).find("a:first");
            var target = $(a).attr("target");
          
            if (target != null && $("#targetlist li[target='" + target + "']").length == 0) {
                var obj = $(template.replace(/\{0\}/g, $(a).html()).replace(/\{1\}/g, target));
                $("#targetlist").append(obj);
                bindMenuDrag(obj);
            }
        }

        function bindMenuDrag(a) {
            $(a).draggable({
                proxy: 'clone',
                revert: true,
                deltaX: 0,
                deltaY: 0,
                axis: 'v'
            }).droppable({
                onDragEnter: function (e, source) {
                    $("#message").html('进入')
                },
                onDragLeave: function (e, source) {
                    $("#message").html('离开')
                },
                onDragOver: function (e, source) {
                    indicator.css({
                        display: 'block',
                        top: $(this).offset().top + $(this).outerHeight() - 5,
                        left: $(this).offset().left
                    });
                    $(this).parent().addClass('over');
                },
                onDragLeave: function (e, source) {
                    indicator.hide();
                    $(this).parent().removeClass('over');

                },
                onDrop: function (e, source) {
                    $("#message").html('放入');
                  
                    //selft li
                    if ($(source).html().indexOf("li") >= 0) {
                        $(source).insertAfter(this);
                    }
                    else {
                        var a = $(source).find("a:first");
                        var target = $(a).attr("target");

                        if (target != null && $("#targetlist *[target='" + target + "']").length == 0) {
                            var obj = $(template.replace(/\{0\}/g, $(a).html()).replace(/\{1\}/g, target));
                            $(obj).insertAfter(this);
                            bindMenuDrag(obj);
                        }
                    }
                    $(this).parent().removeClass('over');
                    indicator.hide();
                }
            });
        }

        function Save() {
            var titles = "", targets = "";
            $("#targetlist li").each(function () {
                titles += $(this).attr("tag") + ",";
                targets += $(this).attr("target") + ",";
            });
            $.ajax({
                type: "POST",
                url: '<%=Url.Action("SaveMomoSetting","Home")%>',
                data: { titles: titles, targets: targets },
                success: function (json) {
                    MsgShow('系统提示', '保存成功。');
                    window.parent.RunBackFunc(); 
                    window.parent.CloseWin();
                }
            });
        } 
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:Loading ID="Loading1" runat="server" />
    <div region="west" split="true" border="true" title="系统菜单(双击或拖拽加入项目)" style="width: 250px;"
        class="sidebarbox_bg">
        <div class="sidebarbox">
            <ul class="lable">
                <MVC:MvcRepeater ID="rpParentMenu" Name="PMenusItems" runat="server">
                    <ItemTemplate>
                        <li>
                            <%# Eval("Name")%></li>
                    </ItemTemplate>
                </MVC:MvcRepeater>
            </ul>
            <div class="content">
                <MVC:MvcRepeater ID="rpTreeMenu" Name="PMenusItems" runat="server">
                    <ItemTemplate>
                        <div class="divbox">
                            <strong>
                                <%# Eval("Name")%></strong>
                            <%# UkeyTech.OA.WebApp.Helper.RenderChildrenNodesVisible(
                                (IWebContext)ViewData["PWebContext"],
                                (List<int>)ViewData["ModuleItems"],
                                Eval("Id").ToString())%>
                        </div>
                    </ItemTemplate>
                </MVC:MvcRepeater>
            </div>
        </div>
    </div>
    <div id="content" region="center" split="true" border="true">
        <div class="easyui-layout" fit="true" border="false" style="background: #ccc;">
            <div id="targetRegion" region="center" title="你可以拖拽列表项目变换顺序" split="true">              
                <ul id="targetlist">
                    <% foreach (var m in (List<UkeyTech.WebFW.Model.UserConfig>)ViewData["PMemoItems"])
                       {%>
                    <li tag="<%=m.Title%>" target="<%=m.ConfigValue%>" class="drag-item"><span class='drag-item-text'><%=m.Title%></span>
                    <span class="drag-item-icon icon icon-delete" onclick="$(this).parent().remove()"></span></li>
                    <%}%>
                </ul>
            </div>
            <div region="south" border="false" style="overflow:hidden !important;height:30px;">
                <a class="easyui-linkbutton" icon="icon-save" href="#" onclick="Save();" id="A1">
                    确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                        onclick="CloseTheWin();" runat="server" id="btnCancel">取消</a> <span id="message"></span>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
