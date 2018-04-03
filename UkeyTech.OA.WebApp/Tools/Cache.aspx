<%@ Page Language="C#" AutoEventWireup="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script runat="server">
        protected void Page_Load(Object sender, EventArgs e)
        {           
            ShowCache(sender, e);
         
        }

        protected void ShowCache(Object sender, EventArgs e)
        {
            Response.Clear();
            Clover.Core.Caching.ICacheBacker backer = StructureMap.ObjectFactory.GetInstance<Clover.Core.Caching.ICacheBacker>();
            Response.Write("Cache Count:" + backer.Count);
            Response.Write("<Br/>");

            var clist = backer.GetList();
            foreach (var c in clist)
            {
                Response.Write("Key:");
                Response.Write(c.Key);
                Response.Write(",Value:");
                Response.Write(c.Value);
                Response.Write(",Expired Date:");
                Response.Write(c.CacheSetting.AbsoluteExpiration);
                Response.Write("<Br/>");
            }
        }

        protected void btnClean_Click(Object sender, EventArgs e)
        {
            Response.Clear();
            Clover.Core.Caching.ICacheBacker backer = StructureMap.ObjectFactory.GetInstance<Clover.Core.Caching.ICacheBacker>();
            backer.RemoveAll(string.Empty);
            Response.Write("缓存已清除");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="btnClean" runat=server OnClick="btnClean_Click" Text="清除" />
      <asp:Button ID="btnRefresh" runat=server OnClick="ShowCache" Text="刷新" />
      </form>
</body>
</html>
