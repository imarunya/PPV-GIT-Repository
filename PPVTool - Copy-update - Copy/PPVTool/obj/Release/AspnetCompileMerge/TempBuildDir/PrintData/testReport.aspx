<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testReport.aspx.cs" Inherits="PPVTool.PrintData.testReport"%>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <title>PPV Report</title>
</head>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<body>
    <hr size="10" font color="black" />
    <h2><font color="maroon">PPV Report</h2>

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
                      <Columns>                
                        

                    </Columns>
             <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
             <SortedAscendingCellStyle BackColor="#FEFCEB" />
             <SortedAscendingHeaderStyle BackColor="#AF0101" />
             <SortedDescendingCellStyle BackColor="#F6F0C0" />
             <SortedDescendingHeaderStyle BackColor="#7E0000" />
        </asp:GridView>
        <br />
        <br />
        <div>         
            <asp:Panel ID="pnlGrd" runat="server"  Height="500px">
                 <%-- <tr bgcolor="#CFC6D2">--%>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="1310px" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px">
                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                    <RowStyle HorizontalAlign="Center" BackColor="White" ForeColor="#330099" />
                    <Columns>
                 
                        <asp:BoundField DataField="Lpnr" HeaderText="CDRnr" />
                        <asp:BoundField DataField="Rec-ind" HeaderText="Rec-ind" />
                        <asp:BoundField DataField="Kval" HeaderText="Kval" />
                        <asp:BoundField DataField="CDR-lager-nr" HeaderText="CDR-lager-nr" />
                        <asp:BoundField DataField="Servid" HeaderText="Servid" />
                        <asp:BoundField DataField="Calltype" HeaderText="Calltype" />
                        <asp:BoundField DataField="Asoc" HeaderText="Asoc" />
                        <asp:BoundField DataField="Chrgb-num-id" HeaderText="Chrgb-num-id" />
                        <asp:BoundField DataField="Datum" HeaderText="Datum" dataformatstring="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="Tid" HeaderText="Tid" />
                        <asp:BoundField DataField="Samtalslngd" HeaderText="Samtalslngd" />
                        <asp:BoundField DataField="B-nr" HeaderText="B-nr" />
                        <asp:BoundField DataField="Intwk-serv-id" HeaderText="service-id" />
                        <asp:BoundField DataField="Belopp" HeaderText="Belopp"  />

                    </Columns>
                
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <SortedAscendingCellStyle BackColor="#FEFCEB" />
                    <SortedAscendingHeaderStyle BackColor="#AF0101" />
                    <SortedDescendingCellStyle BackColor="#F6F0C0" />
                    <SortedDescendingHeaderStyle BackColor="#7E0000" />
                </asp:GridView>
                <br />
                <br />
                <font color="maroon">
            <%--    <asp:Button ID="btnreportpdf" runat="server" OnClick="btnreportpdf_Click" Text="Generate PDF" />--%>
                </font>
            </asp:Panel>
            <br />
        </div>
    </form>
</body>
</html>
