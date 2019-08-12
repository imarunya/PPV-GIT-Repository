using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool.UploadData
{
    public partial class frmAnalysisofTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CurrentUser"] == null)
                {
                    Response.Redirect("../Login.aspx");
                }

                if (!Page.IsPostBack)
                {
                    BindUpRelease();
                }
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }

        private void BindUpRelease()
        {
            string sql = " Select `UP-release` ,B.`Maskin`,Ben , `UP-lpnr` From `up-release` as A Inner JOin `ip-adress` B  On A.`IP-lpnr` = B.`IP-lpnr` Where A.`UP-lpnr` = 12 ";
            DatabaseConnection dc = new DatabaseConnection();
            grdUpRelease.DataSource = dc.GetByDataTable(sql);
            grdUpRelease.DataBind();
        }

        private void BindEnviornment()
        {

            string sql = " SELECT `Prefix`,`IMS-system`,`DB2-lpnr`  FROM `DB2-environment` WHere `Maskin` = '" + hfMskn.Value + "' ";

            DatabaseConnection dc = new DatabaseConnection();
            grdEnvoirnment.DataSource = dc.GetByDataTable(sql);
            grdEnvoirnment.DataBind();

        }


        protected override void Render(HtmlTextWriter writer)
        {

            foreach (GridViewRow gvr in grdUpRelease.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdUpRelease, String.Concat("Select$", gvr.RowIndex), true);
            }

            foreach (GridViewRow gvr in grdEnvoirnment.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdEnvoirnment, String.Concat("Select$", gvr.RowIndex), true);
            }

            base.Render(writer);
        }

        protected void grdEnvoirnment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdEnvoirnment.SelectedRow != null)
            {
                txtEnvoirnment.Text = Server.HtmlDecode(grdEnvoirnment.SelectedRow.Cells[0].Text);
                hfDBlpnr.Value = Server.HtmlDecode(grdEnvoirnment.SelectedRow.Cells[2].Text);
            }
            else
            {
                txtEnvoirnment.Text = "";
                hfDBlpnr.Value = "";
            }
        }

        protected void grdUpRelease_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdUpRelease.SelectedRow != null)
            {
                hfMskn.Value = Server.HtmlDecode(grdUpRelease.SelectedRow.Cells[1].Text);
                txtUpRelease.Text = Server.HtmlDecode(grdUpRelease.SelectedRow.Cells[0].Text);
                hfUplpnr.Value = Server.HtmlDecode(grdUpRelease.SelectedRow.Cells[3].Text);

                BindEnviornment();
            }
            else
            {
                txtUpRelease.Text = "";
                hfUplpnr.Value = "";
                hfMskn.Value = "";
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

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUpRelease.Text == "")
                {
                    throw new Exception("Please select UP-release.");
                }

                if (txtEnvoirnment.Text == "")
                {
                    throw new Exception("Please choose environment.");
                }

                DatabaseConnection dc = new DatabaseConnection();

                if (dc.CheckAnalysisOfTestData(Convert.ToInt32(hfUplpnr.Value), Convert.ToInt32(hfDBlpnr.Value)).Rows.Count > 0)
                {
                    Session.Add("Uplpnr", hfUplpnr.Value);
                    Session.Add("DBlpnr", hfDBlpnr.Value);
                    Session.Add("mskn", hfMskn.Value);

                    Response.Redirect("frmUtdata02.aspx");
                }
                else
                {
                    throw new Exception("There are no test cases to be analyzed for this environment");
                }
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }
    }
}