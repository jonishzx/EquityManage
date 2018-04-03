<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Clover.Web.Core" %>
<%@ Import Namespace="Clover.Config" %>
<%@ Import Namespace="Clover.Web.HTMLRender" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<title><%=WebSiteConfig.Config.WebAppName%></title>
	<script type="text/javascript">
        <%if(System.Web.HttpContext.Current!=null && (System.Web.HttpContext.Current.User.Identity.AuthenticationType == "NTLM" ||
        System.Web.HttpContext.Current.User.Identity.AuthenticationType == "Negotiate " )) {%>
        <!--window Auth-->
        var cssbaseurl = '<%=Url.Content("~/Admin/Home")%>';
        <%}else {%>
        <!--Form Auth-->
	var cssbaseurl = '<%=Url.Content("~/Admin/Account/Login")%>';
        <%} %>
	setTimeout(function(){
		window.location.href = cssbaseurl;
        },1000);
    </script>

</head>
<body >
<%=System.Web.HttpContext.Current.User.Identity.Name %>，你好!浏览器正在跳转中...
</body>
</html>