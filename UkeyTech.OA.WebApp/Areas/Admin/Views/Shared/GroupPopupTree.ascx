<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<input name="<%=ViewData.Eval("ID") %>" id="<%=ViewData.Eval("ID") %>" class="easyui-combotree"  style="width:200px;" url='<%=Url.Action("","Permission", new {Area ="Admin" })%>/GetAllGroupTree'
value="<%=ViewData.Eval("Value") %>">
