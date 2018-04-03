<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Title="用户消息" %>

<%@ Register Assembly="UkeyTech.OA.FrameWork" Namespace="RepeaterInMvc.Codes" TagPrefix="MVC" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript">
        var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>';
        var permission = { Broswe: true, Edit: true, Create: true, Delete: true };
    </script>
    <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/EasyUI/locale/easyui-lang-zh_CN.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/Message/index.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Common.min.js")%>"></script>
    <script>
        function dosearch(value, name) {
            LoadMessageGrid(value, hiddenBoxId, hiddenBoxType);
        }
    </script>
</head>
<body>
    <ld:Loading ID="Loading1" runat="server" />
    <div class="easyui-layout">
        <div id="west" region="west" split="true" title="消息箱" style="width: 120px;">
            <ul id="tMessageBox">
            </ul>
        </div>
        <div id="center" region="center" style="width: auto;">
            <div class="easyui-layout">
                <div region="north" style="width: auto; height: 25px;">
                    <input id="SearchText" class="easyui-searchbox" searcher="dosearch" prompt="请输入搜索的内容"
                        style="width: 300px" />
                </div>
                <div region="center" style="width: auto;">
                    <table id="tbMessage">
                    </table>
                </div>
                <div region="south" split="true" style="width: auto; height: 250px; overflow: hidden;position:relative;">
                    <iframe id="messageContent" name="messageContent" src="" width="100%" frameborder="0">
                    </iframe>
                </div>
            </div>
        </div>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
    <script type="text/javascript" language="javascript">
        $(".easyui-layout").height($(document).height() - 1);
        var baseurl = '<%=Url.Action("","Message")%>';
        var boxtree = <%=ViewData["BoxTreee"]%>;
    </script>
</body>
</html>
