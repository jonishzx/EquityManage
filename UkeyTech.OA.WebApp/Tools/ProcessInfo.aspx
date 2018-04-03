<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script runat="server">
        protected void Page_Load(Object sender, EventArgs e)
        {
            //current Process Info
            ProcessInfo proc = ProcessModelInfo.GetCurrentProcessInfo();
            lblProcID.Text = proc.ProcessID.ToString();
            lblStartTime.Text = proc.StartTime.ToString();
            lblRunningTime.Text = proc.Age.Days + " days, " +
                     proc.Age.Hours + ":" + proc.Age.Minutes;
            lblPeakMem.Text = proc.PeakMemoryUsed.ToString();

            //processinfo list
            dgHistory.DataSource = ProcessModelInfo.GetHistory(10);
            dgHistory.DataBind();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="1" align="center" border="1">
            <caption>
                Current ASP.NET Process
            </caption>
            <tbody>
                <tr>
                    <td align="left">
                        ASP.NET Process ID:
                    </td>
                    <td align="middle">
                        <asp:Label ID="lblProcID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        ASP.NET was Started at:
                    </td>
                    <td align="middle">
                        <asp:Label ID="lblStartTime" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        ASP.NET has been running for:
                    </td>
                    <td align="middle">
                        <asp:Label ID="lblRunningTime" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Peak Memory Used:
                    </td>
                    <td align="middle">
                        <asp:Label ID="lblPeakMem" runat="server"></asp:Label>
                        KB
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:DataGrid ID="dgHistory" runat="server" HorizontalAlign="center" AutoGenerateColumns="False"
            CellPadding="4">
            <HeaderStyle Font-Bold="True" BackColor="#dddddd" />
            <Columns>
                <asp:BoundColumn HeaderText="Process ID" DataField="ProcessID" />
                <asp:BoundColumn HeaderText="Start Time" DataField="StartTime" />
                <asp:BoundColumn HeaderText="Peak Memory Used (in KB)" DataField="PeakMemoryUsed" />
                <asp:BoundColumn HeaderText="Shutdown Reason" DataField="ShutdownReason" />
            </Columns>
        </asp:DataGrid>
    </div>
    </form>
</body>
</html>
