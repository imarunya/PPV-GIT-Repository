<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateRecord.aspx.cs" Inherits="PPVTool.DataM.UpdateRecord" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scm" runat="server"></asp:ScriptManager>
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div>
                    Select Test : 

        <asp:TextBox ID="txtTestNo" runat="server"
            Width="150px" ReadOnly="true" />
                    <asp:Panel runat="server" ID="Panel1"
                        Style="max-height: 200px; display: none; visibility: hidden;">
                        <!-- GridView  goes here -->
                        <asp:GridView ID="grdTest" runat="server"
                            AutoGenerateColumns="false"
                            OnSelectedIndexChanged="grdTest_SelectedIndexChanged">
                            <HeaderStyle BackColor="White" />
                            <RowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Styr-lpnr"
                                    HeaderText="Styr-lpnr" />
                                <asp:BoundField DataField="DB2-lpnr"
                                    HeaderText="DB2-lpnr" />
                            </Columns>
                            <SelectedRowStyle BackColor="#999999" />
                        </asp:GridView>
                    </asp:Panel>
                    <cc1:DropDownExtender ID="DropDownExtender1" runat="server"
                        DropDownControlID="Panel1"
                        TargetControlID="txtTestNo">
                    </cc1:DropDownExtender>
                </div>
                <div>
                    <asp:HiddenField ID="hfTestNo" runat="server" />
                    <asp:HiddenField ID="hfDBLpnr" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div>
            <asp:Button ID="btnUpdate" runat="server" Text="Unlock Test" OnClick="btnUpdate_Click" />
        </div>


        <div>
            <asp:FileUpload ID="fuFileUpload" runat="server" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUpload" runat="server" Text="Upload File" OnClick="btnUpload_Click" />

        </div>
    </form>
</body>
</html>
