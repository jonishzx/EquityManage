<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="UkeyTech.WebFW.DAO" %>
<%@ Import Namespace="UkeyTech.WebFW.Model" %>
<%@ Import Namespace="UkeyTech.OA.WebApp.Extenstion" %>
<%@ Import Namespace="Clover.Web.Core" %>
<%@ Import Namespace="StructureMap" %>
<%if (ViewData.Eval("DictID") != null && ViewData.Eval("IDControlName") != null && ViewData.Eval("TextControlName") != null)
  { 
	var sort = ViewData.Eval("Sort")!=null? System.Web.HttpContext.Current.Server.UrlEncode(ViewData.Eval("Sort").ToString()):"";
    var filter = ViewData.Eval("Filter") != null ? Server.UrlEncode(ViewData.Eval("Filter").ToString()) : "";
	var validselection = ViewData.Eval("ValidSelection")!=null? ViewData.Eval("ValidSelection"):"";
    var modal = ViewData.Eval("Modal") != null && (bool)ViewData.Eval("Modal");
    var winWidth = ViewData.Eval("WinWidth") != null ? ViewData.Eval("WinWidth").ToString() : "500";
    var winHeight = ViewData.Eval("WinHeight") != null ? ViewData.Eval("WinHeight").ToString() : "430";
    var istree = ViewData.Eval("TreeView") != null && (bool)ViewData.Eval("TreeView"); 
    var popUpView = ViewData.Eval("PopUpView");
	var addParams = ViewData.Eval("AddParams");
    var boxmode = ViewData.Eval("BoxMode");
    var allowEdit = ViewData.Eval("AllowEdit") != null && (bool)ViewData.Eval("AllowEdit");
    var val = ViewData.Eval("Value") != null ? ViewData.Eval("Value").ToString() : "";
    if(istree)
        popUpView = Url.Action("PopupTreeGrid", "Utility", new { Area = "Admin" });
  %>
<script type="text/javascript">
    function pop<%=ViewData.Eval("IDControlName") %>(){
		   var urlparam = "";
		   if(typeof(get<%=ViewData.Eval("IDControlName") %>Param) !== "undefined"){
				urlparam = get<%=ViewData.Eval("IDControlName") %>Param();
		   }
           var url = "";
           <% if (popUpView != null)
              { %>
            url = '<%=popUpView%>'; 
           <% }else{ %>
            url = '<%=Url.Action("PopupMutilSelectGrid","Utility",new {Area ="Admin" } )%>';
            <% } %>  
        url += "?<%=ViewData.Eval("MutilSelect") != null && (bool)ViewData.Eval("MutilSelect") ? "Mutil=1&" : "" %><%=ViewData.Eval("SelectAll") != null && (bool)ViewData.Eval("SelectAll") ? "SelectAll=1&" : ""%>TypeId=<%=ViewData.Eval("DictID") %>&ID=<%=ViewData.Eval("IDControlName") %>&Name=<%=ViewData.Eval("TextControlName") %>&Filter=<%=filter%>&callback=<%=ViewData.Eval("CallBack") %>&sort=<%=sort%>&validselection=<%=validselection%>&Params=<%=addParams%>" + urlparam;
            
//like [{Label:'名称',Type:'类型(DateTime|checkbox|Text|Int|ConstDict|SQLDict)',Field:"字段名", DictID:"字典ID"}]
           <%if (ViewData.Eval("GetParmsSMethod") != null)
             {%>
            var p = <%=ViewData.Eval("GetParmsSMethod") %>;
            if(!p)
                return;
            url += p;
            <%} %>
           <%if (ViewData.Eval("ModuleCode") != null)
             {%>
            url += "&ModuleCode=<%=ViewData.Eval("ModuleCode")%>";
            <%} %>
             <%if (ViewData.Eval("FunctionCode") != null)
               {%>
            url += "&FunctionCode=<%=ViewData.Eval("FunctionCode")%>";
           <%} %>
		    <% if (modal)
		       { %>
				openModalWin(url,<%= winWidth %> , <%= winHeight %>); 
			<% }else{ %>
				SetWin(<%= winWidth %> , <%= winHeight %>, url,"数据选择", false); 
            <% } %>
        }
        function clear<%=ViewData.Eval("IDControlName") %>(){
        $("#<%=ViewData.Eval("TextControlName") %>").val('');
            $("#<%=ViewData.Eval("IDControlName") %>").val('');
        }
    
</script>
<%   
               string rst = ViewData.Eval("TextValue") != null ? ViewData.Eval("TextValue").ToString() : "";
               var enabled = ViewData.Eval("Enabled") == null || (ViewData.Eval("Enabled") != null && (bool)ViewData.Eval("Enabled"));
               if (ViewData.Eval("TextValue") == null && ViewData.Eval("Value") != null && ViewData.Eval("DictID") != null)
               {
                   var _webcontext = ObjectFactory.GetInstance<IWebContext>();
                   DictionaryDAO dal = ObjectFactory.GetInstance<DictionaryDAO>();
				   if(addParams == null){
						rst = dal.GetSQLDictTextByValue(_webcontext, ViewData.Eval("DictID").ToString(), val);
					}else{
					   var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>("{" + addParams.ToString() + "}");
                       rst = dal.GetSQLDictTextByValue(_webcontext, ViewData.Eval("DictID").ToString(), dict,  val);
					}
               }%>
<span class="popupcontrol" style="width:<%=ViewData.Eval("Width") != null ? ViewData.Eval("Width") : "100"%>px !important; height: <%=ViewData.Eval("Height") != null ?(int.Parse(ViewData.Eval("Height").ToString())+2).ToString() : "22"%>px !important;">
    <% if (boxmode == "MutilLine")
       { %>
    <textarea class="<%= ViewData.Eval("Enabled") != null && !(bool) ViewData.Eval("Enabled") ? "ym-readonly" : "" %> <%= ViewData.Eval("Required") != null && (bool) ViewData.Eval("Required") ? "easyui-validatebox" : "" %>"
        style="width:<%= ViewData.Eval("Width") != null ? (int.Parse(ViewData.Eval("Width").ToString()) - (enabled ? 42 : 0)).ToString() : (100 - (enabled ? 42 : 0)).ToString() %>px !important;height:<%= ViewData.Eval("Height") != null ? ViewData.Eval("Height") : "20" %>px !important;float: left;"
        <%= ViewData.Eval("Required") != null && (bool) ViewData.Eval("Required") ? "required=\"true\"" : "" %>
        <%=allowEdit ? "":"readonly=\"readonly\"" %> type="text" id="<%= ViewData.Eval("TextControlName") %>" name="<%= ViewData.Eval("TextControlName") %>"><%= rst %></textarea>
    <% }
       else
       { %>
        <input class="<%= ViewData.Eval("Enabled") != null && !(bool) ViewData.Eval("Enabled") ? "ym-readonly" : "" %> <%= ViewData.Eval("Required") != null && (bool) ViewData.Eval("Required") ? "easyui-validatebox" : "" %>"
               style="width:<%= ViewData.Eval("Width") != null ? (int.Parse(ViewData.Eval("Width").ToString()) - (enabled ? 42 : 0)).ToString() : (100 - (enabled ? 42 : 0)).ToString() %>px !important;height:<%= ViewData.Eval("Height") != null ? ViewData.Eval("Height") : "20" %>px !important;float: left;"
            <%= ViewData.Eval("Required") != null && (bool) ViewData.Eval("Required") ? "required=\"true\"" : "" %>
               <%=allowEdit ? "":"readonly=\"readonly\"" %> type="text" value="<%= rst %>" id="<%= ViewData.Eval("TextControlName") %>" name="<%= ViewData.Eval("TextControlName") %>" />
    <% } %> 
    <%= Html.ValidationMessage(ViewData.Eval("IDControlName").ToString())%>
    <%if (enabled)
      {%>
    <div class="popuptool" style="float: right;">
        <a ptype="pop" href="javascript:void(0);" style="margin-left: 0px;" class="icon icon-search2 pop" title="选择" onclick="pop<%=ViewData.Eval("IDControlName") %>()"></a>
        <a ptype="clrpop" href="javascript:void(0);" class="icon icon-del2 pop" title="清空" onclick="clear<%=ViewData.Eval("IDControlName") %>()"></a>
    </div>
    <br style="clear: both;">
    <%} %>
    <input type="hidden" value="<%=ViewData.Eval("Value") %>" <%=ViewData.Eval("Required") != null && (bool)ViewData.Eval("Required") ? "required=\"true\"" : ""%> id="<%=ViewData.Eval("IDControlName") %>" name="<%=ViewData.Eval("IDControlName") %>" />
</span>
<%} %>