using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace PPVTool.DataCreation
{
    public partial class CreateTestData : System.Web.UI.Page
    {

        public int ant_sekt
        {
            get { return Convert.ToInt32(Session["ant_sekt"]); }
            set { Session["ant_sekt"] = value; }
        }

        string MainQuery = " SELECT DISTINCT `Indata-BIC-agree`.`Lpnr`,`Indata-BIC-agree`.`Agree-id` as AgreeId, `Indata-BIC-agree`.`Cust-id` as CustId, `Indata-BIC-agree`.`Prisplan`,  `Indata-BIC-agree`.`Plan-unit-id` as PlanUnitID FROM `Indata-BIC-agree`,`Lager-prisplan` WHERE `Indata-BIC-agree`.BICnr =  @BICnr AND   `Indata-BIC-agree`.`PP-lpnr` = `Lager-prisplan`.`PP-lpnr` ";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentUser"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                Session["CDRCalcKal"] = null;
                Session["DataTablePP"] = null;
                Session["ant_sekt"] = null;
                Session["lpnr"] = null;
                Session["DataTableOptionPP"] = null;
                Session["DataTableCountryPP"] = null;
                Session["DataTableTelePP"] = null;
                Session["DataTableDurationAllowPP"] = null;
                Session["DataTableCDRCalc"] = null;
                Session["dtgrdCDRCalculation"] = null;
                pnlGrid.Visible = false;
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                int temp = ant_sekt;

                BindUpRelease();
                BindEnviornment();

                ddlEnviornment.Items.Insert(0, new ListItem("--Select--", "0"));

                MultiView1.ActiveViewIndex = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindUpRelease()
        {
            string sql = " Select `UP-release` as UPRel,B.`Maskin` From `up-release` as A Inner JOin `ip-adress` B  On A.`IP-lpnr` = B.`IP-lpnr` Where A.`UP-lpnr` = 12 ";
            DatabaseConnection dc = new DatabaseConnection();
            ddlUprelease.DataSource = dc.GetByDataTable(sql);
            ddlUprelease.DataValueField = "Maskin";
            ddlUprelease.DataTextField = "UPRel";
            ddlUprelease.DataBind();
        }

        private void BindEnviornment()
        {

            string sql = " SELECT `Prefix`,`DB2-lpnr` as LPNR FROM `DB2-environment` WHere `Maskin` = '" + ddlUprelease.SelectedValue + "' ";

            DatabaseConnection dc = new DatabaseConnection();
            ddlEnviornment.DataSource = dc.GetByDataTable(sql);
            ddlEnviornment.DataValueField = "LPNR";
            ddlEnviornment.DataTextField = "Prefix";
            ddlEnviornment.DataBind();
        }

        protected void ddlEnviornment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FetchGridData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FetchGridData()
        {
            string sql = " Select DISTINCTROW A.`Prisplan`, A.`Ben`, A.`PP-lpnr` LPNR FROM `Lager-prisplan` as A INNER JOIN `DB2-prisplan` B on A.`PP-lpnr` = B.`PP-lpnr` Where B.`DB2-lpnr`= '" + ddlEnviornment.SelectedValue + "' Group by A.`Prisplan`, A.`Ben` , A.`PP-lpnr` ";
            DatabaseConnection dc = new DatabaseConnection();
            pnlGrid.Visible = false;
            if (dc.GetByDataTable(sql).Rows.Count > 0)
            {
                pnlGrid.Visible = true;
                grdPricePlan.DataSource = dc.GetByDataTable(sql);
                grdPricePlan.DataBind();
            }
        }

        protected void btnNext1_Click(object sender, EventArgs e)
        {
            try
            {
                bool valCheckbox = false;
                foreach (GridViewRow rw in grdPricePlan.Rows)
                {
                    CheckBox chkGrid = (CheckBox)rw.FindControl("chkGrid");

                    if (chkGrid.Checked)
                    {
                        valCheckbox = true;
                    }
                }

                if (!valCheckbox)
                {
                    throw new Exception("please select atleast one price plan.");
                }

                if (txtDescription.Text.Trim() == "")
                {
                    throw new Exception("please enter the value in description.");
                }

                Session.Add("lpnr", SetObject());

                string message = "You have created the main tasks and have been assigned test no = " + Convert.ToString(Session["lpnr"]);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                BindCustomer();
                //ddlCombCust.Items.Insert(0, new ListItem("--Select--", "0"));

                BindPricePlan();
                //ddlPricePlans.Items.Insert(0, new ListItem("--Select--", "0"));

                Session["DataTablePP"] = null;

                ltid.Text = " (id = " + Convert.ToString(Session["lpnr"]) + ")";

                txtCustomerStratDate.Text = "01/01/1998";
                txtCustomerEndDate.Text = "12/31/9999";


                grdPricePlanDtls.DataSource = null;
                grdPricePlanDtls.DataBind();

                lblMsg.Text = "";

                MultiView1.ActiveViewIndex = 2;
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }

        public int SetObject()
        {
            InitiateTest it = new InitiateTest();
            int testlpnr;

            try
            {
                DatabaseConnection dc = new DatabaseConnection();

                int Id = Convert.ToInt32(dc.ExceuteSclare("Select Max(`Styr-lpnr`) from Styrtabell"));

                it.UPlpnr = 12;
                it.DB2lpnr = Convert.ToInt32(ddlEnviornment.SelectedValue);
                it.Ben = txtDescription.Text.Trim();
                it.Styrlpnr = Id + 1;
                it.UserId = Convert.ToString(Session["CurrentUser"]);

                List<PricePlan> pp = SetPricePlan(it.Styrlpnr);

                testlpnr = dc.Insert_Styrtabell_Table(it, pp);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return testlpnr;
        }

        public List<PricePlan> SetPricePlan(int lpnr)
        {
            List<PricePlan> pp = new List<PricePlan>();

            foreach (GridViewRow rw in grdPricePlan.Rows)
            {
                CheckBox ck = (CheckBox)rw.FindControl("chkGrid");
                if (ck.Checked)
                {
                    PricePlan p = new PricePlan();
                    p.Lpnr = lpnr;
                    p.PPLpnr = Convert.ToInt32(rw.Cells[3].Text);
                    pp.Add(p);
                }
            }

            return pp;
        }

        private void BindCustomer()
        {

            //string sql = "SELECT `BIC-lager-nr` as BICNR,Ben FROM `Lager-BIC`;";
            string sql = "SELECT `BIC-lager-nr`,`Ben`,`LAN-userid`,DATE_FORMAT(Datum,'%d-%m-%Y') as Datum,`Kund-typ`,`Agree-typ` FROM `Lager-BIC`";
            DatabaseConnection dc = new DatabaseConnection();
            grdCombCust.DataSource = dc.GetByDataTable(sql);
            grdCombCust.DataBind();
            //ddlCombCust.DataSource = dc.GetByDataTable(sql);
            //ddlCombCust.DataValueField = "BICNR";
            //ddlCombCust.DataTextField = "Ben";
            //ddlCombCust.DataBind();
        }

        protected void grdCombCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdCombCust.SelectedRow != null)
            {
                Session["DataTablePP"] = null;

                txtCombCust.Text = Server.HtmlDecode(grdCombCust.SelectedRow.Cells[0].Text);

                FillCustomerStructure();
                FillAgreement();
            }
            else
            {
                txtCombCust.Text = "";
            }
        }

        protected void grdCustStruc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdCustStruc.SelectedRow != null)
            {
                txtCustStruc.Text = Server.HtmlDecode(grdCustStruc.SelectedRow.Cells[0].Text);
            }
            else
            {
                txtCustStruc.Text = "";
            }
        }

        protected void grdSelectedPricePlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdSelectedPricePlans.SelectedRow != null)
            {
                txtPricePlans.Text = Server.HtmlDecode(grdSelectedPricePlans.SelectedRow.Cells[0].Text);
                hfPriceplanDesc.Value = Server.HtmlDecode(grdSelectedPricePlans.SelectedRow.Cells[2].Text);
            }
            else
            {
                txtPricePlans.Text = "";
                hfPriceplanDesc.Value = "";
            }
        }

        protected void grdAgreements_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdAgreements.SelectedRow != null)
            {
                txtAgreement.Text = Server.HtmlDecode(grdAgreements.SelectedRow.Cells[0].Text);
            }
            else
            {
                txtAgreement.Text = "";
            }
        }

        private void BindPricePlan()
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT DISTINCT E.`Styr-lpnr`, E.`PP-lpnr` as PPLpnr, A.Prisplan, A.Ben,A.`Plan-option` as PO,A.Land,A.Valtnr,A.Durallw ");
            sql.Append("FROM `Lager-prisplan` A ");
            sql.Append(" INNER JOIN `DB2-prisplan` C ON A.`PP-lpnr` = C.`PP-lpnr` ");
            sql.Append(" INNER JOIN `DB2-environment` B ON B.`DB2-lpnr` = C.`DB2-lpnr` ");
            sql.Append(" INNER JOIN Styrtabell D ON B.`DB2-lpnr` = C.`DB2-lpnr` ");
            sql.Append(" INNER JOIN `Prisplan-urval` E ON D.`Styr-lpnr` = E.`Styr-lpnr` AND E.`PP-lpnr`= A.`PP-lpnr` ");
            sql.Append(" WHERE E.`Styr-lpnr`= " + Convert.ToString(Session["lpnr"]) + " ");

            DatabaseConnection dc = new DatabaseConnection();

            DataTable dt = dc.GetByDataTable(sql.ToString());

            grdSelectedPricePlans.DataSource = dt;
            grdSelectedPricePlans.DataBind();

            //ddlPricePlans.DataSource = dt;
            //ddlPricePlans.DataValueField = "PPLpnr";
            //ddlPricePlans.DataTextField = "Prisplan";
            //ddlPricePlans.DataBind();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Durallw"].ToString() == "J")
                {
                    hdp_bic20.Value = "J";
                }
                if (dt.Rows[i]["PO"].ToString() == "J")
                {
                    hdp_bic30.Value = "J";
                }
                if (dt.Rows[i]["Land"].ToString() == "J")
                {
                    hdp_bic35.Value = "J";
                }
                if (dt.Rows[i]["Valtnr"].ToString() == "J")
                {
                    hdp_bic45.Value = "J";
                }
            }

        }

        private void FillAgreement()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT `Agree-id` AgreementID,`Cust-id` CustID FROM `Lager-BIC-agree` WHERE `BIC-lager-nr` = " + txtCombCust.Text + " ");

            DatabaseConnection dc = new DatabaseConnection();
            grdAgreements.DataSource = dc.GetByDataTable(sql.ToString());
            grdAgreements.DataBind();

            //ddlAgreement.DataSource = dc.GetByDataTable(sql.ToString());
            //ddlAgreement.DataValueField = "AgreementID";
            //ddlAgreement.DataTextField = "AgreementID";
            //ddlAgreement.DataBind();

        }

        private void FillCustomerStructure()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT DISTINCT B.`Cust-id` CustID, C.`Bill-ent-id` BillEniId , C.`Chrgb-num-id` ChrgbNumId ");
            sql.Append(" FROM `Lager-BIC-cust` A ");
            sql.Append(" INNER JOIN `Lager-BIC-bill` B ON A.`Cust-id` = B.`Cust-id` ");
            sql.Append(" INNER JOIN `Lager-BIC-chrgb` C ON B.`Bill-ent-id` = c.`Bill-ent-id` ");
            sql.Append(" WHERE A.`BIC-lager-nr`= " + txtCombCust.Text + " ");

            DatabaseConnection dc = new DatabaseConnection();
            grdCustStruc.DataSource = dc.GetByDataTable(sql.ToString());
            grdCustStruc.DataBind();

            //ddlCustStruc.DataSource = dc.GetByDataTable(sql.ToString());
            //ddlCustStruc.DataValueField = "CustID";
            //ddlCustStruc.DataTextField = "CustID";
            //ddlCustStruc.DataBind();
        }

        //protected void ddlCombCust_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["DataTablePP"] = null;
        //        FillCustomerStructure();
        //        FillAgreement();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                if (txtCombCust.Text.Trim() == "")
                {
                    throw new Exception("please choose the customer.");
                }

                if (txtCustStruc.Text.Trim() == "")
                {
                    throw new Exception("please choose the customer structure.");
                }

                if (txtAgreement.Text.Trim() == "")
                {
                    throw new Exception("please choose the agreement.");
                }

                if (txtPricePlans.Text.Trim() == "")
                {
                    throw new Exception("please choose the price plan.");
                }

                if (txtPricePlans.Text != "")
                {
                    DataTable dt = new DataTable();

                    if (Session["DataTablePP"] != null)
                    {

                        int count;

                        dt = (DataTable)Session["DataTablePP"];

                        count = dt.Rows.Count;

                        DataRow dr = dt.NewRow();
                        dr["Agreement"] = txtAgreement.Text;
                        dr["CustId"] = txtCustStruc.Text;
                        dr["PricePlan"] = txtPricePlans.Text;
                        dr["pplpnr"] = hfPriceplanDesc.Value;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        dt.Columns.Add("Agreement");
                        dt.Columns.Add("CustId");
                        dt.Columns.Add("PricePlan");
                        dt.Columns.Add("pplpnr");

                        DataRow dr = dt.NewRow();
                        dr["Agreement"] = txtAgreement.Text;
                        dr["CustId"] = txtCustStruc.Text;
                        dr["PricePlan"] = txtPricePlans.Text;
                        dr["pplpnr"] = hfPriceplanDesc.Value;

                        dt.Rows.Add(dr);

                    }

                    Session["DataTablePP"] = null;

                    Session.Add("DataTablePP", dt);


                    grdPricePlanDtls.DataSource = dt;
                    grdPricePlanDtls.DataBind();

                    grdPricePlanDtls.BorderWidth = 1;

                }

            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }

        #region || Next Button 3 ||
        protected void btnNext3_Click(object sender, EventArgs e)
        {
            MySqlConnection con = null;
            MySqlTransaction tx = null;
            if (txtCombCust.Text== "")
            {
                ShowMessage("Please select value for BiClager.");
            }
            else
            {
                try
                {

                    int bicnr = SetObjectForBICnr(ref con, ref tx);
                    bool r1 = SetObjectKopplad_BIC(bicnr, con, tx);
                    bool r2 = SetObjectForBICagree(bicnr, con, tx);
                    bool r3 = SetCustomerDateBIC05(bicnr, con, tx);
                    bool r4 = SetBillDateBIC10(bicnr, con, tx);

                    if (bicnr > 0 && r1 == true && r2 == true && r3 == true && r4 == true)
                    {
                        hdbicnr.Value = Convert.ToString(bicnr);
                        tx.Commit();
                        hdCustNo.Value = txtCombCust.Text;//ddlCombCust.SelectedValue;
                    }
                    else
                    {
                        tx.Rollback();
                    }

                    if (hdp_bic20.Value == "J" || hdp_bic30.Value == "J" || hdp_bic35.Value == "J" || hdp_bic45.Value == "J")
                    {
                        lblTestNo.Text = " (id = " + Convert.ToString(Session["lpnr"]) + ")";

                        SetDateAtHeader(bicnr);


                        pnl1.Enabled = false;
                        pnl2.Enabled = false;
                        pnl3.Enabled = false;
                        pnl4.Enabled = false;

                        if (hdp_bic20.Value == "J")
                        {
                            BindPricePlanAllowanceDDL();
                            pnl4.Enabled = true;
                        }

                        if (hdp_bic30.Value == "J")
                        {
                            BindPricePlanOptionDDL();
                            BindKomdPlan();
                            pnl1.Enabled = true;
                        }

                        if (hdp_bic35.Value == "J")
                        {
                            BindPricePlanCountryDDL();
                            BindKomdCountry();
                            pnl2.Enabled = true;
                        }

                        if (hdp_bic45.Value == "J")
                        {
                            BindPricePlanTelephoneDDL();
                            pnl3.Enabled = true;
                        }

                        ddlpriceplanoption.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlpriceplancountry.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlPricePlanTelephone.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlDurationAllowwance.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlKombPlan.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlKombCountry.Items.Insert(0, new ListItem("--Select--", "0"));

                        Session["DataTableOptionPP"] = null;
                        Session["DataTableCountryPP"] = null;
                        Session["DataTableTelePP"] = null;
                        Session["DataTableDurationAllowPP"] = null;

                        lblMsg.Text = "";

                        MultiView1.ActiveViewIndex = 3;
                    }
                    else
                    {
                        lblMsg.Text = "";
                        hdp_CDRmeny.Value = "NY";
                        MultiView1.ActiveViewIndex = 4;
                        SetFormValue();
                    }
                }
                catch (Exception ex)
                {
                    if (tx != null)
                    {
                        tx.Rollback();
                        tx.Dispose();
                    }
                    Show(ex.Message, this);
                    ShowMessage(ex.Message);
                }

                finally
                {
                    if (tx != null)
                    {
                        tx.Dispose();
                    }

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
           
        }
        #endregion

        private int SetObjectForBICnr(ref MySqlConnection con, ref MySqlTransaction tx)
        {
            DatabaseConnection dc = new DatabaseConnection();
            int bicnr;
            int Id = Convert.ToInt32(dc.ExceuteSclare("Select Max(BICnr) from `Nytt-BICnr`"));

            bicnr = dc.Insert_NyttBICnr("E", Convert.ToInt32(txtCombCust.Text), Id + 1, Convert.ToString(Session["CurrentUser"]), ref tx, ref con);

            return bicnr;
        }

        private bool SetObjectKopplad_BIC(int bicnr, MySqlConnection con, MySqlTransaction tx)
        {
            DatabaseConnection dc = new DatabaseConnection();
            bool result = dc.Kopplad_BIC(Convert.ToInt32(Session["lpnr"]), bicnr, "J", tx, con);

            return result;
        }

        private bool SetObjectForBICagree(int bicnr, MySqlConnection con, MySqlTransaction tx)
        {
            DatabaseConnection dc = new DatabaseConnection();

            bool result = false;

            int lpnr = Convert.ToInt32(dc.ExceuteSclare("Select Max(Lpnr) from `Indata-BIC-agree`"));

            foreach (GridViewRow rw in grdPricePlanDtls.Rows)
            {

                ClsBICAccess bicaccess = new ClsBICAccess();

                bicaccess.AgreementId = Convert.ToInt32(rw.Cells[0].Text);
                bicaccess.CustId = Convert.ToInt32(rw.Cells[1].Text);
                bicaccess.Priceplan = Convert.ToString(rw.Cells[2].Text);
                bicaccess.PPLpnr = Convert.ToInt32(grdPricePlanDtls.DataKeys[rw.RowIndex].Value);
                bicaccess.Planunit = 1;
                bicaccess.BICnr = bicnr;

                lpnr = lpnr + 1;

                bicaccess.Lpnr = lpnr;

                result = dc.Insert_BICAgree(bicaccess, tx, con);

            }

            return result;

        }

        private bool SetCustomerDateBIC05(int bicnr, MySqlConnection con, MySqlTransaction tx)
        {

            DatabaseConnection dc = new DatabaseConnection();

            int lpnr = Convert.ToInt32(dc.ExceuteSclare("Select Max(Lpnr) from `Indata-BIC-05`"));

            DateTime startdt = Convert.ToDateTime(txtCustomerStratDate.Text);
            DateTime enddt = Convert.ToDateTime(txtCustomerEndDate.Text);

            bool result = dc.Insert_BIC05(bicnr, lpnr + 1, startdt, enddt, tx, con);

            return result;

        }

        private bool SetBillDateBIC10(int bicnr, MySqlConnection con, MySqlTransaction tx)
        {

            DatabaseConnection dc = new DatabaseConnection();

            int lpnr = Convert.ToInt32(dc.ExceuteSclare("Select Max(Lpnr) from `Indata-BIC-10`"));

            DateTime startdt = Convert.ToDateTime(txtBillStartDate.Text);
            DateTime enddt = Convert.ToDateTime(txtBillEndDate.Text);

            bool result = dc.Insert_BIC10(bicnr, lpnr + 1, startdt, enddt, tx, con);

            return result;

        }

        private void SetDateAtHeader(int bicnr)
        {
            string sql = "SELECT DATE_FORMAT(`Start-datum`, '%m/%d/%Y') as StartDate , DATE_FORMAT(`Stopp-datum`, '%m/%d/%Y') StopDate  FROM `Indata-BIC-05`  Where BICnr = " + bicnr;

            DatabaseConnection dc = new DatabaseConnection();
            DataTable dt = dc.GetByDataTable(sql.ToString());

            if (dt.Rows.Count > 0)
            {
                lblDateHeader.Text = " Start Date: " + dt.Rows[0]["StartDate"].ToString() + " Stop Date: " + dt.Rows[0]["StopDate"].ToString();
            }
        }

        #region || View 4 Coding ||

        private DataTable BindPricePlanCountryDDL()
        {
            string sql = MainQuery + " AND `Lager-prisplan`.`Land` = 'J' ORDER BY `Indata-BIC-agree`.`Lpnr`";

            DatabaseConnection dc = new DatabaseConnection();
            DataTable dt = dc.GetByDataTable(sql, Convert.ToInt32(hdbicnr.Value), "@BICnr");
            if (ddlpriceplancountry.SelectedIndex == -1)
            {
                ddlpriceplancountry.DataSource = dt;
                ddlpriceplancountry.DataTextField = "Prisplan";
                ddlpriceplancountry.DataValueField = "Prisplan";
                ddlpriceplancountry.DataBind();
            }
            return dt;
        }

        private DataTable BindPricePlanOptionDDL()
        {
            string sql = MainQuery + " AND `Lager-prisplan`.`Plan-option` = 'J' ORDER BY `Indata-BIC-agree`.`Lpnr`";

            DatabaseConnection dc = new DatabaseConnection();
            DataTable dt = dc.GetByDataTable(sql, Convert.ToInt32(hdbicnr.Value), "@BICnr");

            if (ddlpriceplanoption.SelectedIndex == -1)
            {
                ddlpriceplanoption.DataSource = dt;
                ddlpriceplanoption.DataTextField = "Prisplan";
                ddlpriceplanoption.DataValueField = "Prisplan";
                ddlpriceplanoption.DataBind();
            }
            return dt;
        }

        private DataTable BindPricePlanTelephoneDDL()
        {
            string sql = MainQuery + " AND `Lager-prisplan`.`Valtnr` = 'J' ORDER BY `Indata-BIC-agree`.`Lpnr`";

            DatabaseConnection dc = new DatabaseConnection();
            DataTable dt = dc.GetByDataTable(sql, Convert.ToInt32(hdbicnr.Value), "@BICnr");
            if (ddlPricePlanTelephone.SelectedIndex == -1)
            {
                ddlPricePlanTelephone.DataSource = dt;
                ddlPricePlanTelephone.DataTextField = "Prisplan";
                ddlPricePlanTelephone.DataValueField = "Prisplan";
                ddlPricePlanTelephone.DataBind();
            }
            return dt;
        }

        private DataTable BindPricePlanAllowanceDDL()
        {
            string sql = MainQuery + " AND `Lager-prisplan`.`Durallw` = 'J' ORDER BY `Indata-BIC-agree`.`Lpnr`";

            DatabaseConnection dc = new DatabaseConnection();
            DataTable dt = dc.GetByDataTable(sql, Convert.ToInt32(hdbicnr.Value), "@BICnr");

            if (ddlDurationAllowwance.SelectedIndex == -1)
            {
                ddlDurationAllowwance.DataSource = dt;
                ddlDurationAllowwance.DataTextField = "Prisplan";
                ddlDurationAllowwance.DataValueField = "Prisplan";
                ddlDurationAllowwance.DataBind();
            }
            return dt;
        }

        private void BindKomdPlan()
        {
            string sql = "SELECT `Plan-option` as PlanOption FROM `Lager-planoption` ORDER BY `Plan-option`";

            DatabaseConnection dc = new DatabaseConnection();
            DataTable dt = dc.GetByDataTable(sql);
            ddlKombPlan.DataSource = dt;
            ddlKombPlan.DataTextField = "PlanOption";
            ddlKombPlan.DataValueField = "PlanOption";
            ddlKombPlan.DataBind();
        }

        private void BindKomdCountry()
        {
            string sql = "SELECT Land FROM `Lager-land` ORDER BY Land";

            DatabaseConnection dc = new DatabaseConnection();
            DataTable dt = dc.GetByDataTable(sql);
            ddlKombCountry.DataSource = dt;
            ddlKombCountry.DataTextField = "Land";
            ddlKombCountry.DataValueField = "Land";
            ddlKombCountry.DataBind();
        }

        #endregion

        protected void ImgbtnOption_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlpriceplanoption.SelectedValue != "0" && ddlKombPlan.SelectedValue != "0")
                {
                    DataTable dt = new DataTable();
                    var dtOption = BindPricePlanOptionDDL();

                    if (Session["DataTableOptionPP"] != null)
                    {
                        dt = (DataTable)Session["DataTableOptionPP"];

                        foreach (DataRow r in dtOption.Select("Prisplan= " + ddlpriceplanoption.SelectedValue + ""))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Lpnr"] = r["Lpnr"].ToString();
                            dr["Agree id"] = r["AgreeId"].ToString();
                            dr["Cust id"] = r["CustId"].ToString();
                            dr["Price plan"] = r["Prisplan"].ToString();
                            dr["Plan unit"] = r["PlanUnitID"].ToString();
                            dr["Optn trm val"] = ddlKombPlan.SelectedValue;

                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Lpnr");
                        dt.Columns.Add("Agree id");
                        dt.Columns.Add("Cust id");
                        dt.Columns.Add("Price plan");
                        dt.Columns.Add("Plan unit");
                        dt.Columns.Add("Optn trm val");

                        foreach (DataRow r in dtOption.Select("Prisplan= " + ddlpriceplanoption.SelectedValue + ""))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Lpnr"] = r["Lpnr"].ToString();
                            dr["Agree id"] = r["AgreeId"].ToString();
                            dr["Cust id"] = r["CustId"].ToString();
                            dr["Price plan"] = r["Prisplan"].ToString();
                            dr["Plan unit"] = r["PlanUnitID"].ToString();
                            dr["Optn trm val"] = ddlKombPlan.SelectedValue;

                            dt.Rows.Add(dr);
                        }
                    }

                    Session["DataTableOptionPP"] = null;
                    Session.Add("DataTableOptionPP", dt);

                    grdPPOption.DataSource = dt;
                    grdPPOption.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ImgbtnCountry_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlpriceplancountry.SelectedValue != "0" && ddlKombCountry.SelectedValue != "0")
                {
                    DataTable dt = new DataTable();
                    var dtCntry = BindPricePlanCountryDDL();

                    if (Session["DataTableCountryPP"] != null)
                    {
                        dt = (DataTable)Session["DataTableCountryPP"];

                        foreach (DataRow r in dtCntry.Select("Prisplan= " + ddlpriceplancountry.SelectedValue + ""))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Lpnr"] = r["Lpnr"].ToString();
                            dr["Agree id"] = r["AgreeId"].ToString();
                            dr["Cust id"] = r["CustId"].ToString();
                            dr["Price plan"] = r["Prisplan"].ToString();
                            dr["Plan unit"] = r["PlanUnitID"].ToString();
                            dr["Sel area val"] = ddlKombCountry.SelectedValue;
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Lpnr");
                        dt.Columns.Add("Agree id");
                        dt.Columns.Add("Cust id");
                        dt.Columns.Add("Price plan");
                        dt.Columns.Add("Plan unit");
                        dt.Columns.Add("Sel area val");

                        foreach (DataRow r in dtCntry.Select("Prisplan= " + ddlpriceplancountry.SelectedValue + ""))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Lpnr"] = r["Lpnr"].ToString();
                            dr["Agree id"] = r["AgreeId"].ToString();
                            dr["Cust id"] = r["CustId"].ToString();
                            dr["Price plan"] = r["Prisplan"].ToString();
                            dr["Plan unit"] = r["PlanUnitID"].ToString();
                            dr["Sel area val"] = ddlKombCountry.SelectedValue;

                            dt.Rows.Add(dr);
                        }
                    }

                    Session["DataTableCountryPP"] = null;
                    Session.Add("DataTableCountryPP", dt);

                    grdPPCountry.DataSource = dt;
                    grdPPCountry.DataBind();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ImgbtnTele_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlPricePlanTelephone.SelectedValue != "0" && txtStartTelNo.Text.Trim() != "")
                {
                    int startNo = Convert.ToInt32(txtStartTelNo.Text.Trim());
                    int endNo = txtEndTelNo.Text.Trim() == "" ? startNo : Convert.ToInt32(txtEndTelNo.Text.Trim());

                    DataTable dt = new DataTable();
                    var dtTele = BindPricePlanTelephoneDDL();

                    if (Session["DataTableTelePP"] != null)
                    {
                        dt = (DataTable)Session["DataTableTelePP"];

                        for (int i = 0; i < Convert.ToInt32(ddlRepeat.SelectedValue); i++)
                        {
                            foreach (DataRow r in dtTele.Select("Prisplan= " + ddlPricePlanTelephone.SelectedValue + ""))
                            {

                                DataRow dr = dt.NewRow();
                                dr["Lpnr"] = r["Lpnr"].ToString();
                                dr["Agree id"] = r["AgreeId"].ToString();
                                dr["Cust id"] = r["CustId"].ToString();
                                dr["Price plan"] = r["Prisplan"].ToString();
                                dr["Plan unit"] = r["PlanUnitID"].ToString();
                                dr["Begin Point val"] = startNo;
                                dr["End Point val"] = endNo;

                                dt.Rows.Add(dr);
                            }

                            startNo = startNo + 1;
                            endNo = startNo;
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Lpnr");
                        dt.Columns.Add("Agree id");
                        dt.Columns.Add("Cust id");
                        dt.Columns.Add("Price plan");
                        dt.Columns.Add("Plan unit");
                        dt.Columns.Add("Begin Point val");
                        dt.Columns.Add("End Point val");

                        if (Convert.ToInt32(ddlRepeat.SelectedValue) > 1)
                        {
                            for (int i = 0; i < Convert.ToInt32(ddlRepeat.SelectedValue); i++)
                            {
                                foreach (DataRow r in dtTele.Select("Prisplan= " + ddlPricePlanTelephone.SelectedValue + ""))
                                {

                                    DataRow dr = dt.NewRow();
                                    dr["Lpnr"] = r["Lpnr"].ToString();
                                    dr["Agree id"] = r["AgreeId"].ToString();
                                    dr["Cust id"] = r["CustId"].ToString();
                                    dr["Price plan"] = r["Prisplan"].ToString();
                                    dr["Plan unit"] = r["PlanUnitID"].ToString();
                                    dr["Begin Point val"] = startNo;
                                    dr["End Point val"] = endNo;

                                    dt.Rows.Add(dr);
                                }

                                startNo = startNo + 1;
                                endNo = startNo;
                            }
                        }
                    }

                    Session["DataTableTelePP"] = null;
                    Session.Add("DataTableTelePP", dt);

                    grdPPTelephone.DataSource = dt;
                    grdPPTelephone.DataBind();

                    txtStartTelNo.Text = Convert.ToString(startNo);
                    txtEndTelNo.Text = txtStartTelNo.Text;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void imgbtnAllowance_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlDurationAllowwance.SelectedValue != "0" && txtDurationAllowance.Text.Trim() != "")
                {
                    DataTable dt = new DataTable();
                    var dtAllowance = BindPricePlanAllowanceDDL();

                    if (Session["DataTableDurationAllowPP"] != null)
                    {
                        dt = (DataTable)Session["DataTableDurationAllowPP"];

                        foreach (DataRow r in dtAllowance.Select("Prisplan= " + ddlDurationAllowwance.SelectedValue + ""))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Lpnr"] = r["Lpnr"].ToString();
                            dr["Agree id"] = r["AgreeId"].ToString();
                            dr["Cust id"] = r["CustId"].ToString();
                            dr["Price plan"] = r["Prisplan"].ToString();
                            dr["Plan unit"] = r["PlanUnitID"].ToString();
                            dr["Duration"] = txtDurationAllowance.Text.Trim();
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Lpnr");
                        dt.Columns.Add("Agree id");
                        dt.Columns.Add("Cust id");
                        dt.Columns.Add("Price plan");
                        dt.Columns.Add("Plan unit");
                        dt.Columns.Add("Duration");

                        foreach (DataRow r in dtAllowance.Select("Prisplan= " + ddlDurationAllowwance.SelectedValue + ""))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Lpnr"] = r["Lpnr"].ToString();
                            dr["Agree id"] = r["AgreeId"].ToString();
                            dr["Cust id"] = r["CustId"].ToString();
                            dr["Price plan"] = r["Prisplan"].ToString();
                            dr["Plan unit"] = r["PlanUnitID"].ToString();
                            dr["Duration"] = txtDurationAllowance.Text.Trim();

                            dt.Rows.Add(dr);
                        }
                    }

                    Session["DataTableDurationAllowPP"] = null;
                    Session.Add("DataTableDurationAllowPP", dt);

                    grdPPTelephone.DataSource = dt;
                    grdPPTelephone.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClearOption_Click(object sender, EventArgs e)
        {
            Session["DataTableOptionPP"] = null;
            grdPPOption.DataSource = null;
            grdPPOption.DataBind();
        }

        protected void btnClearCountry_Click(object sender, EventArgs e)
        {
            Session["DataTableCountryPP"] = null;
            grdPPCountry.DataSource = null;
            grdPPCountry.DataBind();
        }

        protected void btnClearTelephone_Click(object sender, EventArgs e)
        {
            Session["DataTableTelePP"] = null;
            grdPPTelephone.DataSource = null;
            grdPPTelephone.DataBind();
        }

        protected void btnClearAllownce_Click(object sender, EventArgs e)
        {
            Session["DataTableDurationAllowPP"] = null;
            grdPPAllowance.DataSource = null;
            grdPPAllowance.DataBind();
        }

        protected void btnNxtCountry_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCountryButtonData();
                SetFormValue();
                Session["DataTableCDRCalc"] = null;
                lblMsg.Text = "";
                hdp_CDRmeny.Value = "NY";
                MultiView1.ActiveViewIndex = 4;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region || Country Next Button Save Methods ||

        private void SaveCountryButtonData()
        {
            if (grdPPOption.Rows.Count > 0)
            {
                DatabaseConnection dc = new DatabaseConnection();
                bool rslt = dc.InsertCountryData(grdPPOption, Convert.ToDateTime(txtCustomerStratDate.Text), Convert.ToDateTime(txtCustomerEndDate.Text), "30", "P");
            }

            if (grdPPCountry.Rows.Count > 0)
            {
                DatabaseConnection dc = new DatabaseConnection();
                bool rslt = dc.InsertCountryData(grdPPCountry, Convert.ToDateTime(txtCustomerStratDate.Text), Convert.ToDateTime(txtCustomerEndDate.Text), "35", "L");
            }

            if (grdPPTelephone.Rows.Count > 0)
            {
                DatabaseConnection dc = new DatabaseConnection();
                bool rslt = dc.InsertCountryData(grdPPTelephone, Convert.ToDateTime(txtCustomerStratDate.Text), Convert.ToDateTime(txtCustomerEndDate.Text), "45", "T");
            }

            if (grdPPAllowance.Rows.Count > 0)
            {
                DatabaseConnection dc = new DatabaseConnection();
                bool rslt = dc.UpdateBICAgree(grdPPAllowance, Convert.ToInt32(hdbicnr.Value));
            }
        }

        #endregion


        #region CDR tab functionility Started 
        private void SetFormValue()
        {
            txtCallCostAmount.Text = "200";
            txtCallCostOre.Text = "00";

            #region Filling Hour Minute and Second

            var dthh = new DataTable();
            dthh.Columns.Add("Hour");

            for (int i = 0; i < 24; i++)
            {
                DataRow dr = dthh.NewRow();

                if (i < 10)
                {
                    dr["Hour"] = "0" + i.ToString();
                }
                else
                {
                    dr["Hour"] = i.ToString();
                }

                dthh.Rows.Add(dr);
            }

            ddlHH.DataSource = dthh;
            ddlHH.DataValueField = "Hour";
            ddlHH.DataTextField = "Hour";
            ddlHH.DataBind();

            ddlCDHH.DataSource = dthh;
            ddlCDHH.DataValueField = "hour";
            ddlCDHH.DataTextField = "hour";
            ddlCDHH.DataBind();

            var dtmm = new DataTable();
            dtmm.Columns.Add("min");

            for (int i = 0; i < 60; i++)
            {
                DataRow dr = dtmm.NewRow();

                if (i < 10)
                {
                    dr["min"] = "0" + i.ToString();
                }
                else
                {
                    dr["min"] = i.ToString();
                }

                dtmm.Rows.Add(dr);
            }

            ddlMM.DataSource = dtmm;
            ddlMM.DataValueField = "min";
            ddlMM.DataTextField = "min";
            ddlMM.DataBind();

            ddlCDMM.DataSource = dtmm;
            ddlCDMM.DataValueField = "min";
            ddlCDMM.DataTextField = "min";
            ddlCDMM.DataBind();

            ddlSS.DataSource = dtmm;
            ddlSS.DataValueField = "min";
            ddlSS.DataTextField = "min";
            ddlSS.DataBind();

            ddlCDSS.DataSource = dtmm;
            ddlCDSS.DataValueField = "min";
            ddlCDSS.DataTextField = "min";
            ddlCDSS.DataBind();


            #endregion

            DatabaseConnection dc = new DatabaseConnection();

            #region Filling Customer Combo
            grdCust.DataSource = dc.GetCustomerFrom(Convert.ToInt32(hdCustNo.Value));
            grdCust.DataBind();
            #endregion

            #region Filling CDR Layer

            grdCDRNo.DataSource = dc.GetCDRLayer();
            grdCDRNo.DataBind();

            #endregion

            #region Filling Service Identifier

            grdServiceIdentifier.DataSource = UtilityClass.FillServiceIdetifier();
            grdServiceIdentifier.DataBind();

            #endregion


        }

        protected void imgCalculateCDR_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (txtCustomer.Text == "")
                {
                    throw new Exception("select any customer.");
                }

                if (txtCDRNo.Text == "")
                {
                    throw new Exception("select any CDR from the CDR layer");
                }

                if (txtBNo.Text == "")
                {
                    throw new Exception("select any B no.");
                }

                //if (ddlCNo.SelectedValue == "0")
                //{
                //    throw new Exception("select any C no.");
                //}

                if (txtServiceIdentifier.Text == "")
                {
                    throw new Exception("select any service identifier.");
                }

                CalculateCDR();


            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }

        }

        private void CalculateCDR()
        {
            try
            {
                DataTable dt = new DataTable();
                int lpnr = 1;
                if (Session["DataTableCDRCalc"] != null)
                {
                    dt = (DataTable)Session["DataTableCDRCalc"];
                    int count = dt.Rows.Count;
                    lpnr = Convert.ToInt32(dt.Rows[count - 1]["Order number"].ToString()) + 1;
                    DataRow dr = dt.NewRow();
                    dr["Order number"] = lpnr;
                    dr["Rec"] = ddlRec.SelectedValue;
                    dr["Chrgb-no"] = txtCustomer.Text;
                    dr["CDR-no"] = txtCDRNo.Text;
                    dr["Servid/Calltype"] = UtilityClass.GetSelectedCDR(Convert.ToInt32(txtCDRNo.Text), "-");
                    dr["Date"] = txtCallStartDate.Text;
                    dr["Time"] = ddlHH.SelectedValue + ":" + ddlMM.SelectedValue + ":" + ddlSS.SelectedValue;
                    dr["Call Duration"] = ddlCDHH.SelectedValue + ":" + ddlCDMM.SelectedValue + ":" + ddlCDSS.SelectedValue;
                    dr["Amount"] = txtCallCostAmount.Text + "," + txtCallCostOre.Text;
                 //   dr["Amount"] = txtCallCostAmount.Text;
                    dr["B No"] = txtBNo.Text;
                    dr["C No"] = ddlCNo.SelectedValue == "0" ? "" : ddlCNo.SelectedValue;
                    dr["Qualifying-ind"] = ddlQualifyingInd.SelectedValue;
                    dr["Ben"] = UtilityClass.GetSelectedCDRBen(Convert.ToInt32(txtCDRNo.Text));
                    dr["Asoc"] = UtilityClass.GetSelectedCDRAsoc(Convert.ToInt32(txtCDRNo.Text));
                    dr["ServiceIdentifier"] = txtServiceIdentifier.Text;
                    dt.Rows.Add(dr);
                }
                else
                {

                    dt.Columns.Add("Order number");
                    dt.Columns.Add("Rec");
                    dt.Columns.Add("Chrgb-no");
                    dt.Columns.Add("CDR-no");
                    dt.Columns.Add("Servid/Calltype");
                    dt.Columns.Add("Date");
                    dt.Columns.Add("Time");
                    dt.Columns.Add("Call Duration");
                    dt.Columns.Add("Amount");
                    dt.Columns.Add("B No");
                    dt.Columns.Add("C No");
                    dt.Columns.Add("Qualifying-ind");
                    dt.Columns.Add("Ben");
                    dt.Columns.Add("Asoc");
                    dt.Columns.Add("ServiceIdentifier");

                    DataRow dr = dt.NewRow();

                    dr["Order number"] = lpnr;
                    dr["Rec"] = ddlRec.SelectedValue;
                    dr["Chrgb-no"] = txtCustomer.Text;
                    dr["CDR-no"] = txtCDRNo.Text;
                    dr["Servid/Calltype"] = UtilityClass.GetSelectedCDR(Convert.ToInt32(txtCDRNo.Text), "-");
                    dr["Date"] = txtCallStartDate.Text;
                    dr["Time"] = ddlHH.SelectedValue + ":" + ddlMM.SelectedValue + ":" + ddlSS.SelectedValue;
                    dr["Call Duration"] = ddlCDHH.SelectedValue + ":" + ddlCDMM.SelectedValue + ":" + ddlCDSS.SelectedValue;
                    dr["Amount"] = txtCallCostAmount.Text + txtCallCostOre.Text;
                   // dr["Amount"] = txtCallCostAmount.Text + "," + txtCallCostOre.Text;
                    dr["B No"] = txtBNo.Text;
                    dr["C No"] = ddlCNo.SelectedValue;
                    dr["Qualifying-ind"] = ddlQualifyingInd.SelectedValue;
                    dr["Ben"] = UtilityClass.GetSelectedCDRBen(Convert.ToInt32(txtCDRNo.Text));
                    dr["Asoc"] = UtilityClass.GetSelectedCDRAsoc(Convert.ToInt32(txtCDRNo.Text));
                    dr["ServiceIdentifier"] = txtServiceIdentifier.Text;

                    dt.Rows.Add(dr);
                }

                Session["DataTableCDRCalc"] = null;
                Session.Add("DataTableCDRCalc", dt);

                grdCDRFinal.DataSource = dt;
                grdCDRFinal.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdCDRFinal_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                var dt = (DataTable)Session["DataTableCDRCalc"];
                dt.Rows.RemoveAt(e.RowIndex);
                grdCDRFinal.DataSource = dt;
                grdCDRFinal.DataBind();

                if (dt.Rows.Count > 0)
                {
                    Session.Add("DataTableCDRCalc", dt);
                }
                else
                {
                    Session["DataTableCDRCalc"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCDRnext_Click(object sender, EventArgs e)
        {
            try
            {
                if (InsertIntoTempCDR())
                {
                    lblheader.Text = "Expected result (id = " + Convert.ToString(Session["lpnr"]) + ")";

                    lblMsg.Text = "";
                    MultiView1.ActiveViewIndex = 5;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool InsertIntoTempCDR()
        {

            List<CDR> cdr = new List<CDR>();

            foreach (GridViewRow row in grdCDRFinal.Rows)
            {
                CDR mcdr = new CDR();

                mcdr.Lpnr = Convert.ToInt32(row.Cells[1].Text);
                mcdr.Rec = row.Cells[2].Text;
                mcdr.Chrgno = row.Cells[3].Text;
                mcdr.CDRNo = row.Cells[4].Text;
                mcdr.Servid_Calltype = row.Cells[5].Text;
                mcdr.Date = Convert.ToDateTime(row.Cells[6].Text);
                mcdr.Time = row.Cells[7].Text;
                mcdr.CallDuration = row.Cells[8].Text;
                mcdr.Amount = Convert.ToDecimal(row.Cells[9].Text);
                mcdr.BNo = row.Cells[10].Text;
                mcdr.CNo = row.Cells[11].Text;
                mcdr.QualifyingInd = row.Cells[12].Text;
                mcdr.Ben = row.Cells[13].Text;
                mcdr.Asoc = row.Cells[14].Text;
                mcdr.ServiceIdentifier = row.Cells[15].Text;
                cdr.Add(mcdr);

            }

            DatabaseConnection dc = new DatabaseConnection();

            return dc.InsertCDRData(cdr);
        }

        protected void grdCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // assign firstname
                if (grdCust.SelectedRow != null)
                {
                    txtCustomer.Text = Server.HtmlDecode(grdCust.SelectedRow.Cells[0].Text);

                    DatabaseConnection dc = new DatabaseConnection();

                    grdCDRPricePlan.DataSource = dc.GetAgeementByBICnr(Convert.ToInt32(hdbicnr.Value));
                    grdCDRPricePlan.DataBind();

                    string typ = UtilityClass.GetTypeFrom(Convert.ToInt32(hdbicnr.Value));

                    int billid;

                    if (typ == "E")
                    {
                        billid = 0;
                    }
                    else
                    {
                        billid = Convert.ToInt32(grdCust.SelectedRow.Cells[1].Text);
                    }

                    string billStartDate = UtilityClass.GetBillDate(Convert.ToInt32(hdbicnr.Value), billid);

                    txtCallStartDate.Text = billStartDate;
                    lblCallStartDate.Text = "Bill-stop-date = " + billStartDate;

                }
                else
                {
                    txtCustomer.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdCDRPricePlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // assign value to textbox
                if (grdCDRPricePlan.SelectedRow != null)
                {
                    txtCDRPricePlan.Text = Server.HtmlDecode(grdCDRPricePlan.SelectedRow.Cells[0].Text);
                }
                else
                {
                    txtCDRPricePlan.Text = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdCDRNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // assign value to textbox
                if (grdCDRNo.SelectedRow != null)
                {
                    txtCDRNo.Text = Server.HtmlDecode(grdCDRNo.SelectedRow.Cells[0].Text);

                    lblCDRNo.Text = UtilityClass.GetSelectedCDR(Convert.ToInt32(grdCDRNo.SelectedRow.Cells[0].Text));

                    txtBNo.Text = "";
                    grdBNo.DataSource = UtilityClass.FillBNo(Convert.ToInt32(grdCDRNo.SelectedRow.Cells[0].Text));
                    grdBNo.DataBind();

                    ddlCNo.DataSource = UtilityClass.FillCNo();
                    ddlCNo.DataTextField = "B-nr";
                    ddlCNo.DataValueField = "B-nr";
                    ddlCNo.DataBind();

                    ddlCNo.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                else
                {
                    txtCDRNo.Text = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdServiceIdentifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // assign value to textbox
                if (grdServiceIdentifier.SelectedRow != null)
                {
                    txtServiceIdentifier.Text = Server.HtmlDecode(grdServiceIdentifier.SelectedRow.Cells[0].Text);
                }
                else
                {
                    txtServiceIdentifier.Text = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdBNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // assign value to textbox
                if (grdBNo.SelectedRow != null)
                {
                    txtBNo.Text = Server.HtmlDecode(grdBNo.SelectedRow.Cells[0].Text);
                }
                else
                {
                    txtBNo.Text = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        protected void btnExpectedRes_Click(object sender, EventArgs e)
        {
            try
            {
                SetCDRCalculationFormValues();

                MultiView1.ActiveViewIndex = 6;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        protected void SetCDRCalculationFormValues()
        {

            DatabaseConnection dc = new DatabaseConnection();

            bool b = dc.Delete_CDR_Temp_Details();

            for (int i = 1; i < 21; i++)
            {

                ListItem li = new ListItem();
                if (i < 10)
                {
                    li.Text = "0" + i.ToString();
                    li.Value = "0" + i.ToString();
                }
                else
                {
                    li.Text = i.ToString();
                    li.Value = i.ToString();
                }

                ddlSelection.Items.Add(li);
            }

            for (int i = 0; i < 101; i++)
            {
                ListItem li = new ListItem();
                if (i < 10)
                {
                    li.Text = "0" + i.ToString();
                    li.Value = "0" + i.ToString();
                }
                else
                {
                    li.Text = i.ToString();
                    li.Value = i.ToString();
                }

                ddlProcent1.Items.Add(li);
                ddlProcent2.Items.Add(li);
            }

            ddlProcent1.Items.Insert(0, new ListItem("Integer", "-1"));
            ddlProcent2.Items.Insert(0, new ListItem("Decimals", "-1"));


            var dtRoundOff = new DataTable();

            dtRoundOff.Columns.Add("Round off");
            dtRoundOff.Columns.Add("Description");

            string[,] item =
            {
                    { "Normal", "Ner om 0-4,Upp om 5-9" },
                    { "UP","Alltid upp"  },
                    { "Down","Alltid ner"}
                };

            for (int i = 0; i < 3; i++)
            {
                DataRow dr = dtRoundOff.NewRow();
                dr["Round off"] = item[i, 0];
                dr["Description"] = item[i, 1];

                dtRoundOff.Rows.Add(dr);
            }

            grdRoundOff.DataSource = dtRoundOff;
            grdRoundOff.DataBind();

            var dtPrecision = new DataTable();

            dtPrecision.Columns.Add("Precision");
            dtPrecision.Columns.Add("decimals");

            string[,] itemdtPrecision =
            {
                    { "1.0","0" },
                    { "0.1","1" },
                    { "0.01","2" },
                    { "0.001","3"},
                    { "0.0001","4"},
                    { "0.00001","5"},
                    { "0.000001","6"}
                };

            for (int i = 0; i < 7; i++)
            {
                DataRow dr = dtPrecision.NewRow();
                dr["Precision"] = itemdtPrecision[i, 0];
                dr["decimals"] = itemdtPrecision[i, 1];

                dtPrecision.Rows.Add(dr);
            }

            grdPrecision.DataSource = dtPrecision;
            grdPrecision.DataBind();

            grdCreatedCDR.DataSource = UtilityClass.GetTempCDRData().Rows.Count > 0 ? UtilityClass.GetTempCDRData() : null;
            grdCreatedCDR.DataBind();

            grdCDRCalculation.DataSource = null;
            grdCDRCalculation.DataBind();

            grvCDRFinalCalc.DataSource = null;
            grvCDRFinalCalc.DataBind();


            txtRoundOff.Text = Server.HtmlDecode(grdRoundOff.Rows[0].Cells[0].Text);
            hfRoundOff.Value = Server.HtmlDecode(grdRoundOff.Rows[0].Cells[1].Text);

            txtPrecision.Text = Server.HtmlDecode(grdPrecision.Rows[2].Cells[0].Text);
            hfPrecision.Value = Server.HtmlDecode(grdPrecision.Rows[2].Cells[1].Text);
        }

        protected void grdPrecision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // assign value to textbox
                if (grdPrecision.SelectedRow != null)
                {
                    txtPrecision.Text = Server.HtmlDecode(grdPrecision.SelectedRow.Cells[0].Text);
                    hfPrecision.Value = Server.HtmlDecode(grdPrecision.SelectedRow.Cells[1].Text);
                }
                else
                {
                    txtPrecision.Text = "";
                    hfPrecision.Value = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdRoundOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // assign value to textbox
                if (grdRoundOff.SelectedRow != null)
                {
                    txtRoundOff.Text = Server.HtmlDecode(grdRoundOff.SelectedRow.Cells[0].Text);
                    hfRoundOff.Value = Server.HtmlDecode(grdRoundOff.SelectedRow.Cells[1].Text);
                }
                else
                {
                    txtRoundOff.Text = Server.HtmlDecode(grdRoundOff.Rows[1].Cells[0].Text);
                    hfRoundOff.Value = Server.HtmlDecode(grdRoundOff.Rows[1].Cells[1].Text);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gvr in grdCust.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdCust, String.Concat("Select$", gvr.RowIndex), true);
            }

            foreach (GridViewRow gvrpp in grdCDRPricePlan.Rows)
            {
                gvrpp.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdCDRPricePlan, String.Concat("Select$", gvrpp.RowIndex), true);
            }

            foreach (GridViewRow gvr in grdCDRNo.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdCDRNo, String.Concat("Select$", gvr.RowIndex), true);
            }

            foreach (GridViewRow gvr in grdServiceIdentifier.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdServiceIdentifier, String.Concat("Select$", gvr.RowIndex), true);
            }

            foreach (GridViewRow gvr in grdBNo.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdBNo, String.Concat("Select$", gvr.RowIndex), true);
            }


            foreach (GridViewRow gvr in grdRoundOff.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdRoundOff, String.Concat("Select$", gvr.RowIndex), true);
            }

            foreach (GridViewRow gvr in grdPrecision.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdPrecision, String.Concat("Select$", gvr.RowIndex), true);
            }


            foreach (GridViewRow gvr in grdCombCust.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdCombCust, String.Concat("Select$", gvr.RowIndex), true);

            }
            foreach (GridViewRow gvr in grdCustStruc.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdCustStruc, String.Concat("Select$", gvr.RowIndex), true);
            }

            foreach (GridViewRow gvr in grdAgreements.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdAgreements, String.Concat("Select$", gvr.RowIndex), true);
            }

            foreach (GridViewRow gvr in grdSelectedPricePlans.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdSelectedPricePlans, String.Concat("Select$", gvr.RowIndex), true);
            }

            base.Render(writer);
        }

        protected void rboProcentVol_CheckedChanged(object sender, EventArgs e)
        {
            ddlProcent1.Enabled = true;
            ddlProcent2.Enabled = true;
            txtBel1.Enabled = false;
            txtBel2.Enabled = false;
            ClearGrdCDRCalculation();
        }

        protected void rboProcent_CheckedChanged(object sender, EventArgs e)
        {
            ddlProcent1.Enabled = true;
            ddlProcent2.Enabled = true;
            txtBel1.Enabled = false;
            txtBel2.Enabled = false;
            ClearGrdCDRCalculation();
        }

        protected void rboBelopp_CheckedChanged(object sender, EventArgs e)
        {
            ddlProcent1.Enabled = false;
            ddlProcent2.Enabled = false;
            txtBel1.Enabled = true;
            txtBel2.Enabled = true;
            ClearGrdCDRCalculation();
        }


        protected void ImgBtnShiftRight_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string wflytt = string.Empty;
                string wprc10 = string.Empty;
                string wtkn = string.Empty;

                if (RdoAdd.Checked)
                {
                    wprc10 = "0.";
                    wtkn = "";
                }
                else
                {
                    wprc10 = "1.";
                    wtkn = "-";
                }


                if (rboProcent.Checked || rboProcentVol.Checked)
                {
                    if (ddlProcent1.SelectedIndex < 0 || ddlProcent2.SelectedIndex < 0)
                    {
                        throw new Exception("You have not selected percentage units.");
                    }

                    wflytt = wprc10 + ddlProcent1.SelectedValue + ddlProcent2.SelectedValue;
                }
                else
                {
                    if (txtBel1.Text.Trim() == "" || txtBel2.Text.Trim() == "")
                    {
                        throw new Exception("You have not selected the amount.");
                    }
                    wflytt = wtkn + txtBel1.Text.Trim() + "." + txtBel2.Text.Trim();
                }


                if (wflytt != "")
                {
                    ant_sekt = ant_sekt + 1;

                    if (ant_sekt > Convert.ToInt32(ddlSelection.SelectedValue))
                    {
                        ant_sekt = ant_sekt - 1;
                        throw new Exception("You have given the values ​​for each section!Create BURCs now!");
                    }
                    else
                    {
                        if (ant_sekt == Convert.ToInt32(ddlSelection.SelectedValue))
                        {
                            imgbuttonDown.Enabled = true;
                        }

                        DataTable dt = new DataTable();

                        DataRow dr;

                        if (Session["dtgrdCDRCalculation"] != null)
                        {
                            dt = (DataTable)Session["dtgrdCDRCalculation"];
                            dr = dt.NewRow();

                            dr["Selection"] = ant_sekt;
                            dr["DataVal"] = wflytt;
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dt.Columns.Add("Selection");
                            dt.Columns.Add("DataVal");
                            dr = dt.NewRow();

                            dr["Selection"] = ant_sekt;
                            dr["DataVal"] = wflytt;
                            dt.Rows.Add(dr);
                        }

                        grdCDRCalculation.DataSource = dt;
                        grdCDRCalculation.DataBind();
                        grdCDRCalculation.BorderWidth = 1;

                        Session["dtgrdCDRCalculation"] = null;
                        Session.Add("dtgrdCDRCalculation", dt);
                    }
                }

                if (grvCDRFinalCalc.Rows.Count <= 0)
                {
                    grvCDRFinalCalc.DataSource = null;
                    grvCDRFinalCalc.DataBind();
                    grvCDRFinalCalc.BorderWidth = 0;
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

        protected void btnClearCDR_Click(object sender, EventArgs e)
        {
            ClearGrdCDRCalculation();
        }

        private void ClearGrdCDRCalculation()
        {
            ant_sekt = 0;
            Session["dtgrdCDRCalculation"] = null;

            grdCDRCalculation.DataSource = null;
            grdCDRCalculation.DataBind();
            grdCDRCalculation.BorderWidth = 0;

            if (grvCDRFinalCalc.Rows.Count <= 0)
            {
                Session["CDRCalcKal"] = null;
                grvCDRFinalCalc.DataSource = null;
                grvCDRFinalCalc.DataBind();
                grvCDRFinalCalc.BorderWidth = 0;
            }
        }

        protected void imgbuttonDown_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (txtPrecision.Text.Trim() == "")
                {
                    throw new Exception("You have not chosen precision");
                }

                if (txtRoundOff.Text.Trim() == "")
                {
                    throw new Exception("You have not selected rounding");
                }

                decimal wbel = 0;
                string Lpnr = string.Empty;
                string BURCnr = string.Empty;
                string Sectprocent = string.Empty;
                string Sectbelopp = string.Empty;
                string Sectbelopp_ack = string.Empty;
                decimal wavrbel = 0;
                decimal wbel2 = 0;

                bool validate = false;

                foreach (GridViewRow gr in grdCreatedCDR.Rows)
                {
                    RadioButton rboSelect = (RadioButton)gr.FindControl("rboSelect");

                    if (rboSelect.Checked)
                    {
                        validate = true;

                        wbel = Convert.ToDecimal(grdCreatedCDR.Rows[gr.RowIndex].Cells[7].Text);

                        foreach (GridViewRow grv in grdCDRCalculation.Rows)
                        {
                            Lpnr = grdCreatedCDR.Rows[gr.RowIndex].Cells[1].Text;
                            BURCnr = grdCDRCalculation.Rows[grv.RowIndex].Cells[0].Text;


                            foreach (GridViewRow gvr in grvCDRFinalCalc.Rows)
                            {
                                if (Lpnr == grvCDRFinalCalc.Rows[gvr.RowIndex].Cells[0].Text)
                                {
                                    throw new Exception("There is already entry of this Lpnr.");
                                }
                            }

                            if (rboProcent.Checked) // 1 - Procent
                            {
                                Sectprocent = string.Format("{0:N4}", Convert.ToDecimal(grdCDRCalculation.Rows[grv.RowIndex].Cells[1].Text, CultureInfo.InvariantCulture));
                                wavrbel = wbel * Convert.ToDecimal(grdCDRCalculation.Rows[grv.RowIndex].Cells[1].Text, CultureInfo.InvariantCulture);
                                Sectbelopp = string.Format("{0:N6}", wavrbel);
                                Sectbelopp_ack = Convert.ToString(string.Format("{0:N6}", wbel - Convert.ToDecimal(Sectbelopp, CultureInfo.InvariantCulture)));
                                wbel = Convert.ToDecimal(Sectbelopp_ack);
                            }
                            if (rboProcentVol.Checked) // 3 - Procent (volmetod)
                            {
                                wbel = Convert.ToDecimal(grdCreatedCDR.Rows[gr.RowIndex].Cells[7].Text, CultureInfo.InvariantCulture);
                                Sectprocent = string.Format("{0:N4}", Convert.ToDecimal(grdCDRCalculation.Rows[grv.RowIndex].Cells[1].Text, CultureInfo.InvariantCulture));
                                wavrbel = wbel * Convert.ToDecimal(grdCDRCalculation.Rows[grv.RowIndex].Cells[1].Text, CultureInfo.InvariantCulture);
                                Sectbelopp = string.Format("{0:N6}", wavrbel);
                                Sectbelopp_ack = Convert.ToString(string.Format("{0:N6}", wbel - Convert.ToDecimal(Sectbelopp, CultureInfo.InvariantCulture) - wbel2));
                            }
                            if (rboBelopp.Checked) // 2 - Belopp
                            {
                                Sectbelopp_ack = string.Format("{0:N6}", Convert.ToDecimal(grdCDRCalculation.Rows[grv.RowIndex].Cells[1].Text, CultureInfo.InvariantCulture));
                                Sectbelopp = string.Format("{0:N6}", 0);
                                Sectprocent = string.Format("{0:N4}", 0);
                            }
                        }

                        DataTable dt = new DataTable();
                        DataRow dr;


                        if (Session["CDRCalcKal"] != null)
                        {

                            dt = (DataTable)Session["CDRCalcKal"];

                            dr = dt.NewRow();

                            dr["Lpnr"] = Lpnr;
                            dr["section"] = BURCnr;
                            dr["AmtPro"] = Sectbelopp;
                            dr["netchrge"] = Sectbelopp_ack;
                            dr["dscntpcnt"] = Sectprocent;
                            dt.Rows.Add(dr);

                        }
                        else
                        {
                            dt.Columns.Add("Lpnr");
                            dt.Columns.Add("section");
                            dt.Columns.Add("AmtPro");
                            dt.Columns.Add("netchrge");
                            dt.Columns.Add("dscntpcnt");

                            dr = dt.NewRow();

                            dr["Lpnr"] = Lpnr;
                            dr["section"] = BURCnr;
                            dr["AmtPro"] = Sectbelopp;
                            dr["netchrge"] = Sectbelopp_ack;
                            dr["dscntpcnt"] = Sectprocent;
                            dt.Rows.Add(dr);
                        }

                        grvCDRFinalCalc.DataSource = dt;
                        grvCDRFinalCalc.DataBind();

                        grvCDRFinalCalc.BorderWidth = 1;

                        Session["CDRCalcKal"] = null;
                        Session.Add("CDRCalcKal", dt);

                        DatabaseConnection dc = new DatabaseConnection();

                        bool b = dc.Insert_CDR_Temp_Details(Convert.ToInt32(Lpnr), Convert.ToInt32(BURCnr), Convert.ToDouble(Sectbelopp), Convert.ToDecimal(Sectbelopp_ack), Convert.ToDouble(Sectprocent));

                        if (!b)
                        {
                            throw new Exception("error in saving CDR...");
                        }
                    }
                }

                if (!validate)
                {
                    throw new Exception("You have not selected any CDR");
                }

                lblMsg.Text = "";

            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }

        protected void btnNextCDRFinal_Click(object sender, EventArgs e)
        {
            try
            {
                if (grvCDRFinalCalc.Rows.Count > 0)
                {
                    lblHead1.Text = "Expected result (id = " + Convert.ToString(Session["lpnr"]) + ")";

                    lblMsg.Text = "";
                    MultiView1.ActiveViewIndex = 7;
                }
                else
                {
                    throw new Exception("Select CDR for Calculation.");
                }

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                Show(ex.Message, this);
            }
        }

        protected void btnASOCBURC_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnection dc = new DatabaseConnection();

                int i = dc.upd_CDR_indata(hdp_CDRmeny.Value, Convert.ToString(Session["CurrentUser"]), Convert.ToInt32(Session["lpnr"]));

                if (hdp_CDRmeny.Value == "NY")
                {

                    lblV9Head.Text = "Daily run parameter (id = " + Convert.ToString(Session["lpnr"]) + ") ";

                    MultiView1.ActiveViewIndex = 8;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                Show(ex.Message, this);
            }
        }

        protected void btnV9Next_Click(object sender, EventArgs e)
        {
            try
            {
                string winit = string.Empty;

                if (rbv9Yes.Checked)
                {
                    winit = "J";
                }
                else
                {
                    winit = "N";
                }

                DatabaseConnection dc = new DatabaseConnection();

                bool b = dc.Update_Styrtabell(Convert.ToInt32(Session["lpnr"]), winit);


                if (rbv9Yes.Checked)
                {

                }
                else
                {
                    Show("OK input feed completed.Test run now !!!", this);
                    Response.Redirect("/Default.aspx");
                }
            }

            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                Show(ex.Message, this);
            }
        }

        protected void rboSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rboSelect = (RadioButton)sender;
                GridViewRow gvr = (GridViewRow)rboSelect.NamingContainer;
                string lpnr = gvr.Cells[1].Text;
                BindgrvCDRFinalCalc(lpnr);
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }

        private void BindgrvCDRFinalCalc(string lpnr)
        {
            DataTable dt = UtilityClass.GetDetailsofTempCDR(Convert.ToInt32(lpnr));

            DataTable dts = (DataTable)Session["CDRCalcKal"];

            if (dts != null)
            {
                if (dts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dts.Rows)
                    {
                        dt.ImportRow(dr);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                grvCDRFinalCalc.DataSource = dt;
                grvCDRFinalCalc.DataBind();
                grvCDRFinalCalc.BorderWidth = 1;
            }
            else
            {
                if (grdCDRCalculation.Rows.Count <= 0)
                {
                    grdCDRCalculation.DataSource = null;
                    grdCDRCalculation.DataBind();
                    grdCDRCalculation.BorderWidth = 0;
                }

                grvCDRFinalCalc.DataSource = null;
                grvCDRFinalCalc.DataBind();
                grvCDRFinalCalc.BorderWidth = 0;
            }

        }

        protected void grvCDRFinalCalc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "DELETE")
                {
                    DatabaseConnection dc = new DatabaseConnection();
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow gvr = grvCDRFinalCalc.Rows[rowIndex];

                    int lpnr = Convert.ToInt32(gvr.Cells[0].Text);
                    bool b = dc.Delete_CDR_Temp_Details(lpnr);

                    if (b)
                    {
                        DataTable dt = (DataTable)Session["CDRCalcKal"];
                        if (dt.Rows.Count > 0)
                        {
                            dt.Rows.RemoveAt(rowIndex);
                            Session.Add("CDRCalcKal", dt);
                        }

                        BindgrvCDRFinalCalc(lpnr.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
                ShowMessage(ex.Message);
            }
        }

        protected void grvCDRFinalCalc_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtPricePlans.Text = "";
            txtCombCust.Text = "";
           // ddRepeatFrequency.SelectedIndex = 0;
            txtCustStruc.Text = "";
            txtAgreement.Text = "";
            txtCombCust.Focus();
        }
    }
}