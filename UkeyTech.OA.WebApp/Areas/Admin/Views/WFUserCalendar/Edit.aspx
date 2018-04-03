<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="UkeyTech.OA.WebApp.Extenstion.EnchanceViewPage<FireWorkflow.Net.Engine.Impl.UserCalendar>" %>

<%@ Register Src="~/Areas/Admin/Views/Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Import Namespace="UkeyTech.OA.WebApp.Extenstion" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    用户日程信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
<script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>    

<style type="text/css">
.w110{width:110px !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center" class="CenterForm">
            <%using (Html.BeginForm())
              { %>
            <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="ym-form-fields">
                    <div class="form-message">
                        <%=ViewData["StateText"]%></div>
                    <div class="ym-fbox-text">
                        <label class=" required ">
                           <sup class="ym-required">*</sup> 日程名称：</label>
                        <div class="form-element">
                            <INPUT id="Name" name="Name" type="text" class="form-item-text easyui-validatebox" value="<%=Model.Name %>"
                                maxlength="50" required="true" />
                            <%= Html.ValidationMessage("Name")%></div>
                        <div class="form-clear-left">
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label class=" required ">
                           <sup class="ym-required">*</sup> 开始日期：</label>
                        <div class="form-element">
                            <INPUT id="BeginDate" name="BeginDate" type="text" class="form-item-text easyui-validatebox" value="<%=Model.BeginDate.ToString("yyyy-MM-dd HH:mm") %>"
                                maxlength="25" required="true" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',maxDate:'#F{$dp.$D(\'EndDate\',{d:0});}'})" />
                            <%= Html.ValidationMessage("BeginDate")%></div>
                        <div class="form-clear-left">
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label class=" required ">
                           <sup class="ym-required">*</sup> 结束日期：</label>
                        <div class="form-element">
                            <INPUT id="EndDate" name="EndDate" type="text" class="form-item-text easyui-validatebox" value="<%=Model.EndDate.ToString("yyyy-MM-dd HH:mm") %>"
                                maxlength="25" required="true" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',minDate:'#F{$dp.$D(\'BeginDate\',{d:0});}'})"/>
                            <%= Html.ValidationMessage("EndDate")%></div>
                        <div class="form-clear-left">
                        </div>
                    </div>
                    <div class="ym-fbox-text" style="display:none;">
                        <label class=" required ">
                           <sup class="ym-required">*</sup>日程类型：</label>
                        <div class="form-element">
                            <INPUT id="CalendarType" name="CalendarType" type="text" class="form-item-text easyui-validatebox"  
                            value="请假" maxlength="25" required="true" />
                            <%= Html.ValidationMessage("CalendarType")%></div>
                        <div class="form-clear-left">
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label class=" w110 required ">
                            备 注：</label>
                        <div class="form-element">
                            <textarea name="Descn" class="form-item-text textarea w300 easyui-validatebox" data-options="validType:'length[0,200]'"><%=Model.Comment%></textarea>
                            <div class="form-clear-left">
                            </div>
                        </div>
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
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
