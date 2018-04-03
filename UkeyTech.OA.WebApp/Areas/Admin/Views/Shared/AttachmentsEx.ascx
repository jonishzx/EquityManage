<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (ViewData.Eval("TargetID") != null && ViewData.Eval("TargetType") != null)
   { %>
<%: Styles.Render("~/styles/attachment") %>
<%: Scripts.Render("~/bundles/attachment") %>
<%
       {
           bool autohide = ViewData.Eval("AutoHide") != null && (bool)ViewData.Eval("AutoHide");
           bool popup = ViewData.Eval("Popup") == null || (bool)ViewData.Eval("Popup");
           string title = ViewData.Eval("Title") != null ? ViewData.Eval("Title").ToString() : "";
           string uploadconfig = ViewData.Eval("FileUploadConfig") != null ? ViewData.Eval("FileUploadConfig").ToString() : "CommonUpload";
           bool pagation = ViewData.Eval("Pagination") != null && (bool)ViewData.Eval("Pagination");
           bool autoload = ViewData.Eval("AutoLoad") != null && (bool)ViewData.Eval("AutoLoad");

           bool allowEditAttachment = !(ViewData.Eval("AllowEditAttachment") != null && !(bool)ViewData.Eval("AllowEditAttachment"));
           bool allowDelAttachment = !(ViewData.Eval("AllowDelAttachment") != null && !(bool)ViewData.Eval("AllowDelAttachment"));

           string targetId = ViewData.Eval("TargetID") != null ? ViewData.Eval("TargetID").ToString() : "";

           //引用其他附件
           bool allowRefAttachment = (ViewData.Eval("AllowRefAttachment") != null && (bool)ViewData.Eval("AllowRefAttachment"));
           string refDataTitle = ViewData.Eval("RrefDataTitle") != null ? ViewData.Eval("RrefDataTitle").ToString() : "";
           string refDataUrl = ViewData.Eval("RefDataUrl") != null ? ViewData.Eval("RefDataUrl").ToString() : "";

           string id = ViewData.Eval("ID") != null ? ViewData.Eval("ID").ToString() : "";
           if (autohide)
           { %>
<div class="attachlinkdescn">
    <%= ViewData.Eval("Title") %>共有(<a id="span_<%= id %>_Count" class="attachLink" onclick="popup_<%= id %>()"><%= ViewData.Eval("AttachCount") %></a>)
    <a id="A1" class="attachLink" onclick="popup_<%= id %>()">上传</a>
</div>
<% } %>
<div id="AttContainer_<%= id %>" class="attcontainer" style="<%= autohide ? "display:none;": "" %>">
    <span style="display: none;"><a href="#" id="autoPreviewlink_<%= id %>" target="_blank">
    </a></span>
    <table id="tbAttachGrid_<%= id %>" class="attachTable" title="<%= title %>列表">
    </table>
    <% if (allowRefAttachment)
       { %>
    <div id="RefAttachment_<%= id %>">
        <div>
            <div style="padding: 2px;">
                编码:
                <input name="iCode" style="width: 110px">
                标题:
                <input name="iTitle" style="width: 110px">
                <a href="#" class="easyui-linkbutton" iconcls="icon-search" onclick="query_ref<%= id %>(this)">
                    查询</a>
            </div>
            <div>
                <table id="tbRefAttachment_<%= id %>" class="easyui-datagrid" style="width: 480px;
                    height: 330px" fit="false" data-options="fitColumns:true,pagination:true">
                </table>
            </div>
        </div>
    </div>
    <% } %>
</div>
<script type="text/javascript">
    var pvUrlAddr = '<%= !string.IsNullOrEmpty(Request.ApplicationPath) ? (Request.ApplicationPath ) : "" %>Extenstion/PreviewAttachment.ashx?attachId=';
    function query_ref<%= id %>(obj) {
        var code = $(obj).parent().find("input[name='iCode']").val();
        var text = $(obj).parent().find("input[name='iTitle']").val();

        $("#tbRefAttachment_<%= id %>").datagrid("load", { title: text, targetid: code });
    };
    function popup_<%= id %>() {
        $("#AttContainer_<%= id %>").show();
        <% if (autohide && popup)
       { %>
         var win = $('#AttContainer_<%= id %>');
         var windowWidth = document.documentElement.clientWidth;
         var windowHeight = document.documentElement.clientHeight;
         win.window({
            title: "附件信息",
            width: windowWidth* 0.8,
            modal: true,
            shadow: false,
            closed: true,
            top: Math.max(0, (windowHeight * 0.2) / 4),
            left: Math.max(0, (windowWidth * 0.2) / 2),
            height: windowHeight * 0.8,
            resizable: false
        });
        win.window('open');
       <% } %>
        
        initAttachment_<%= id %>();
    }
    
    function initAttachment_<%= id %>() {
        
        $("#tbAttachGrid_<%= id %>").css("width", $(document).width() - 35);
        $('#tbAttachGrid_<%= id %>').datagrid({
            nowrap: false,
            striped: true,
            fit: <%= autohide && popup ? "true" : "false" %>,
            width:"<%= ViewData.Eval("Width") != null ? ViewData.Eval("Width") : "auto"%>",
            height:"<%= ViewData.Eval("Height") != null ? ViewData.Eval("Height") : "auto"%>",
            border: true,
            url: '<%= ViewData.Eval("ActionName").ToString().IndexOf("/") < 0 ? Url.Action(ViewData.Eval("ActionName").ToString(), "Attachment", new {Area = "Admin"})   : ViewData.Eval("ActionName") %>',
            queryParams: {cfgtype:'<%=uploadconfig%>', TargetID: '<%= targetId %>', TargetType: '<%= ViewData.Eval("TargetType") %>' <%= pagation?"":",page:1,rows:100000" %> },
            idField: 'AttachmentID',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'Title', title: '标题', width:  GetWidth(0.25), align: 'left' },
               // { field: 'FileName', title: '文件名称', width:  GetWidth(0.25), align: 'left' },
                { field: 'TargetType', title: '类型', width: 60, align: 'left',
                    formatter: function (value, rec) {
                        return rec.FileName.toString().match(/\.(\w)+$/)[0];
                    }
                },
                { field: 'Bytes', title: '文件大小', width: 80, align: 'left',
                    formatter: function (value, rec) {
                           return getFileSize(value);
                    }
                },
                { field: 'UpdateTime', title: '上传日期', width: 80, align: 'left',
                         formatter: function (value, rec) {
                             return ShortDateHandler(value);
                         }
                     },
                { field: 'Descn', title: '备注', width:  GetWidth(0.10), align: 'left' }
                ,
                { field: 'abk', title: '预览', width: 40, align: 'center',
                    formatter: function (value, rec) {
						var pvUrl =  pvUrlAddr + rec.AttachmentID;
						return '<a title="预览" target="_blank" href="' + pvUrl + '">' +
                                '<img src="/Content/Images/viewxml.gif" title="' + rec.Title + '" />' +
                                '</a>';
                    } 
                }
            ]],
            onBeforeLoad: function (row, param) {
            },
           <%if(pagation){ %>
            pagination: true,
            pageSize: 15,
            pageList: [15, 25, 35, 50],
            pageNumber: 1,
            <%} %>
            rownumbers: true,
            singleSelect: false,
            toolbar: [
                <% if (allowEditAttachment)
               { %>
                {
                    text: permission.Create ? '单文件上传' : '',
                    iconCls: permission.Create ? 'icon-add' : "null",
                    handler: function () {
                        var param = '&cfgtype=<%=uploadconfig %>&TargetType=<%= ViewData.Eval("TargetType") %>&TargetID=<%= targetId %>'
                        SetBackFunc(function(){$('#tbAttachGrid_<%= id %>').datagrid("reload");});
                        SetWin(480,350, '<%= Url.Action("", "Attachment", new {Area = "Admin"}) %>/CreateAttachment?' + param, '上传文件');
                    }
                },
                <% if (ViewData.Eval("MutilUpload") != null && (bool) ViewData.Eval("MutilUpload"))
                       { %>
                {
                    text: permission.Create ? '多文件上传' : '',
                    iconCls: permission.Create ? 'icon-add' : "null",
                    handler: function () {
                        var param = '?cfgtype=<%=uploadconfig %>&TargetType=<%= ViewData.Eval("TargetType") %>&TargetID=<%= targetId %>'
                        SetBackFunc(function(){$('#tbAttachGrid_<%= id %>').datagrid("reload");});
                        SetWin(700,350, '<%= Url.Action("MutilFileUpload", "Attachment", new {Area = "Admin"}) %>' + param, '上传多个文件');
                    }
                },
                <% }%>
                
                 <% if (allowRefAttachment)
                       { %>
                {
                    text: permission.Create ? '引用' : '',
                    iconCls: permission.Create ? 'icon-add' : "null",
                    handler: function () {
                        
                       var dg_<%= id %> = $("#tbRefAttachment_<%= id %>").datagrid(
                           { 
                            url:'<%=refDataUrl %>',
                            columns:[[
                                    {field:'ck',checkbox:true,width: 30},
                                    {field:'TargetID',title:'代码',width:100},
                                    {field:'Title',title:'标题',width:120},
                                    { field: 'TargetType', title: '类型', width: 60, align: 'left',
                                        formatter: function (value, rec) {
                                            return rec.FileName.toString().match(/\.(\w)+$/)[0];
                                        }
                                    },
                                   { field: 'UpdateTime', title: '上传日期', width: 80, align: 'left',
                                         formatter: function (value, rec) {
                                             return ShortDateHandler(value);
                                         }
                                    },
                                   { field: 'abk', title: '预览', width: 40, align: 'center',
                                        formatter: function (value, rec) {
						                    var pvUrl =  pvUrlAddr + rec.AttachmentID;
						                    return '<a title="预览" target="_blank" href="' + pvUrl + '">' +
                                                    '<img src="/Content/Images/viewxml.gif" title="' + rec.Title + '" />' +
                                                    '</a>';
                                        } 
                                    }
                                ]],
                             toolbar: [{
                                text: '确定',
                                iconCls: 'icon-ok',
                                handler: function () {
                                     var ids = getGridSelections('#tbRefAttachment_<%= id %>', 'AttachmentID');
                                    //将内容复制新的数据表
                                    post("<%= Url.Action("RefAttachment", "Attachment", new {Area = "Admin"}) %>", 
                                        {sourceIds : ids, targetId: '<%= targetId %>'},function(e) {
                                            $('#tbAttachGrid_<%= id %>').datagrid("reload");
                                            $('#RefAttachment_<%= id %>').dialog("close");
                                        });
                                }
                            }]
                        });
                        

                        $('#RefAttachment_<%= id %>').dialog({
                            title: '<%=refDataTitle%>引用附件',
                            width: 500,
                            height: 400,
                            closed: false,
                            cache: false,
                            modal: true
                        });
                      
                    }
                },
                <% }%>
                '-',
                <%if (allowDelAttachment)
                   { %>
                {
                    text: permission.Delete ? '删除' : '',
                    iconCls: permission.Delete ? 'icon-cut' : "null",
                    handler: function () {
                        //删除附件
                        var ids = getGridSelections('#tbAttachGrid_<%= id %>', 'AttachmentID');

                        if (ids) {

                            $.messager.confirm('Question', '确定要删除附件信息?', function (r) {
                                if (r) {
                                    $.ajax({
                                        type: "POST",
                                        url: '<%= Url.Action("", "Attachment", new {Area = "Admin"}) %>/DeleteAttachment',
                                        data: { delids: ids },
                                        dataType: "text",
                                        success: function (json) {
                                            setTimeout("initAttachment_<%= id %>();", 500);
                                            setTimeout("MsgShow('系统提示','删除成功。');", 1000);
                                        }
                                    });
                                }
                            });
                        }
                    }
                },
                '-',
                
                <% }
               } %>
                {
                    text: permission.Edit ? '下载' : '',
                    iconCls: permission.Edit ? 'icon-save' : "null",
                    handler: function () {
                        var id;
                        id = getGridSelection('#tbAttachGrid_<%= id %>', 'AttachmentID');

                        if (id) {
                            window.open('/Extenstion/DownloadAttachment.ashx?attachId=' + id);
                        }
                    }
                }],
            onBeforeLoad: function () {
                RemoveForbidButton();
            },
            onDblClickRow: function (idx, data) {
                var newurl = pvUrlAddr + data.AttachmentID;
                lnk = document.getElementById("autoPreviewlink_<%= id %>");
                lnk.href = newurl;
                lnk.click();
            },
            onLoadSuccess : function(data){
                if ($("#tbAttachGrid_<%= id %>").datagrid("getRows").length > 0) {
                    $("#tbAttachGrid_<%= id %>").find(".yoxviewLink").colorbox({ rel: 'yoxviewLink', transition: "fade"});
                }

                $("#span_<%= id %>_Count").html(data.total);
                
            }
        });
    }
    <% if (!autohide || autoload)
       { %>
    $(function() {
        initAttachment_<%= id %>();
    });
    <% } %>
</script>
<% }
   } %>
