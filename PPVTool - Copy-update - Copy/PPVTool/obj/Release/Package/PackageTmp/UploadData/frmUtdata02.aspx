<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmUtdata02.aspx.cs" Inherits="PPVTool.UploadData.frmUtdata02" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
        function RadioCheck(rb) {
            var gv = document.getElementById("<%=grdTestCases.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 100%;">
        <table style="width: 70%;" class="table">
            <tr>
                <th style="text-align: center;" colspan="2">SELECT TESTS THAT SHOULD ANALYSIS STORMPLAS                            
                </th>
            </tr>
            <tr>
                <td>
                    <b>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label></b>
                </td>
            </tr>
            <tr>
                <td>
                    <b>
                        <asp:Label ID="lblHeader" runat="server"></asp:Label></b>
                </td>
                <tr>
                    <td>Choose test cases :
                    </td>
                </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlGrd" runat="server" ScrollBars="Vertical" Height="300px">
                        <asp:GridView ID="grdTestCases" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="Select Test">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="RboGrid" runat="server" onclick="RadioCheck(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Styr-lpnr" HeaderText="Styr-lpnr" />
                                <asp:BoundField DataField="Prefix" HeaderText="Prefix" />
                                <asp:BoundField DataField="LAN-userid" HeaderText="LAN-userid" />
                                <asp:BoundField DataField="Datum" HeaderText="Datum" />
                                <asp:BoundField DataField="Ben" HeaderText="Ben" />
                                <asp:BoundField DataField="DB2-lpnr" HeaderText="DB2-lpnr" />
                                <asp:BoundField DataField="UP-lpnr" HeaderText="UP-lpnr" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>            
            <tr>
                <td style="text-align-last: left;">
                    <fieldset>
                        <legend>Analysis result</legend>
                        <asp:RadioButton ID="rbAlternativknapp14" runat="server" GroupName="rbAR" Text="Open test for changes" /> <br />
                        <asp:RadioButton ID="rbAlternativknapp16" runat="server" GroupName="rbAR" Text="Close test with remark" /> <br />
                        <asp:RadioButton ID="rbAlternativknapp18" runat="server" GroupName="rbAR" Text="Close test WITHOUT remark" />
                    </fieldset>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" />
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
