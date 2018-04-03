<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"  %>
<%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
<link rel="stylesheet" type="text/css" media="screen" href="<%=Url.Content("~/Content/Admin/FONT.css") %>" />
<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/Scripts/Common.min.js")%>?v=1.1"></script>