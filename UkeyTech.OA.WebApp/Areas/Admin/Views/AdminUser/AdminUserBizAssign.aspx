<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="用户流程任务委托(管理员)" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.edatagrid.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/UserBizAssignForAdmin.js")%>?v=1.0"></script>
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","AdminUser")%>';


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="Div1" region="west" title="用户列表" style="width: 300px;">
        <div class="easyui-layout" fit="true">
            <div region="north" style="height: 35px;">
                <div class="SearchDiv">
                    代码或名称：<input type="text" id="txtSelNameOrCode" style="width: 120px;" />
                    <a href="javascript:void(0);" id="btnSubmit" class="easyui-linkbutton" icon="icon-search" onclick="querySelUserData();">
                        查询</a>
                </div>
            </div>
            <div region="center">
                <table id="SelUserGrid">
                </table>
            </div>
        </div>
    </div>
    <div id="center" region="center" title="业务列表" style="width: 300px;">
        <table id="DataGrid">
        </table>
    </div>
    <div id="right" region="east" title="用户委托列表" style="width: 300px">
        <table id="UserAssignGrid">
        </table>
    </div>
   <div id="AllDateAlter" style="display: none;">
        <div region="north" id="dvTitleMessage" class="NorthForm form-action" 
        style="font-size:15px;font-weight:bold;height:20px;padding:10px;text-align:center;">            
            
        </div>
        <div region="center" class="form-action">
            <div>
               <label style="padding:0 10px 0 10px;">委托开始日期：</label> <input id="AllStartAppDate" name="AllStartAppDate" type="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});" style="width: 130px !important"
                    class="form-item-text Wdate" value="<%= DateTime.Now.ToString("yyyy-MM-dd 00:00")%>"
                    maxlength="16" />
           </div>
           <div>
               <label style="padding:0 10px 0 10px;">委托结束日期：</label>  <input id="AllEndAppDate" name="AllEndAppDate" type="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});" style="width: 130px !important"
                    class="form-item-text Wdate" value="<%= DateTime.Now.AddDays(3).ToString("yyyy-MM-dd 18:00")%>"
                    maxlength="16" />
            </div>
        </div>
        <div region="south" border="false" style="height: 30px;padding:20px 0 0 60px;">
             <a class="easyui-linkbutton" icon="icon-ok" href="#" id="alterSubmit">确定</a> <a class="easyui-linkbutton"
                icon="icon-cancel" href="javascript:void(0)" onclick="CloseAlterDateDialog();" id="A2">
                取消</a>
        </div>
   </div>
    <div id="Dialog" style="display: none;">
        <%-- <div class="easyui-layout" fit="true">--%>
        <div region="north" style="height: 35px;">
            <div class="SearchDiv">
                代码或名称：<input type="text" id="adminname" style="width: 120px;" />
                <a href="javascript:void(0);"  class="easyui-linkbutton" icon="icon-search" onclick="queryUserData();">
                    查询</a>
            </div>
        </div>
        <div region="center" id="userlist" style="height: 350px;">
            <table id="UserGrid" fit="true">
            </table>
        </div>
        <div region="south" border="false" style="height: 70px;">
            <div class="SouthForm form-action">
                <div>
                    委托开始日期：<input id="StartAppDate" name="StartAppDate" type="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                        style="width: 130px !important" class="form-item-text Wdate" value="<%= DateTime.Now.ToString("yyyy-MM-dd 00:00") %>"
                        maxlength="10" />
                    ~ 委托结束日期：<input id="EndAppDate" name="EndAppDate" type="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});"
                        style="width: 130px !important" class="form-item-text Wdate" value="<%= DateTime.Now.AddDays(3).ToString("yyyy-MM-dd 18:00")%>"
                        maxlength="10" />
                </div>
                <a class="easyui-linkbutton" icon="icon-ok" href="#" id="submit">确定</a> <a class="easyui-linkbutton"
                    icon="icon-cancel" href="javascript:void(0)" onclick="CloseDialog();" id="btnCancel">取消</a>
            </div>
        </div>
        <%-- </div>--%>
    </div>
</asp:Content>
