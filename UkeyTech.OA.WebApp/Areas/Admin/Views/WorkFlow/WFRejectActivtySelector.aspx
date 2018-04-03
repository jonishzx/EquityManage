<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<List<LastFinishActivity>>" %>
<%@ Import Namespace="FireWorkflow.Net.Model.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    请选择退回到的业务活动
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#dvRejectToActivity > ul >li").click(function () {
                if (window.parent.rejectCallback != undefined ) {
                    window.parent.rejectCallback($(this).attr("activityId"));
                    CloseTheWin();
                }
            }).hover(function () {
                $(this).addClass("hover");
            }, function () {
                $(this).removeClass("hover");
            });
        });
    </script>
    <style type="text/css">
        #dvRejectToActivity
        {
            margin:5px 1% 0 1%;
            width: 95%;
        }
        #dvRejectToActivity ul
        {
            font-size: 15px;
        }
        #dvRejectToActivity li
        {
            color: #1D96E8;
            border: 1px solid #F0F0F0;
            margin: 2px;
            padding: 5px;
            cursor: pointer;
        }
        #dvRejectToActivity div
        {
            font-size: 12px;
            color: gray;
        }
        .hover
        {
            background-color: #EFEFEF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center" class="CenterForm">
        <div id="dvRejectToActivity">
            <ul>
                <%foreach (LastFinishActivity act in Model)
                  {%><li activityid='<%=act.Sn%>'>
              <%=act.DisplayName%><div>
                  <%=act.LastFinishActorName%>
                  <i><%=act.LastFinishTime %></i></div>
          </li>
                <%}%>
            </ul>
        </div>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" style="display:none;" icon="icon-reject" href="javascript:void(0)" onclick="window.parent.rejectCallback(null);CloseTheWin();"
            id="A1">直接退回到上一个步骤</a>
        <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)" onclick="CloseTheWin();"
            id="btnCancel">关闭</a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
