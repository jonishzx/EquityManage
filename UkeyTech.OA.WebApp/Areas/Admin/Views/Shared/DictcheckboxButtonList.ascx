<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="UkeyTech.WebFW.DAO" %>
<%@ Import Namespace="UkeyTech.WebFW.Model" %>
<%@ Import Namespace="StructureMap" %>
<% if (ViewData.Eval("DictID") != null)
    {
        var id = ViewData.Eval("ID").ToString();
        var value = ViewData.Eval("Value") ?? (ViewData.Eval("Default") ?? null);
        bool enabled = true;
        if (ViewData.Eval("Enabled") != null)
        {
            enabled = (bool)ViewData.Eval("Enabled");
        }
        var dictitemDAO = ObjectFactory.GetInstance<DictItemDAO>();
        var rst = dictitemDAO.GetListByDictID(ViewData.Eval("DictID").ToString());
%>
<span class="radiobuttonlist">
    <% if (!enabled && value != null)
        {
            var it = rst.Find(x => value.ToString().Contains(x.Code));
            if (it != null) %>
    <%= rst.Find(x => value.ToString().Contains(x.Code)).Name %>
    <input id="<%= id %>" style="display: none;" name="<%= id %>" type="checkbox" checked="checked" value="<%= value %>" />
    <% }
        else
        {
            var i = 0;
            foreach (var m in rst)
            {
                i++;
    %><input id='<%= id + "_" + i %>' <%= !enabled ? "disabled" : "" %> name="<%= id %>" style="width: auto !important;" <%= ViewData.Eval("Attr") %> value='<%= m.Code %>' <%= (value != null &&  value.ToString().Contains(m.Code)) ? "checked=\"checked\"" : "" %> type="checkbox" /><label for="<%= id + "_" + i %>" style="width: auto !important;"><%= m.Name %></label>
    <% }
            }
        } %>
</span>