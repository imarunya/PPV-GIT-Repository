using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool.UploadData
{
    public partial class frmReadOutput : System.Web.UI.Page
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //Put PC files and data sets
                string pc_dir = "~/DownloadFiles/";

                string pc_fil = Server.MapPath(pc_dir + "ppvget.txt");
                string pc_fil2 = Server.MapPath(pc_dir + "ppvbic.txt");

                //Read the PC file containing BURC
                var lines = File.ReadAllLines(pc_fil);

                List<UpdataBURC> lstupdBURC = new List<UpdataBURC>(); ;
                List<UpdataBURCDetails> lstupdBURCDtls = new List<UpdataBURCDetails>();
                DatabaseConnection dc = new DatabaseConnection();
                CompareResult CR = new CompareResult();


                string wfirst = "J";

                foreach (var line in lines)
                {
                    string[] fields = line.Split(',');

                    string a = fields[0].Replace('"', ' ').Trim();

                    if (fields[0].Replace('"', ' ').Trim() == "01")
                    {
                        if (wfirst == "J")
                        {
                            wfirst = "N";
                            CR = dc.Upd_Inline_Test(fields[1].Replace('"', ' ').Trim());
                        }

                        UpdataBURC updBURC = new UpdataBURC();

                        updBURC.wtest = Convert.ToInt32(fields[1].Replace('"', ' ').Trim());
                        updBURC.wlpnr = Convert.ToInt32(fields[2].Replace('"', ' ').Trim());
                        updBURC.wind = fields[3].Replace('"', ' ').Trim();
                        updBURC.werr = fields[4].Replace('"', ' ').Trim();

                        string wbase = fields[5].Replace('"', ' ').Trim() + "." + fields[6].Replace('"', ' ').Trim();
                        string wdtail = fields[7].Replace('"', ' ').Trim() + "." + fields[8].Replace('"', ' ').Trim();

                        updBURC.wbase = Convert.ToDecimal (wbase);
                        updBURC.wdtail = Convert.ToDecimal(wdtail);
                        updBURC.worig = fields[9].Replace('"', ' ').Trim();
                        updBURC.wterm = fields[10].Replace('"', ' ').Trim();
                        updBURC.wcat = fields[11].Replace('"', ' ').Trim();
                        updBURC.wasoc = fields[12].Replace('"', ' ').Trim();
                        updBURC.wdat1 = fields[13].Replace('"', ' ').Trim();
                        updBURC.wtid1 = fields[14].Replace('"', ' ').Trim();
                        updBURC.wdat2 = fields[15].Replace('"', ' ').Trim();
                        updBURC.wtid2 = fields[16].Replace('"', ' ').Trim();
                        updBURC.wcount = Convert.ToInt32(fields[17].Replace('"', ' ').Trim());

                        lstupdBURC.Add(updBURC);
                    }
                    else if (fields[0].Replace('"', ' ').Trim() == "02")
                    {

                        UpdataBURCDetails updBURCDtls = new UpdataBURCDetails();

                        updBURCDtls.wtest = Convert.ToInt32(fields[1].Replace('"', ' ').Trim());
                        updBURCDtls.wlpnr = Convert.ToInt32(fields[2].Replace('"', ' ').Trim());
                        updBURCDtls.wnr = Convert.ToInt32(fields[3].Replace('"', ' ').Trim());
                        updBURCDtls.wpp = fields[4].Replace('"', ' ').Trim();
                        updBURCDtls.wsect = fields[5].Replace('"', ' ').Trim();

                        string wamt = fields[6].Replace('"', ' ').Trim() + "." + fields[7].Replace('"', ' ').Trim();
                        string wpcnt = fields[8].Replace('"', ' ').Trim() + "." + fields[9].Replace('"', ' ').Trim();

                        updBURCDtls.wamt = Convert.ToDecimal(wamt);
                        updBURCDtls.wpcnt = Convert.ToDecimal(wpcnt);
                        updBURCDtls.wptyp = fields[10].Replace('"', ' ').Trim();
                        updBURCDtls.wca = fields[11].Replace('"', ' ').Trim();
                        updBURCDtls.wai = fields[12].Replace('"', ' ').Trim();
                        updBURCDtls.wpi = fields[13].Replace('"', ' ').Trim();

                            lstupdBURCDtls.Add(updBURCDtls);
                    }
                }

                bool result = dc.UploadResultData(lstupdBURC, lstupdBURCDtls);

                if (result)
                {
                    int wfel = 0;
                    string wflagg;
                    double wbel1;
                    double wbel2;

                    int wfel1 = 0;
                    string wflagg1;
                    double wbel11;
                    double wbel22;

                    DataTable dt = dc.FetchInDataCDR(Convert.ToInt32(CR.p_CDRnr));

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataTable dtBURC = dc.FetchUDataBURD(CR.p_styr_lpnr, Convert.ToInt32(dt.Rows[i]["Lpnr"].ToString()));
                            wflagg = "OK";

                            if (dtBURC.Rows.Count > 0)
                            {
                                wbel1 = Convert.ToDouble(dt.Rows[i]["Burc-belopp"].ToString());
                                wbel2 = Convert.ToDouble(dtBURC.Rows[0]["Dtail-net-amt"].ToString());

                                if (wbel1 != wbel2)
                                {
                                    wflagg = "FEL";
                                    wfel = wfel + 1;
                                }

                                if (dt.Rows[i]["Burc-ASOC"].ToString() != "?")
                                {
                                    if (dt.Rows[i]["Burc-ASOC"].ToString() != dtBURC.Rows[0]["Usage-cat-code"].ToString())
                                    {
                                        wflagg = "FEL";
                                        wfel = wfel + 1;
                                    }
                                }

                                if (dt.Rows[i]["Burc-ind"].ToString() != "?")
                                {
                                    if (dt.Rows[i]["Burc-ind"].ToString() != dtBURC.Rows[0]["Ind"].ToString())
                                    {
                                        wflagg = "FEL";
                                        wfel = wfel + 1;
                                    }
                                }
                            }
                            else
                            {
                                wflagg = "ERR";
                                wfel = wfel + 1;
                            }

                            //Update Table 
                            bool res = dc.UpdateIndataCDR(wflagg, Convert.ToInt32(CR.p_CDRnr), Convert.ToInt32(dt.Rows[i]["Lpnr"].ToString()));

                        }
                    }
                    else
                    {
                        throw new Exception("Current test missing in Indata CDR ???");
                    }

                    DataTable dtCDRDtls = dc.FetchInDataCDRDtls(Convert.ToInt32(CR.p_CDRnr));
                    if (dtCDRDtls.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtCDRDtls.Rows.Count; j++)
                        {
                            DataTable dtBURCDtls = dc.FetchUDataBURDDtls(CR.p_styr_lpnr, Convert.ToInt32(dtCDRDtls.Rows[j]["Lpnr"].ToString()), Convert.ToInt32(dtCDRDtls.Rows[j]["BURCnr"].ToString()));
                            wflagg1 = "Ok";
                            if (dtBURCDtls.Rows.Count > 0)
                            {
                                wbel11 = Convert.ToDouble(dtCDRDtls.Rows[j]["Sectbelopp-ack"].ToString());
                                wbel22 = Convert.ToDouble(dtBURCDtls.Rows[0]["Sect-net-amt"].ToString());

                                if (wbel11 != wbel22)
                                {
                                    wflagg1 = "FEL";
                                    wfel1 = wfel1 + 1;
                                }

                                wbel11 = Convert.ToDouble(dtCDRDtls.Rows[j]["Sectprocent"].ToString());
                                wbel22 = Convert.ToDouble(dtBURCDtls.Rows[0]["Sect-dscnt-pcnt"].ToString());
                                if (wbel11 != 0)
                                {
                                    if (wbel11 != wbel22)
                                    {
                                        wflagg1 = "FEL";
                                        wfel1 = wfel1 + 1;
                                    }
                                }
                            }
                            else
                            {
                                wflagg1 = "ERR";
                                wfel1 = wfel1 + 1;
                            }
                            //Update Error
                            bool b = dc.UpdateInDataCDRDtls(wflagg1, Convert.ToInt32(CR.p_CDRnr), Convert.ToInt32(dtCDRDtls.Rows[j]["Lpnr"].ToString()), Convert.ToInt32(dtCDRDtls.Rows[j]["BURCnr"].ToString()));
                        }
                    }

                    int toterr = wfel + wfel1;

                    //Update control table with number of errors
                    bool b1 = dc.Update_Styrtabell_Error(CR.p_styr_lpnr, toterr);

                    if (b1 && toterr > 0)
                    {
                        Show("Number of errors: " + toterr.ToString(), this);
                        ShowMessage("Number of errors: " + toterr.ToString());
                    }
                    else
                    {
                        Show("No errors found !!!", this);
                        ShowMessage("No errors found !!!");
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