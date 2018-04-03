<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Calendar.Model.WorkWeekCalendar>" Title="WorkWeekCalendar" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"%>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
     <script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="center" border="false" class="CenterForm" style="position:relative;left:0;top:0;">       
        <%using (Html.BeginForm()){%>
        <div class="ym-form linearize-form ym-columnar zcolumn">
            <div class="ym-form-fields">
                 <div class="ym-fbox-select w50p">
                    <label for="YearNum"><sup class="ym-required">*</sup>年：</label>
                    <input id="YearNum" name="YearNum" type="text" 
                    class="form-item-text easyui-validatebox"   value="<%= Model.YearNum %>" />                                      
                </div>  
                <div class="ym-fbox-select w50p">
                    <label for="WeekNum"><sup class="ym-required">*</sup>周：</label>
                    <input id="WeekNum" name="WeekNum" type="text" 
                    class="form-item-text easyui-validatebox"   value="<%= Model.WeekNum %>" />                                      
                </div> 
               
                <div class="ym-fbox-text w50p">
                    <label for="StartDate">本周开始时间：</label>
                    <input id="StartDate" name="StartDate" type="text" onfocus="WdatePicker({isShowWeek:true,onpicked:function() {$('#WeekNum').val($dp.cal.getP('W','WW'));$('#YearNum').val($dp.cal.getP('y','yyyy'));$('#WeekRange').val($('#StartDate').val());}});"
                    class="form-item-text easyui-validatebox Wdate" value="<%= Model.StartDate != null ? Model.StartDate.Value.ToString("yyyy-MM-dd") : ""  %>" maxlength="10" />                                      
                </div>
                <div class="ym-fbox-text w50p">
                    <label for="EndDate">本周结束时间：</label>
                    <input id="EndDate" name="EndDate" type="text" onfocus="WdatePicker({isShowWeek:true,onpicked:function() {$('#WeekRange').val( $('#WeekRange').val() + '~' + $('#EndDate').val());}});"
                    class="form-item-text easyui-validatebox Wdate" value="<%= Model.EndDate != null ? Model.EndDate.Value.ToString("yyyy-MM-dd") : ""  %>" maxlength="10" />                                      
                </div>
                <div class="ym-fbox-select w100p">
                    <label for="WeekRange">日期范围：</label>
                    <input id="WeekRange" name="WeekRange" type="text" 
                    class="form-item-text easyui-validatebox"   value="<%= Model.WeekRange %>" />                                      
                </div> 
                <div class="ym-fbox-text w50p" style="margin-top: 2px;">
                    <label for="IsWorkWeek">是否工作周：</label>
                     <%var radioButtonListStatus = new SelectList(new List<ListItem> {
                                    new ListItem { Text = "是", Value="1", Selected=true},
                                    new ListItem { Text = "否", Value="0",}}, "Value", "Text", Model.IsWorkWeek);
                        var htmlAttributesStatus = new Dictionary<string, object> {
                                    { "class", "radioButtonListStatus" }, 
                                    { "style", "width:20px !important;" }                          
                        };
                        foreach (var radiobutton in radioButtonListStatus)
                        { %>
                        <%=Html.RadioButton("IsWorkWeek", radiobutton.Value, radiobutton.Selected, htmlAttributesStatus)%>
                        <label style="text-align: left;width:30px !important;">
                            <%=radiobutton.Text%></label>
                        <% } %> 
                </div> 
            </div>
        </div>
        <%}%>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" runat="server" id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>

