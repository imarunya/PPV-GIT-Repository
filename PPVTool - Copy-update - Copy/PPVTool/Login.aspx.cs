using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace PPVTool
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnection DB = new DatabaseConnection();

                if (DB.ValidateUser(txtUsername.Text.Trim(), txtPassword.Text.Trim()))
                {
                    Session.Add("CurrentUser", txtUsername.Text);
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Incorrect username or password";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }
    }
}