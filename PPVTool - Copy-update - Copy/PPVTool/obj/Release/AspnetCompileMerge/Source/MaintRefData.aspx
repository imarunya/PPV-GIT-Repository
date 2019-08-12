<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="MaintRefData.aspx.cs" Inherits="PPVTool.WebForm2" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <title>Maintenance Reference Data</title>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

        <script src="scripts/jquery.js" type="text/javascript"></script>
        <%--        <script type="text/javascript">
            $(document).ready(function () {
                $("#btnBICLayerUpdate").closest("form").submit(function () {
                    //if (Page_IsValid) {
                        alert("alert page is value");
                   // }
                });
            });
        </script>--%>

        <script type='text/javascript'>
            //$(document).ready(function () {
            //    $(".btn").click(function () {
            //        $("#btnBICLayerUpdate").collapse({ toggle: false });
            //    });
            //});

            $(document).ready(function () {
                $('#btnBICLayerUpdate').click(function (e) {
                    $('#collapseTwo').toggle(); //Show or Hide
                    e.preventDefault();
                });
                function clientfunction() {
                    // Do the client side validations here.
                    //Page_ClientValidate();
                    //if (Page_IsValid) {
                    //alert("alert page is value");
                    //$('#collapseOne').collapse('hide');
                    //$('#collapseTwo').collapse('show');
                    //}
                    // Now call the server side button event explicitly
                    //__doPostBack('OnClick', 'btnBICLayerUpdate');
                }
            }
        </script>

    </head>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True" EnablePartialRendering="True">
        </asp:ScriptManager>

        <div class="container" style="width: 100%;">
            <h2>Maintenance Reference Data</h2>
            <%--<p>Please select the appropriate tab to proceed:</p>--%>
            <ul class="nav nav-tabs">
                <li class="active" style="width: 180px; text-align: center"><a data-toggle="tab" href="#home">BIC Layer</a></li>
                <li style="width: 180px; text-align: center"><a data-toggle="tab" href="#menu1">CDR Layer</a></li>
                <li style="width: 180px; text-align: center"><a data-toggle="tab" href="#menu2">Price Plan Layer</a></li>
                <li style="width: 180px; text-align: center"><a data-toggle="tab" href="#menu3">Category Code Layer</a></li>
                <li style="width: 180px; text-align: center"><a data-toggle="tab" href="#menu4">Plan Option Layer</a></li>
                <li style="width: 180px; text-align: center"><a data-toggle="tab" href="#menu5">Insert New Country</a></li>
            </ul>

            <div class="tab-content">
                <%--BIC Layer--%>
                <div id="home" class="tab-pane fade in active">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                <asp:View runat="server" ID="View1">
                                    <table>
                                        <tr>
                                            <td>
                                                <%--<asp:Panel ID="Panel5" runat="server">--%>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label4" runat="server" Text="Label">Customer Type:</asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="ddBICLayerCustType" runat="server" Height="25" Width="180" TabIndex="1" ValidationGroup="BICLayer" CausesValidation="True"></asp:DropDownList>
                                                            <br />
                                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Please select Customer Type" ControlToValidate="ddBICLayerCustType" Display="Dynamic" ForeColor="Red" OnServerValidate="CustomValidator2_ServerValidate" ValidationGroup="BICLayer" SetFocusOnError="True" EnableClientScript="False"></asp:CustomValidator>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label35" runat="server" Text="Label">Organisation No:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerOrganisationNo" runat="server" TabIndex="6" MaxLength="10" ValidationGroup="BICLayer"></asp:TextBox>
                                                            <br />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Organisation No cannot be blank." ControlToValidate="txtBICLayerOrganisationNo" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label36" runat="server" Text="Label">Bill-Cycle Major:</asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="ddBICLayerBillCycleMajor" runat="server" Width="180" Height="25" TabIndex="2" CausesValidation="True" ValidationGroup="BICLayer"></asp:DropDownList>
                                                            <br />
                                                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="Please select Bill-Cycle Major" ControlToValidate="ddBICLayerBillCycleMajor" Display="Dynamic" ForeColor="Red" OnServerValidate="CustomValidator3_ServerValidate" ValidationGroup="BICLayer" EnableClientScript="False" SetFocusOnError="True"></asp:CustomValidator>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label37" runat="server" Text="Label">Bill-Cycle-Len:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerBillCycleLen" runat="server" TabIndex="7" MaxLength="3" ValidationGroup="BICLayer"></asp:TextBox>
                                                            <br />
                                                            <asp:RangeValidator runat="server" Type="Integer" MinimumValue="0" MaximumValue="999" ControlToValidate="txtBICLayerBillCycleLen" ValidationGroup="BICLayer" ErrorMessage="Bill-Cycle-Len must be a number." Display="Dynamic" ForeColor="Red" />

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Bill-Cycle-Len cannot be blank." ControlToValidate="txtBICLayerBillCycleLen" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label38" runat="server" Text="Label">Corporate/Private:</asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="ddBICLayerCorpPrivate" runat="server" Width="180" Height="25" TabIndex="3" CausesValidation="True" ValidationGroup="BICLayer"></asp:DropDownList>
                                                            <br />
                                                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="Please select Corporate/Private" ControlToValidate="ddBICLayerCorpPrivate" Display="Dynamic" ForeColor="Red" OnServerValidate="CustomValidator4_ServerValidate" ValidationGroup="BICLayer" EnableClientScript="False" SetFocusOnError="True"></asp:CustomValidator>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label39" runat="server" Text="Label">Bill-Cycle-Code:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerBillCycleCode" runat="server" TabIndex="8" MaxLength="2" ValidationGroup="BICLayer"></asp:TextBox>
                                                            <br />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Bill-Cycle-Code cannot be blank." ControlToValidate="txtBICLayerBillCycleCode" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label40" runat="server" Text="Label">Ev Center Type:</asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="ddBICLayerEVCenterType" runat="server" Width="180" Height="25" TabIndex="4" ValidationGroup="BICLayer"></asp:DropDownList>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label41" runat="server" Text="Label">Bill-Ind:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerBillInd" runat="server" TabIndex="9" MaxLength="1" ValidationGroup="BICLayer"></asp:TextBox>
                                                            <br />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtBICLayerBillInd" ValidationExpression="[a-zA-Z ]*$" ErrorMessage="Valid characters: Alphabets Only" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Bill-Ind cannot be blank." ControlToValidate="txtBICLayerBillInd" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label42" runat="server" Text="Label">Tim-split:</asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="ddBICLayerTimSplit" runat="server" Width="180" Height="25" TabIndex="5" CausesValidation="True" ValidationGroup="BICLayer"></asp:DropDownList>
                                                            <br />
                                                            <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="Please select Tim-split" ControlToValidate="ddBICLayerTimSplit" Display="Dynamic" ForeColor="Red" OnServerValidate="CustomValidator5_ServerValidate" ValidationGroup="BICLayer" EnableClientScript="False" SetFocusOnError="True"></asp:CustomValidator>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label43" runat="server" Text="Label">A number:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerANumber" runat="server" TabIndex="10" ValidationGroup="BICLayer" MaxLength="20"></asp:TextBox>
                                                            <br />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="A number cannot be blank." ControlToValidate="txtBICLayerANumber" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:RangeValidator runat="server" Type="Integer" MinimumValue="0" MaximumValue="50000" ControlToValidate="txtBICLayerANumber" ValidationGroup="BICLayer" ErrorMessage="Value must be a number." Display="Dynamic" ForeColor="Red" />
                                                        </td>
                                                        <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </tr>
                                                </table>
                                                <%--</asp:Panel>--%>
                                            </td>

                                            <td>
                                                <%--<asp:Panel ID="Panel6" runat="server">--%>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label13" runat="server" Text="Label">Street Address:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerStreetAddress" runat="server" Rows="2" TextMode="MultiLine" Width="400" TabIndex="11" ValidationGroup="BICLayer"></asp:TextBox>
                                                            <br />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Street Address cannot be blank." ControlToValidate="txtBICLayerStreetAddress" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtBICLayerStreetAddress" Display="Dynamic" ValidationGroup="BICLayer" ErrorMessage="Please limit to 55 characters or fewer." ValidationExpression="[\s\S]{1,55}" ForeColor="Red" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label14" runat="server" Text="Label">Mailing Address:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerMailingAddress" runat="server" Rows="2" TextMode="MultiLine" Width="400" TabIndex="12" ValidationGroup="BICLayer"></asp:TextBox>
                                                            <br />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Mailing Address cannot be blank." ControlToValidate="txtBICLayerMailingAddress" ValidationGroup="BICLayer" Display="Dynamic" ForeColor="Red" EnableClientScript="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtBICLayerMailingAddress" Display="Dynamic" ValidationGroup="BICLayer" ErrorMessage="Please limit to 55 characters or fewer." ValidationExpression="[\s\S]{1,55}" ForeColor="Red" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="Label15" runat="server" Text="Label">Optional Text:</asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtBICLayerOptionalText" runat="server" Rows="2" TextMode="MultiLine" Width="400" TabIndex="13"></asp:TextBox>
                                                            <br />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtBICLayerOptionalText" Display="Dynamic" ValidationGroup="BICLayer" ErrorMessage="Please limit to 30 characters or fewer." ValidationExpression="[\s\S]{1,30}" ForeColor="Red" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: bottom">
                                                            <br />
                                                            <br />
                                                            <asp:Button ID="btnBICLayerUpdate" runat="server" Text="Next" TabIndex="14" ValidationGroup="BICLayer" OnClick="btnBICLayerUpdate_Click" />
                                                            &nbsp;&nbsp;
                                                                    <asp:Button ID="btnBICLayerReset" runat="server" Text="Reset" TabIndex="15" CausesValidation="False" OnClick="btnBICLayerReset_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--</asp:Panel>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="View2" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr style="vertical-align: top">
                                                    <td>
                                                        <br />
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblBICCustCount" runat="server" Text="Select Number of Customers: "></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddBICCustCount" runat="server" Height="25" Width="180" ValidationGroup="CustCount" CausesValidation="True">
                                                                        <asp:ListItem Selected="True">-Select-</asp:ListItem>
                                                                        <asp:ListItem>1</asp:ListItem>
                                                                        <asp:ListItem>2</asp:ListItem>
                                                                        <asp:ListItem>3</asp:ListItem>
                                                                        <asp:ListItem>4</asp:ListItem>
                                                                        <asp:ListItem>5</asp:ListItem>
                                                                        <asp:ListItem>6</asp:ListItem>
                                                                        <asp:ListItem>7</asp:ListItem>
                                                                        <asp:ListItem>8</asp:ListItem>
                                                                        <asp:ListItem>9</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnBICCustCountOk" runat="server" Text="Ok" OnClick="btnBICCustCountOk_Click" ValidationGroup="CustCount" />
                                                                    <br />
                                                                    <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="Please select number of Customers." ControlToValidate="ddBICCustCount" Display="Dynamic" OnServerValidate="CustomValidator6_ServerValidate" ForeColor="Red" ValidationGroup="CustCount"></asp:CustomValidator>
                                                                </td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                    <asp:Label ID="lblBICBillIDCount" runat="server" Text="" Visible="false"></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddBICBillIDCount" runat="server" Height="25" Width="180" Visible="false" ValidationGroup="BillCount" CausesValidation="True">
                                                                        <asp:ListItem Selected="True">-Select-</asp:ListItem>
                                                                        <asp:ListItem>1</asp:ListItem>
                                                                        <asp:ListItem>2</asp:ListItem>
                                                                        <asp:ListItem>3</asp:ListItem>
                                                                        <asp:ListItem>4</asp:ListItem>
                                                                        <asp:ListItem>5</asp:ListItem>
                                                                        <asp:ListItem>6</asp:ListItem>
                                                                        <asp:ListItem>7</asp:ListItem>
                                                                        <asp:ListItem>8</asp:ListItem>
                                                                        <asp:ListItem>9</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    &nbsp;&nbsp;
                                                            <asp:Button ID="btnBICBillIDCountOk" runat="server" Text="Ok" Visible="false" OnClick="btnBICBillIDCountOk_Click" ValidationGroup="BillCount" />
                                                                    <br />
                                                                    <asp:CustomValidator ID="CustomValidator7" runat="server" ErrorMessage="Please select number of Bill Accounts." ControlToValidate="ddBICBillIDCount" Display="Dynamic" OnServerValidate="CustomValidator7_ServerValidate" ForeColor="Red" ValidationGroup="BillCount"></asp:CustomValidator>
                                                                </td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                    <asp:Label ID="lblBICChargeBillCount" runat="server" Text="" Visible="false"></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddBICChargeBillCount" runat="server" Height="25" Width="180" Visible="false" ValidationGroup="ChargeCount" CausesValidation="True">
                                                                        <asp:ListItem Selected="True">-Select-</asp:ListItem>
                                                                        <asp:ListItem>1</asp:ListItem>
                                                                        <asp:ListItem>2</asp:ListItem>
                                                                        <asp:ListItem>3</asp:ListItem>
                                                                        <asp:ListItem>4</asp:ListItem>
                                                                        <asp:ListItem>5</asp:ListItem>
                                                                        <asp:ListItem>6</asp:ListItem>
                                                                        <asp:ListItem>7</asp:ListItem>
                                                                        <asp:ListItem>8</asp:ListItem>
                                                                        <asp:ListItem>9</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnBICChargeBillCountOk" runat="server" Text="Ok" Visible="false" OnClick="btnBICChargeBillCountOk_Click" ValidationGroup="ChargeCount" />
                                                                    <br />
                                                                    <asp:CustomValidator ID="CustomValidator8" runat="server" ErrorMessage="Please select valid Chargeable Number." ControlToValidate="ddBICChargeBillCount" Display="Dynamic" OnServerValidate="CustomValidator8_ServerValidate" ForeColor="Red" ValidationGroup="ChargeCount"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                    <br />
                                                                    <%--<asp:Label ID="lblBICSubmitTree" runat="server" Text="Click Next to proceed : " Visible="False"></asp:Label>--%>
                                                                    <asp:Button ID="btnBICSubmitTree" runat="server" Text="Next" OnClick="btnBICSubmitTree_Click" Visible="False" />
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnBICResetTree" runat="server" Text="Reset" OnClick="btnBICResetTree_Click" Visible="False" />
                                                                    <%--<asp:Label ID="lblBICResetTree" runat="server" Text="Click Reset to re-enter the values : " Visible="False"></asp:Label>--%>                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <div style="width: 300px; height: 360px; overflow: auto">
                                                            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows">
                                                                <HoverNodeStyle Font-Underline="false" />
                                                                <NodeStyle Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" HorizontalPadding="7px" NodeSpacing="5px" VerticalPadding="2px" />
                                                                <ParentNodeStyle Font-Bold="False" />
                                                                <%--<SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />--%>
                                                            </asp:TreeView>
                                                        </div>
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="panelBICAgreement" runat="server" Visible="false">
                                                            <br />
                                                            <asp:Label ID="lblBICAgreement" runat="server" Text="Enter Agreement Details:" Font-Size="Large" Font-Bold="true"></asp:Label>
                                                            <br />
                                                            <br />
                                                            <asp:RadioButton ID="rbBICAgreementOne" runat="server" Checked="True" GroupName="AgreeType" OnCheckedChanged="rbBICAgreementOne_CheckedChanged" AutoPostBack="True" />
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblBICAgreementOne" runat="server" Text="A customer owns All agreements?"></asp:Label>
                                                            <br />
                                                            <asp:RadioButton ID="rbBICAgreeDifferent" runat="server" GroupName="AgreeType" OnCheckedChanged="rbBICAgreeDifferent_CheckedChanged" AutoPostBack="True" />
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblBICAgreementDifferent" runat="server" Text="Link agreements customer wise?"></asp:Label>
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="lblBICAgreementCount" runat="server" Text="Select number of agree-id (1-9)"></asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="ddBICAgreementCount" runat="server" Height="25" Width="180" ValidationGroup="AgreeCount" CausesValidation="True" Visible="false">
                                                                <asp:ListItem Value="0" Selected="True">-Select-</asp:ListItem>
                                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                                <asp:ListItem Value="6">6</asp:ListItem>
                                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <%--&nbsp;&nbsp;
                                                            <asp:Button ID="btnBICAgreementOk" runat="server" Text="Ok" OnClick="btnBICAgreementOk_Click" ValidationGroup="AgreeCount" />--%>
                                                            <br />
                                                            <asp:CustomValidator ID="CustomValidator9" runat="server" ErrorMessage="Please select number of Customers." ControlToValidate="ddBICAgreementCount" Display="Dynamic" OnServerValidate="CustomValidator9_ServerValidate" ForeColor="Red" ValidationGroup="AgreeCount"></asp:CustomValidator>
                                                            <br />
                                                            <br />
                                                            <asp:Button ID="btnBICFinalSubmit" runat="server" Text="Submit" OnClick="btnBICFinalSubmit_Click" Visible="true" ValidationGroup="AgreeCount" />
                                                            &nbsp;&nbsp;
                                                            <asp:Button ID="btnBICFinalReset" runat="server" Text="Reset" OnClick="btnBICFinalReset_Click" Visible="true" />
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="lblBICSubmitResult" runat="server" ForeColor="#3366FF" Font-Italic="True"></asp:Label>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:View>
                            </asp:MultiView>
                            <%--<div class="panel panel-default">
                                <div class="panel-heading" data-toggle="collapse" data-parent="#accordion" data-target="#collapseOne" aria-expanded="true">
                                    <h4 class="panel-title">
                                        <a>
                                            <strong>PANEL 1</strong></a>
                                    </h4>
                                </div>
                                <div id="collapseOne" class="panel-collapse collapse in">
                                    <div class="panel-body">

                                        <p align="justify">We are in an age of specialization.  As times have changed, and in the advent of complex integration of various Laws you need to keep up with all areas and complexities of it, and that is why our team is comprised of the top legal minds, each with their own area of practice.  Our firm is also unique in that while we all specialize in various aspects of the law, the size of our legal team allows us to offer counsel on nearly any facet of the law including commercial and general litigation in various Courts/ Tribunals of the Country.</p>
                                    </div>
                                </div>
                            </div>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <div class="panel panel-default">
                                <div class="panel-heading" data-toggle="collapse" data-parent="#accordion" data-target="#collapseTwo" aria-expanded="true">
                                    <h4 class="panel-title">
                                        <a >
                                            <strong>PANEL 1</strong></a>
                                    </h4>
                                </div>
                                <div id="collapseTwo" class="panel-collapse collapse">
                                    <div class="panel-body">

                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" Text="Select Number of Customers: "></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddBICCustCount" runat="server" Height="25" Width="180" AutoPostBack="True">
                                                        <asp:ListItem Selected="True">-Select-</asp:ListItem>
                                                        <asp:ListItem>1</asp:ListItem>
                                                        <asp:ListItem>2</asp:ListItem>
                                                        <asp:ListItem>3</asp:ListItem>
                                                        <asp:ListItem>4</asp:ListItem>
                                                        <asp:ListItem>5</asp:ListItem>
                                                        <asp:ListItem>6</asp:ListItem>
                                                        <asp:ListItem>7</asp:ListItem>
                                                        <asp:ListItem>8</asp:ListItem>
                                                        <asp:ListItem>9</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
                <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel7" runat="server" Visible="false">
                                next template
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>


                <%--CDR Layer--%>
                <div id="menu1" class="tab-pane fade">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <%--<h3>CDR Layer</h3>--%>
                            <table>
                                <tr>
                                    <td style="vertical-align: top">
                                        <br />
                                        <asp:RadioButton ID="rbCreateNewCDR" runat="server" GroupName="CDR" OnCheckedChanged="rbCreateNewCDR_CheckedChanged" AutoPostBack="True" />
                                        &nbsp;Create New CDR
                                    <br />
                                        <br />
                                        <asp:RadioButton ID="rbBnrToCDR" runat="server" GroupName="CDR" OnCheckedChanged="rbBnrToCDR_CheckedChanged" AutoPostBack="True" />
                                        &nbsp;Connect B-number to CDR
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top;" id="tblCellNewCDR">
                                        <br />
                                        <asp:Panel ID="Panel1" runat="server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label21" runat="server" Text="Label" >Servid:</asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtCDRServId" runat="server" ToolTip="Serv ID should be an integer of max length 4"></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Servid cannot be blank." ControlToValidate="txtCDRServId" ValidationGroup="CreateCDR" Display="Dynamic" ForeColor="Red" EnableClientScript="true" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:RangeValidator runat="server" Type="Integer" MinimumValue="0001" MaximumValue="9999" ControlToValidate="txtCDRServId" ValidationGroup="CreateCDR" ErrorMessage="Servid must be a number of length 4." Display="Dynamic" ForeColor="Red" EnableClientScript="true" SetFocusOnError="True" />
                                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[0-9\s]{4,4}$" ErrorMessage="Please enter 4 digits number only" ControlToValidate="txtCDRServId" SetFocusOnError="True" ValidationGroup="CreateCDR" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Servid cannot be blank." SetFocusOnError="True" ControlToValidate="txtCDRServId" ForeColor="Red" ValidationGroup="CreateCDR" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RangeValidator runat="server" Type="Integer" MinimumValue="1000" MaximumValue="9999" ControlToValidate="txtCDRServId"  ValidationGroup="CreateCDR"  ErrorMessage="Please enter 4 digits number only." Display="Dynamic" ForeColor="Red" />--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label22" runat="server" Text="Label" ToolTip =" ">Call Type:</asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtCDRCallType" runat="server" ToolTip=" Call Type should be an integer of max length 4" ></asp:TextBox>
                                                        <br />

                                                        <%-- <asp:RangeValidator runat="server" Type="Integer" MinimumValue="1000" MaximumValue="9999" ControlToValidate="txtCDRCallType"  ValidationGroup="CreateCDR"  ErrorMessage="Please enter 4 digits number only." Display="Dynamic" ForeColor="Red" />
                                                        --%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Call Type cannot be blank." SetFocusOnError="True" ControlToValidate="txtCDRCallType" ForeColor="Red" ValidationGroup="CreateCDR" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^[0-9\s]{4,4}$" ErrorMessage="Please enter 4 digits number only" ControlToValidate="txtCDRCallType" SetFocusOnError="True" ValidationGroup="CreateCDR" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label23" runat="server" Text="Label">ASOC:</asp:Label>
                                                        <br />
                                                        <%--  <asp:TextBox ID="TextBox1" runat="server" maxlength="8" type ="number" min="10000000" max="99999999"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtCDRASOC" runat="server" ToolTip="ASOC ID should be an integer of max length 8"></asp:TextBox>
                                                        <br />
                                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationExpression="^[0-9\s]{8,8}$" ErrorMessage="Please enter 8 digits numbers only" ControlToValidate="txtCDRASOC" SetFocusOnError="True" ValidationGroup="CreateCDR" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="ASOC cannot be blank." SetFocusOnError="True" ControlToValidate="txtCDRASOC" ForeColor="Red" ValidationGroup="CreateCDR" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label24" runat="server" Text="Label">Description:</asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtCDRDescription" runat="server" Rows="2" TextMode="MultiLine" Width="400" MaxLength="30"></asp:TextBox>
                                                        <br />
                                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationExpression="^[a-zA-Z0-9\s]{1,30}$" ErrorMessage="Max length is 30 characters" ControlToValidate="txtCDRDescription" SetFocusOnError="True" ValidationGroup="CreateCDR" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Description cannot be blank." SetFocusOnError="True" ControlToValidate="txtCDRDescription" ForeColor="Red" ValidationGroup="CreateCDR" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: bottom">
                                                    <td>
                                                        <br />
                                                        <asp:Button ID="btnCDR" runat="server" Text="Create CDR" ValidationGroup="CreateCDR" OnClick="btnCDR_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnCDRReset" runat="server" Text="Reset" OnClick="btnCDRReset_Click" />
                                                        <%--<input type="reset" value="Reset" />--%>
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="lblCreateCDR" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="vertical-align: top;" id="tblCellBnrToCDR">
                                        <br />
                                        <asp:Panel ID="Panel2" runat="server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label17" runat="server" Text="Label">Select CDR:</asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddSelectCDR" runat="server" Width="180px" Height="25px" TabIndex="5" ValidationGroup="BnrToCDR"></asp:DropDownList>
                                                        <br />
                                                        <asp:CustomValidator ID="CustomValidatorddlselectcdr" runat="server" ErrorMessage="CustomValidator" ControlToValidate="ddSelectCDR" OnServerValidate="CustomValidatorddlselectcdr_ServerValidate" ValidationGroup="BnrToCDR" Display="Dynamic" ForeColor="Red" SetFocusOnError="True"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <br />

                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label18" runat="server" Text="Label">B number:</asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtBnumber" runat="server" ValidationExpression="\d+"  ControlToValidate="txtBnumber" ValidationGroup="BnrToCDR" ErrorMessage="Only Numbers allowed"></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="B number cannot be blank." ControlToValidate="txtBnumber" ValidationGroup="BnrToCDR" SetFocusOnError="True" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <%-- <asp:RangeValidator runat="server" Type="Integer" MinimumValue="0" MaximumValue="99999999" ControlToValidate="txtBnumber"  ValidationGroup="BnrToCDR"  ErrorMessage="Value must be a number." Display="Dynamic" ForeColor="Red" />--%>

                                                        <%--<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator" ControlToValidate="txtBnumber" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="BnrToCDR" Display="Dynamic" ForeColor="Red" SetFocusOnError="True"></asp:CustomValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label19" runat="server" Text="Label">Repeat Frequency:</asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddRepeatFrequency" runat="server" Width="180" Height="25" TabIndex="5" ValidationGroup="BnrToCDR"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label20" runat="server" Text="Label">Optional Text:</asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtBnrOptionalText" runat="server" Rows="2" TextMode="MultiLine" Width="400" WatermarkText="Ex. : Which Country, Area code, etc (Max. 20 characters)" ValidationGroup="BnrToCDR" CausesValidation="True" onkeypress="return this.value.length<=20"></asp:TextBox>
                                                        <br />
                                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ValidationExpression="^[a-zA-Z0-9\s]{1,20}$" ErrorMessage="Optional text cannot have more than 20 characters" ControlToValidate="txtBnrOptionalText" SetFocusOnError="True" ValidationGroup="BnrToCDR" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                        <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtBnrOptionalText_TextBoxWatermarkExtender" TargetControlID="txtBnrOptionalText" ID="txtBnrOptionalText_TextBoxWatermarkExtender" WatermarkText="Ex. : Which Country, Area code, etc"></cc1:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: bottom">
                                                    <td>
                                                        <br />
                                                        <asp:Button ID="btnBnrToCDR" runat="server" Text="Connect Bnr to CDR" OnClick="btnBnrToCDR_Click" ValidationGroup="BnrToCDR" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="lblBnrToCDRReset" runat="server" Text="Reset" OnClick="btnBnrToCDRReset_Click" />
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="lblBnrToCDR" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>

                <%--Price Plan Layer--%>
                <div id="menu2" class="tab-pane fade">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <%--<h3>Price Plan Layer</h3>--%>
                            <table>
                                <tr>
                                    <td style="vertical-align: top">
                                        <br />
                                        <asp:RadioButton ID="rbCreateNewPricePlan" runat="server" GroupName="PricePlan" AutoPostBack="True" OnCheckedChanged="rbCreateNewPricePlan_CheckedChanged" />
                                        &nbsp;Create New Price Plan
                                    <br />
                                        <br />
                                        <asp:RadioButton ID="rbLinkPricePlanDb2" runat="server" GroupName="PricePlan" AutoPostBack="True" OnCheckedChanged="rbLinkPricePlanDb2_CheckedChanged" />
                                        &nbsp;Link Price Plan to DB2
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top;" id="tblCellNewPricePlan">
                                        <br />
                                        <asp:Panel ID="Panel3" runat="server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="Label25" runat="server" Text="Label">Price Plan</asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtPricePlan" runat="server"></asp:TextBox>
                                                        <br />
                                                        <asp:RangeValidator runat="server" Type="Integer" MinimumValue="0" MaximumValue="99999999" ControlToValidate="txtPricePlan" ValidationGroup="CreatePricePlan" ErrorMessage="Price Plan must be a number." Display="Dynamic" ForeColor="Red" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Price Plan cannot be blank" ControlToValidate="txtPricePlan" Display="Dynamic" ValidationGroup="CreatePricePlan" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="Label28" runat="server" Text="Label">Price Plan contains Plan Option?</asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbPlanOptionYes" runat="server" GroupName="PPContainPO" TabIndex="8" />&nbsp;Yes
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbPlanOptionNo" runat="server" GroupName="PPContainPO" Checked="True" TabIndex="9" />&nbsp;No
                                                        <br />
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="Label31" runat="server" Text="Label">Price Plan contains Duration allowance?</asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbDurationAllowanceYes" runat="server" GroupName="PPContainDuAllow" TabIndex="14" />&nbsp;Yes
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbDurationAllowanceNo" runat="server" GroupName="PPContainDuAllow" Checked="True" TabIndex="15" />&nbsp;No
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label26" runat="server" Text="Label">Choose the type of Plan:</asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="RadioButton1" runat="server" GroupName="TypeOfPP" Checked="True" TabIndex="1" />&nbsp;B=Bill period plan
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="RadioButton2" runat="server" GroupName="TypeOfPP" TabIndex="2" />&nbsp;O=Bonus plan (Volym)
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="RadioButton3" runat="server" GroupName="TypeOfPP" TabIndex="3" />&nbsp;P=Plan period plan
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="RadioButton4" runat="server" GroupName="TypeOfPP" TabIndex="4" />&nbsp;G=Global plan
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="RadioButton5" runat="server" GroupName="TypeOfPP" TabIndex="5" />&nbsp;U=Usage category plan
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label29" runat="server" Text="Label">Price Plan contains a Country Group?</asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbCountryGroupYes" runat="server" GroupName="PPContainCountryGroup" TabIndex="10" />&nbsp;Yes
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbCountryGroupNo" runat="server" GroupName="PPContainCountryGroup" Checked="True" TabIndex="11" />&nbsp;No
                                                        <br />
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label323" runat="server" Text="Label">Select DB2 Environment to connect this Price Plan:</asp:Label>
                                                        <br />
                                                        (Optional)
                                                        <br />
                                                        <asp:DropDownList ID="ddPricePlanConnectDB2" runat="server" Height="25" Width="180" TabIndex="16"></asp:DropDownList>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label27" runat="server" Text="Label">Choose an alternative:</asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbAlternativeN" runat="server" GroupName="PlanAlternative" Checked="True" TabIndex="6" />&nbsp;N=Normal Price Plan
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbAlternativeC" runat="server" GroupName="PlanAlternative" TabIndex="7" />&nbsp;C=Centrex Price Plan
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="Label30" runat="server" Text="Label">Price Plan contains a Pick-a-point?</asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbPickPointYes" runat="server" GroupName="PPContainPickPoint" TabIndex="12" />&nbsp;Yes
                                                        <br />
                                                        &nbsp;&nbsp;<asp:RadioButton ID="rbPickPointNo" runat="server" GroupName="PPContainPickPoint" Checked="True" TabIndex="13" />&nbsp;No
                                                        <br />
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:Label ID="lblCreatePricePlan" runat="server" Font-Italic="True" ForeColor="#3366FF"></asp:Label>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:Label ID="Label33" runat="server" Text="Label">Optional Text:</asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtPricePlanOptionalText" runat="server" Rows="2" TextMode="MultiLine" Width="400" TabIndex="17"></asp:TextBox>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnCreatePricePlan" runat="server" Text="Create Price Plan" OnClick="btnCreatePricePlan_Click" ValidationGroup="CreatePricePlan" TabIndex="18" />
                                            &nbsp; &nbsp;
                                            <asp:Button ID="btnPricePlanReset" runat="server" Text="Reset" OnClick="btnPricePlanReset_Click" TabIndex="19" />
                                        </asp:Panel>
                                    </td>
                                    <td style="vertical-align: top;" id="tblCellLinkPPToDB">
                                        <asp:Panel ID="Panel4" runat="server" Visible="false">
                                            <br />
                                            <table>
                                                <tr>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="Label32" runat="server" Text="Select Price Plans:" Font-Bold="True"></asp:Label>
                                                        <br />
                                                        <div id="divGridView" style="height: 330px; width: 450px; overflow: auto;">
                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvSelectPricePlan" runat="server" AutoGenerateColumns="False">
                                                                        <AlternatingRowStyle BackColor="#F0F0F0" />
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="chkRow_CheckedChanged" AutoPostBack="true" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField HeaderText="Price Plan" DataField="Prisplan" ItemStyle-Width="130px" HeaderStyle-Height="35px">
                                                                                <ItemStyle Width="100px" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField HeaderText="Description" DataField="Ben" ItemStyle-Width="230px" HeaderStyle-Height="35px">
                                                                                <ItemStyle Width="200px" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField HeaderText="Price Plan No." DataField="PPlpnr" ItemStyle-Width="130px" HeaderStyle-Height="35px">
                                                                                <ItemStyle Width="110px" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#9900FF" ForeColor="White" HorizontalAlign="Center" />
                                                                        <RowStyle Height="30px" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <div style="width: 250px;">
                                                            <asp:Label ID="Label34" runat="server" Text="Selected Price Plans:" Font-Bold="True"></asp:Label>
                                                            <br />
                                                            <asp:ListBox ID="lbSelectedPricePlan" runat="server" Height="150px" Width="240px" Enabled="False" ValidationGroup="PricePlanLink"></asp:ListBox>
                                                            <br />
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please select at least 1 Price Plan from." ControlToValidate="lbSelectedPricePlan" ValidationGroup="PricePlanLink"></asp:RequiredFieldValidator>--%>
                                                            <br />
                                                            <asp:Label ID="Label354" runat="server" Text="Select DB2 Environment to link:" Font-Bold="True"></asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="ddDB2Environment" runat="server" AutoPostBack="True" Height="25" Width="100"></asp:DropDownList>
                                                            &nbsp;
                                                            <asp:Button ID="btnLinkPPtoDb2Environment" runat="server" Text="Update" OnClick="btnLinkPPtoDb2Environment_Click" ValidationGroup="PricePlanLink" />
                                                            &nbsp;
                                                            <asp:Button ID="btnLinkPPtoDB2Reset" runat="server" Text="Reset" OnClick="btnLinkPPtoDB2Reset_Click" />
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="lblLinkPPtoDb2Environment" runat="server" ForeColor="#3366FF" Font-Italic="True"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <%--Category Code Layer--%>
                <div id="menu3" class="tab-pane fade">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <%--<h3>Category Code Layer</h3>--%>
                            <p>
                                <br />
                                <asp:Label ID="Label2" runat="server" Text="Label">Call Category:</asp:Label>
                                <br />
                                <asp:TextBox ID="txtCallCategory" runat="server" ControlToValidate="txtCallCategory" ValidationGroup="CallCategory"></asp:TextBox>
                                <%--<asp:RangeValidator runat="server" Type="Integer" MinimumValue="0" MaximumValue="999999999"  ControlToValidate="txtCallCategory"  ValidationGroup="CallCategory"  ErrorMessage="Value must be a number." Display="Dynamic" ForeColor="Red" />--%>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationExpression = "^[\s\S]{8,8}$" ErrorMessage="Please enter 8 characters" ControlToValidate="txtCallCategory" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                <cc1:AutoCompleteExtender runat="server" TargetControlID="txtCallCategory" ID="txtCallCategory_AutoCompleteExtender1" MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="False" CompletionSetCount="1" ServiceMethod="CallCategoryAutoComplete" ServicePath="WebService1.asmx" FirstRowSelected="True"></cc1:AutoCompleteExtender>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Call Category cannot be blank." SetFocusOnError="True" ControlToValidate="txtCallCategory" ForeColor="Red" ValidationGroup="CallCategory" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[0-9\s]{8,8}$" ErrorMessage="Please enter 8 digits." ControlToValidate="txtCallCategory" SetFocusOnError="True" ValidationGroup="CallCategory" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>

                                <br />
                                <asp:Label ID="Label3" runat="server" Text="Label">Name:</asp:Label>
                                <br />
                                <asp:TextBox ID="txtName" runat="server" MaxLength="30" ValidationGroup="CallCategory"></asp:TextBox>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Name cannot be blank." SetFocusOnError="True" ControlToValidate="txtName" ForeColor="Red" ValidationGroup="CallCategory"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                                <asp:Button ID="btnCallCategoryUpdate" runat="server" Text="Update" ValidationGroup="CallCategory" OnClick="btnCallCategoryUpdate_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnCallCategoryReset" runat="server" Text="Reset" OnClick="btnCallCategoryReset_Click" CausesValidation="False" />
                                <br />
                                <br />
                                <asp:Label ID="lblCallCategory" runat="server" ForeColor="#3366FF" Font-Italic="True"></asp:Label>
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <%--Plan Option Layer--%>
                <div id="menu4" class="tab-pane fade">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <%--<h3>Plan Option Layer</h3>--%>
                            <p>
                                <br />
                                <asp:Label ID="Label1" runat="server" Text="Label">Plan Option:</asp:Label>
                                <br /> <br />
                                <asp:TextBox ID="txtPlanOption" runat="server" ValidationGroup="PlanOption"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" TargetControlID="txtPlanOption" ID="txtPlanOption_AutoCompleteExtender" MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="False" CompletionSetCount="1" ServiceMethod="PlanOptionAutoComplete" ServicePath="WebService1.asmx" FirstRowSelected="True"></cc1:AutoCompleteExtender>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Plan Option cannot be blank." ControlToValidate="txtPlanOption" ForeColor="Red" SetFocusOnError="True" ValidationGroup="PlanOption"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                                <asp:Button ID="btnPlanOptionUpdate" runat="server" Text="Update" OnClick="btnPlanOptionUpdate_Click" ValidationGroup="PlanOption" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnPlanOptionReset" runat="server" Text="Reset" OnClick="btnPlanOptionReset_Click" CausesValidation="False" />
                                <br />
                                <br />
                                <asp:Label ID="lblPlanOption" runat="server" ForeColor="#3366FF" Font-Italic="True"></asp:Label>
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                    <%--New Country Entry--%>
                <div id="menu5" class="tab-pane fade">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                           
                            <p>
                                <br />
                                <asp:Label ID="Label5" runat="server" Text="Label">Country Name:</asp:Label>
                                <br />
                                <asp:TextBox ID="txtNewCountryInsert" runat="server" ></asp:TextBox>                                
                                <asp:Button ID="BtnNewCountryInsert" runat="server" Text="Update" OnClick="btnNewCountryInsert_Click"  />
                                &nbsp;&nbsp;
                              <%--  <asp:Button ID="Button2" runat="server" Text="Reset" OnClick="btnPlanOptionReset_Click" />--%>
                                <br />
                                <br />
                                <asp:Label ID="lblcountryoption" runat="server" ForeColor="#3366FF" Font-Italic="True"></asp:Label>
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--        <asp:HiddenField ID="TabName" runat="server" />
        <script type="text/javascript">
            $(function () {
                var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "personal";
                $('#Tabs a[href="#' + tabName + '"]').tab('show');
                $("#Tabs a").click(function () {
                    $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
                });
            });
        </script>--%>
    </body>
    </html>

</asp:Content>
