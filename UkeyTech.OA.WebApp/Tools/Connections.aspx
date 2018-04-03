<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Clover.Data" %>
<%@ Import Namespace="Dapper" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script runat="server">
        protected void Page_Load(Object sender, EventArgs e)
        {
            var poollist = Clover.Data.BaseDAO.ManualConnectionPool;

            Response.Write("The Pool Count is " + poollist.Count.ToString());
            Response.Write("<br/>");
           
        }

        protected void CleanConnections(Object sender, EventArgs e)
        {
            string sql = @"set nocount on
declare @databasename varchar(100)
declare @query varchar(max)
set @query = ''

set @databasename = '{0}'
if db_id(@databasename) < 4
begin
	print 'system database connection cannot be killeed'
return
end

select @query=coalesce(@query,',' )+'kill '+convert(varchar, spid)+ '; '
from master..sysprocesses where dbid=db_id(@databasename) and status = 'sleeping'

if len(@query) > 0
begin
print @query
	exec(@query)
end";
            
            var conn = Clover.Data.BaseDAO.DbService("master");
            var conn2 = Clover.Data.BaseDAO.DbService();
            Response.Write(conn2.Database);
            conn.Execute(string.Format(sql, conn2.Database));
            ShowConnections(sender, e);

        }
        
        protected void ShowConnections(Object sender, EventArgs e)
        {
            string sql = @"select 
    db_name(dbid) as [Database Name], 
    count(dbid) as [No Of Connections],
    loginame as [Login Name]
from
    sys.sysprocesses
where 
    dbid > 0
group by 
    dbid, loginame";
            var dt = Clover.Data.BaseDAO.GetDataTable(sql);
            gvConnectionStat.DataSource = dt;
            gvConnectionStat.DataBind();
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
      <asp:Button ID="btnRefresh" runat=server OnClick="ShowConnections" Text="显示连接数" />
      <asp:Button ID="btnClean" runat=server OnClick="CleanConnections" Text="清除连接数" />
      <asp:GridView ID="gvConnectionStat"  runat=server AutoGenerateColumns=true></asp:GridView>
    </form>
</body>
</html>
