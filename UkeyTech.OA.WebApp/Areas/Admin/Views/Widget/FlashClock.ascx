<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UkeyTech.WebFW.Model.Widget>" %>
<script type="text/javascript" src="<%=Url.Content("~/Scripts/swfobject.js")%>"></script>
<script type="text/javascript">
    try {
        swfobject.registerObject("FlashTime", "8.0.0", '<%=Url.Content("~/Scripts/Flash/expressInstall.swf")%>');
    } catch (e) { }
    </script>
<center>
    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="80" height="80"
        id="FlashTime" align="middle">
        <param name="movie" value="<%=Url.Content("~/Scripts/Flash/wp-clock-16.swf")%>" />
        <param name="menu" value="false" />
        <param name="wmode" value="transparent" />
        <param name="allowscriptaccess" value="always" />
        <!--[if !IE]>-->
        <object type="application/x-shockwave-flash" data="<%=Url.Content("~/Scripts/Flash/wp-clock-16.swf")%>" width="80" height="80"
            align="middle">
            <param name="menu" value="false" />
            <param name="wmode" value="transparent" />
            <param name="allowscriptaccess" value="always" />
            <!--<![endif]-->
            <a href="http://www.adobe.com/go/getflashplayer">
                <img width="100" height="40" src="<%=Url.Content("~/Content/Images/")%>get_flash_player.gif"
                    alt="Get Adobe Flash player" /></a>
            <!--[if !IE]>-->
        </object>
        <!--<![endif]-->
    </object>
</center>
