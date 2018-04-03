<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%if(ViewData.Eval("DictID") != null){
      var isCombo = ViewData.Eval("Combo") != null && (bool)ViewData.Eval("Combo");
      var id = ViewData.Eval("ID").ToString();
      var value = ViewData.Eval("Value");
      var addempit = ViewData.Eval("AddEmptyItem");
      var filter = ViewData.Eval("Filter");
      var enabled = ViewData.Eval("Enabled") == null || (bool)ViewData.Eval("Enabled"); 
%>
<script type="text/javascript">
    $(function () {

        $('#<%=ViewData.Eval("ID") %>').combotree({
            valueField: 'DictID',
            textField: 'Name',
            editable: false,
            checkbox: false,
            url: '<%=Url.Action("","System", new {Area ="Admin" })%>/GetDictionaryTreeWithItems?parentid=<%=ViewData.Eval("DictID") %>'

            , onSelect: function (node) {
                if ($(node.target).siblings('ul').length > 0) {
                    $("#err<%=ViewData.Eval("ID") %>").html('请选择子节点');
                    $('#<%=ViewData.Eval("ID") %>').combotree("clear");
                    return false;
                }
                $("#parent<%=ViewData.Eval("ID") %>").html("");
                $("#err<%=ViewData.Eval("ID") %>").html("");
                var ptext = $("div[node-id='" + node.id + "']").parent().parent().siblings().text();                                                
                $("#parent<%=ViewData.Eval("ID") %>").html(ptext + ">");
            }, onLoadSuccess:function(){
                var currid = $("#<%=ViewData.Eval("ID") %>").val();
                var ptext = $("div[node-id='" + currid + "']").parent().parent().siblings().text();
                if(ptext)
                    $("#parent<%=ViewData.Eval("ID") %>").html(ptext + ">");
            }

        });
        fixCobmboInIE();
    });
</script>
<span id="parent<%=ViewData.Eval("ID") %>" style="color:blue"></span>
<input name="<%=ViewData.Eval("ID") %>" id="<%=ViewData.Eval("ID") %>" <%= enabled ? "readonly='readonly'":"" %>
<%=ViewData.Eval("Required") != null && (bool)ViewData.Eval("Required") ? "easyui-validatebox":""%>"
<%=ViewData.Eval("Required") != null && (bool)ViewData.Eval("Required") ? "required=\"true\"" : ""%>
style="width:200px;" value="<%=ViewData.Eval("Value") %>"/>
<span id="err<%=ViewData.Eval("ID") %>" style="color:red"></span>
<%} %>