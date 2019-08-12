<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="PPVTool.Test" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .invisibleColumn {
            display: none;
            width: 0px;
        }

        body {
            font-family: Arial;
            font-size: 12px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="scm" runat="server"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="table">
                        <tr>
                            <td style="width: 220px;">Choose customer(Chrgb No) :</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtFriend" runat="server"
                                    Width="150px" ReadOnly="true" />
                                <asp:Panel runat="server" ID="FriendDropDown"
                                    Style="max-height: 500px; overflow: scroll; display: none; visibility: hidden;">
                                    <!-- GridView  goes here -->
                                    <asp:GridView ID="FriendGridView" runat="server"
                                        AutoGenerateColumns="false"
                                        OnSelectedIndexChanged="FriendGridView_SelectedIndexChanged">
                                        <%--<RowStyle BackColor="#DDDDDD" />--%>
                                        <RowStyle BackColor ="White" />
                                        <Columns>

                                            <asp:BoundField DataField="CDR-lager-nr"
                                                HeaderStyle-CssClass="invisibleColumn"
                                                ItemStyle-CssClass="invisibleColumn" />
                                            <asp:BoundField DataField="CDR-lager-nr"
                                                HeaderText="CDR-lager-nr" />
                                            <asp:BoundField DataField="Ben"
                                                HeaderText="Ben" />
                                            <asp:BoundField DataField="Servid"
                                                HeaderText="Servid" />
                                        </Columns>
                                        <HeaderStyle BackColor="Blue" ForeColor="White" />                                        
                                        <SelectedRowStyle BackColor="#999999" />
                                    </asp:GridView>
                                </asp:Panel>
                                <cc1:DropDownExtender ID="DropDownExtender1" runat="server"
                                    DropDownControlID="FriendDropDown"
                                    TargetControlID="txtFriend">
                                </cc1:DropDownExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                This is the testing of the application, and this is the test page.
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
