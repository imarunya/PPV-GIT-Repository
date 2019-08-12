<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateTestData.aspx.cs" Inherits="PPVTool.DataCreation.CreateTestData" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $("#<%=txtCustomerStratDate.ClientID %>").datepicker();
            $("#<%=txtCustomerEndDate.ClientID %>").datepicker();
            $("#<%=txtBillStartDate.ClientID %>").datepicker();
            $("#<%=txtBillEndDate.ClientID %>").datepicker();

        });


        function RadioCheck(rb) {
            var gv = document.getElementById("<%=grdCreatedCDR.ClientID%>");
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

    <style>
        legend {
            font-size: 12pt;
            font-weight: normal;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="scm" runat="server"></asp:ScriptManager>
    <div style="width: 100%;">
        <div style="text-align: center;">
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
        </div>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <div class="container" style="padding-left: 180px; padding-top: 70px; width: 60%;">
                    <fieldset>
                        <legend>Choose an alternative</legend>
                        <table style="width: 100%;" class="table">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb1" runat="server" Text="Create a new test case" GroupName="rb" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb2" runat="server" Text="Change existing test case" GroupName="rb" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb3" runat="server" Text="Create regression test" GroupName="rb" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb4" runat="server" Text="Copy test cases" GroupName="rb" />
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top: none; text-align: right; padding-right: 550px;">
                                    <asp:Button ID="btnNext" Text="Next" runat="server" OnClick="btnNext_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View runat="server" ID="View2">
                <div class="container" style="width: 80%;">
                    <fieldset>
                        <legend style="text-align: center;">KEY FEATURES FOR NEW TESTS</legend>
                        <table style="width: 100%;" class="table">
                            <tr>
                                <td style="width: 20%">Select UP-release
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlUprelease" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Choose environment
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlEnviornment" runat="server" OnSelectedIndexChanged="ddlEnviornment_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Choose price plan
                                </td>
                                <td style="width: 45%">
                                    <asp:Panel ID="pnlGrid" runat="server" Width="100%" ScrollBars="Vertical" Height="300px">
                                        <asp:GridView ID="grdPricePlan" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="4" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" ForeColor="Black">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select Plan">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkGrid" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Prisplan" HeaderText="Prisplan" />
                                                <asp:BoundField DataField="Ben" HeaderText="Ben" />
                                                <asp:BoundField DataField="LPNR" HeaderText="PP-lpnr" />
                                            </Columns>
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td><%--Number of pp:<asp:Label ID="lblCount" runat="server" Text="0"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Optional text
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="350px" Height="60px"></asp:TextBox>
                                </td>
                                <td><%--Number of tkn:<asp:Label ID="lblCountWords" runat="server"></asp:Label>--%>
                                </td>

                            </tr>
                            <%--                            <tr>
                                <td>Word document
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDocument" runat="server"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="3" style="padding-left: 650px;">
                                    <asp:Button ID="btnNext1" Text="Next" runat="server" OnClick="btnNext1_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>


                </div>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <div class="container" style="width: 80%">
                    <fieldset>
                        <legend style="text-align: center;">Merge price plans with customers
                            <asp:Literal ID="ltid" runat="server"></asp:Literal></legend>
                        <table style="width: 100%;" class="table">
                            <tr>
                                <td style="width: 20%">Choose a customer from BiClager
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upCompCust" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtCombCust" runat="server"
                                                Width="150px" ReadOnly="true" />
                                            <asp:Panel runat="server" ID="Panel1"
                                                Style="max-height: 200px; display: none; visibility: hidden;">
                                                <!-- GridView  goes here -->
                                                <asp:GridView ID="grdCombCust" runat="server"
                                                    AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="grdCombCust_SelectedIndexChanged">
                                                    <HeaderStyle BackColor="White" />
                                                    <RowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="BIC-lager-nr"
                                                            HeaderText="ID" />
                                                        <asp:BoundField DataField="Ben"
                                                            HeaderText="Benämning" />
                                                        <asp:BoundField DataField="LAN-userid"
                                                            HeaderText="Userid" />
                                                        <asp:BoundField DataField="Datum"
                                                            HeaderText="Datum" />
                                                        <asp:BoundField DataField="Kund-typ"
                                                            HeaderText="Kund-typ" />
                                                        <asp:BoundField DataField="Agree-typ"
                                                            HeaderText="Agree-koppling" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#999999" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <cc1:DropDownExtender ID="DropDownExtender2" runat="server"
                                                DropDownControlID="Panel1"
                                                TargetControlID="txtCombCust">
                                            </cc1:DropDownExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 15%">View Customer Structure
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upCustStruc" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtCustStruc" runat="server"
                                                Width="150px" ReadOnly="true" />
                                            <asp:Panel runat="server" ID="Panel2"
                                                Style="max-height: 200px; display: none; visibility: hidden;">
                                                <!-- GridView  goes here -->
                                                <asp:GridView ID="grdCustStruc" runat="server"
                                                    AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="grdCustStruc_SelectedIndexChanged">
                                                    <HeaderStyle BackColor="White" />
                                                    <RowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="CustID"
                                                            HeaderText="Cust-id" />
                                                        <asp:BoundField DataField="BillEniId"
                                                            HeaderText="Bill-ent-id" />
                                                        <asp:BoundField DataField="ChrgbNumId"
                                                            HeaderText="Chrgb-num-id" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#999999" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <cc1:DropDownExtender ID="DropDownExtender3" runat="server"
                                                DropDownControlID="Panel2"
                                                TargetControlID="txtCustStruc">
                                            </cc1:DropDownExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="text-align: center;">Merge agreement with price plans</td>
                            </tr>
                            <tr>
                                <td>Agreement
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtAgreement" runat="server"
                                                Width="150px" ReadOnly="true" />
                                            <asp:Panel runat="server" ID="Panel3"
                                                Style="max-height: 200px; display: none; visibility: hidden;">
                                                <!-- GridView  goes here -->
                                                <asp:GridView ID="grdAgreements" runat="server"
                                                    AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="grdAgreements_SelectedIndexChanged">
                                                    <HeaderStyle BackColor="White" />
                                                    <RowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="AgreementID"
                                                            HeaderText="Agree-id" />
                                                        <asp:BoundField DataField="CustID"
                                                            HeaderText="Cust-id" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#999999" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <cc1:DropDownExtender ID="DropDownExtender4" runat="server"
                                                DropDownControlID="Panel3"
                                                TargetControlID="txtAgreement">
                                            </cc1:DropDownExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td rowspan="2" style="vertical-align: middle;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/arrow.png" Height="50px" Width="50px" OnClick="ImageButton1_Click" />
                                </td>
                                <td rowspan="2" colspan="2">
                                    <asp:GridView ID="grdPricePlanDtls" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="pplpnr" BorderWidth="0">
                                        <Columns>
                                            <asp:BoundField DataField="Agreement" HeaderText="Agreement" />
                                            <asp:BoundField DataField="CustId" HeaderText="Cust Id" />
                                            <asp:BoundField DataField="PricePlan" HeaderText="Price Plan" />
                                            <asp:BoundField DataField="pplpnr" HeaderText="PP Lpnr" Visible="false" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div>
                                                <table cellspacing="0" rules="all" border="1" style="width: 100%; border-collapse: collapse;">
                                                    <tr>
                                                        <th scope="col">Agreement</th>
                                                        <th scope="col">Cust Id</th>
                                                        <th scope="col">Price Plan</th>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>Price Plans
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hfPriceplanDesc" runat="server" />
                                            <asp:TextBox ID="txtPricePlans" runat="server"
                                                Width="150px" ReadOnly="true" />
                                            <asp:Panel runat="server" ID="Panel4"
                                                Style="max-height: 200px; display: none; visibility: hidden;">
                                                <!-- GridView  goes here -->
                                                <asp:GridView ID="grdSelectedPricePlans" runat="server"
                                                    AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="grdSelectedPricePlans_SelectedIndexChanged">
                                                    <HeaderStyle BackColor="White" />
                                                    <RowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Prisplan"
                                                            HeaderText="Prisplan" />
                                                        <asp:BoundField DataField="Ben"
                                                            HeaderText="Benämning" />
                                                        <asp:BoundField DataField="PPLpnr"
                                                            HeaderText="ID" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#999999" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <cc1:DropDownExtender ID="DropDownExtender5" runat="server"
                                                DropDownControlID="Panel4"
                                                TargetControlID="txtPricePlans">
                                            </cc1:DropDownExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>


                                </td>
                            </tr>
                            <tr>
                                <td>Customer start date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerStratDate" runat="server"></asp:TextBox>
                                </td>
                                <td>Customer end date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerEndDate" runat="server"></asp:TextBox>
                                </td>
                                <td rowspan="2" colspan="2">
                                    <asp:HiddenField ID="hdp_bic20" runat="server" Value="N" />
                                    <asp:HiddenField ID="hdp_bic30" runat="server" Value="N" />
                                    <asp:HiddenField ID="hdp_bic35" runat="server" Value="N" />
                                    <asp:HiddenField ID="hdp_bic45" runat="server" Value="N" />
                                    <asp:HiddenField ID="hdbicnr" runat="server" />
                                    <asp:HiddenField ID="hdCustNo" runat="server" />
                                    <asp:HiddenField ID="hdp_CDRmeny" runat="server" />

                                </td>
                            </tr>
                            <tr>
                                <td>Bill-start date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillStartDate" runat="server"></asp:TextBox>
                                </td>

                                <td>Bill-end date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillEndDate" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="padding-left: 750px;">
                                    <asp:Button ID="btnNext3" Text="Next" runat="server" OnClick="btnNext3_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View4" runat="server">
                <div style="width: 80%">
                    <div style="text-align: center; font-size: medium; font-weight: bold;">
                        Supplementary connections
                            <asp:Literal ID="lblTestNo" runat="server"></asp:Literal>
                    </div>
                    <div style="text-align: center;">
                        <asp:Literal ID="lblDateHeader" runat="server"></asp:Literal>
                    </div>
                    <asp:Panel ID="pnl1" runat="server">
                        <fieldset>
                            <legend style="font-size: small; font-weight: normal;">Merge a price plan / agreement with option</legend>
                            <table style="width: 100%" class="table">
                                <tr>
                                    <td style="width: 15%">Price plan / agreement</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlpriceplanoption" runat="server"></asp:DropDownList>
                                    </td>
                                    <td rowspan="2" style="vertical-align: middle; align-items: flex-start;">
                                        <asp:ImageButton ID="ImgbtnOption" runat="server" ImageUrl="~/Images/arrow.png" Height="50px" Width="50px" OnClick="ImgbtnOption_Click" /></td>
                                    <td rowspan="2" style="vertical-align: middle;">
                                        <asp:GridView ID="grdPPOption" runat="server"></asp:GridView>
                                    </td>
                                    <td rowspan="2" style="vertical-align: middle;">
                                        <asp:Button ID="btnClearOption" runat="server" Text="Clear" OnClick="btnClearOption_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">Select option</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlKombPlan" runat="server"></asp:DropDownList></td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnl2" runat="server">
                        <fieldset>
                            <legend style="font-size: small; font-weight: normal;">Merge a price plan / agreement with country</legend>
                            <table style="width: 100%" class="table">
                                <tr>
                                    <td style="width: 15%">Price plan / agreement</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlpriceplancountry" runat="server"></asp:DropDownList></td>
                                    <td rowspan="2" style="vertical-align: middle; align-items: flex-start;">
                                        <asp:ImageButton ID="ImgbtnCountry" runat="server" ImageUrl="~/Images/arrow.png" Height="50px" Width="50px" OnClick="ImgbtnCountry_Click" /></td>
                                    <td rowspan="2" style="vertical-align: middle;">
                                        <asp:GridView ID="grdPPCountry" runat="server"></asp:GridView>
                                    </td>
                                    <td rowspan="2" style="vertical-align: middle;">
                                        <asp:Button ID="btnClearCountry" runat="server" Text="Clear" OnClick="btnClearCountry_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">Select country</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlKombCountry" runat="server"></asp:DropDownList></td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnl3" runat="server">
                        <fieldset>
                            <legend style="font-size: small; font-weight: normal;">Merge a price plan / agreement with Telephone</legend>
                            <table style="width: 100%" class="table">
                                <tr>
                                    <td style="width: 15%">Price plan / agreement</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlPricePlanTelephone" runat="server"></asp:DropDownList></td>
                                    <td rowspan="4" style="vertical-align: middle; align-items: flex-start;">
                                        <asp:ImageButton ID="ImgbtnTele" runat="server" ImageUrl="~/Images/arrow.png" Height="50px" Width="50px" OnClick="ImgbtnTele_Click" /></td>
                                    <td rowspan="4" style="vertical-align: middle;">
                                        <asp:GridView ID="grdPPTelephone" runat="server"></asp:GridView>
                                    </td>
                                    <td rowspan="4" style="vertical-align: middle;">
                                        <asp:Button ID="btnClearTelephone" runat="server" Text="Clear" OnClick="btnClearTelephone_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">Start telephone no:</td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtStartTelNo" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">Stop telephone no:</td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtEndTelNo" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">Enter repeat</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlRepeat" runat="server">
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnl4" runat="server">
                        <fieldset>
                            <legend style="font-size: small; font-weight: normal;">Link price plan / agreement with duration allowance</legend>
                            <table style="width: 100%" class="table">
                                <tr>
                                    <td style="width: 15%">Price plan / agreement</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlDurationAllowwance" runat="server"></asp:DropDownList></td>
                                    <td rowspan="4" style="vertical-align: middle; align-items: flex-start;">
                                        <asp:ImageButton ID="imgbtnAllowance" runat="server" ImageUrl="~/Images/arrow.png" Height="50px" Width="50px" OnClick="imgbtnAllowance_Click" /></td>
                                    <td rowspan="4" style="vertical-align: middle;">
                                        <asp:GridView ID="grdPPAllowance" runat="server"></asp:GridView>
                                    </td>
                                    <td rowspan="4" style="vertical-align: middle;">
                                        <asp:Button ID="btnClearAllownce" runat="server" Text="Clear" OnClick="btnClearAllownce_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">Enter duration:
                                        <br />
                                        (Max 11 digits, of which 2 are decimal places)</td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtDurationAllowance" runat="server"></asp:TextBox><br />
                                        (in seconds)
                                        
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <div style="padding-left: 750px;">
                        <asp:Button ID="btnNxtCountry" Text="Next" runat="server" OnClick="btnNxtCountry_Click" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID="View5" runat="server">
                <div style="width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%" class="table">
                                <tr>
                                    <td colspan="2" style="width: 100%">
                                        <asp:GridView ID="grdCDRFinal" runat="server" AutoGenerateDeleteButton="true" OnRowDeleting="grdCDRFinal_RowDeleting"></asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center; width: 100%">
                                        <asp:ImageButton ID="imgCalculateCDR" runat="server" ImageUrl="~/Images/uparrow.jpg" Width="30" Height="30" OnClick="imgCalculateCDR_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%" class="table">
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <legend>Who has called</legend>
                                                        <table class="table">
                                                            <tr>
                                                                <td style="width: 180px;">Choose customer(Chrgb No) :</td>
                                                                <td style="text-align: left; width: 180px;">
                                                                    <asp:TextBox ID="txtCustomer" runat="server"
                                                                        Width="150px" ReadOnly="true" />
                                                                    <asp:Panel runat="server" ID="pnlCust"
                                                                        Style="max-height: 200px; display: none; visibility: hidden;">
                                                                        <!-- GridView  goes here -->
                                                                        <asp:GridView ID="grdCust" runat="server"
                                                                            AutoGenerateColumns="false"
                                                                            OnSelectedIndexChanged="grdCust_SelectedIndexChanged">
                                                                            <HeaderStyle BackColor="White" />
                                                                            <RowStyle BackColor="White" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Cust-id"
                                                                                    HeaderText="Cust-id" />
                                                                                <asp:BoundField DataField="Bill-ent-id"
                                                                                    HeaderText="Bill-ent-id" />
                                                                                <asp:BoundField DataField="Chrgb-num-id"
                                                                                    HeaderText="Chrgb-num-id" />
                                                                                <asp:BoundField DataField="Anr"
                                                                                    HeaderText="Anr" />
                                                                            </Columns>
                                                                            <SelectedRowStyle BackColor="#999999" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                    <cc1:DropDownExtender ID="DropDownExtender1" runat="server"
                                                                        DropDownControlID="pnlCust"
                                                                        TargetControlID="txtCustomer">
                                                                    </cc1:DropDownExtender>
                                                                </td>
                                                                <td style="width: 160px;">Customer price plans :</td>
                                                                <td style="text-align: left; width: 160px;">
                                                                    <%--<asp:DropDownList ID="ddlCDRPricePlan" runat="server"></asp:DropDownList>--%>
                                                                    <asp:TextBox ID="txtCDRPricePlan" runat="server" Width="150px" ReadOnly="true" />
                                                                    <asp:Panel runat="server" ID="pnlCDRPricePlan"
                                                                        Style="max-height: 200px; overflow: scroll; display: none; visibility: hidden;">
                                                                        <!-- GridView  goes here -->
                                                                        <asp:GridView ID="grdCDRPricePlan" runat="server"
                                                                            AutoGenerateColumns="false"
                                                                            OnSelectedIndexChanged="grdCDRPricePlan_SelectedIndexChanged">
                                                                            <HeaderStyle BackColor="White" />
                                                                            <RowStyle BackColor="White" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Cust-id"
                                                                                    HeaderText="Cust-id" />
                                                                                <asp:BoundField DataField="Agree-id"
                                                                                    HeaderText="Agree-id" />
                                                                                <asp:BoundField DataField="Prisplan"
                                                                                    HeaderText="Prisplan" />
                                                                                <asp:BoundField DataField="PP-lpnr"
                                                                                    HeaderText="PP-lpnr" />
                                                                            </Columns>
                                                                            <SelectedRowStyle BackColor="#999999" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                    <cc1:DropDownExtender ID="ddeCDRPricePlan" runat="server"
                                                                        DropDownControlID="pnlCDRPricePlan"
                                                                        TargetControlID="txtCDRPricePlan">
                                                                    </cc1:DropDownExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <legend>Type of call? Is the call for the price plan qualified? </legend>
                                                        <table class="table">
                                                            <tr>
                                                                <td style="width: 180px;">Select CDR no :</td>
                                                                <td style="text-align: left; width: 180px;">
                                                                    <%--<asp:DropDownList ID="ddlCDRNo" runat="server" OnSelectedIndexChanged="ddlCDRNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                                                    <asp:TextBox ID="txtCDRNo" runat="server" Width="150px" ReadOnly="true" />
                                                                    <asp:Panel runat="server" ID="pnlCDRNo"
                                                                        Style="max-height: 500px; overflow: scroll; display: none; visibility: hidden;">
                                                                        <!-- GridView  goes here -->
                                                                        <asp:GridView ID="grdCDRNo" runat="server"
                                                                            AutoGenerateColumns="false"
                                                                            OnSelectedIndexChanged="grdCDRNo_SelectedIndexChanged">
                                                                            <RowStyle BackColor="White" />
                                                                            <HeaderStyle BackColor="White" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CDR-lager-nr"
                                                                                    HeaderText="CDR-lager-nr" />
                                                                                <asp:BoundField DataField="Servid"
                                                                                    HeaderText="Servid" />
                                                                                <asp:BoundField DataField="Calltype"
                                                                                    HeaderText="Calltype" />
                                                                                <asp:BoundField DataField="Asoc"
                                                                                    HeaderText="Asoc" />
                                                                                <asp:BoundField DataField="Ben"
                                                                                    HeaderText="Ben" />
                                                                            </Columns>
                                                                            <SelectedRowStyle BackColor="#999999" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                    <cc1:DropDownExtender ID="ddeCDRNo" runat="server"
                                                                        DropDownControlID="pnlCDRNo"
                                                                        TargetControlID="txtCDRNo">
                                                                    </cc1:DropDownExtender>
                                                                </td>
                                                                <td style="width: 160px;">Qualifying-Ind :</td>
                                                                <td style="text-align: left">
                                                                    <asp:DropDownList ID="ddlQualifyingInd" runat="server">
                                                                        <asp:ListItem Value="N">No - CDR is not qualified</asp:ListItem>
                                                                        <asp:ListItem Value="J">Yes - CDR is qualified</asp:ListItem>
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCDRNo" runat="server" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td>Rec :
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:DropDownList ID="ddlRec" runat="server">
                                                                        <asp:ListItem Value="20">20 - NORMAL CDR</asp:ListItem>
                                                                        <asp:ListItem Value="30">30 - Reprocessing CDR</asp:ListItem>
                                                                        <asp:ListItem Value="50">50 - Kredit CDR</asp:ListItem>
                                                                        <asp:ListItem Value="60">60 - Debit CDR</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Select B No :
                                                                </td>
                                                                <td>
                                                                    <%--<asp:DropDownList ID="ddlBNo" runat="server"></asp:DropDownList>--%>

                                                                    <asp:TextBox ID="txtBNo" runat="server" Width="150px" ReadOnly="true" />
                                                                    <asp:Panel runat="server" ID="pnlBNo"
                                                                        Style="max-height: 300px; overflow: scroll; display: none; visibility: hidden;">
                                                                        <!-- GridView  goes here -->
                                                                        <asp:GridView ID="grdBNo" runat="server"
                                                                            AutoGenerateColumns="false"
                                                                            OnSelectedIndexChanged="grdBNo_SelectedIndexChanged">
                                                                            <RowStyle BackColor="White" />
                                                                            <HeaderStyle BackColor="White" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="B-nr"
                                                                                    HeaderText="B-nr" />
                                                                                <asp:BoundField DataField="Ben"
                                                                                    HeaderText="Ben" />
                                                                                <asp:BoundField DataField="CDR-lager-nr"
                                                                                    HeaderText="CDR-lager-nr" />
                                                                            </Columns>
                                                                            <SelectedRowStyle BackColor="#999999" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                    <cc1:DropDownExtender ID="ddeBNo" runat="server"
                                                                        DropDownControlID="pnlBNo"
                                                                        TargetControlID="txtBNo">
                                                                    </cc1:DropDownExtender>

                                                                </td>
                                                                <td>Service Identifier :
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <%--<asp:DropDownList ID="ddlServiceIdentifier" runat="server"></asp:DropDownList>--%>
                                                                    <asp:TextBox ID="txtServiceIdentifier" runat="server" Width="150px" ReadOnly="true" />
                                                                    <asp:Panel runat="server" ID="pnlServiceIdentifier"
                                                                        Style="max-height: 200px; overflow: scroll; display: none; visibility: hidden;">
                                                                        <!-- GridView  goes here -->
                                                                        <asp:GridView ID="grdServiceIdentifier" runat="server"
                                                                            AutoGenerateColumns="false"
                                                                            OnSelectedIndexChanged="grdServiceIdentifier_SelectedIndexChanged">
                                                                            <RowStyle BackColor="White" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Servid-nr"
                                                                                    HeaderText="Servid-nr" />
                                                                                <asp:BoundField DataField="Beskrivning"
                                                                                    HeaderText="Beskrivning" />
                                                                            </Columns>
                                                                            <SelectedRowStyle BackColor="#999999" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                    <cc1:DropDownExtender ID="ddeServiceIdentifier" runat="server"
                                                                        DropDownControlID="pnlServiceIdentifier"
                                                                        TargetControlID="txtServiceIdentifier">
                                                                    </cc1:DropDownExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Select C No :
                                                                </td>
                                                                <td style="text-align: left" colspan="3">
                                                                    <asp:DropDownList ID="ddlCNo" runat="server"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <legend>How much has the call cost?</legend>
                                                        <table class="table">
                                                            <tr>
                                                                <td style="width: 100px;">Amount :</td>
                                                                <td style="text-align: left; width: 100px;">
                                                                    <asp:TextBox ID="txtCallCostAmount" runat="server" Width="150px"></asp:TextBox></td>
                                                                <td style="text-align: left; width: 100px;">Öre :</td>
                                                                <td style="text-align: left; width: 100px;">
                                                                    <asp:TextBox ID="txtCallCostOre" runat="server" Width="150px"></asp:TextBox></td>
                                                                <td style="text-align: left">
                                                                    <asp:Button ID="btnCallCostClear" runat="server" Text="Clear" /></td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="text-align: left;">
                                        <table style="width: 100%" class="table">
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <legend>Change date through calendar</legend>
                                                        <div>
                                                            <asp:RadioButton ID="rbDateNo" runat="server" Text="No (hide calendar)" GroupName="dd" Checked="true" />
                                                            <asp:RadioButton ID="rbDateYes" runat="server" Text="Yes (show calendar)" GroupName="dd" />
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <legend>When did the call start? How long ?</legend>
                                                        <table class="table">
                                                            <tr>
                                                                <td>Date : 
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txtCallStartDate" runat="server" Width="80px"></asp:TextBox>
                                                                    &nbsp;&nbsp;
                                                            <asp:Label ID="lblCallStartDate" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>HH &nbsp;&nbsp;&nbsp; MM &nbsp;&nbsp;&nbsp; SS</td>
                                                            </tr>
                                                            <tr>
                                                                <td>Time :</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlHH" runat="server"></asp:DropDownList>
                                                                    &nbsp&nbsp&nbsp
                                                            <asp:DropDownList ID="ddlMM" runat="server"></asp:DropDownList>
                                                                    &nbsp&nbsp&nbsp
                                                            <asp:DropDownList ID="ddlSS" runat="server"></asp:DropDownList>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Call duration :</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlCDHH" runat="server"></asp:DropDownList>
                                                                    &nbsp;&nbsp;&nbsp;
                                                            <asp:DropDownList ID="ddlCDMM" runat="server"></asp:DropDownList>
                                                                    &nbsp;&nbsp;&nbsp;
                                                            <asp:DropDownList ID="ddlCDSS" runat="server"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div style="padding-left: 1000px;">
                        <asp:Button ID="btnCDRnext" Text="Next" runat="server" OnClick="btnCDRnext_Click" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID="View6" runat="server">
                <div class="container" style="padding-left: 280px; padding-top: 70px; width: 60%;">
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblheader" runat="server"></asp:Label></legend>
                        <table style="width: 100%;" class="table">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RdoYes" runat="server" Text="Yes" GroupName="rber" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RdoNo" runat="server" Text="No" GroupName="rber" />
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top: none; text-align: right;">
                                    <asp:Button ID="btnExpectedRes" Text="Next" runat="server" OnClick="btnExpectedRes_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View7" runat="server">
                <div style="width: 100%; text-align: left;">
                    <table class="table">
                        <tr>
                            <td colspan="6">

                                <asp:GridView ID="grdCreatedCDR" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rboSelect" runat="server" onclick="RadioCheck(this);" AutoPostBack="true" OnCheckedChanged="rboSelect_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Lpnr" HeaderText="Lpnr" />
                                        <asp:BoundField DataField="Kval" HeaderText="Kval" />
                                        <asp:BoundField DataField="Ben" HeaderText="Ben" />
                                        <asp:BoundField DataField="Datum" HeaderText="Date" />
                                        <asp:BoundField DataField="Tid" HeaderText="Time" />
                                        <asp:BoundField DataField="Samtalslngd" HeaderText="Samtalslngd" />
                                        <asp:BoundField DataField="Belopp" HeaderText="Amount" />
                                        <asp:BoundField DataField="B-nr" HeaderText="B-nr" />
                                        <asp:BoundField DataField="Calltype" HeaderText="Call Type" />
                                        <asp:BoundField DataField="CDR-Asoc" HeaderText="CDR-Asoc" />
                                        <asp:BoundField DataField="CDR-lager-nr" HeaderText="CDR Larger No" />
                                        <asp:BoundField DataField="Chrgb-num-id" HeaderText="Chargb No Id" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 150px;">
                                <fieldset>
                                    <legend>Select the type of entry</legend>
                                    <div>
                                        <asp:RadioButton ID="rboProcentVol" runat="server" GroupName="GroupEntry" Text="Percent (volmetod) " OnCheckedChanged="rboProcentVol_CheckedChanged" AutoPostBack="true" /><br />
                                        <asp:RadioButton ID="rboProcent" runat="server" GroupName="GroupEntry" Text="Percent " OnCheckedChanged="rboProcent_CheckedChanged" AutoPostBack="true" /><br />
                                        <asp:RadioButton ID="rboBelopp" runat="server" GroupName="GroupEntry" Text="Amount" OnCheckedChanged="rboBelopp_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                </fieldset>
                            </td>
                            <td style="width: 150px;">
                                <fieldset>
                                    <legend>Choose ant sections</legend>
                                    &nbsp;
                                    <asp:DropDownList ID="ddlSelection" runat="server"></asp:DropDownList>
                                </fieldset>
                            </td>
                            <td style="width: 200px;">
                                <fieldset>
                                    <legend>Select expected result / section</legend>
                                    <div>
                                        Percent :
                                                <asp:DropDownList ID="ddlProcent1" runat="server" Width="60px"></asp:DropDownList>
                                        &nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlProcent2" runat="server" Width="60px"></asp:DropDownList>
                                    </div>
                                    <br />
                                    <div>
                                        Amount  :
                                                <asp:TextBox ID="txtBel1" runat="server" Width="57px"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="txtBel2" runat="server" Width="66px"></asp:TextBox>

                                    </div>
                                    <br />
                                    <div>
                                        <fieldset>
                                            <legend>Add and Replace Indicator</legend>
                                            <asp:RadioButton ID="RdoAdd" runat="server" Text="Add" GroupName="RPIndicator" Checked="true" />
                                            &nbsp; &nbsp;&nbsp;
                                            <asp:RadioButton ID="RdoReplace" runat="server" Text="Replace" GroupName="RPIndicator" />
                                        </fieldset>
                                    </div>
                                </fieldset>
                            </td>
                            <td style="vertical-align: central; text-align: center; width: 70px; padding-top: 80px;">
                                <asp:ImageButton ID="ImgBtnShiftRight" ImageUrl="~/Images/arrow.png" Height="50px" Width="50px" runat="server" OnClick="ImgBtnShiftRight_Click" />
                            </td>
                            <td style="width: 100px;">
                                <asp:GridView ID="grdCDRCalculation" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" BorderWidth="0">
                                    <Columns>
                                        <asp:BoundField DataField="Selection" HeaderText="Selection" />
                                        <asp:BoundField DataField="DataVal" HeaderText="Percent" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div>
                                            <table cellspacing="0" rules="all" border="1" style="width: 100%; border-collapse: collapse;">
                                                <tr>
                                                    <th scope="col">Selection</th>
                                                    <th scope="col">Percent</th>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                            <td style="width: 70px; text-align: left; vertical-align: central; padding-top: 80px;">
                                <asp:Button ID="btnClearCDR" runat="server" Text="Clear" OnClick="btnClearCDR_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UP21" style="width: 150px;">
                                    <ContentTemplate>
                                        <div style="width: 200px;">
                                            Round off : &nbsp; &nbsp; 
                                            <asp:TextBox ID="txtRoundOff" runat="server" Width="80px" ReadOnly="true" />
                                            <asp:HiddenField ID="hfRoundOff" runat="server" />
                                            <asp:Panel runat="server" ID="pnlRoundOff"
                                                Style="max-height: 200px; overflow: scroll; display: none; visibility: hidden;">
                                                <!-- GridView  goes here -->
                                                <asp:GridView ID="grdRoundOff" runat="server"
                                                    AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="grdRoundOff_SelectedIndexChanged">
                                                    <RowStyle BackColor="White" />
                                                    <HeaderStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Round off"
                                                            HeaderText="Round off" />
                                                        <asp:BoundField DataField="Description"
                                                            HeaderText="Description" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#999999" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <cc1:DropDownExtender ID="ddeRoundOff" runat="server"
                                                DropDownControlID="pnlRoundOff"
                                                TargetControlID="txtRoundOff">
                                            </cc1:DropDownExtender>
                                        </div>
                                        <br />
                                        <div style="width: 200px;">
                                            Precision : &nbsp;&nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtPrecision" runat="server" Width="80px" ReadOnly="true" />
                                            <asp:HiddenField ID="hfPrecision" runat="server" />
                                            <asp:Panel runat="server" ID="pnlPrecision"
                                                Style="max-height: 200px; overflow: scroll; display: none; visibility: hidden;">
                                                <!-- GridView  goes here -->
                                                <asp:GridView ID="grdPrecision" runat="server"
                                                    AutoGenerateColumns="false"
                                                    OnSelectedIndexChanged="grdPrecision_SelectedIndexChanged">
                                                    <RowStyle BackColor="White" />
                                                    <HeaderStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Precision"
                                                            HeaderText="Precision" />
                                                        <asp:BoundField DataField="decimals"
                                                            HeaderText="Number of decimals" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#999999" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <cc1:DropDownExtender ID="ddePrecision" runat="server"
                                                DropDownControlID="pnlPrecision"
                                                TargetControlID="txtPrecision">
                                            </cc1:DropDownExtender>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="align-items: center; vertical-align: central; padding-top: 30px; padding-left: 60px;">
                                <asp:ImageButton ID="imgbuttonDown" ImageUrl="~/Images/arrow.png" Height="50px" Width="50px" runat="server" Enabled="False" OnClick="imgbuttonDown_Click" />
                            </td>
                            <td colspan="3">
                                <asp:GridView ID="grvCDRFinalCalc" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" BorderWidth="0" OnRowCommand="grvCDRFinalCalc_RowCommand" OnRowDeleting="grvCDRFinalCalc_RowDeleting">
                                    <Columns>
                                        <asp:BoundField DataField="Lpnr"
                                            HeaderText="Lpnr" />
                                        <asp:BoundField DataField="section"
                                            HeaderText="Section" />
                                        <asp:BoundField DataField="AmtPro"
                                            HeaderText="Amount*Procent" />
                                        <asp:BoundField DataField="netchrge"
                                            HeaderText="Sect-net-chrge-amt" />
                                        <asp:BoundField DataField="dscntpcnt"
                                            HeaderText="Sect-dscnt-pcnt" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div>
                                            <table cellspacing="0" rules="all" border="1" style="width: 100%; border-collapse: collapse;">
                                                <tr>
                                                    <th scope="col">Lpnr</th>
                                                    <th scope="col">Section</th>
                                                    <th scope="col">Amount*Procent</th>
                                                    <th scope="col">Sect-net-chrge-amt</th>
                                                    <th scope="col">Sect-dscnt-pcnt</th>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                            <td style="align-items: center; vertical-align: central; padding-top: 30px;">
                                <asp:Button ID="btnNextCDRFinal" runat="server" Text="Next" OnClick="btnNextCDRFinal_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:View>
            <asp:View ID="View8" runat="server">
                <div class="container" style="padding-left: 280px; padding-top: 70px; width: 60%;">
                    <asp:Label ID="lblHead1" runat="server"></asp:Label>
                    <fieldset>
                        <legend>Desired ASOC as well as BURC indicator</legend>
                        <table style="width: 100%;" class="table">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rboYes" runat="server" Text="Yes" GroupName="rbCDR" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rboNo" runat="server" Text="No" GroupName="rbCDR" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top: none; text-align: right;">
                                    <asp:Button ID="btnASOCBURC" Text="Next" runat="server" OnClick="btnASOCBURC_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View9" runat="server">
                <div class="container" style="padding-left: 280px; padding-top: 70px; width: 60%;">
                    <asp:Label ID="lblV9Head" runat="server"></asp:Label>
                    <fieldset>
                        <legend>Should this test case initiate Dailyrun?</legend>
                        <table style="width: 100%;" class="table">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbv9Yes" runat="server" Text="Yes" GroupName="rbCDR" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbv9No" runat="server" Text="No" GroupName="rbCDR" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top: none; text-align: right;">
                                    <asp:Button ID="btnV9Next" Text="Next" runat="server" OnClick="btnV9Next_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </asp:View>

        </asp:MultiView>
    </div>
</asp:Content>
