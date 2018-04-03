<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="UkeyTech.WebFW.DAO" %>
<%@ Import Namespace="UkeyTech.WebFW.Model" %>
<%@ Import Namespace="StructureMap" %>
<%if(ViewData.Eval("DictID") != null){ %>

    <input style="width:180px;" id="<%=ViewData.Eval("ID")%>" <%=ViewData.Eval("Enabled") != null && !(bool)ViewData.Eval("Enabled") ? "disabled" : ""%> <%=ViewData.Eval("Attr")%> name="<%=ViewData.Eval("ID") %>" />

     <script type="text/javascript">
        $(function () {
          $('#<%=ViewData.Eval("ID")%>').combogrid({
                panelWidth: 450,
                value: '<%=ViewData.Eval("Value") %>',
                queryParams: { dictid: '<%= ViewData.Eval("DictID") %>', rows : 999999  },               
                url: '<%=Url.Action("GetDictSQLDataList","System", new {Area ="Admin" })%>',
         <%
            DictionaryDAO dictdal = ObjectFactory.GetInstance<DictionaryDAO>();
            var model =dictdal.GetModel(ViewData.Eval("DictID").ToString());
         %>
                idField: '<%=ViewData.Eval("IDField") %>',
                textField: '<%=ViewData.Eval("TextField") %>',
                columns: [[
                <% if (!string.IsNullOrEmpty(model.ExtAttr))
                   { %>
                <%= model.ExtAttr %>
                    <% }
                   else
                   { 
                        var columns = dictdal.GetSQLDictColumns(ViewData.Eval("DictID").ToString());
                   %>

         <% foreach (var col in columns)
            {
         %>{ field: '<%= col.ColumnName %>', title: '<%= col.Caption %>', width: <%= col.Caption.Length*30 %>, width:  ('<%= col.Caption %>'.length) * 15 < 100 ? 100 : '<%= col.Caption %>'.length * 15 },
         <% }
                   } %>
                    ,{ field: 'ANC', title:'', width: 1}
				    ]]
            });
        });
    </script>
<%} %>


