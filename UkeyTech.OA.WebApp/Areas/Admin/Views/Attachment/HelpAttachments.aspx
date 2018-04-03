<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="帮助文档" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        var pvUrlAddr = '/Extenstion/PreviewAttachment.ashx?attachId=';
         function init() {
             loadDataGrid();
         }

        $(document).ready(function () {
            //权限获取
            LoadPageModuleFunction("HelpAttachments", init);
        });
        function loadDataGrid() {
            $('#tbAttachGrid').datagrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: true,
                remoteSort: false,
                url: '<%= Url.Action("GetAttachmentList", "Attachment", new {Area = "Admin"})%>',
                queryParams: { TargetID: "HelpAttachments", TargetType: "HelpAttachments", page: 1, rows: 100000 },
                //idField: 'AttachmentID',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'Title', title: '标题', width: 250, align: 'left', sortable: true },
                    { field: 'FileName', title: '文件名称', width: 250, align: 'left', sortable: true },
                    { field: 'TargetType', title: '类型', width: 60, align: 'left', sortable: true,
                        formatter: function (value, rec) {
                            return rec.FileName.toString().match(/\.(\w)+$/)[0];
                        }
                    },
                    { field: 'Bytes', title: '文件大小', width: 80, align: 'left', sortable: true,
                        formatter: function (value, rec) {
                            return value > 1024 ? Math.round(value / 1024) + "kb" : value + "字节";
                        }
                    },
                    { field: 'abk', title: '预览', width: 40, align: 'center',
                        formatter: function (value, rec) {
                            var pvUrl = pvUrlAddr + rec.AttachmentID;
                            return '<a title="预览" target="_blank" href="' + pvUrl + '">' +
                                '<img src="/Content/Images/viewxml.gif" title="' + rec.Title + '" />' +
                                '</a>';
                        }
                    }
                ]],
                onBeforeLoad: function (row, param) {
                },
                rownumbers: true,
                singleSelect: false,
                toolbar: [
                    {
                        text: permission.Create ? '单文件上传' : '',
                        iconCls: permission.Create ? 'icon-add' : "null",
                        handler: function () {
                            var param = '&TargetType=HelpAttachments&TargetID=HelpAttachments';
                            SetBackFunc(function () { $('#tbAttachGrid').datagrid("reload"); });
                            SetWin(480, 400, '<%= Url.Action("", "Attachment", new {Area = "Admin"}) %>/CreateAttachment?' + param, '上传文件');
                        }
                    },
                     '-',
                    {
                        text: permission.Create ? '多文件上传' : '',
                        iconCls: permission.Create ? 'icon-add' : "null",
                        handler: function () {
                            var param = '?TargetType=HelpAttachments&TargetID=HelpAttachments';
                            SetBackFunc(function () { $('#tbAttachGrid').datagrid("reload"); });
                            SetWin(700, 400, '<%= Url.Action("MutilFileUpload", "Attachment", new {Area = "Admin"}) %>' + param, '上传多个文件');
                        }
                    },
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
                    {
                        text: '下载',
                        iconCls: 'icon-save',
                        handler: function () {
                            var id;
                            id = getGridSelection('#tbAttachGrid', 'AttachmentID');

                            if (id) {
                                window.open('/Extenstion/DownloadAttachment.ashx?attachId=' + id);
                            }
                        }
                    }],
                    onBeforeLoad: function () {
                        RemoveForbidButton();
                    }
            });
            }

            function DeleteAttachments() {
                var ids = getGridSelections('#tbAttachGrid', 'AttachmentID');

                if (ids) {

                    $.messager.confirm('Question', '确定要删除附件信息?', function (r) {
                        if (r) {
                            $.ajax({
                                type: "POST",
                                url: '<%=Url.Action("","Attachment", new{Area = "Admin"})%>/DeleteAttachment',
                                data: { delids: ids },
                                dataType: "text",
                                success: function (json) {
                                    setTimeout("loadDataGrid();", 500);
                                    setTimeout("MsgShow('系统提示','删除成功。');", 1000);
                                }
                            });
                        }
                    });
                }
            }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div id="center" region="center" title="" style="width: auto;">
        <table id="tbAttachGrid">
        </table>
    </div>    
    
    
</asp:Content>
