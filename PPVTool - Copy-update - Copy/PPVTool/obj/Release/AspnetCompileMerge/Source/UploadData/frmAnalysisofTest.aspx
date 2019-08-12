﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAnalysisofTest.aspx.cs" Inherits="PPVTool.UploadData.frmAnalysisofTest" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="scm" runat="server"></asp:ScriptManager>
    <div style="width: 100%;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table style="width: 50%;" class="table">
                    <tr>
                        <th style="text-align: center;" colspan="2">ANALYSIS OF TESTS                           
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>
                                <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 35%;">Select UP-release :
                            <asp:HiddenField ID="hfUplpnr" runat="server" />
                            <asp:HiddenField ID="hfMskn" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtUpRelease" runat="server"
                                Width="150px" ReadOnly="true" />
                            <asp:Panel runat="server" ID="Panel1"
                                Style="max-height: 200px; display: none; visibility: hidden;">
                                <!-- GridView  goes here -->
                                <asp:GridView ID="grdUpRelease" runat="server"
                                    AutoGenerateColumns="false"
                                    OnSelectedIndexChanged="grdUpRelease_SelectedIndexChanged">
                                    <HeaderStyle BackColor="White" />
                                    <RowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="UP-release"
                                            HeaderText="UP-release" />
                                        <asp:BoundField DataField="Maskin"
                                            HeaderText="Maskin" />
                                        <asp:BoundField DataField="Ben"
                                            HeaderText="Ben" />
                                        <asp:BoundField DataField="UP-lpnr"
                                            HeaderText="UP-lpnr" />
                                    </Columns>
                                    <SelectedRowStyle BackColor="#999999" />
                                </asp:GridView>
                            </asp:Panel>
                            <cc1:DropDownExtender ID="DropDownExtender1" runat="server"
                                DropDownControlID="Panel1"
                                TargetControlID="txtUpRelease">
                            </cc1:DropDownExtender>
                        </td>
                    </tr>

                    <tr>
                        <td>Choose environment :
                            <asp:HiddenField ID="hfDBlpnr" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtEnvoirnment" runat="server"
                                Width="150px" ReadOnly="true" />
                            <asp:Panel runat="server" ID="pnlCust"
                                Style="max-height: 200px; display: none; visibility: hidden;">
                                <!-- GridView  goes here -->
                                <asp:GridView ID="grdEnvoirnment" runat="server"
                                    AutoGenerateColumns="false"
                                    OnSelectedIndexChanged="grdEnvoirnment_SelectedIndexChanged">
                                    <HeaderStyle BackColor="White" />
                                    <RowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="Prefix"
                                            HeaderText="Prefix" />
                                        <asp:BoundField DataField="IMS-system"
                                            HeaderText="IMS-system" />
                                        <asp:BoundField DataField="DB2-lpnr"
                                            HeaderText="DB2-lpnr" />
                                    </Columns>
                                    <SelectedRowStyle BackColor="#999999" />
                                </asp:GridView>
                            </asp:Panel>
                            <cc1:DropDownExtender ID="ddlEnvoirnment" runat="server"
                                DropDownControlID="pnlCust"
                                TargetControlID="txtEnvoirnment">
                            </cc1:DropDownExtender>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="text-align: center; width: 50%;">
            <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" />
        </div>
    </div>
</asp:Content>
