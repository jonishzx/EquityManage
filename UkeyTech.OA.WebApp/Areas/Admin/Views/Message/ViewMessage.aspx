<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<Clover.Message.Model.Message>" %>
    <%@ Import Namespace="UkeyTech.OA.WebApp" %>
    <%@ Import Namespace="Clover.Message.DAO" %>
     <%@ Import Namespace="Clover.Message.Model" %>
<%@ Register Src="~/Areas/Admin/Views/Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    消息内容
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        var permission = { Broswe: true, Edit: true, Create: true, Delete: true };
        $(function () {

           // initAttachmentGrid();
        });
      
    </script>
    <style type="text/css">
        #MessageStatusLog{float:left;}
        .shortcontent{color:#000;margin:0 5px 0 5px;}
        * {font-size:12px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="center" style="position:relative;">
        <%using (Html.BeginForm())
          { %>
        <div class="ym-form linearize-form ym-columnar workflowform ">
            <div class="form-message">
                <%=ViewData["StateText"]%></div>
            <div class="ym-form-fields">
                <div class="ym-fbox-text">
                   <label for="Title">标题：</label>
                   <%= Model.Title %>
                </div>
                <div class="ym-fbox-text">
                   <label for="SenderName">发件人：</label>
                   <%=Model.CreatorName%>
                </div>
                <div class="ym-fbox-text">
                   <label for="Receivers">收件人：</label>
                    <%=UkeyTech.WebFW.DAO.AdminDAO.getAdminNames(Model.Receivers)%>
                </div>
                <%if (ViewData["BoxMessage"] != null)
                  { %>
                 <div class="ym-fbox-text">
                    <label for="Receivers">收件情况：</label>
                    <%Html.RenderPartial("MessageReadStatus", ViewData["BoxMessage"]);%>
                 </div>
                 <%} %>
                 <div class="ym-fbox-text">
                   <label for="SendTime" >发送时间：</label>
                   <span class="spleft shortcontent"><%= Model.SendTime %></span>
                   <label for="StatusName" style="width:100px !important;">状态：</label>
                   <span class="spleft shortcontent"><%= Model.StatusName%></span>
                </div>
                 <div class="ym-fbox-text">
                    <label for="MessageBody">信息内容：</label>
                    <div class="spleft">
                        <%= Model.MessageBody %>
                    </div>                                   
                </div>
                <%if (!string.IsNullOrEmpty(Model.TargetId))
                  {                   
                %>
                <%if (!string.IsNullOrEmpty(Model.MessageAction))
                  {%>
                <div class="ym-fbox-text">
                  <label for="Receivers">关联的业务信息：</label>
                  <a href="javascript:void(0)" target="" class="easyui-linkbutton" icon="icon-ok" onclick="$('#MessageAction').attr('src','<%=Model.OperationAction.Replace("~/","/")   + Model.TargetId%>');$(this).hide()">打开</a>
                  <iframe id="MessageAction" name="MessageAction" src="#" width="100%" height="300px;" frameborder=0></iframe>
                </div>
                <%} %>
                <%if (!string.IsNullOrEmpty(Model.OperationAction))
                  {%>
                <div class="ym-fbox-text">
                  <label for="Receivers">关联的业务操作：</label>
                  <!--<iframe src="<%=Model.OperationAction.Replace("~/","/")   + Model.TargetId%>" width="100%" height="300px;" frameborder=0></iframe>-->
                  <a href="javascript:void(0)" class="easyui-linkbutton" icon="icon-ok" onclick="SetWinWithMaxSize('消息操作', '<%=Model.OperationAction.Replace("~/","/")   + Model.TargetId%>');">打开</a>
                </div>
                <%} %>
                <%} %>
               <%if ((string.IsNullOrEmpty(Request["mode"]) || Request["mode"] != "inframe") 
                     && (Model.NeedRead.HasValue && Model.NeedRead.Value))
               { %>
                <div class="ym-fbox-text borderBottom">
                <label for="ReadComment">已读批注：</label>
                <input type=text style="width:200px;" value="已阅" id="ReadComment" name="ReadComment" class="form-item-text easyui-validatebox spleft"/>
                <span class="spleft">常用批注：</span>
                <select class="spleft" style="width:80px;" onchange="$('#ReadComment').val($(this).val())">
                    <option>已阅</option>
                    <option>已批</option>
                    <option>同意</option>
                    <option>接受</option>
                    <option>不同意</option>
                    <option>不接受</option>
                </select>
               
                </div>
                <%} %>
                  <%if ((string.IsNullOrEmpty(Request["mode"]) || Request["mode"] != "inframe")
                        && (Model.NeedAccept.HasValue && Model.NeedAccept.Value))
               { %>
                <div class="ym-fbox-text borderBottom">
                <label for="OpComment">任务批注：</label>
                <input type=text style="width:200px;"  value="同意" id="OpComment" name="OpComment" class="form-item-text easyui-validatebox spleft"/>
                <span class="spleft"> 常用批注</span>
                <select style="width:80px;" class="spleft" onchange="$('#OpComment').val($(this).val())">
                    <option>同意</option>
                    <option>接受</option>
                    <option>不同意</option>
                    <option>不接受</option>
                </select>
                </div>
                <%} %>
            </div>
            <%} %>
            <div class="ym-fbox-text">
                    <% Html.RenderPartial(Helper.AttachmentExPath,
                            new ViewDataDictionary(new
                                {
                                    AllowEditAttachment = string.IsNullOrEmpty(Model.Status),
                                    TargetID = Model.MessageId,
                                    TargetType = "Message",
                                    ActionName = "GetMessageList",
                                    MutilUpload = false,
                                    AutoHide = false,
                                    AutoLoad = true,
                                    Title = "消息附件"
                                })); %>
                </div>
        </div>
        </div>
        <%if(string.IsNullOrEmpty(Request["mode"]) || Request["mode"] != "inframe") {%>
        <div region="south" border="false" class="SouthForm form-action">
            <%if (Model.NeedRead.HasValue && Model.NeedRead.Value)
              { %>
            <a class="easyui-linkbutton" icon="icon-next" href="#" onclick="SubmitForm('Read');" id="A2">
             标记为已读</a> 
            <%} %>
            <%if (Model.NeedAccept.HasValue && Model.NeedAccept.Value)
              { %>
             <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="if(confirm('你确定已处理该事项'))SubmitForm('Accept');" id="A3">
                处理</a> 
             <a class="easyui-linkbutton" icon="icon-reject" href="#" onclick="if(confirm('你确定拒绝处理该事项?'))SubmitForm('Reject');" id="A4">
                拒绝</a>
             <%} %>
             <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                    onclick="CloseTheWin();" id="btnCancel">关闭</a>
        </div>
        <%} %>
           <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
