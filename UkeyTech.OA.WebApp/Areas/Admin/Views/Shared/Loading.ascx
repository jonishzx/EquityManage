﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="BBQPOPLOADINGWINDOW" class="divLoading" style=" position: absolute;left: 0px;top: 0px;width: 100%;height: 100%;z-index: 10000;background-color: white;border: 0;">
    <img src="~/Content/Images/loading.gif" runat=server style="display: block;float: left;margin: 21% 0 0 40%;" />
    <div id="loadingtext" style="float: left;margin: 22% 0 0 5px;">读取中...</div>
</div>
<noscript>
    <div class="divLoading">
        <img src="~/Content/Images/noscript.gif" runat=server alt='抱歉，请开启脚本支持！' />
    </div>
</noscript>
<script type="text/javascript">
    $(document).ready(function() {
        $(".divLoading").fadeOut("slow", function() {
            $(this).remove();
        });
    });
</script>