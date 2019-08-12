<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testAnalysys.aspx.cs" Inherits="PPVTool.PrintData.testAnalysys"%> <%--    culture="sv-SE"--%>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>PPV Analysis</title>
</head>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<body>
    <hr size="10" font color="black" />
    <h2><font color="maroon">PPV Analysis</h2>
    <hr size="10" font color="black" />
    <form id="form1" runat="server">
        <div>
            &nbsp; 
            <asp:GridView ID="GridView2" runat="server" Width="1310px" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
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
        </div>
        <div>
            <asp:Panel ID="pnlGrd" runat="server" Height="500px">
                <br />
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="1310px" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px">
                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                    <RowStyle HorizontalAlign="Center" BackColor="White" ForeColor="#330099" />
                    <Columns>
                        <asp:BoundField DataField="Lpnr" HeaderText="CDRnr" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="Belopp" HeaderText="Base-chrg-amt"  />
                        <asp:BoundField DataField="Call-cat-code" HeaderText="Servid/Calltype" />
                        <asp:BoundField DataField="Burc-belopp" HeaderText="Forvantat" />
                        <asp:BoundField DataField="Dtail-net-amt" HeaderText="UtFall" />
                        <asp:BoundField DataField="Burc-ASOC" HeaderText="Förväntad" />
                        <asp:BoundField DataField="Usage-cat-code" HeaderText="UtFall" />
                        <asp:BoundField DataField="Burc-ind" HeaderText="Förväntad" />
                        <asp:BoundField DataField="Ind" HeaderText="Utfall" />
                        <asp:BoundField DataField="Err-code" HeaderText="Err-code" />
                    </Columns>

                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <SortedAscendingCellStyle BackColor="#FEFCEB" />
                    <SortedAscendingHeaderStyle BackColor="#AF0101" />
                    <SortedDescendingCellStyle BackColor="#F6F0C0" />
                    <SortedDescendingHeaderStyle BackColor="#7E0000" />
                </asp:GridView>
                <br />
                <br />
               <%-- <asp:Button ID="btnanalysisreport" runat="server" OnClick="btnanalysisreport_Click" Text="Generate PDF" />--%>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
