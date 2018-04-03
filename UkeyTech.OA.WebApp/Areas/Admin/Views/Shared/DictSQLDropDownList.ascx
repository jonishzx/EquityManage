<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="UkeyTech.WebFW.DAO" %>
<%@ Import Namespace="UkeyTech.WebFW.Model" %>
<%@ Import Namespace="StructureMap" %>
<%@ Import Namespace="System.Data" %>
<%if (ViewData.Eval("DictID") != null && ViewData.Eval("DataValueField") != null && ViewData.Eval("DataTextField") != null)
  {
      
      var isCombo = ViewData.Eval("Combo") != null && (bool)ViewData.Eval("Combo");
      var id = ViewData.Eval("ID").ToString();
      var dictid = ViewData.Eval("DictID").ToString();
      var value = ViewData.Eval("Value");
      var addempit = ViewData.Eval("AddEmptyItem");
      var filter = ViewData.Eval("Filter");
      string datavaluefield = ViewData.Eval("DataValueField").ToString();
      string datatextfield = ViewData.Eval("DataTextField").ToString();
      var enabled = ViewData.Eval("Enabled") == null || (bool)ViewData.Eval("Enabled"); 
     
%>

<select id="<%=id%>" class="<%=isCombo ? "\"easyui-combobox\"" : ""%> <%=ViewData.Eval("Required") != null && (bool)ViewData.Eval("Required") ? "easyui-validatebox":""%>" 
    <%=ViewData.Eval("Attr")%>
    <%=ViewData.Eval("Required") != null && (bool)ViewData.Eval("Required") ? "required=\"true\"" : ""%>
    name="<%=id %>" 
    style="width:<%=ViewData.Eval("width") != null?ViewData.Eval("width").ToString():"100"%>px;border: 1px solid #82a6c4;">
    <%if (addempit != null && (bool)addempit)
      {%><option value='' <%= (value == null || string.IsNullOrEmpty(value.ToString())) ? "selected=selected" : "" %>>
      </option>
    <%} %>
    <%
         var Dictionarydal = ObjectFactory.GetInstance<DictionaryDAO>();
         var context = ObjectFactory.GetInstance<Clover.Web.Core.IWebContext>();
         int rowscount = 0;
         try
         {
             DataTable dt = Dictionarydal.GetSQLDictData(context, dictid, 100, 1, out rowscount);

             foreach (DataRow dr in dt.Rows)
             {
                 if ((!enabled && value == null) || (!enabled && value != null && value.ToString() != dr[datavaluefield].ToString())
                     || (filter != null && dr[datavaluefield].ToString() == filter))
                     continue;

            
    %><option value='<%= isCombo ? dr[datatextfield].ToString() :dr[datavaluefield].ToString()%>'
        <%=(value!=null && value.ToString() == dr[datavaluefield].ToString()) ? "selected=\"selected\"" : "" %>>
        <%=dr[datatextfield].ToString()%></option>
    <%}
         }
         catch (Exception ex) {%>
    <option value="">加载失败：<%=ex.Message %></option>
    <%}%>
</select>
<%} else {%>
无效的 DictID|DataValueField|DataTextField 参数
<%} %>
