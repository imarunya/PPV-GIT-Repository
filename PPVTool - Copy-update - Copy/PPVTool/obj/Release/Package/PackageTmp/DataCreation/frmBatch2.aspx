<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmBatch2.aspx.cs" Inherits="PPVTool.DataCreation.frmBatch2" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        legend {
            font-size: 12pt;
            font-weight: normal;
        }
    </style>

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
        <table style="width: 80%;" class="table">
            <tr>
                <th style="text-align: center;" colspan="2">CHOOSE TESTS AND "START BATCH"                            
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
                    <asp:Panel ID="pnlGrd" runat="server" ScrollBars="Vertical" Height="500px">
                        <asp:GridView ID="grdTestCases" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="Select Test">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="RboGrid" runat="server" GroupName="GroupEntry" onclick="RadioCheck(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Styr-lpnr" HeaderText="Styr-lpnr" />
                                <asp:BoundField DataField="Prefix" HeaderText="Prefix" />
                                <asp:BoundField DataField="LAN-userid" HeaderText="LAN-userid" />
                                <asp:BoundField DataField="Datum" HeaderText="Datum" />
                                <asp:BoundField DataField="Ben" HeaderText="Ben" />
                                <asp:BoundField DataField="DB2-lpnr" HeaderText="DB2-lpnr" />
                                <asp:BoundField DataField="UP-lpnr" HeaderText="UP-lpnr" />
                                <asp:BoundField DataField="Init" HeaderText="Init" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" />
                </td>
            </tr>
            <tr>
                <td>

                    <a href="frmDownload.aspx?filename=JCL.txt" runat="server" id="hlJCL" visible="false">Download JCL File</a>
                    <br />

                    <a href="frmDownload.aspx?filename=PPVData.txt" runat="server" id="hlPPV" visible="false">Download PPV Data File</a>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hfwprefix" runat="server" />
                    <asp:HiddenField ID="hfp_styr_lpnr" runat="server" />
                    <asp:HiddenField ID="hfp_db2_lpnr" runat="server" />
                    <asp:HiddenField ID="hfp_up_lpnr" runat="server" />

                    <asp:HiddenField ID="hfwip" runat="server" Value="?" />
                    <asp:HiddenField ID="HiddenField6" runat="server" />
                    <asp:HiddenField ID="HiddenField7" runat="server" />
                    <asp:HiddenField ID="HiddenField8" runat="server" />
                    <asp:HiddenField ID="HiddenField9" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
