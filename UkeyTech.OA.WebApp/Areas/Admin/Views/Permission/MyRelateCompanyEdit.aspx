<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master" Inherits="UkeyTech.WebOA.EnchanceViewPage<UkeyTech.WebFW.Bussiness.Model.MyRelateCompany>" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Register src="~/Areas/Admin/Views/Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>
<%@ Register src="~/Areas/Admin/Views/Shared/PopupWin.ascx" tagname="PopupWin" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	关联公司-<%=ViewData["EditStatus"] %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script> 
    <style type="text/css">
        .wp50combo label, .wp100combo label
        {
            width: 90px !important;
        }       
       
        .w100p label,.w50p label,.w30p label{width:90px !important;}
    </style> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
    <ld:Loading ID="Loading1" runat="server" />
    <div region="center" border="false" class="CenterForm" style="position:relative;left:0;top:0;">  
        <div align="center" region="center">
            <%using (Html.BeginForm())
              { %>
            <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="ym-form-fields">
                     <div class="ym-fbox-text w50p">
                        <label for="MyCompanyName">
                            企业名称：</label>                        
                            <input name="MyCompanyName" type="text" class="form-item-text easyui-validatebox" value="<%=Model.MyCompanyName %>" 
                                maxlength="200"  required="true"/>
                            <%= Html.ValidationMessage("MyCompanyName")%>                        
                     </div>
                    
                     <div class="ym-fbox-text w50p">
                        <label for="ShortName">
                            简称：</label>                        
                            <input name="ShortName" type="text" class="form-item-text easyui-validatebox" value="<%=Model.ShortName %>" 
                                maxlength="200"  required="true"/>
                            <%= Html.ValidationMessage("ShortName")%>                        
                        </div>
                    
                </div>
            </div>
            <%} %>        
    </div>

    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" 
                id="btnCancel">取消</a>
    </div>

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="FootBlock" runat="server">
<uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
