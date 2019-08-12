using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool.DataCreation
{
    public partial class frmBatch2 : System.Web.UI.Page
    {

        int p_ant_ut = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int p_up_lpnr;
                int p_db2_lpnr;

                if (!Page.IsPostBack)
                {
                    lblHeader.Text = "These tests are directed at the MVS machine: " + Request.QueryString.Get("mskn");

                    p_up_lpnr = Convert.ToInt32(Request.QueryString.Get("uplpnr"));
                    p_db2_lpnr = Convert.ToInt32(Request.QueryString.Get("dblpnr"));

                    grdTestCases.DataSource = UtilityClass.GetTestCases(p_up_lpnr, p_db2_lpnr);
                    grdTestCases.DataBind();
                }
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
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
                bool validate = false;

                foreach (GridViewRow gr in grdTestCases.Rows)
                {
                    RadioButton rboSelect = (RadioButton)gr.FindControl("RboGrid");

                    if (rboSelect.Checked)
                    {
                        validate = true;

                        hfwprefix.Value = grdTestCases.Rows[gr.RowIndex].Cells[2].Text;
                        hfp_styr_lpnr.Value = grdTestCases.Rows[gr.RowIndex].Cells[1].Text;
                        hfp_db2_lpnr.Value = grdTestCases.Rows[gr.RowIndex].Cells[6].Text;
                        hfp_up_lpnr.Value = grdTestCases.Rows[gr.RowIndex].Cells[7].Text;

                        //'Check if the current machine is closed

                        DataTable dtMachineStatus = UtilityClass.CheckMachineStatus(Convert.ToInt32(hfp_up_lpnr.Value));

                        if (dtMachineStatus.Rows.Count > 0)
                        {
                            if (dtMachineStatus.Rows[0]["Status"].ToString().Trim() == "9")
                            {
                                throw new Exception("The current machine is closed until further notice.Contact Telia ProSoft");
                            }
                            else
                            {
                                hfwip.Value = dtMachineStatus.Rows[0]["IP-adress2"].ToString().Trim();
                            }
                        }

                        // Check if it is running in the current DB2 environment or if the DB2 environment is closed
                        // If all, ok, save DB2 environment into variables

                        DataTable dtdb2Env = UtilityClass.CheckDB2EnviornmenrVariables(Convert.ToInt32(hfp_db2_lpnr.Value));

                        if (dtdb2Env.Rows.Count > 0)
                        {
                            if (dtMachineStatus.Rows[0]["Status"].ToString().Trim() == "1")
                            {
                                throw new Exception("Driving is in progress in the current environment");
                            }
                            else if (dtMachineStatus.Rows[0]["Status"].ToString().Trim() == "9")
                            {
                                throw new Exception("The environment is closed until further notice.Contact Telia ProSoft");
                            }
                            else
                            {

                                DB2EnviornmentVariable db2 = new DB2EnviornmentVariable();

                                db2.wmaskin = dtdb2Env.Rows[0]["Maskin"].ToString();
                                db2.wIMS = dtdb2Env.Rows[0]["IMS-system"].ToString();
                                db2.wDSN = dtdb2Env.Rows[0]["DSN-system"].ToString();
                                db2.wPSB = dtdb2Env.Rows[0]["PSB"].ToString();
                                db2.walias = dtdb2Env.Rows[0]["Alias"].ToString();
                                db2.wcreator = dtdb2Env.Rows[0]["Creator"].ToString();
                                db2.wvol = dtdb2Env.Rows[0]["Volcreator"].ToString();

                                Session.Add("DB2EnviornmentVariable", db2);
                            }
                        }

                        // Check for BIC data is associated with this test

                        DataTable dtbicdata = UtilityClass.CheckBICData(Convert.ToInt32(hfp_styr_lpnr.Value));

                        BICData bicData;

                        if (dtbicdata.Rows.Count > 0)
                        {
                            bicData = new BICData();

                            bicData.p_BICnr = dtbicdata.Rows[0]["BICnr"].ToString();
                            bicData.p_BIC_lager_nr = dtbicdata.Rows[0]["BIC-lager-nr"].ToString();
                            bicData.wtyp = dtbicdata.Rows[0]["Typ"].ToString();

                            Session.Add("bicData", bicData);
                        }
                        else
                        {
                            throw new Exception("You have not created BIC data for this test");
                        }

                        //Check for CDR data is associated with this test

                        int p_CDRnr = UtilityClass.GetCDRNo(Convert.ToInt32(hfp_styr_lpnr.Value));

                        if (p_CDRnr == 0)
                        {
                            throw new Exception("You have not created CDR data for this test.");
                        }

                        //Control of LOC data is associated with this test

                        int p_LOKnr = UtilityClass.GetLOCNo(Convert.ToInt32(hfp_styr_lpnr.Value));

                        if (p_LOKnr == 0)
                        {
                            p_LOKnr = 0;
                        }

                        // Download Country Type
                        int wagr = UtilityClass.GetCountryType(Convert.ToInt32(bicData.p_BIC_lager_nr));

                        if (wagr == 0)
                        {
                            throw new Exception("The customer is missing in the BIC layer.");
                        }

                        //Get email address and MVS user                        
                        DataTable dtusd = UtilityClass.GetEmailandUser(Convert.ToString(Session["CurrentUser"]));

                        UserDetails usd;

                        if (dtusd.Rows.Count > 0)
                        {
                            usd = new UserDetails();

                            usd.wemail = dtusd.Rows[0]["Email"].ToString();
                            usd.p_mvs_user = dtusd.Rows[0]["IBM-userid"].ToString();
                        }
                        else
                        {
                            throw new Exception("There is no information about your LAN user");
                        }

                        //Download drive type
                        string winit = UtilityClass.GetDriveType(Convert.ToInt32(hfp_styr_lpnr.Value));
                        if (winit == "")
                        {
                            throw new Exception("There is no information about the test.");
                        }




                        string path = "~/DownloadFiles/PPVData.txt";
                        string jclPath = "~/DownloadFiles/JCL.txt";
                        string p_ppvdb = UtilityClass.GetDatabaseVersion();

                        StringBuilder fileData = new StringBuilder();
                        string wtxt = string.Empty;
                        wtxt = "\"00 STRT " + DateTime.Now.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + " Email=" + usd.wemail + CreateWhiteSpace(80, usd.wemail.Length) + "\"";
                        //fileData.Append(wtxt + Environment.NewLine);
                        AppentDataToFile(wtxt, ref fileData);

                        //Write INFO mail
                        string wtst = hfp_styr_lpnr.Value.PadLeft(5, '0');
                        string wdb2 = hfp_db2_lpnr.Value.PadLeft(5, '0');
                        wtxt = "\"01 INFO TESTNR=" + wtst + " DB2NR=" + wdb2 + " LAN-USER=" + Convert.ToString(Session["CurrentUser"]) + "\"";
                        //fileData.Append(wtxt + Environment.NewLine);
                        AppentDataToFile(wtxt, ref fileData);

                        //Type ENVIRONMENTAL POST
                        DB2EnviornmentVariable dbEnviornment = (DB2EnviornmentVariable)Session["DB2EnviornmentVariable"];
                        //var wvol = dbEnviornment.wvol.PadRight(8 - dbEnviornment.wvol.Length, ' ').Replace(" ", " ");
                        var wvol = dbEnviornment.wvol + CreateWhiteSpace(8, dbEnviornment.wvol.Length);

                        //var wDNS = dbEnviornment.wDSN.PadRight(4 - dbEnviornment.wDSN.Length, ' ').Replace(" ", " ");
                        var wDNS = dbEnviornment.wDSN + CreateWhiteSpace(4, dbEnviornment.wDSN.Length);

                        wtxt = "\"02 MILJ IMS=" + dbEnviornment.wIMS + " PSB=" + dbEnviornment.wPSB + " ALIAS=" + dbEnviornment.walias;
                        wtxt = wtxt + " CREATOR=" + dbEnviornment.wcreator + " DSN=" + wDNS + " VOL=" + wvol;
                        wtxt = wtxt + " INIT=" + winit + " PCDB=" + p_ppvdb + "\"";

                        //fileData.Append(wtxt + Environment.NewLine);
                        AppentDataToFile(wtxt, ref fileData);

                        Write_BIC(bicData.wtyp, wagr, winit, ref fileData, bicData);

                        Write_CDR(p_CDRnr, ref fileData);

                        if (p_LOKnr > 0)
                        {
                            Write_LOKNO(p_LOKnr, ref fileData);
                        }

                        string want = p_ant_ut.ToString();
                        if (want.Length < 5)
                        {
                            want = want.PadLeft(5, '0');
                        }

                        wtxt = "\"99 SLUT Antal poster: " + want + "\"";

                        AppentDataToFile(wtxt, ref fileData);

                        DatabaseConnection dc = new DatabaseConnection();

                        if (dc.UpdateTables(Convert.ToInt32(hfp_db2_lpnr.Value), Convert.ToInt32(hfp_styr_lpnr.Value)))
                        {

                            string path1 = Server.MapPath(path);
                            File.WriteAllText(Server.MapPath(path), fileData.ToString());
                            File.WriteAllText(Server.MapPath(jclPath), CreateJCLFile(hfwprefix.Value, dbEnviornment.wDSN).ToString());

                            hlJCL.Visible = true;
                            hlPPV.Visible = true;

                            Show("OK files created!Please run the manual routine to Mainframe!", this);
                            ShowMessage("OK files created!Please run the manual routine to Mainframe!");
                        }

                    }
                }

                if (!validate)
                {
                    throw new Exception("Please select a test !!!");
                }
            }
            catch (Exception ex)
            {
                hlJCL.Visible = false;
                hlPPV.Visible = false;

                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }

        private StringBuilder AppentDataToFile(string wtxt, ref StringBuilder fileData)
        {
            if (wtxt != "")
            {
                fileData.Append(wtxt + Environment.NewLine);
                p_ant_ut = p_ant_ut + 1;
            }

            return fileData;
        }

        #region "BIC Data"

        public StringBuilder Write_BIC(string wtyp, int wagr, string winit, ref StringBuilder fileData, BICData bicData)
        {
            string wtxt = "\"03 BIC-00 TYP=" + wtyp + " AGREETYP=" + wagr + "\"";

            fileData.Append(wtxt + Environment.NewLine);

            DataTable dt = UtilityClass.GetBICData(Convert.ToInt32(bicData.p_BIC_lager_nr));

            if (dt.Rows.Count > 0)
            {
                string wid = dt.Rows[0]["Bill-cycle-len"].ToString().PadLeft(3, '0');

                wtxt = "\"03 BIC-01 BILLCODE=" + dt.Rows[0]["Bill-cycle-code"].ToString();
                wtxt = wtxt + " BILLDUR=" + dt.Rows[0]["Bill-cycle-dur"].ToString();
                wtxt = wtxt + " BILLLEN=" + wid;
                wtxt = wtxt + " BILLIND=" + dt.Rows[0]["Bill-ind"].ToString();
                wtxt = wtxt + " BUS-RES=" + dt.Rows[0]["Bus-res-type"].ToString();
                wtxt = wtxt + " OPERIND=" + dt.Rows[0]["Oper-ind"].ToString();
                wtxt = wtxt + " TIMSSPLT=" + dt.Rows[0]["Tims-split"].ToString() + "\"";

                fileData.Append(wtxt + Environment.NewLine);

                //Adress
                wtxt = "\"03 BIC-02 ORGNR=" + dt.Rows[0]["Prson-org-nm"].ToString();
                wtxt = wtxt + " ADR2=Test";
                //wtxt = wtxt + " ADR2=" + dt.Rows[0]["Line-2-addr"].ToString();
                wtxt = wtxt + " ADR3=" + dt.Rows[0]["Line-3-addr"].ToString() + "\"";

                //fileData.Append(wtxt + Environment.NewLine);
                AppentDataToFile(wtxt, ref fileData);
            }
            else
            {
                throw new Exception("Customer is missing in BIClager. Contact Telia ProSoft.");
            }

            //Print BIC-AG: Agree Link (cust/agreeid/prisplan/plan-type)

            DataTable dtBicAgree = UtilityClass.GetBICAgreeData(Convert.ToInt32(bicData.p_BICnr));

            if (dtBicAgree.Rows.Count > 0)
            {
                for (int i = 0; i < dtBicAgree.Rows.Count; i++)
                {
                    string wid1 = dtBicAgree.Rows[i]["Cust-id"].ToString().PadLeft(9, '0');
                    string wid2 = dtBicAgree.Rows[i]["Agree-id"].ToString().PadLeft(9, '0');

                    //Write Agree mail

                    wtxt = "\"03 BIC-AG CUST=" + wid1 + " AGRE=" + wid2;

                    //string pPlan = dtBicAgree.Rows[i]["Prisplan"].ToString().PadRight(8 - dtBicAgree.Rows[i]["Prisplan"].ToString().Length, ' ');

                    string pPlan = dtBicAgree.Rows[i]["Prisplan"].ToString() + CreateWhiteSpace(8, dtBicAgree.Rows[i]["Prisplan"].ToString().Length);

                    wtxt = wtxt + " PLAN=" + pPlan + " PTYP=" + dtBicAgree.Rows[i]["Plan-type-ind"].ToString();

                    string durallwsec = "";

                    if (!string.IsNullOrEmpty(dtBicAgree.Rows[i]["Dur-allw-sec"].ToString()))
                    {
                        durallwsec = dtBicAgree.Rows[i]["Dur-allw-sec"].ToString();
                    }

                    durallwsec = durallwsec.PadLeft(11, '0');

                    wtxt = wtxt + " DUR=" + durallwsec;

                    string planUnitid = dtBicAgree.Rows[i]["Plan-unit-id"].ToString().PadLeft(3, '0');

                    wtxt = wtxt + " UNITID=" + planUnitid + "\"";

                    //fileData.Append(wtxt + Environment.NewLine);
                    AppentDataToFile(wtxt, ref fileData);
                }
            }
            else
            {
                throw new Exception("Agree poster missing in INDATA - BIC - AGREE.Contact Telia ProSoft");
            }

            //Print BIC-KS: Customer Structure
            DataTable dtBICKS = UtilityClass.GetBICKSCustomerStructure(Convert.ToInt32(bicData.p_BIC_lager_nr));

            if (dtBICKS.Rows.Count > 0)
            {
                for (int i = 0; i < dtBICKS.Rows.Count; i++)
                {
                    string custid = dtBICKS.Rows[i]["Cust-id"].ToString().PadLeft(9, '0');
                    string billEntId = dtBICKS.Rows[i]["Bill-ent-id"].ToString().PadLeft(9, '0');
                    string chargeNoId = dtBICKS.Rows[i]["Chrgb-num-id"].ToString().PadLeft(9, '0');

                    wtxt = "\"03 BIC-KS CUST=" + custid + " BILL=" + billEntId + " CHRG=" + chargeNoId;

                    string Extchrgbnumid = UtilityClass.GetBICKSChargeNoId(Convert.ToInt32(chargeNoId));

                    wtxt = wtxt + " EXT=" + Extchrgbnumid + "\"";

                    //fileData.Append(wtxt + Environment.NewLine);
                    AppentDataToFile(wtxt, ref fileData);
                }
            }
            else
            {
                throw new Exception("Customer structure is missing in BIClager LAGER - BIC - CUST.Contact Telia ProSoft.");
            }

            //Print BIC-05: data entry for BIC05
            DataTable dtBIC05 = UtilityClass.GetBIC05Data(Convert.ToInt32(bicData.p_BICnr));

            if (dtBIC05.Rows.Count > 0)
            {
                for (int i = 0; i < dtBIC05.Rows.Count; i++)
                {
                    string wCustid = dtBIC05.Rows[i]["Cust-id"].ToString().PadLeft(9, '0');
                    string wDat1 = dtBIC05.Rows[i]["Start-datum"].ToString();
                    string wdat2 = dtBIC05.Rows[i]["Stopp-datum"].ToString();

                    wtxt = "\"03 BIC-05 DATUM=" + wDat1 + wdat2 + " CUST=" + wCustid + "\"";
                    //fileData.Append(wtxt + Environment.NewLine);
                    AppentDataToFile(wtxt, ref fileData);
                }
            }
            else
            {
                throw new Exception("Date records missing for BIC - 05.Contact Telia ProSoft.");
            }

            //Print BIC-10: data entry for BIC10
            DataTable dtBIC10 = UtilityClass.GetBIC10Data(Convert.ToInt32(bicData.p_BICnr));

            if (dtBIC10.Rows.Count > 0)
            {
                for (int i = 0; i < dtBIC10.Rows.Count; i++)
                {
                    string wBillentid = dtBIC10.Rows[i]["Bill-ent-id"].ToString().PadLeft(9, '0');
                    string wdat0 = dtBIC10.Rows[i]["startdt"].ToString();
                    string wdat1 = dtBIC10.Rows[i]["Start-datum"].ToString();
                    string wdat2 = dtBIC10.Rows[i]["Stopp-datum"].ToString();
                    string wdat3 = "";
                    if (winit == "J")
                    {
                        wdat3 = dtBIC10.Rows[i]["stopdtadd"].ToString();
                    }
                    else
                    {
                        wdat3 = dtBIC10.Rows[i]["startdtminus"].ToString();
                    }

                    wtxt = "\"03 BIC-10 DATUM=" + wdat1 + wdat2 + " BILL=" + wBillentid + " END=" + wdat0;
                    wtxt = wtxt + " TIMDAT=" + wdat3 + "\"";

                    //fileData.Append(wtxt + Environment.NewLine);
                    AppentDataToFile(wtxt, ref fileData);
                }
            }
            else
            {
                throw new Exception("Date records missing for BIC - 10.Contact Telia ProSoft.");
            }

            //Print BIC-15: data item for BIC15
            DataTable dtBIC15 = UtilityClass.GetBIC15Data(Convert.ToInt32(bicData.p_BICnr));
            if (dtBIC15.Rows.Count > 0)
            {
                for (int i = 0; i < dtBIC15.Rows.Count; i++)
                {
                    string wChrgbnumid = dtBIC15.Rows[i]["Chrgb-num-id"].ToString().PadLeft(9, '0');
                    string wdat1 = dtBIC15.Rows[i]["Start-datum"].ToString();
                    string wdat2 = dtBIC15.Rows[i]["Stopp-datum"].ToString();

                    wtxt = "\"03 BIC-15 DATUM=" + wdat1 + wdat2 + " CHRG=" + wChrgbnumid + " TYP=" + wtyp + "\"";

                    // fileData.Append(wtxt + Environment.NewLine);
                    AppentDataToFile(wtxt, ref fileData);
                }
            }
            else
            {
                if (wtyp != "E")
                {
                    throw new Exception("Date records missing for BIC - 15.Contact Telia ProSoft.");
                }
            }

            // Print BIC-20 to BIC-45: data entry for BIC20 - 45

            DataTable dtBIC20 = UtilityClass.GetBIC20Data(Convert.ToInt32(bicData.p_BICnr));

            if (dtBIC20.Rows.Count > 0)
            {

                for (int i = 0; i < dtBIC20.Rows.Count; i++)
                {
                    string wtxt2 = string.Empty;
                    int wnr = 0;

                    string wCustid = dtBIC20.Rows[i]["Cust-id"].ToString().PadLeft(9, '0');
                    string wAgreeid = dtBIC20.Rows[i]["Agree-id"].ToString().PadLeft(9, '0');
                    string wdat1 = dtBIC20.Rows[i]["Start-datum"].ToString();
                    string wdat2 = dtBIC20.Rows[i]["Stopp-datum"].ToString();

                    wtxt = "\"03 BIC-" + dtBIC20.Rows[i]["Post"].ToString();
                    wtxt = wtxt + " DATUM=" + wdat1 + wdat2;
                    wtxt = wtxt + " CUST=" + wCustid + " AGRE=" + wAgreeid;

                    //string wprisplan = dtBIC20.Rows[i]["Prisplan"].ToString().PadRight(8 - dtBIC20.Rows[i]["Prisplan"].ToString().Length, ' ');
                    string wprisplan = dtBIC20.Rows[i]["Prisplan"].ToString() + CreateWhiteSpace(8, dtBIC20.Rows[i]["Prisplan"].ToString().Length);

                    wtxt = wtxt + " PLAN=" + wprisplan;
                    string wPlanUnitId = dtBIC20.Rows[i]["Plan-unit-id"].ToString().PadLeft(3, '0');
                    wtxt = wtxt + " UNIT=" + wPlanUnitId;

                    if (Convert.ToInt32(dtBIC20.Rows[i]["Post"].ToString()) < 30)
                    {
                        wtxt = wtxt + "\"";
                        //fileData.Append(wtxt + Environment.NewLine);
                        AppentDataToFile(wtxt, ref fileData);
                    }
                    else if (Convert.ToInt32(dtBIC20.Rows[i]["Post"].ToString()) == 30)
                    {
                        DataTable dtBIC30 = UtilityClass.GetBIC30Data(Convert.ToInt32(dtBIC20.Rows[i]["Underlpnr"].ToString()));
                        if (dtBIC30.Rows.Count > 0)
                        {
                            wnr = 0;
                            for (int j = 0; j < dtBIC30.Rows.Count; j++)
                            {
                                wnr = wnr + 1;
                                string wnrx = wnr.ToString().PadLeft(3, '0');
                                string optntrmval = dtBIC30.Rows[j]["Optn-trm-val"].ToString();

                                //wtxt2 = wtxt + " SEL=" + wnrx + " OPTN=" + optntrmval.PadRight(30 - optntrmval.Length, ' ') + "\"";
                                wtxt2 = wtxt + " SEL=" + wnrx + " OPTN=" + optntrmval + CreateWhiteSpace(30, optntrmval.Length) + "\"";

                                //fileData.Append(wtxt2 + Environment.NewLine);
                                AppentDataToFile(wtxt2, ref fileData);
                            }
                        }
                    }
                    else if (Convert.ToInt32(dtBIC20.Rows[i]["Post"].ToString()) == 35)
                    {
                        DataTable dtBIC35 = UtilityClass.GetBIC35Data(Convert.ToInt32(dtBIC20.Rows[i]["Underlpnr"].ToString()));

                        if (dtBIC35.Rows.Count > 0)
                        {
                            wnr = 0;
                            for (int k = 0; k < dtBIC35.Rows.Count; k++)
                            {
                                wnr = wnr + 1;
                                string wnrx = wnr.ToString().PadLeft(3, '0');
                                string selAreaVal = dtBIC35.Rows[k]["Sel-area-val"].ToString();
                                //wtxt2 = wtxt + " SEL=" + wnrx + " IND=" + dtBIC35.Rows[k]["Area-type-ind"].ToString() + " LAND=" + selAreaVal.PadRight(30 - selAreaVal.Length, ' ') + "\"";
                                wtxt2 = wtxt + " SEL=" + wnrx + " IND=" + dtBIC35.Rows[k]["Area-type-ind"].ToString() + " LAND=" + selAreaVal + CreateWhiteSpace(30, selAreaVal.Length) + "\"";

                                AppentDataToFile(wtxt2, ref fileData);
                            }
                        }
                    }
                    else
                    {
                        DataTable dtBIC45 = UtilityClass.GetBIC45Data(Convert.ToInt32(dtBIC20.Rows[i]["Underlpnr"].ToString()));

                        if (dtBIC45.Rows.Count > 0)
                        {
                            wnr = 0;
                            for (int k = 0; k < dtBIC45.Rows.Count; k++)
                            {
                                wnr = wnr + 1;
                                string wnrx = wnr.ToString().PadLeft(3, '0');
                                string beginPointVal = dtBIC45.Rows[k]["Begin-point-val"].ToString();
                                string endPointVal = dtBIC45.Rows[k]["End-point-val"].ToString();
                                int lenBPV = beginPointVal.Length;

                                //wtxt2 = wtxt + " SEL=" + wnrx + " TEL1=" + beginPointVal.PadRight(21 - beginPointVal.Length, ' ') + " TEL2=" + endPointVal.PadRight(21 - endPointVal.Length, ' ') + " LEN=" + lenBPV.ToString().PadLeft(2, '0') + "\"";

                                wtxt2 = wtxt + " SEL=" + wnrx + " TEL1=" + beginPointVal + CreateWhiteSpace(21, beginPointVal.Length) + " TEL2=" + endPointVal + CreateWhiteSpace(21, endPointVal.Length) + " LEN=" + lenBPV.ToString().PadLeft(2, '0') + "\"";
                                AppentDataToFile(wtxt2, ref fileData);
                            }
                        }
                    }
                }
            }
            else
            {
                if (wtyp != "E")
                {
                    throw new Exception("Date records missing for BIC-20. Contact Telia ProSoft.");
                }
            }

            return fileData;
        }

        #endregion

        #region CDRDATA

        public StringBuilder Write_CDR(int p_CDRNo, ref StringBuilder fileData)
        {
            DataTable dtCDR = UtilityClass.GetCDRData(p_CDRNo);

            if (dtCDR.Rows.Count > 0)
            {
                for (int i = 0; i < dtCDR.Rows.Count; i++)
                {
                    string wtxt = string.Empty;

                    string wCustid1 = "?????????";
                    string wBillid2 = "?????????";
                    string anr = "";

                    //Read Cust ID and Bill ent ID
                    DataTable dtCustBillId = UtilityClass.GetCustIdandBillId(Convert.ToInt32(dtCDR.Rows[i]["Chrgb-num-id"]));
                    if (dtCustBillId.Rows.Count > 0)
                    {
                        wCustid1 = dtCustBillId.Rows[0]["Cust-id"].ToString();
                        wBillid2 = dtCustBillId.Rows[0]["Bill-ent-id"].ToString();
                        anr = dtCustBillId.Rows[0]["Anr"].ToString();

                        wCustid1 = wCustid1.PadLeft(9, '0');
                        wBillid2 = wBillid2.PadLeft(9, '0');
                        //anr = anr.PadRight(20 - anr.Length, ' ');
                        anr = anr + CreateWhiteSpace(20, anr.Length);
                    }

                    //Save
                    string lpnr = dtCDR.Rows[i]["Lpnr"].ToString();
                    lpnr = lpnr.PadLeft(5, '0');
                    string chrgbNumId = dtCDR.Rows[i]["Chrgb-num-id"].ToString();
                    chrgbNumId = chrgbNumId.PadLeft(9, '0');

                    //Print
                    wtxt = "\"04 CDR LPNR=" + lpnr + "REC=" + dtCDR.Rows[i]["Rec-ind"].ToString();
                    wtxt = wtxt + " CUST=" + wCustid1 + " BILL=" + wBillid2 + " CHRG=" + chrgbNumId;
                    wtxt = wtxt + " SRV=" + dtCDR.Rows[i]["Servid"].ToString();
                    wtxt = wtxt + " CTP=" + dtCDR.Rows[i]["Calltype"].ToString();

                    string wasoc = "";
                    if (string.IsNullOrEmpty(dtCDR.Rows[i]["Asoc"].ToString()))
                    {
                        wasoc = "12345678";
                    }
                    else
                    {
                        wasoc = dtCDR.Rows[i]["Asoc"].ToString();
                    }

                    wtxt = wtxt + " ASOC=" + wasoc;
                    wtxt = wtxt + " DAT=" + dtCDR.Rows[i]["Datum"].ToString() + dtCDR.Rows[i]["Tid"].ToString() + dtCDR.Rows[i]["Samtalslngd"].ToString();

                    //Edit amount
                    string wid = dtCDR.Rows[i]["Belopp"].ToString();
                    int ore = 0;

                    string wid1 = "";
                    string wid2 = "";

                    for (int j = 0; j < wid.Length; j++)
                    {
                        if (wid.Substring(j, 1) == ".")
                        {
                            ore = 1;
                        }
                        else
                        {
                            if (ore > 1)
                            {
                                wid2 = wid2 + wid.Substring(j, 1);
                            }
                            else
                            {
                                wid1 = wid1 + wid.Substring(j, 1);
                            }
                        }
                    }


                    wid1 = wid1.PadLeft(6, '0');  //'Kr
                    wid2 = wid2.PadRight(2 - wid2.Length, '0'); //'Öre

                    wtxt = wtxt + " BEL=" + wid1 + wid2;
                    string bnr = dtCDR.Rows[i]["B-nr"].ToString();
                    //bnr = bnr.PadRight(29 - bnr.Length, ' ');
                    bnr = bnr + CreateWhiteSpace(29, bnr.Length);
                    wtxt = wtxt + "BNR=" + bnr;


                    string cno = "";
                    if (string.IsNullOrEmpty(dtCDR.Rows[i]["C-nr"].ToString()) || dtCDR.Rows[i]["C-nr"].ToString().Length == 0)
                    {
                        wtxt = wtxt + "CNR=                             ";
                    }
                    else
                    {
                        cno = dtCDR.Rows[i]["C-nr"].ToString();
                        //cno = cno.PadRight(29 - cno.Length, ' ');
                        cno = cno + CreateWhiteSpace(29, cno.Length);

                        wtxt = wtxt + "CNR=" + cno;
                    }

                    string intwkServId = dtCDR.Rows[i]["Intwk-serv-id"].ToString();
                    intwkServId = intwkServId.PadLeft(5, '0');

                    wtxt = wtxt + "ISID=" + intwkServId + " ";
                    wtxt = wtxt + "ANR=" + anr + "\"";

                    //fileData.Append(wtxt + Environment.NewLine);
                    AppentDataToFile(wtxt, ref fileData);
                }
            }
            else
            {
                throw new Exception("CDR records are missing.Contact Telia ProSoft.");
            }

            return fileData;
        }

        #endregion


        private string CreateWhiteSpace(int maxlength, int valuelength)
        {
            StringBuilder space = new StringBuilder();

            for (int i = 0; i < maxlength - valuelength; i++)
            {
                space.Append(" ");
            }

            return space.ToString();
        }

        #region LOKNo
        public StringBuilder Write_LOKNO(int p_LOKnr, ref StringBuilder fileData)
        {
            string LOKnr = UtilityClass.GetLOKNo(p_LOKnr);

            string wtxt = string.Empty;
            int ore = 0;
            string wid1 = "";
            string wid2 = "";

            for (int j = 0; j < LOKnr.Length; j++)
            {
                if (LOKnr.Substring(j, 1) == ".")
                {
                    ore = 1;
                }
                else
                {
                    if (ore > 1)
                    {
                        wid2 = wid2 + LOKnr.Substring(j, 1);
                    }
                    else
                    {
                        wid1 = wid1 + LOKnr.Substring(j, 1);
                    }
                }
            }

            wid1 = wid1.PadLeft(10, '0');
            wid2 = wid2.PadRight(2 - wid2.Length, '0');

            wtxt = "\"05 LOK BEL=" + wid1 + wid2 + "\"";

            //fileData.Append(wtxt + Environment.NewLine);

            AppentDataToFile(wtxt, ref fileData);
            return fileData;
        }
        #endregion

        #region WriteJCLFile
        private StringBuilder CreateJCLFile(string wprfx, string wDNS)
        {
            string value1 = string.Empty;
            string value2 = string.Empty;
            StringBuilder strB = new StringBuilder();

            if (wprfx + wDNS.Trim() == "U1DSN" || wprfx + wDNS.Trim() == "U2DSN")
            {
                value1 = "AE25";
            }
            else if (wprfx + wDNS.Trim() == "C4DSN6" || wprfx + wDNS.Trim() == "U3DSN" || wprfx + wDNS.Trim() == "D2DSN6")
            {
                value1 = "A951";
            }

            if (wprfx + wDNS.Trim() == "U1DSN")
            {
                value2 = "AQ66.SYST";
            }
            else if (wprfx + wDNS.Trim() == "U3DSN")
            {
                value2 = "AQ66.UTVP";
            }
            else if (wprfx + wDNS.Trim() == "U2DSN")
            {
                value2 = "AQ91.SYST";
            }
            else if (wprfx + wDNS.Trim() == "C4DSN6")
            {
                value2 = "AX74.SYST";
            }
            else if (wprfx + wDNS.Trim() == "D2DSN6")
            {
                value2 = "AX82.UTVP";
            }

            strB.Append("//PPVSTART JOB (" + value1 + ",0,10),'* CFBT *',MSGCLASS=T,CLASS=A," + Environment.NewLine);
            strB.Append("//         REGION=7M,MSGLEVEL=(1,1),NOTIFY=&SYSUID                      00020000" + Environment.NewLine);
            strB.Append("//*************************************************************" + Environment.NewLine);
            strB.Append("//* MSGCLASS=X    A.S                                         *" + Environment.NewLine);
            strB.Append("//* MSGCLASS=T    A.Å                                         *" + Environment.NewLine);
            strB.Append("//*************************************************************" + Environment.NewLine);
            strB.Append("//JS001    EXEC PGM=IKJEFT01,DYNAMNBR=50,REGION=2048K" + Environment.NewLine);
            strB.Append("//SYSPROC  DD   DSN=" + value2 + ".PPV15.XPPV.CLIST,DISP=SHR" + Environment.NewLine);
            strB.Append("//ISPPROF  DD   DSN=" + value2 + ".PPV15.XPPV.ISPPROF,DISP=SHR" + Environment.NewLine);
            strB.Append("//ISPPLIB  DD   DSN=SYS1.ISPF.PANELS,DISP=SHR" + Environment.NewLine);
            strB.Append("//ISPMLIB  DD   DSN=SYS1.ISPF.MSGS,DISP=SHR" + Environment.NewLine);
            strB.Append("//ISPLOG   DD   DSN=" + value2 + ".PPV15.XPPV.SPFLOG1,DISP=SHR" + Environment.NewLine);
            strB.Append("//ISPSLIB  DD   DSN=" + value2 + ".PPV15.XPPV.SKELS,DISP=SHR" + Environment.NewLine);
            strB.Append("//ISPTLIB  DD   DSN=SYS1.ISPF.TABLES,DISP=SHR" + Environment.NewLine);
            strB.Append("//ISPTABL  DD   DSN=SYS1.ISPF.TABLES,DISP=SHR" + Environment.NewLine);
            strB.Append("//SYSTSPRT DD SYSOUT=*" + Environment.NewLine);
            strB.Append("//SYSUDUMP DD SYSOUT=*" + Environment.NewLine);
            strB.Append("//SYSPRINT DD SYSOUT=*" + Environment.NewLine);
            strB.Append("//SYSTSIN  DD *" + Environment.NewLine);

            strB.Append("ISPSTART CMD(%PPVSTART PRFX(" + wprfx + ") JOB(DAILY))" + Environment.NewLine);
            strB.Append("//**");

            return strB;
        }

        #endregion

    }
}