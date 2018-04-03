<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/CustomError.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    系统异常提示
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    系统访问失败了,你可以选择<a href="javascript:window.history.back(-1);">返回</a>。
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function toogle() {
            var d = document.getElementById("dvDetail");
            if (d.style.display == "none") {
                d.style.display = "block";
            }
            else {
                d.style.display = "none";
            }
        }
    </script>
    <%if (Model != null)
      {
          var m = (HandleErrorInfo)Model; %>
    <div id="errContainer">
        <div>
            <p style="color:Blue;">
                <label>
                    提示：</label><%=m.Exception.Message%>【<a href="#" onclick="javascript:toogle()">详情</a>】
            </p>
            <p  style="color:Blue;">
                <label>
                    访问的页面：</label><span style="text-decoration: underline;word-break: break-word;word-wrap: break-word;"><%=TempData["QueryPath"]%></span>
            </p>
        </div>
        <div id="dvDetail" style="display: none;width:100%;height:120px;overflow:auto;">
            <fieldset>
                <legend>异常详情</legend>
                <div>
                    <label>
                        控制器：</label>
                        <div ><%=m.ControllerName%></div>
                </div>
                <div>
                    <label>
                        操作：</label>
                        <div style="word-break: break-word;word-wrap: break-word;"><%=m.ActionName%></div>
                </div>
                <div>
                    <label>
                        跟踪信息：</label>
                    <div style="word-break: break-word;word-wrap: break-word;">
                        <%=m.Exception.StackTrace%>
                    </div>
                </div>
                <%if (m.Exception.InnerException != null)
                  {%>
                  <div>
                    <label>
                        内部错误：</label>
                    <div style="word-break: break-word;word-wrap: break-word;">
                        <%=m.Exception.InnerException.Message%>
                    </div>
                    <div style="word-break: break-word;word-wrap: break-word;">
                        <%=m.Exception.InnerException.StackTrace%>
                    </div>
                </div>
                <%} %>
            </fieldset>
        </div>
        <div style="color: Red;">
            请跟管理员报告改问题，管理员会尽快处理，谢谢！</div>
    </div>
    <%}
      else if (HttpContext.Current.AllErrors != null && HttpContext.Current.AllErrors.Length > 0)
      { %>
    <div>
        <%foreach(var ex in HttpContext.Current.AllErrors){ %>
            <p> <label>提示：</label><%=ex.Message %></p>
            <p> <label>跟踪信息：</label><%=ex.StackTrace%></p>
        <%} %>
    </div>
    <%} %>
</asp:Content>
