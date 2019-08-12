using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool.UploadData
{
    public partial class frmUtdata02 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["CurrentUser"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            int p_up_lpnr;
            int p_db2_lpnr;

            if (!Page.IsPostBack)
            {
                lblHeader.Text = "These tests are directed at the MVS machine: " + (string)Session["mskn"];

                p_up_lpnr = Convert.ToInt32(Session["Uplpnr"]);
                p_db2_lpnr = Convert.ToInt32(Session["DBlpnr"]);

                DatabaseConnection dc = new DatabaseConnection();

                grdTestCases.DataSource = dc.GetAnalysisOfTestData(p_up_lpnr, p_db2_lpnr);
                grdTestCases.DataBind();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string wstat1 = String.Empty; string wstat2 = String.Empty; string wmsg = String.Empty;

            bool option = false;

            try
            {
                int p_styr_lpnr = 0;

                foreach (GridViewRow gr in grdTestCases.Rows)
                {
                    RadioButton rboSelect = (RadioButton)gr.FindControl("RboGrid");

                    if (option)
                    {
                        break;
                    }

                    if (!option)
                    {
                        if (rboSelect.Checked)
                        {
                            p_styr_lpnr = Convert.ToInt32(grdTestCases.Rows[gr.RowIndex].Cells[1].Text);
                            option = true;
                        }
                    }
                }

                if (!option)
                {
                    throw new Exception("Please select a test !!!");
                }

                if (!rbAlternativknapp14.Checked && !rbAlternativknapp16.Checked && !rbAlternativknapp18.Checked)
                {
                    throw new Exception("Choose analysis results please !!!");
                }

                wstat1 = " ";
                if (rbAlternativknapp14.Checked)
                {
                    wstat1 = "0";
                    wstat2 = "0";
                    wmsg = "OK test open for changes!";
                }
                else if (rbAlternativknapp16.Checked)
                {
                    wstat1 = "9";
                    wstat2 = "8";
                    wmsg = "OK test closed with note !";
                }
                else if (rbAlternativknapp18.Checked)
                {
                    wstat1 = "9";
                    wstat2 = "9";
                    wmsg = "OK test closed without remark !";
                }

                //Check if current test is not double-marked
                DataTable dt = UtilityClass.CheckCurrentTestStatus(p_styr_lpnr);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Status1"].ToString() != "2")
                    {
                        throw new Exception("You have already analyzed the current test !!!");
                    }

                    //Update control table status values
                    DatabaseConnection dc = new DatabaseConnection();
                    if (dc.Update_Control_Table_Status(p_styr_lpnr, wstat1, wstat2))
                    {
                        throw new Exception(wmsg);
                    }
                }
                else
                {
                    throw new Exception("The test marked is not available ???? Contact Telia ProSoft");
                }


            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                Show(ex.Message, this);
            }
        }

        private void ShowMessage(string message)
        {
            lblMsg.Text = message;
            lblMsg.Visible = true;
        }

        public static void Show(string message, Control owner)
        {
            Page page = (owner as Page) ?? owner.Page;
            if (page == null) return;
            page.ClientScript.RegisterStartupScript(owner.GetType(), "ShowMessage", string.Format("<script type='text/javascript'>alert('{0}')</script>", message));
        }
    }


}