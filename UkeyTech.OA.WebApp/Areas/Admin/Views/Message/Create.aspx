<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<Clover.Message.Model.Message>" %>
<%@ Register src="~/Areas/Admin/Views/Shared/PopupWin.ascx" tagname="PopupWin" tagprefix="uc2" %>
    <%@ Import Namespace="UkeyTech.OA.WebApp" %>
    <%@ Import Namespace="Clover.Message.DAO" %>
     <%@ Import Namespace="Clover.Message.Model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    消息内容
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        var permission = { Broswe: true, Edit: true, Create: true, Delete: true };
        $(function () {

            //initAttachmentGrid();
        });
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="center">
        <%using (Html.BeginForm())
          { %>
        <div class="ym-form linearize-form ym-columnar">
            <div class="form-message">
                <%=ViewData["StateText"]%></div>
            <div class="ym-form-fields">
                <div class="ym-fbox-text">
                   <label for="Title">标题：<sup class="ym-required">*</sup></label>
                    <input id="Title" name="Title" type="text"  required="true"
                    class="form-item-text easyui-validatebox" value="<%= Model.Title %>"  maxlength="50" />
                    <input type=hidden id="MessageId" name="MessageId" value="<%=Model.MessageId %>" />    
                </div>
                <div class="ym-fbox-text">
                   <label for="Receivers">收件人：<sup class="ym-required">*</sup></label>
                      <%Html.RenderPartial(Helper.PopupControlPath, 
                          new ViewDataDictionary(new { 
                              MutilSelect = true,
                              IDControlName = "Receivers", 
                              Required = true,
                              TextControlName = "ReceiversName",
                              DictID = "ValidUser",
                              Value = Model.Receivers,
                              CallBack = "nothinghappen",
                              width = "600"}));%>
                </div>
                
                <div class="ym-fbox-text">
                    <label for="MessageBody">信息内容：<sup class="ym-required">*</sup></label>
                    <textarea id="MessageBody" name="MessageBody" type="text" style="height:300px;" required="true"
                    class="form-item-text easyui-validatebox"><%= Model.MessageBody %></textarea>                                      
                </div>
                
                <div class="ym-fbox-text w50p" style="display:none;">
                    <label for="BeginDateTime">开始日期：</label>
                    <input id="BeginDateTime" name="BeginDateTime" type="text" onfocus="WdatePicker();"
                    class="form-item-text easyui-validatebox Wdate" value="<%= Model.BeginDateTime != null ? Model.BeginDateTime.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd")  %>" maxlength="4" />                                      
                </div>
                
                <div class="ym-fbox-text w50p" style="display:none;">
                    <label for="EndDateTime">结束日期：</label>
                    <input id="EndDateTime" name="EndDateTime" type="text" onfocus="WdatePicker();"
                    class="form-item-text easyui-validatebox Wdate" value="<%= Model.EndDateTime != null ? Model.EndDateTime.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd")  %>" maxlength="4" />                                      
                </div>
                
                <%if (!string.IsNullOrEmpty(Model.TargetId) && !string.IsNullOrEmpty(Model.TemplateCode))
                  {
                      var templatedao = new MessageTemplateDAO();
                      var template = templatedao.GetModel(Model.TemplateCode);
                      if (template == null) return; 
                %>
                <%if(!string.IsNullOrEmpty(template.MessageAction)) {%>
                <div class="ym-fbox-text">
                  <label for="Receivers">关联的业务信息：</label>
                  <iframe src="<%=template.MessageAction  +  Request["TargetId"]%>" width="100%" height="300px;" frameborder=0></iframe>
                </div>
                <%} %>
                <%if (!string.IsNullOrEmpty(template.OperationAction))
                  {%>
                <div class="ym-fbox-text">
                  <label for="Receivers">关联的业务操作：</label>
                  <iframe src="<%=template.OperationAction  +  Request["TargetId"]%>" width="100%" height="300px;" frameborder=0></iframe>
                </div>
                <%} %>
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
           <%} %>
        </div>
        <div region="south" border="false" class="SouthForm form-action">
           <a class="easyui-linkbutton" icon="icon-save" href="#" onclick="SubmitForm('Save');" id="A2">
                保存</a> 
            <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm('Send');" id="A1">
                发送</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                    onclick="CloseTheWin();" id="btnCancel">取消</a>
        </div>
     <uc2:PopupWin ID="PopupWin1" runat="server" />

</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
