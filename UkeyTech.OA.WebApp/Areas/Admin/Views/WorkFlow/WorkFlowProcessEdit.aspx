<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<FireWorkflow.Net.Engine.Definition.WorkflowDefinition>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">  
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
                    <label for="Name">
                        流程代码:<sup class="ym-required">*</sup></label>
                    <input name="Name" type="text" class="form-item-text easyui-validatebox" value="<%=Model.Name %>"
                        maxlength="25" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("Name")%>
                </div>
                 <div class="ym-fbox-text">
                    <label for="DisplayName">
                        流程名称:<sup class="ym-required">*</sup></label>
                    <input name="DisplayName" type="text" class="form-item-text easyui-validatebox" value="<%=Model.DisplayName %>"
                        maxlength="25" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("DisplayName")%>
                </div>
                <div class="ym-fbox-text">
                    <label for="State">
                    是否已发布:</label>
                    <input type="checkbox" class="checkbox" id="State" name="State" value='true'
                        <%=Model.State ? "checked" : ""%>  />
                </div>
                <div class="ym-fbox-text">
                    <label for="Description">
                        说明:</label>
                    <textarea name="Description"><%=Model.Description%></textarea>
                    <div class="form-clear-left">
                    </div>
                </div>
            </div>
            <%} %>
        </div>
        <div region="south" border="false" class="SouthForm form-action">
            <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
                确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                    onclick="CloseTheWin();" id="btnCancel">取消</a>
        </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">

</asp:Content>
