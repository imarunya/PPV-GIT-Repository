<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PPVTool.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" href="Content/styles.css" type="text/css" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            margin: 0;
        }

        .topnav {
            overflow: hidden;
            background-color: #efe7f7;
        }

            .topnav a {
                float: right;
                display: block;
                vertical-align: top;
                color: #efe7f7;
                text-align: right;
                padding: 20px 24px;
                text-decoration: none;
                font-size: 30px;
            }

            .topnav a {
                background-color: #efe7f7;
                color: #663399;
            }

        .label {
            color: #efe7f7;
            padding: 8px;
            font-family: Arial;
        }

        .success {
            background-color: #7892c2;
        }
        /* Green */

        .mybtn {
            background-color: #7892c2;
            -moz-border-radius: 25px;
            -webkit-border-radius: 25px;
            border-radius: 25px;
            border: 2px solid #4e6096;
            display: inline-block;
            cursor: pointer;
            color: #ffffff;
            font-family: Arial;
            font-size: 17px;
            padding: 11px 31px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #283966;
        }

        .text {
            width: 100%;
            padding: 12px 20px;
            margin: 8px 0;
            display: inline-block;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }
    </style>
</head>
<body>
    <form id="login" runat="server">
        <table border="0" style="width: 100%; border-spacing: 0px; margin: 0 auto;">
            <tr>
                <td colspan="2">

                    <div class="topnav">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/capture.png" Style="width: 200px; height: auto;" />
                        <a class="active">PPV TOOL</a>
                    </div>
                </td>
            </tr>

            <%--  <tr>
                    <td colspan="2">
                        <div class="topnav">
                            <a class="active">                                       
                            PPV TOOL</a>
                        </div>
                    </td>
                </tr>--%>

            <tr>
                <td colspan="2" style="padding-left: 150px;">
                    <br />
                    <br />
                    <br />
                    <div>
                        <table>
                            <tr class="label success">
                                <td style="font-weight: bold;">Log in 
                                        <br />
                                    <br />
                                    <br />
                                </td>
                            </tr>

                            <tr>
                                <td>Username
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox class="text" ID="txtUsername" runat="server" ></asp:TextBox>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>Password
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox class="text" ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <br />
                                    <br />
                                </td>

                            </tr>

                            <tr>
                                <td>
                                    <asp:Button class="mybtn" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="color: red; font-weight: bold;">
                                    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-left: 150px;">
                    <br />
                    <br />
                    <br />
                    <div>
                        <table>
                            <tr>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/oval.png" Style="width: 400px; height: 370px; margin-left: 550px; margin-top: -300px" />

                            </tr>

                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
