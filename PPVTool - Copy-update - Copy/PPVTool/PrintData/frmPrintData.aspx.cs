using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;

namespace PPVTool.PrintData
{
    public partial class frmPrintData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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

            foreach (GridViewRow gvr in grdChooseTestCases.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdChooseTestCases, String.Concat("Select$", gvr.RowIndex), true);
            }


            base.Render(writer);
        }

        protected void grdEnvoirnment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdEnvoirnment.SelectedRow != null)
            {
                txtEnvoirnment.Text = Server.HtmlDecode(grdEnvoirnment.SelectedRow.Cells[0].Text);
                hfDBlpnr.Value = Server.HtmlDecode(grdEnvoirnment.SelectedRow.Cells[2].Text);

                BindTestCase(Convert.ToInt32(hfUplpnr.Value), Convert.ToInt32(hfDBlpnr.Value));
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



        protected void grdChooseTestCases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdEnvoirnment.SelectedRow != null)
            {
                txtChoosetestcases.Text = Server.HtmlDecode(grdChooseTestCases.SelectedRow.Cells[0].Text);
                //hfDBlpnr.Value = Server.HtmlDecode(grdEnvoirnment.SelectedRow.Cells[2].Text);

                // BindTestCase(Convert.ToInt32(hfUplpnr.Value), Convert.ToInt32(hfDBlpnr.Value));
            }
            else
            {
                txtChoosetestcases.Text = "";
                // hfDBlpnr.Value = "";
            }
        }

        private void ShowMessage(string message)
        {
            lblMsg.Text = message;
            lblMsg.Visible = true;
        }


        private void BindTestCase(int p_up_lpnr, int p_db2_lpnr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT `Styr-lpnr`,`Status1`,`Status2`,`Datum`,`LAN-userid`,`Ben`,`Word-dok` ");
            sb.Append(" FROM `Styrtabell` WHERE `UP-lpnr` = " + p_up_lpnr + " AND `DB2-lpnr` = " + p_db2_lpnr + "  ORDER BY Datum DESC  ");

            DatabaseConnection dc = new DatabaseConnection();

            grdChooseTestCases.DataSource = dc.GetByDataTable(sb.ToString());
            grdChooseTestCases.DataBind();
            Session.Add("p_up_lpnr", p_up_lpnr);

            Session["p_db2_lpnr"] = p_db2_lpnr;

        }

        public static void Show(string message, Control owner)
        {
            Page page = (owner as Page) ?? owner.Page;
            if (page == null) return;
            page.ClientScript.RegisterStartupScript(owner.GetType(), "ShowMessage", string.Format("<script type='text/javascript'>alert('{0}')</script>", message));
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string miljo = txtEnvoirnment.Text;
            string wstyr = txtChoosetestcases.Text;
            String uprls = txtUpRelease.Text;
            string ws, wcdrnr, wCdnr;
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT `CDRnr` ");
            sb.Append(" FROM `Kopplad-CDR` WHERE `Styr-lpnr` = " + wstyr + " ");
            DatabaseConnection dc = new DatabaseConnection();
            wcdrnr = dc.ExceuteSclare(sb.ToString());



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


                {
                    if (ChkBoxReportTestData.Checked == true)
                    {

                        if (wcdrnr == "")
                        {
                            throw new Exception("Current test lacks connected CDRs ???  Technical error ???.");
                        }
                        else
                        {
                            Session["miljo"] = miljo;
                            Session["hfMskn"] = hfMskn.Value;
                            Session["uprls"] = uprls;
                            Session["Uplpnr"] = hfUplpnr.Value;
                            Session["wcdrnr"] = wcdrnr;
                            Session["wstyr"] = wstyr;

                            string pageurl = "testReport.aspx";
                            Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
                        }
                    }


                    if (CheckBox2.Checked == true)
                    {

                        foreach (GridViewRow gr in grdChooseTestCases.Rows)
                        {
                            hfp_styr_lpnr.Value = grdChooseTestCases.Rows[gr.RowIndex].Cells[1].Text;
                        }

                        if (wcdrnr == "")
                        {
                            throw new Exception("Current test lacks connected CDRs ???  Technical error ???.");
                        }
                        else
                        {
                            Session["miljo"] = miljo;
                            Session["hfMskn"] = hfMskn.Value;
                            Session["uprls"] = uprls;
                            string wtst = hfp_styr_lpnr.Value;
                            StringBuilder sa = new StringBuilder();
                            sa.Append("SELECT *");
                            sa.Append(" FROM `Utdata-BURC` WHERE 'Styr-lpnr' = " + wstyr + " ");
                            Session["Uplpnr"] = hfUplpnr.Value;
                            Session["wcdrnr"] = wcdrnr;
                            Session["wstyr"] = wstyr;

                            // Response.Redirect("testAnalysys.aspx");
                            string pageurl = "testAnalysys.aspx";
                            Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
                        }

                    }
                    if (CheckBox3.Checked == true)
                    {

                        foreach (GridViewRow gr in grdChooseTestCases.Rows)
                        {
                            hfp_styr_lpnr.Value = grdChooseTestCases.Rows[gr.RowIndex].Cells[1].Text;
                        }

                        if (wcdrnr == "")
                        {
                            throw new Exception("Current test lacks connected CDRs ???  Technical error ???.");
                        }
                        else
                        {
                            Session["miljo"] = miljo;
                            Session["hfMskn"] = hfMskn.Value;
                            Session["uprls"] = uprls;
                            string wtst = hfp_styr_lpnr.Value;
                            StringBuilder sa = new StringBuilder();
                            sa.Append("SELECT *");
                            sa.Append(" FROM `Utdata-BURC` WHERE 'Styr-lpnr' = " + wstyr + " ");
                            Session["Uplpnr"] = hfUplpnr.Value;
                            Session["wcdrnr"] = wcdrnr;
                            Session["wstyr"] = wstyr;

                            // Response.Redirect("testBURC.aspx");
                            string pageurl = "test_BURC.aspx";
                            Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
                        }

                    }
                    if ((CheckBox2.Checked == false) && (ChkBoxReportTestData.Checked == false) && (CheckBox3.Checked==false))
                    {
                        throw new Exception("Please select any Report");
                    }
                    


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
