<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if(ViewData.Eval("TargetID")!=null && ViewData.Eval("TargetType")!=null){
       bool pagation = ViewData.Eval("Pagination") != null && (bool)ViewData.Eval("Pagination");
       %>

<%:Styles.Render("~/styles/attachment")%>
<%:Scripts.Render("~/bundles/attachment")%>
<table id="tbAttachmentGrid" title="<%= (bool)ViewData.Eval("AllowEditAttachment") ? "相关文件【IE10以上版本，Mozilla Firefox，Google Chrome浏览器可以使用多文件拖拽上传功能】":"" %>">
</table>

<script type="text/javascript">
    function initAttachmentGrid() {
        
        $("#tbAttachmentGrid").css("width", $(document).width() - 35);
        $('#tbAttachmentGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: false,
            border: false,
            url: '<%=ViewData.Eval("ActionName").ToString().IndexOf("/") < 0 ?  Url.Action(ViewData.Eval("ActionName").ToString(),"Attachment", new{Area = "Admin"}) : ViewData.Eval("ActionName")%>',
            queryParams: { TargetID: '<%= ViewData.Eval("TargetID") %>', TargetType: '<%= ViewData.Eval("TargetType") %>' <%= pagation?"":",page:1,rows:100000" %> },
            idField: 'AttachmentID',
            columns: [[
                     { field: 'Title', title: '标题', width: 150, align: 'left' },
                    // { field: 'FileName', title: '文件名称', width: 150, align: 'left' },
                     { field: 'TargetType', title: '类型', width: 60, align: 'left',
                         formatter: function (value, rec) {
                             return rec.FileName.toString().match(/\.(\w)+$/)[0];
                         }
                     },
                     { field: 'Bytes', title: '文件大小', width: 80, align: 'left',
                         formatter: function (value, rec) {
                             return value > 1024 ? Math.round(value / 1024) + "kb" : value + "字节";
                         }
                     },
                      { field: 'UpdateTime', title: '上传日期', width: 80, align: 'left',
                         formatter: function (value, rec) {
                             return ShortDateHandler(value);
                         }
                     },
                     { field: 'Descn', title: '备注', width: 200, align: 'left' }
                     ,
                     { field: 'abk', title: '预览', width: 40, align: 'center',
                         formatter: function (value, rec) {
                            var pvUrl = rec.PreviewFilePath ? rec.PreviewFilePath.replace("~",""):"";
                            var fileUrl = rec.FilePath.replace("~","");
                            var isimg = /\.(gif|jpg|jpeg|png|GIF|JPG|PNG)$/.test(fileUrl);
                             return '<a title="预览" target="_blank" href="' + (pvUrl ? pvUrl : fileUrl) + '">' +
                                '<img src="/Content/Images/viewxml.gif" title="' + rec.Title + '" />' +
                                '</a>';
                           /*
                            return '<a class="' + (isimg ? 'yoxviewLink' :'') + '" title="预览" ' + (!isimg?" target='_blank' ":"") + ' href="' + (pvUrl ? pvUrl : fileUrl) + '">' +
                                '<img src="/Content/Images/viewxml.gif" title="' + rec.Title + '" />' +
                                '</a>';*/


                         } 
                     }
                    ]],
            onBeforeLoad: function (row, param) {
            },
            <%if(pagation){ %>
            pagination: true,
            pageSize: 15,
            pageList: [10, 15, 20, 30],
            <%} %>
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,
            toolbar: [
            <%if((bool)ViewData.Eval("AllowEditAttachment")){ %>
                    {
                        text: permission.Create ? '单文件上传' : '',
                        iconCls: permission.Create ? 'icon-add' : "null",
                        handler: function () {
                            var param = '&TargetType=<%= ViewData.Eval("TargetType") %>&TargetID=<%= ViewData.Eval("TargetID") %>'
                            SetBackFunc(SaveAttachmentSuccess);
                            SetWin(480, 400, '<%=Url.Action("","Attachment", new{Area = "Admin"})%>/CreateAttachment?' + param, '上传文件');
                        }
                    },
                    <% if (ViewData.Eval("MutilUpload") != null && (bool)ViewData.Eval("MutilUpload")){%>
                    {
                        text: permission.Create ? '多文件上传' : '',
                        iconCls: permission.Create ? 'icon-add' : "null",
                        handler: function () {
                             var param = '?TargetType=<%= ViewData.Eval("TargetType") %>&TargetID=<%= ViewData.Eval("TargetID") %>'
                            SetBackFunc(SaveAttachmentSuccess);
                            SetWin(700, 400, '<%=Url.Action("MutilFileUpload","Attachment", new{Area = "Admin"})%>' + param, '上传多个文件');
                        }
                    },
                    <%} %>
//                    '-',
//                    {
//                        text: permission.Edit ? '修改' : '',
//                        iconCls: permission.Edit ? 'icon-edit' : "null",
//                        handler: function () {
//                            var id;
//                            id = getGridSelection('#tbAttachmentGrid', 'AttachmentID');

//                            if (id) {
//                                var param = '&TargetType=<%= ViewData.Eval("TargetType") %>&TargetID=<%= ViewData.Eval("TargetID") %>'
//                                SetBackFunc(SaveAttachmentSuccess);
//                                SetWin(480, 400, '<%=Url.Action("","Attachment", new{Area = "Admin"})%>/EditAttachment/?id=' + id + param, '修改备注信息');
//                            }
//                        }
//                    },
                    '-',
                    {
                        text: permission.Delete ? '删除' : '',
                        iconCls: permission.Delete ? 'icon-cut' : "null",
                        handler: function () {
                            //删除附件
                            DeleteAttachments();
                        }
                    },
                     '-',
                    <%} %>
                    {
                        text: permission.Edit ? '下载' : '',
                        iconCls: permission.Edit ? 'icon-save' : "null",
                        handler: function () {
                            var id;
                            id = getGridSelection('#tbAttachmentGrid', 'AttachmentID');

                            if (id) {
                                window.open('/Extenstion/DownloadAttachment.ashx?attachId=' + id);
                            }
                        }
                    }],
            onBeforeLoad: function () {
                RemoveForbidButton();
            },
            onLoadSuccess : function(){
                loadPreview();   
            }
        });
        }
        function SaveAttachmentSuccess(){
            loadAttachments();
        }
        function loadAttachments() {
            $('#tbAttachmentGrid').datagrid("reload");
        }

        function DeleteAttachments() {
            var ids = getGridSelection('#tbAttachmentGrid', 'AttachmentID');

            if (ids) {

                $.messager.confirm('Question', '确定要删除附件信息?', function (r) {
                    if (r) {
                        $.ajax({
                            type: "POST",
                            url: '<%=Url.Action("","Attachment", new{Area = "Admin"})%>/DeleteAttachment',
                            data: { delids: ids },
                            dataType: "text",
                            success: function (json) {
                                setTimeout("loadAttachments();", 500);
                                setTimeout("MsgShow('系统提示','删除成功。');", 1000);
                            }
                        });
                    }
                });
            }
        }
        
        function loadPreview(){
            if ($("#tbAttachmentGrid").datagrid("getRows").length > 0) {
                $(".yoxviewLink").colorbox({ rel: 'yoxviewLink', transition: "fade"});
            }
        }
</script>
<%} %>