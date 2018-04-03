<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UkeyTech.WebFW.Model.Widget>" %>
<table id="tbDataGrid<%=Model.WidgetCode %>">
</table>
<script type="text/javascript">
     var baseurl = '<%=Url.Action("","WorkFlow")%>';
     var ArticleUrl = '<%=Url.Action("","Article", new { area="CMS"})%>';
     function popupArticle(id) {
            SetWin($(document).width(), $(document).height(), ArticleUrl + '/ViewArticle/?ArticleId=' + id, '文章阅读');
        }
     function LoadDataGrid<%=Model.WidgetCode %>() {
        $('#tbDataGrid<%=Model.WidgetCode %>').datagrid({
            nowrap: false,
            striped: true,
            fit:true,
            border: false,
            url:'<%=Model.Target %>',
            columns:[
                <%=Model.Parameters %>]
            ,
            singleSelect: true,
            pagination: true,
            pageSize: 15,
            pageList: [10, 15, 20, 30],
            rownumbers: true

        });
    }

    setTimeout(function(){
        LoadDataGrid<%=Model.WidgetCode %>();
    },200);
</script>
