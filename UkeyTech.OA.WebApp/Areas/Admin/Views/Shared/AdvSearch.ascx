<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UkeyTech.OA.WebApp.Extenstion.AdvSearchCondition[]>" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Import Namespace="UkeyTech.OA.WebApp.Extenstion" %>
<script type="text/javascript">
    function getFilterStr() {
        var rst = "[ ";
        $("advSearchPanel .intput").each(function () {
            if ($(this).attr("id").indexOf('F_') == 0) {
                rst += "{Field:\"" + $(this).attr("id").substring(2) + "\",";
                rst += "Value:\"" + $(this).val() + "\"},";
            }
        });
        rst = rst.substring(0, rst.length - 1);
        rst += "]"
        return rst;
    }
</script>
<div class="advSearchPanel">
    <div class="defpanel">
      <%foreach(var con in Model) {
    
      %>
       
    <label><%=con.Label %></label>
    <%switch(con.DataType){%>
        <%case AdvSearchCondition.Text: %>
            <input name="F_<%=con.Field %>" Field="<%=con.Field %>" type="text" value="<%=con.DefaultValue%>"/>
        <% break; %>
        <%case AdvSearchCondition.CheckBox: %>
            <input name="F_<%=con.Field %>"  Field="<%=con.Field %>" type="checkbox" <%=!string.IsNullOrEmpty(con.DefaultValue) ? "checked" : ""%> value="<%=con.DefaultValue%>" />
        <% break; %>
        <%case AdvSearchCondition.DateTime: %>
            <input name="F_<%=con.Field %>"  Field="<%=con.Field %>" class="Wdate" onfocus="WdatePicker()" type="text" value="<%=con.DefaultValue%>"/>
        <% break; %>
        <%case AdvSearchCondition.ConstDict: %>
            <%Html.RenderPartial(Helper.DictDropDownListPath,
                                new ViewDataDictionary(new
                                {
                                    ID = "F_" + con.Field,
                                    DictID = con.DictID,
                                    AddEmptyItem = true,
                                    Value = con.DefaultValue,
                                    Attr = "Field=\"" + con.Field + "\"" 
                                }));%>
        <% break; %>
        <%case AdvSearchCondition.SQLDict: %>
                <%Html.RenderPartial(Helper.PopupControlPath,
                                    new ViewDataDictionary(new
                                    {
                                        IDControlName = "F_" + con.Field,
                                        TextControlName = "FT_" + con.Field,
                                        DictID = con.DictID,
                                        TextValue = "",
                                        Value = con.DefaultValue,
                                        Attr = "Field=\"" + con.Field + "\""
                                    }));%>
        <% break; %>
    <%} %>
<%} %>
    <span><a href="javascript:void(0);" onclick='$(".hidepanel").show();'></a></span>
    </div>
</div>