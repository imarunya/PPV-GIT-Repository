<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrintMaintRefData.aspx.cs" Inherits="PPVTool.PrintMaintRefData" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <title>Print Maintenance Reference Data</title>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    </head>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container" style="width: 100%;">
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr style="vertical-align: top">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Input SQL query and hit Run button : " Font-Bold="True"></asp:Label>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtQueryInput" runat="server" Rows="3" TextMode="MultiLine" Width="400px"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" MinimumPrefixLength="1" DelimiterCharacters="`" BehaviorID="txtQueryInput_AutoCompleteExtender" TargetControlID="txtQueryInput" ID="txtQueryInput_AutoCompleteExtender" CompletionInterval="10" EnableCaching="False" CompletionSetCount="1" ServiceMethod="ShowDatabaseAutoComplete" ServicePath="WebService1.asmx" FirstRowSelected="True"></cc1:AutoCompleteExtender>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnQueryRun" runat="server" Text="Run" OnClick="btnQueryRun_Click" ValidationGroup="QueryResult" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnQueryReset" runat="server" Text="Reset" OnClick="btnQueryReset_Click" CausesValidation="False" />
                                <br />
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Query box cannot be blank." ControlToValidate="txtQueryInput" ForeColor="Red" Display="Dynamic" SetFocusOnError="True" ValidationGroup="QueryResult"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblQueryInvalid" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <hr style="border-style: dashed" />
<%--            <div id="divGridViewHeader" style="height: 30px; overflow:hidden;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvQueryResultHeader" runat="server" Width="100%">
                            <AlternatingRowStyle BackColor="#F0F0F0" />
                            <HeaderStyle BackColor="#9900FF" ForeColor="White" HorizontalAlign="Center" Height="30px" VerticalAlign="Middle" />
                            <RowStyle Height="30px" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>--%>
            <div id="divGridView" style="height: 330px; overflow: auto;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvQueryResult" runat="server" Width="100%">
                            <AlternatingRowStyle BackColor="#F0F0F0" />
                            <HeaderStyle BackColor="#9900FF" ForeColor="White" HorizontalAlign="Center" Height="30px" VerticalAlign="Middle" />
                            <RowStyle Height="30px" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </body>
    </html>

</asp:Content>
