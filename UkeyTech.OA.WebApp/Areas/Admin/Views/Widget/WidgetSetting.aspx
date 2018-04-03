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
      
        function LoadWidgetGrid(code) {
            $('#DataGrid').datagrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: false,
                    url: '<%=Url.Action("GetNoSelectedWidgetList","Widget") %>',
                columns: [[
                { field: 'ck', checkbox: true },
				{ field: 'WidgetName', title: '名称', width: 120 }
			    ]],
                pagination: false,
                pageSize: 15,
                pageList: [10, 15, 20, 30],
                rownumbers: true,
                pageNumber: 1,
                singleSelect: true,
                toolbar: [
        {
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
              
                addItemToList();
            }
        }],
                onBeforeLoad: function () {
                    RemoveForbidButton();
                }
            });
        }


        $(function () {

            LoadWidgetGrid();

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

            $('.targetlist').droppable({
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
                    if ($(indicator).css("display") == "none") {
                        $(this).append($(source).parent());
                    }
                    indicator.hide();
                    $(this).removeClass('over');
                }
            });

            bindMenuDrag(".targetlist li");
        })

        var template = "<li tag='{0}' code='{2}' val='p{1}' class='layout-drag-item'><div class='drag-item-text'>{0}</div><span class='drag-item-icon icon icon-delete' onclick='removeItem($(this).parent())'></span></li>";
        function addItemToList() {
            var selrows = $('#DataGrid').datagrid("getSelections");
            
            var ids = getGridSelections("DataGrid", "WidgetID");
            var names = getGridSelections("DataGrid", "WidgetName");
            var codes = getGridSelections("DataGrid", "WidgetCode");
            //添加到列表上

            for (var i = 0; i < selrows.length; i++) {
                var obj = $(template.replace(/\{0\}/g, selrows[i].WidgetName)
                .replace(/\{1\}/g, selrows[i].WidgetID)
                .replace(/\{2\}/g, selrows[i].WidgetCode));

                $(".targetlist:first").append(obj);
                bindMenuDrag(obj);
            }
          
            $(".datagrid-row-selected").remove();
        }

        function removeItem(obj){
        
            $(obj).remove();
            Save();            
            setTimeout("LoadWidgetGrid()", 500);
        }

        function bindMenuDrag(a) {
            var target = $(a).find(".drag-item-text");
            $(target).draggable({
                proxy: 'clone',
                revert: true,
                deltaX: -250,
                deltaY: 0
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
                    if ($(source).parent().html().indexOf("li") >= 0) {
                        $(source).parent().insertAfter(this);
                    }
                    else {
                        var a = $(source).parent().find("a:first");
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

            $("li>span").draggable({ disabled: true });
        }

        function Save() {
            var states = "";
            $(".targetlist").each(function () {
                $(this).find("li").each(function () {
                    states += $(this).attr("val") + ",";
                });
                states += ":";
            });

            states = states.substring(0, states.length - 1);

            $.ajax({
                type: "POST",
                url: '<%=Url.Action("SaveUserWidget","Widget")%>',
                data: { layout: states},
                success: function (json) {
                    
                    window.parent.RunBackFunc(); 
                   
                }
            });
        } 
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:Loading ID="Loading1" runat="server" />
    <div region="west" split="true" border="true" title="系统菜单(选中项目点击添加)" style="width: 250px;"
        class="sidebarbox_bg">
        <table id="DataGrid">
        </table>
    </div>
    <div id="content" region="center" split="true" border="true">
        <div class="easyui-layout" fit="true" border="false" style="background: #ccc;">
            <div id="targetRegion" region="center" title="你可以拖拽列表项目变换顺序" split="true">
                <%
                  var dict = ViewData["UserWidgetLayout"] as Dictionary<int,List<UkeyTech.WebFW.Model.Widget>>;
                  int i = 0;
                  foreach(int key in dict.Keys)
                  {
                      i++;
                      if (i > 2) //只显示两列
                          break;%>
                    <ul id="<%=key%>" class="targetlist">
                        <% foreach (var m in (List<UkeyTech.WebFW.Model.Widget>)dict[key])
                           {%>
                        <li tag="<%=m.WidgetName%>" val="p<%=m.WidgetID%>" code="<%=m.WidgetCode%>" class="layout-drag-item">
                            <div class='drag-item-text'><%=m.WidgetName%></div>
                            <span class="drag-item-icon icon icon-delete" onclick="removeItem($(this).parent())"></span>
                        </li>
                        <%}%>
                    </ul>
                <%} %>
            </div>
            <div region="south" border="false" style="overflow:hidden !important;height:30px;">
                <a class="easyui-linkbutton" icon="icon-save" href="#" onclick="Save(); window.parent.CloseWin();" id="A1">
                    确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                        onclick="CloseTheWin();" runat="server" id="btnCancel">取消</a> <span id="message"></span>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
