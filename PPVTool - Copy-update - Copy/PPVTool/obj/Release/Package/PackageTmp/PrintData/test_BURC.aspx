<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="test_BURC.aspx.cs" Inherits="PPVTool.PrintData.test_BURC"  %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <title>PPV Burc</title>
    <style type="text/css">
        .auto-style1 {
            width: 196px;
        }
    </style>
</head>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<body>
    <hr size="10" font color="black" />
    <h2><font color="maroon">PPV BURC</h2>

    <form id="form111" runat="server">

        <hr size="10" font color="black" />
        <br />
    
  <tr bgcolor="#CFC6D2">
                <td>
        <asp:GridView ID="GridView2" runat="server" Width="1310px" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4">
             <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
             <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
             <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
             <RowStyle HorizontalAlign="Center" BackColor="White" ForeColor="#330099" />
             <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
             <SortedAscendingCellStyle BackColor="#FEFCEB" />
             <SortedAscendingHeaderStyle BackColor="#AF0101" />
             <SortedDescendingCellStyle BackColor="#F6F0C0" />
             <SortedDescendingHeaderStyle BackColor="#7E0000" />
        </asp:GridView>
        <br />
        <br />
        <div>         
            <asp:Panel ID="pnlGrd" runat="server" Height="500px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="1310px" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px">
                  <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                    <RowStyle HorizontalAlign="Center" BackColor="White" ForeColor="#330099" />
                    <Columns>
                 
                        <asp:BoundField DataField="Lpnr" HeaderText="CDRnr" />
                        <asp:BoundField DataField="Array-nr" HeaderText="Array-nr" />
                        <asp:BoundField DataField="Prisplan" HeaderText="Prisplan" />
                        <asp:BoundField DataField="Sect-code" HeaderText="Sect-code" />
                        <asp:BoundField DataField="Sect-net-amt" HeaderText="Sect-net-amt" />
                        <asp:BoundField DataField="Sect-dscnt-pcnt" HeaderText="Sect-dscnt-pcnt" />
                        <asp:BoundField DataField="Dtail-price-type" HeaderText="Dtail-price-type" />
                        <asp:BoundField DataField="Sect-chrge-appld" HeaderText="Sect-chrge-appld" />
                        <asp:BoundField DataField="Sumry-appld-ind" HeaderText="Sumry-appld-ind" />
                        
                    </Columns>
                      <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <SortedAscendingCellStyle BackColor="#FEFCEB" />
                    <SortedAscendingHeaderStyle BackColor="#AF0101" />
                    <SortedDescendingCellStyle BackColor="#F6F0C0" />
                    <SortedDescendingHeaderStyle BackColor="#7E0000" />
                </asp:GridView>
                <br />
                <br />
               <%-- <asp:Button ID="btnPDF" runat="server" OnClick="btnPDF_Click" Text="Generate PDF" />--%>
            </asp:Panel>
            <br />
        </div>
    </form>
</body>
</html>
