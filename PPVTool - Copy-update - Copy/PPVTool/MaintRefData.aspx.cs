using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

namespace PPVTool
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        TreeNode TvHome;
        TreeNode TvCust;
        TreeNode TvBill;
        TreeNode TvCharge;
        public static int CustCount = 0;
        public static int BillIDCount = 0;
        public static int ChargeNumberCount = 0;
        public static int[] CustArray = new int[9];
        public static int[] BillIDArray = new int[100];
        public static int[,] BillEntIDArray = new int[9, 9];
        public static int[] ChargeNumberArray = new int[100];
        public static int ii = 1, j = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string constr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT Ben,`CDR-lager-nr` FROM `Lager-CDR`"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        ddSelectCDR.DataSource = cmd.ExecuteReader();
                        ddSelectCDR.DataTextField = "Ben";
                        ddSelectCDR.DataValueField = "CDR-lager-nr";
                        ddSelectCDR.DataBind();
                        con.Close();
                    }

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        ddRepeatFrequency.DataSource = Enumerable.Range(1, 100);
                        ddRepeatFrequency.DataBind();
                        con.Close();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT Prefix,`DB2-lpnr` FROM `DB2-environment`"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        ddDB2Environment.DataSource = cmd.ExecuteReader();
                        ddDB2Environment.DataTextField = "Prefix";
                        ddDB2Environment.DataValueField = "DB2-lpnr";
                        ddDB2Environment.DataBind();

                        con.Close();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT Prefix,`DB2-lpnr` FROM `DB2-environment`"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();

                        ddPricePlanConnectDB2.DataSource = cmd.ExecuteReader();
                        ddPricePlanConnectDB2.DataTextField = "Prefix";
                        ddPricePlanConnectDB2.DataValueField = "DB2-lpnr";
                        ddPricePlanConnectDB2.DataBind();

                        con.Close();
                    }

                    // GridView

                    AdminDB db = new AdminDB();
                    string sql = "SELECT Prisplan, Ben, `PP-lpnr` as PPlpnr FROM `lager-prisplan` ORDER BY Prisplan";
                    gvSelectPricePlan.DataSource = db.GetDataByDataTable(sql);
                    gvSelectPricePlan.DataBind();
                }

                ddSelectCDR.Items.Insert(0, new ListItem("--Select--", "0"));
                //ddRepeatFrequency.Items.Insert(0, new ListItem("--Select--", "0"));
                ddDB2Environment.Items.Insert(0, new ListItem("--Select--", "0"));
                ddPricePlanConnectDB2.Items.Insert(0, new ListItem("--Select--", "0"));

                //BIC Layer
                ddBICLayerCustType.Items.Insert(0, new ListItem("--Select--", "0"));
                ddBICLayerCustType.Items.Add(new ListItem("N-Normal Customer","N"));
                ddBICLayerCustType.Items.Add(new ListItem("C-Centrex Customer","C"));

                ddBICLayerBillCycleMajor.Items.Insert(0, new ListItem("--Select--", "0"));
                ddBICLayerBillCycleMajor.Items.Add(new ListItem("M-Monthly","M"));
                ddBICLayerBillCycleMajor.Items.Add(new ListItem("W-Weekly","W"));
                ddBICLayerBillCycleMajor.Items.Add(new ListItem("D-Daily","D"));

                ddBICLayerCorpPrivate.Items.Insert(0, new ListItem("--Select--", "0"));
                ddBICLayerCorpPrivate.Items.Add(new ListItem("R-Residential","R"));
                ddBICLayerCorpPrivate.Items.Add(new ListItem("B-Residential","B"));

                ddBICLayerEVCenterType.Items.Insert(0, new ListItem("--Select--", "0"));
                ddBICLayerEVCenterType.Items.Add("I");
                ddBICLayerEVCenterType.ForeColor = System.Drawing.Color.DarkGray;
                ddBICLayerEVCenterType.Enabled = false;
                ddBICLayerEVCenterType.ToolTip = "Disabled by Admin";

                ddBICLayerTimSplit.Items.Insert(0, new ListItem("--Select--", "0"));
                ddBICLayerTimSplit.Items.Add(new ListItem("Yes","Y"));
                ddBICLayerTimSplit.Items.Add(new ListItem("No","N"));
            }
        }

        protected void btnPlanOptionUpdate_Click(object sender, EventArgs e)
        {
            string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            MySqlConnection con = new MySqlConnection(cs1);
            MySqlCommand cmd = new MySqlCommand();

            MySqlParameter sp1 = new MySqlParameter("@txtPlanOption", MySqlDbType.VarChar);
            sp1.Value = txtPlanOption.Text;

            cmd.Parameters.Add(sp1);
            cmd.Connection = con;

            if (txtPlanOption.Text != "")
            {
                cmd.CommandText = "SELECT `Plan-Option` FROM `lager-planoption` WHERE trim(upper(`Plan-Option`)) = trim(upper(@txtPlanOption))";
                con.Open();

                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblPlanOption.ForeColor = System.Drawing.Color.Red;
                    lblPlanOption.Font.Italic = false;
                    lblPlanOption.Text = "Plan option already exists!";
                    dr.Close();
                    con.Close();
                    return;
                }
                dr.Close();
                cmd.CommandText = "INSERT INTO `lager-planoption` VALUES(@txtPlanOption)";
                //con.Open();

                cmd.ExecuteNonQuery();
                con.Close();

                txtPlanOption.Text = null;

                lblPlanOption.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                lblPlanOption.Font.Italic = true;
                lblPlanOption.Text = "A New Plan Option : " + sp1.Value + " has been created Successfully!";
            }

        }
        protected void btnNewCountryInsert_Click(object sender, EventArgs e)
        {
            string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            MySqlConnection con = new MySqlConnection(cs1);
            MySqlCommand cmd = new MySqlCommand();

            MySqlParameter sp1 = new MySqlParameter("@txtNewCountryInsert", MySqlDbType.VarChar);
            sp1.Value = txtNewCountryInsert.Text;

            cmd.Parameters.Add(sp1);
            cmd.Connection = con;

            if (txtNewCountryInsert.Text != "")
            {
                cmd.CommandText = "Select `Land` FROM `lager-land` WHERE trim(upper(`Land`)) = trim(upper(@txtNewCountryInsert))";
                con.Open();

                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblcountryoption.ForeColor = System.Drawing.Color.Red;
                    lblcountryoption.Font.Italic = false;
                    lblcountryoption.Text = "Country already exists!";
                    dr.Close();
                    con.Close();
                    return;
                }
                dr.Close();
                cmd.CommandText = "INSERT INTO `lager-land` VALUES(@txtNewCountryInsert)";
                //con.Open();

                cmd.ExecuteNonQuery();
                con.Close();

                txtNewCountryInsert.Text = null;

                lblcountryoption.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                lblcountryoption.Font.Italic = true;
                lblcountryoption.Text = "A Country with name : " + sp1.Value + " has been created Successfully!";
            }

        }

        protected void rbCreateNewCDR_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCreateNewCDR.Checked == true)
            {
                Panel1.Visible = true;
                Panel2.Visible = false;
                txtCDRServId.Focus();
            }
        }

        protected void rbBnrToCDR_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBnrToCDR.Checked == true)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                ddSelectCDR.Focus();
            }
        }

        protected void rbCreateNewPricePlan_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCreateNewPricePlan.Checked == true)
            {
                Panel3.Visible = true;
                Panel4.Visible = false;
                txtPricePlan.Focus();
            }

        }

        protected void rbLinkPricePlanDb2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLinkPricePlanDb2.Checked == true)
            {
                Panel3.Visible = false;
                Panel4.Visible = true;
            }
        }

        protected void chkRow_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;
            //lbSelectedPricePlan.Items.Add(new ListItem(gvSelectPricePlan.DataKeys[gr.RowIndex].Value.ToString()));

            lblLinkPPtoDb2Environment.Text = "";

            if (gr.RowType == DataControlRowType.DataRow)
            {
                if (chk.Checked)
                {
                    //string priceplan = gr.Cells[1].Text;
                    lbSelectedPricePlan.Items.Add(new ListItem(gr.Cells[1].Text + " [ " + gr.Cells[3].Text + " ]"));
                }
                else if (!chk.Checked)
                {
                    lbSelectedPricePlan.Items.Remove(gr.Cells[1].Text + " [ " + gr.Cells[3].Text + " ]");
                }

                gr.Cells[1].Focus();
            }
        }

        protected void btnCallCategoryUpdate_Click(object sender, EventArgs e)
        {
            string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            MySqlConnection con = new MySqlConnection(cs1);
            MySqlCommand cmd = new MySqlCommand();

            MySqlParameter sp1 = new MySqlParameter("@txtCallCategory", MySqlDbType.VarChar);
            sp1.Value = txtCallCategory.Text;

            MySqlParameter sp2 = new MySqlParameter("@txtName", MySqlDbType.VarChar);
            sp2.Value = txtName.Text;

            cmd.Parameters.Add(sp1);
            cmd.Parameters.Add(sp2);
            cmd.Connection = con;
            string s = txtCallCategory.Text;
            int result;
            if (int.TryParse(s, out result))
            {

                if (txtCallCategory.Text != "" && txtName.Text != "" && result > 1)
                {
                    cmd.CommandText = "SELECT `CAT-cod` FROM `Lager-CCTB` WHERE trim(upper(`CAT-cod`)) = trim(upper(@txtCallCategory))";
                    con.Open();

                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblCallCategory.ForeColor = System.Drawing.Color.Red;
                        lblCallCategory.Font.Italic = false;
                        lblCallCategory.Text = "Category Code already exists!";
                        dr.Close();
                        con.Close();
                        return;
                    }
                    dr.Close();
                    cmd.CommandText = "INSERT INTO `Lager-CCTB` VALUES(@txtCallCategory, @txtName)";
                    //con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();

                    txtCallCategory.Text = null;

                    lblCallCategory.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                    lblCallCategory.Font.Italic = true;
                    lblCallCategory.Text = "A New Category Code : " + sp1.Value + " has been created Successfully!";
                }
            }
        }

        protected void btnLinkPPtoDb2Environment_Click(object sender, EventArgs e)
        {
            string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            MySqlConnection con = new MySqlConnection(cs1);

            int startIndex, endIndex, pplpnr, db2lpnr = 0;
            string str;

            if (ddDB2Environment.SelectedIndex > 0)
            {
                lblLinkPPtoDb2Environment.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                lblLinkPPtoDb2Environment.Font.Italic = true;
                db2lpnr = Int32.Parse(ddDB2Environment.SelectedItem.Value);
            }
            else
            {
                lblLinkPPtoDb2Environment.ForeColor = System.Drawing.Color.Red;
                lblLinkPPtoDb2Environment.Font.Italic = false;
                lblLinkPPtoDb2Environment.Text = "Please select DB2 Environment.";
                ddDB2Environment.Focus();
                return;
            }


            MySqlParameter sp1 = new MySqlParameter("@db2lpnr", MySqlDbType.Int32);
            sp1.Value = db2lpnr;
            String skipLinks = "";

            if (lbSelectedPricePlan.Items.Count > 0)
            {
                for (int i = 0; i < lbSelectedPricePlan.Items.Count; i++)
                {
                    str = lbSelectedPricePlan.Items[i].ToString();
                    startIndex = str.IndexOf("[");
                    endIndex = str.IndexOf("]");

                    pplpnr = Int32.Parse(str.Substring(startIndex + 1, (endIndex - startIndex) - 1).Trim());

                    MySqlParameter sp2 = new MySqlParameter("@pplpnr", MySqlDbType.Int32);
                    sp2.Value = pplpnr;

                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Parameters.Add(sp1);
                    cmd.Parameters.Add(sp2);
                    cmd.Connection = con;

                    cmd.CommandText = "SELECT `DB2-lpnr`, `PP-lpnr` FROM `DB2-prisplan` WHERE `DB2-lpnr` = @db2lpnr AND `PP-lpnr` = @pplpnr";
                    con.Open();

                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        skipLinks = skipLinks + ", " + sp2.Value.ToString();
                        //lblLinkPPtoDb2Environment.Text = "Price Plan : " + sp2.Value + " is already linked to DB2 Environment : " + ddDB2Environment.SelectedItem.Text + "!";
                        //lblLinkPPtoDb2Environment.ForeColor = System.Drawing.Color.Red;
                        //lblLinkPPtoDb2Environment.Font.Bold = true;
                        dr.Close();
                        con.Close();
                        continue;
                    }
                    dr.Close();


                    cmd.Connection = con;

                    cmd.CommandText = "INSERT INTO `DB2-prisplan` VALUES(@db2lpnr, @pplpnr)";
                    //con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                if (skipLinks != "")
                {
                    lblLinkPPtoDb2Environment.Text = "Selected Price Plans (except " + skipLinks + " as they are already linked) linked to DB2 Environment : " + ddDB2Environment.SelectedItem.Text + " successfully!";
                }
                else
                    lblLinkPPtoDb2Environment.Text = "Selected Price Plans linked to DB2 Environment : " + ddDB2Environment.SelectedItem.Text + " successfully!";
            }
            else
            {
                lblLinkPPtoDb2Environment.ForeColor = System.Drawing.Color.Red;
                lblLinkPPtoDb2Environment.Font.Italic = false;
                lblLinkPPtoDb2Environment.Text = "Please select at least 1 Price Plan.";
                return;
            }
        }

        protected void btnCDR_Click(object sender, EventArgs e)
        {
            string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            MySqlConnection con = new MySqlConnection(cs1);
            MySqlCommand cmd = new MySqlCommand();

            MySqlParameter sp1 = new MySqlParameter("@txtCDRServId", MySqlDbType.VarChar);
            sp1.Value = txtCDRServId.Text;

            MySqlParameter sp2 = new MySqlParameter("@txtCDRCallType", MySqlDbType.VarChar);
            sp2.Value = txtCDRCallType.Text;

            MySqlParameter sp3 = new MySqlParameter("@txtCDRASOC", MySqlDbType.VarChar);
            sp3.Value = txtCDRASOC.Text;

            MySqlParameter sp4 = new MySqlParameter("@txtCDRDescription", MySqlDbType.VarChar);
            sp4.Value = txtCDRDescription.Text;

            cmd.Parameters.Add(sp1);
            cmd.Parameters.Add(sp2);
            cmd.Parameters.Add(sp3);
            cmd.Parameters.Add(sp4);
            cmd.Connection = con;
           
            try
            {
                if (txtCDRServId.Text != "" && txtCDRCallType.Text != "")
            
                {
                    cmd.CommandText = "SELECT `CDR-lager-nr` FROM `Lager-CDR` WHERE trim(upper(`Servid`)) = trim(upper(@txtCDRServId)) AND trim(upper(`Calltype`)) = trim(upper(@txtCDRCallType))";
                    con.Open();

                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblCreateCDR.ForeColor = System.Drawing.Color.Red;
                        lblCreateCDR.Font.Italic = false;
                        lblCreateCDR.Text = "Servid + Call Type is already posted!";
                        dr.Close();
                        con.Close();
                        return;
                    }
                    dr.Close();
                    cmd.CommandText = "INSERT INTO `Lager-CDR`(Servid, Calltype, Asoc, Ben) VALUES(@txtCDRServId, @txtCDRCallType, @txtCDRASOC, @txtCDRDescription)";
                    //con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();

                    lblCreateCDR.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                    lblCreateCDR.Font.Italic = true;
                    lblCreateCDR.Text = "A New Call Detail Record has been created Successfully!";
                }
            }
            catch (Exception ex)
            {

                lblCreateCDR.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                lblCreateCDR.Font.Italic = true;
                lblCreateCDR.Text = ex.Message;
            }

               
        }

        protected void btnCDRReset_Click(object sender, EventArgs e)
        {
            txtCDRServId.Text = null;
            txtCDRCallType.Text = null;
            txtCDRASOC.Text = null;
            txtCDRDescription.Text = null;
            lblCreateCDR.Text = "";
            txtCDRServId.Focus();
        }

        protected void btnBnrToCDR_Click(object sender, EventArgs e)
        {
            string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            MySqlConnection con = new MySqlConnection(cs1);

            string strBnr = txtBnumber.Text, last4 = "";
            int last4Plus1 = 0;

            MySqlCommand cmd = new MySqlCommand();

            MySqlParameter sp1 = new MySqlParameter("@ddSelectCDR", MySqlDbType.Int32);
            sp1.Value = ddSelectCDR.SelectedValue;

            MySqlParameter sp2 = new MySqlParameter("@txtBnumber", MySqlDbType.VarChar);
            sp2.Value = strBnr;

            MySqlParameter sp3 = new MySqlParameter("@txtBnrOptionalText", MySqlDbType.VarChar);
            sp3.Value = txtBnrOptionalText.Text;

            cmd.Parameters.Add(sp1);
            cmd.Parameters.Add(sp2);
            cmd.Parameters.Add(sp3);
            cmd.Connection = con;
            con.Open();
            if (ddSelectCDR.SelectedIndex == 0)
            {
                //Do Nothing
                //Label110.Text = "Please Select CDR type.";
            }

            else// (ddSelectCDR.SelectedIndex>0 )  /* && txtBnumber.Text != ""*/
            {
                try
                {
                    for (int i = 0; i < Int32.Parse(ddRepeatFrequency.SelectedItem.Text); i++)
                    {
                        cmd.CommandText = "SELECT lc.`CDR-lager-nr`, lcb.`B-nr` FROM `Lager-CDR` lc INNER JOIN `Lager-CDR-Bnr` lcb ON (lc.`CDR-lager-nr` = lcb.`CDR-lager-nr`) WHERE lc.`CDR-lager-nr` = @ddSelectCDR AND trim(upper(lcb.`B-nr`)) = trim(upper(\"" + strBnr + "\"))";

                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            lblBnrToCDR.ForeColor = System.Drawing.Color.Red;
                            lblBnrToCDR.Font.Italic = false;
                            lblBnrToCDR.Text = "B Number : " + strBnr + " is already posted!";
                            dr.Close();
                            con.Close();
                            return;
                        }

                        dr.Close();

                        cmd.CommandText = "INSERT INTO `Lager-CDR-Bnr` VALUES(@ddSelectCDR, \"" + strBnr + "\", @txtBnrOptionalText)";
                        cmd.ExecuteNonQuery();

                        if (Int32.Parse(ddRepeatFrequency.SelectedItem.Text) > 1)
                        {
                            last4 = strBnr.Substring(strBnr.Length - 4, 4);

                            last4Plus1 = Int32.Parse(last4) + 1;
                            strBnr = strBnr.Substring(0, strBnr.Length - 4) + last4Plus1.ToString();

                            sp2 = new MySqlParameter("@txtBnumber", MySqlDbType.VarChar);
                            sp2.Value = strBnr;
                        }
                    }

                    con.Close();

                    lblBnrToCDR.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                    lblBnrToCDR.Font.Italic = true;
                    lblBnrToCDR.Text = "Linking is done Successfully!";
                }
                catch (Exception ex)
                {
                   
                    lblBnrToCDR.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                    lblBnrToCDR.Font.Italic = true;
                    lblBnrToCDR.Text = ex.Message;
                }
               
            }
            //Label5.Text = "Select Value for CDR !";

        }
        protected void CustomValidatorddlselectcdr_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(ddSelectCDR.SelectedIndex ==0 )
            {
                CustomValidatorddlselectcdr.ErrorMessage = "Please Select CDR Type.";
                args.IsValid = false;
                return;
            }
        }

        //protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    lblBnrToCDR.Text = "";

        //    if (txtBnumber.Text.Trim().Length > 10)
        //    {
        //        CustomValidator1.ErrorMessage = "B number cannot have more than 10 characters.";
        //        args.IsValid = false;
        //        return;
        //    }
           
        //    else if (Int32.Parse(ddRepeatFrequency.SelectedItem.Text) > 1 && txtBnumber.Text.Length < 5)
        //    {
        //        CustomValidator1.ErrorMessage = "B number must contain more than 4 digits if Repeat > 1";
        //        args.IsValid = false;
        //        return;
        //    }
        //    else if (Int32.Parse(ddRepeatFrequency.SelectedItem.Text) > 1)
        //    {
        //        if (Int32.Parse(txtBnumber.Text.Substring((txtBnumber.Text.Length) - 4)) >= 8000)
        //        {
        //            CustomValidator1.ErrorMessage = "The last 4 digits in B number should be less than 8000";
        //            args.IsValid = false;
        //            return;
        //        }
        //    }
        //}

        protected void btnBnrToCDRReset_Click(object sender, EventArgs e)
        {
            ddSelectCDR.SelectedIndex = 0;
            txtBnumber.Text = "";
            ddRepeatFrequency.SelectedIndex = 0;
            txtBnrOptionalText.Text = "";
            lblBnrToCDR.Text = "";
            ddSelectCDR.Focus();
        }

        protected void btnCreatePricePlan_Click(object sender, EventArgs e)
        {
            if (txtPricePlan.Text.Trim() != "")
            {
                string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
                MySqlConnection con = new MySqlConnection(cs1);
                MySqlCommand cmd = new MySqlCommand();

                MySqlParameter sp1 = new MySqlParameter("@txtPricePlan", MySqlDbType.VarChar);
                sp1.Value = txtPricePlan.Text;

                MySqlParameter sp2 = new MySqlParameter("@lokusId", MySqlDbType.VarChar);
                sp2.Value = "L" + txtPricePlan.Text;

                MySqlParameter sp3 = new MySqlParameter("@PricePlanType", MySqlDbType.VarChar);
                if (RadioButton1.Checked)
                    sp3.Value = "B";
                else if (RadioButton2.Checked)
                    sp3.Value = "O";
                else if (RadioButton3.Checked)
                    sp3.Value = "P";
                else if (RadioButton4.Checked)
                    sp3.Value = "G";
                else if (RadioButton5.Checked)
                    sp3.Value = "U";
                else
                    sp3.Value = "X";

                MySqlParameter sp4 = new MySqlParameter("@PricePlanAlternative", MySqlDbType.VarChar);
                if (rbAlternativeN.Checked)
                    sp4.Value = "N";
                else if (rbAlternativeC.Checked)
                    sp4.Value = "C";

                MySqlParameter sp5 = new MySqlParameter("@PricePlanOption", MySqlDbType.VarChar);
                if (rbPlanOptionYes.Checked)
                    sp5.Value = "J";
                else if (rbPlanOptionNo.Checked)
                    sp5.Value = "N";

                MySqlParameter sp6 = new MySqlParameter("@PricePlanCountryGroup", MySqlDbType.VarChar);
                if (rbCountryGroupYes.Checked)
                    sp6.Value = "J";
                else if (rbCountryGroupNo.Checked)
                    sp6.Value = "N";

                MySqlParameter sp7 = new MySqlParameter("@PricePlanPickAPoint", MySqlDbType.VarChar);
                if (rbPickPointYes.Checked)
                    sp7.Value = "J";
                else if (rbPickPointNo.Checked)
                    sp7.Value = "N";

                MySqlParameter sp8 = new MySqlParameter("@PricePlanDurationAllowance", MySqlDbType.VarChar);
                if (rbDurationAllowanceYes.Checked)
                    sp8.Value = "J";
                else if (rbDurationAllowanceNo.Checked)
                    sp8.Value = "N";

                MySqlParameter sp9 = new MySqlParameter("@txtPricePlanOptionalText", MySqlDbType.VarChar);
                sp9.Value = txtPricePlanOptionalText.Text;

                cmd.Parameters.Add(sp1);
                cmd.Parameters.Add(sp2);
                cmd.Parameters.Add(sp3);
                cmd.Parameters.Add(sp4);
                cmd.Parameters.Add(sp5);
                cmd.Parameters.Add(sp6);
                cmd.Parameters.Add(sp7);
                cmd.Parameters.Add(sp8);
                cmd.Parameters.Add(sp9);
                cmd.Connection = con;

                cmd.CommandText = "SELECT `Prisplan` FROM `Lager-prisplan` WHERE trim(upper(`Prisplan`)) = trim(upper(@txtPricePlan))";
                con.Open();

                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblCreatePricePlan.ForeColor = System.Drawing.Color.Red;
                    lblCreatePricePlan.Font.Italic = false;
                    lblCreatePricePlan.Text = "Price Plan : " + txtPricePlan.Text + " is already posted!";
                    dr.Close();
                    con.Close();
                    return;
                }

                dr.Close();

                cmd.CommandText = "INSERT INTO `Lager-prisplan`(`Prisplan`, `Lokus-id`, `Plan-type-ind`, `Typ`, `Plan-option`, `Land`, `Valtnr`, `Durallw`, `Ben`) VALUES(@txtPricePlan, @lokusId, @PricePlanType, @PricePlanAlternative, @PricePlanOption, @PricePlanCountryGroup, @PricePlanPickAPoint, @PricePlanDurationAllowance, @txtPricePlanOptionalText)";
                cmd.ExecuteNonQuery();

                int pplpnr = 0;

                if (ddPricePlanConnectDB2.SelectedIndex > 0)
                {
                    MySqlCommand cmdDb2 = new MySqlCommand();

                    MySqlParameter pricePlan = new MySqlParameter("@txtPricePlan", MySqlDbType.VarChar);
                    pricePlan.Value = txtPricePlan.Text;

                    cmdDb2.Parameters.Add(pricePlan);
                    cmdDb2.Connection = con;
                    cmdDb2.CommandText = "SELECT `PP-lpnr` FROM `Lager-prisplan` WHERE trim(upper(`Prisplan`)) = trim(upper(@txtPricePlan))";

                    MySqlDataReader drDb2 = cmdDb2.ExecuteReader();
                    if (drDb2.Read())
                        pplpnr = drDb2.GetInt32("PP-lpnr");

                    drDb2.Close();

                    MySqlParameter ddDb2 = new MySqlParameter("@ddPricePlanConnectDB2", MySqlDbType.Int32);
                    ddDb2.Value = ddPricePlanConnectDB2.SelectedValue;

                    MySqlParameter pplpnrDb2 = new MySqlParameter("@pplpnr", MySqlDbType.Int32);
                    pplpnrDb2.Value = pplpnr;

                    cmdDb2.Parameters.Add(ddDb2);
                    cmdDb2.Parameters.Add(pplpnrDb2);
                    cmdDb2.Connection = con;

                    cmdDb2.CommandText = "INSERT INTO `DB2-prisplan` VALUES(@ddPricePlanConnectDB2, @pplpnr)";
                    cmdDb2.ExecuteNonQuery();
                }

                con.Close();
                lblCreatePricePlan.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3366FF");
                lblCreatePricePlan.Font.Italic = true;
                lblCreatePricePlan.Text = "A New Price Plan : " + txtPricePlan.Text + " has been created Successfully!";
            }
        }

        protected void btnPricePlanReset_Click(object sender, EventArgs e)
        {
            RadioButton1.Checked = true;
            RadioButton2.Checked = false;
            RadioButton3.Checked = false;
            RadioButton4.Checked = false;
            RadioButton5.Checked = false;

            rbAlternativeN.Checked = true;
            rbAlternativeC.Checked = false;

            rbPlanOptionNo.Checked = true;
            rbPlanOptionYes.Checked = false;

            rbCountryGroupNo.Checked = true;
            rbCountryGroupYes.Checked = false;

            rbPickPointNo.Checked = true;
            rbPickPointYes.Checked = false;

            rbDurationAllowanceNo.Checked = true;
            rbDurationAllowanceYes.Checked = false;

            ddPricePlanConnectDB2.SelectedIndex = 0;

            txtPricePlanOptionalText.Text = "";
            lblCreatePricePlan.Text = "";

            txtPricePlan.Text = "";
            txtPricePlan.Focus();
        }

        protected void btnPlanOptionReset_Click(object sender, EventArgs e)
        {
            txtPlanOption.Text = "";
            lblPlanOption.Text = "";
            txtPlanOption.Focus();
        }

        protected void btnCallCategoryReset_Click(object sender, EventArgs e)
        {
            txtCallCategory.Text = "";
            txtName.Text = "";
            lblCallCategory.Text = "";
            txtCallCategory.Focus();
        }

        protected void btnLinkPPtoDB2Reset_Click(object sender, EventArgs e)
        {
            AdminDB db = new AdminDB();
            string sql = "SELECT Prisplan, Ben, `PP-lpnr` as PPlpnr FROM `lager-prisplan` ORDER BY Prisplan";
            gvSelectPricePlan.DataSource = db.GetDataByDataTable(sql);
            gvSelectPricePlan.DataBind();

            lbSelectedPricePlan.Items.Clear();
            ddDB2Environment.SelectedIndex = 0;
            lblLinkPPtoDb2Environment.Text = "";
        }

        private void resetBICHome()
        {
            ddBICLayerCustType.SelectedIndex = 0;
            ddBICLayerBillCycleMajor.SelectedIndex = 0;
            ddBICLayerCorpPrivate.SelectedIndex = 0;
            ddBICLayerEVCenterType.SelectedIndex = 0;
            ddBICLayerTimSplit.SelectedIndex = 0;

            txtBICLayerOrganisationNo.Text = "";
            txtBICLayerBillCycleLen.Text = "";
            txtBICLayerBillCycleCode.Text = "";
            txtBICLayerBillInd.Text = "";
            txtBICLayerANumber.Text = "";
            txtBICLayerStreetAddress.Text = "";
            txtBICLayerMailingAddress.Text = "";
            txtBICLayerOptionalText.Text = "";
        }
        protected void btnBICLayerReset_Click(object sender, EventArgs e)
        {
            resetBICHome();
            ddBICLayerCustType.Focus();
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddBICLayerCustType.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICLayerCustType.Focus();
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddBICLayerBillCycleMajor.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICLayerBillCycleMajor.Focus();
            }
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddBICLayerCorpPrivate.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICLayerCorpPrivate.Focus();
            }
        }

        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddBICLayerTimSplit.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICLayerTimSplit.Focus();
            }
        }

        protected void btnBICLayerUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                MultiView1.ActiveViewIndex = 1;

                Array.Clear(CustArray, 0, CustArray.Length);
                Array.Clear(BillIDArray, 0, BillIDArray.Length);
                Array.Clear(ChargeNumberArray, 0, ChargeNumberArray.Length);
                j = 1;
                ii = 1;

                ddBICCustCount.Focus();          
            }
        }

        protected void btnBICCustCountOk_Click(object sender, EventArgs e)
        {
            if (ddBICCustCount.SelectedIndex == 0)
                return;

            if (ddBICCustCount.SelectedIndex > 0)
            {
                //TvHome = new TreeNode("Home","Home");
                int i, j;
                CustCount = 1;
                for (i = 0; i < Int32.Parse(ddBICCustCount.SelectedValue); i++)
                {
                    j = i + 1;
                    TvCust = new TreeNode("Customer : " + j, "CUST" + j.ToString());
                    TvCust.SelectAction = TreeNodeSelectAction.None;
                    TreeView1.Nodes.Add(TvCust);
                    //CustArray[i] = i + 1;                    
                }

                TreeView1.ExpandAll();

                ddBICCustCount.ForeColor = System.Drawing.Color.DarkGray;
                ddBICCustCount.Enabled = false;
                btnBICCustCountOk.ForeColor = System.Drawing.Color.DarkGray;
                btnBICCustCountOk.Enabled = false;

                ddBICBillIDCount.Visible = true;
                btnBICBillIDCountOk.Visible = true;
                lblBICBillIDCount.Visible = true;
                lblBICBillIDCount.Text = "Select Number of Bill Accounts for <br>'Customer : " + CustCount + "'";

                ddBICBillIDCount.Focus();
            }
        }

        protected void btnBICBillIDCountOk_Click(object sender, EventArgs e)
        {
            int i;

            if (ddBICBillIDCount.SelectedIndex == 0)
                return;

            if (Int32.Parse(ddBICCustCount.SelectedValue) == 1)
            {
                BillIDArray[CustCount - 1] = Int32.Parse(ddBICBillIDCount.SelectedValue);
                BillIDCount = BillIDArray[CustCount - 1];
                ddBICBillIDCount.SelectedIndex = 0;

                TreeNode node = TreeView1.FindNode(Server.HtmlEncode("CUST" + CustCount));
                for (i = 1; i <= BillIDArray[CustCount - 1]; i++)
                {
                    TvBill = new TreeNode("Bill Account : " + i, "BILL" + i);
                    TvBill.SelectAction = TreeNodeSelectAction.None;
                    node.ChildNodes.Add(TvBill);                    
                }
                                
                TreeView1.ExpandAll();
                
                ddBICBillIDCount.Enabled = false;
                ddBICBillIDCount.ForeColor = System.Drawing.Color.DarkGray;
                btnBICBillIDCountOk.Enabled = false;
                btnBICBillIDCountOk.ForeColor = System.Drawing.Color.DarkGray;

                ChargeNumberCount = 1;
                lblBICChargeBillCount.Visible = true;
                ddBICChargeBillCount.Visible = true;
                btnBICChargeBillCountOk.Visible = true;
                //lblBICChargeBillCount.Text = "Select Chargeable Number for Bill Account : " + ChargeNumberCount;
                lblBICChargeBillCount.Text = "Select Chargeable Number for <br>'Customer : 1' :: 'Bill Account : " + ChargeNumberCount + "'";

                ddBICChargeBillCount.Focus();
                return;                
            }

            if(CustCount == 1 && Int32.Parse(ddBICCustCount.SelectedValue) > 1)
            {
                BillIDArray[CustCount - 1] = Int32.Parse(ddBICBillIDCount.SelectedValue);
                BillIDCount = BillIDArray[CustCount - 1];
                ddBICBillIDCount.SelectedIndex = 0;

                TreeNode node = TreeView1.FindNode(Server.HtmlEncode("CUST" + CustCount));
                for (i = 1; i <= BillIDArray[CustCount - 1]; i++)
                {
                    TvBill = new TreeNode("Bill Account : " + i, "BILL" + i);
                    TvBill.SelectAction = TreeNodeSelectAction.None;
                    node.ChildNodes.Add(TvBill);                    
                }

                CustCount++;
                lblBICBillIDCount.Text = "Select Number of Bill Accounts for <br>'Customer : " + CustCount + "'";
                ddBICBillIDCount.Focus();
                return;            
            }

            if (CustCount > 1 && CustCount <= Int32.Parse(ddBICCustCount.SelectedValue) && Int32.Parse(ddBICCustCount.SelectedValue) > 1)
            {
                BillIDArray[CustCount - 1] = Int32.Parse(ddBICBillIDCount.SelectedValue);
                BillIDCount = BillIDCount + BillIDArray[CustCount - 1];
                ddBICBillIDCount.SelectedIndex = 0;

                TreeNode node = TreeView1.FindNode(Server.HtmlEncode("CUST" + CustCount));
                for (i = 1; i <= BillIDArray[CustCount - 1]; i++)
                {
                    TvBill = new TreeNode("Bill Account : " + i, "BILL" + i);
                    TvBill.SelectAction = TreeNodeSelectAction.None;
                    node.ChildNodes.Add(TvBill);

                }

                if (CustCount != Int32.Parse(ddBICCustCount.SelectedValue))
                {
                    CustCount++;
                    lblBICBillIDCount.Text = "Select Number of Bill Accounts for <br>'Customer : " + CustCount + "'";
                    ddBICBillIDCount.Focus();
                }
                else
                {
                    ddBICBillIDCount.Enabled = false;
                    ddBICBillIDCount.ForeColor = System.Drawing.Color.DarkGray;
                    btnBICBillIDCountOk.Enabled = false;
                    btnBICBillIDCountOk.ForeColor = System.Drawing.Color.DarkGray;

                    ChargeNumberCount = 1;
                    lblBICChargeBillCount.Visible = true;
                    ddBICChargeBillCount.Visible = true;
                    btnBICChargeBillCountOk.Visible = true;
                    lblBICChargeBillCount.Text = "Select Chargeable Number for <br>'Customer : 1' :: 'Bill Account : " + ChargeNumberCount + "'";

                    ddBICChargeBillCount.Focus(); 
                }
            }            
        }

        protected void btnBICChargeBillCountOk_Click(object sender, EventArgs e)
        {
            TreeNode node;

            if (ddBICChargeBillCount.SelectedIndex == 0)
                return;

            TreeView1.ExpandAll();

            if (j <= CustCount)
            {
                if (ii <= BillIDArray[j - 1])
                {
                    ChargeNumberArray[ChargeNumberCount - 1] = Int32.Parse(ddBICChargeBillCount.SelectedValue);
                    ddBICChargeBillCount.SelectedIndex = 0;
                    node = TreeView1.FindNode(Server.HtmlEncode("CUST" + j + "/BILL" + ii));
                    TvCharge = new TreeNode("Chargeable Number : " + ChargeNumberArray[ChargeNumberCount - 1], "CHARGE" + ChargeNumberCount);
                    TvCharge.SelectAction = TreeNodeSelectAction.None;
                    node.ChildNodes.Add(TvCharge);

                    if (ii == BillIDArray[j - 1])
                    {
                        ii = 0;
                        j++;
                    }
                    if (ChargeNumberCount < BillIDCount)
                    {
                        ii++;
                        ChargeNumberCount++;
                        lblBICChargeBillCount.Text = "Select Chargeable Number for <br>'Customer : " + j + "' :: 'Bill Account : " + ii + "'";
                        ddBICChargeBillCount.Focus();
                    }
                    else
                    {
                        ddBICChargeBillCount.Enabled = false;
                        ddBICChargeBillCount.ForeColor = System.Drawing.Color.DarkGray;
                        btnBICChargeBillCountOk.Enabled = false;
                        btnBICChargeBillCountOk.ForeColor = System.Drawing.Color.DarkGray;

                        btnBICSubmitTree.ForeColor = System.Drawing.Color.Empty;
                        btnBICSubmitTree.Enabled = true;
                        btnBICSubmitTree.Visible = true;
                        btnBICResetTree.Visible = true;

                        btnBICSubmitTree.Focus();
                    }

                }
            }         
        }

        private void resetBICAgreementPanel()
        {
            rbBICAgreementOne.Enabled = true;
            rbBICAgreementOne.Checked = true;
            lblBICAgreementOne.ForeColor = System.Drawing.Color.Empty;
            rbBICAgreeDifferent.Enabled = true;
            lblBICAgreementDifferent.ForeColor = System.Drawing.Color.Empty;

            lblBICAgreementCount.Visible = true;

            ddBICAgreementCount.Visible = true;
            ddBICAgreementCount.SelectedIndex = 0;
            ddBICAgreementCount.ForeColor = System.Drawing.Color.Empty;
            ddBICAgreementCount.Enabled = true;

            btnBICFinalSubmit.ForeColor = System.Drawing.Color.Empty;
            btnBICFinalSubmit.Enabled = true;

            lblBICSubmitResult.Text = "";

            //btnBICAgreementOk.Visible = true;
            //btnBICAgreementOk.ForeColor = System.Drawing.Color.Empty;
            //btnBICAgreementOk.Enabled = true;

            //btnBICFinalSubmit.Visible = false;
            //btnBICFinalReset.Visible = false;
        }

        protected void btnBICSubmitTree_Click(object sender, EventArgs e)
        {
            btnBICSubmitTree.ForeColor = System.Drawing.Color.DarkGray;
            btnBICSubmitTree.Enabled = false;
            //btnBICResetTree.ForeColor = System.Drawing.Color.DarkGray;
            //btnBICResetTree.Enabled = false;

            panelBICAgreement.Visible = true;
            resetBICAgreementPanel();

            ddBICAgreementCount.Focus();
        }

        private void resetBICTree()
        {
            Array.Clear(CustArray, 0, CustArray.Length);
            Array.Clear(BillIDArray, 0, BillIDArray.Length);
            Array.Clear(ChargeNumberArray, 0, ChargeNumberArray.Length);
            CustCount = 0;
            BillIDCount = 0;
            ChargeNumberCount = 0;
            TreeView1.Nodes.Clear();
            j = 1;
            ii = 1;

            ddBICCustCount.Enabled = true;
            ddBICBillIDCount.Enabled = true;
            ddBICChargeBillCount.Enabled = true;

            ddBICCustCount.ForeColor = System.Drawing.Color.Empty;
            ddBICBillIDCount.ForeColor = System.Drawing.Color.Empty;
            ddBICChargeBillCount.ForeColor = System.Drawing.Color.Empty;

            btnBICCustCountOk.Enabled = true;
            btnBICBillIDCountOk.Enabled = true;
            btnBICChargeBillCountOk.Enabled = true;

            btnBICCustCountOk.ForeColor = System.Drawing.Color.Empty;
            btnBICBillIDCountOk.ForeColor = System.Drawing.Color.Empty;
            btnBICChargeBillCountOk.ForeColor = System.Drawing.Color.Empty;

            btnBICSubmitTree.Visible = false;
            btnBICResetTree.Visible = false;

            ddBICCustCount.SelectedIndex = 0;
            ddBICBillIDCount.SelectedIndex = 0;
            ddBICChargeBillCount.SelectedIndex = 0;

            lblBICBillIDCount.Visible = false;
            lblBICChargeBillCount.Visible = false;
            ddBICBillIDCount.Visible = false;
            ddBICChargeBillCount.Visible = false;
            btnBICBillIDCountOk.Visible = false;
            btnBICChargeBillCountOk.Visible = false;

            resetBICAgreementPanel();
            panelBICAgreement.Visible = false;
        }

        protected void btnBICResetTree_Click(object sender, EventArgs e)
        {
            resetBICTree();
            ddBICCustCount.Focus();
        }

        protected void CustomValidator6_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddBICCustCount.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICCustCount.Focus();
            }
        }

        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddBICBillIDCount.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICBillIDCount.Focus();
            }
        }

        protected void CustomValidator8_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddBICChargeBillCount.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICChargeBillCount.Focus();
            }

        }

        //Without ExecuteScaler() btnBICFinalSubmit

        //protected void btnBICFinalSubmit_Click(object sender, EventArgs e)    
        //{
        //    if (!Page.IsValid)
        //        return;

        //    btnBICFinalSubmit.ForeColor = System.Drawing.Color.DarkGray;
        //    btnBICFinalSubmit.Enabled = false;

        //    string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
        //    MySqlConnection con = new MySqlConnection(cs1);

        //    MySqlDataReader dr;
        //    MySqlCommand cmd =new MySqlCommand();
        //    MySqlTransaction tran = null;

        //    con.Open();
        //    tran = con.BeginTransaction();

        //    cmd.Connection = con;
        //    //cmd.Transaction = tran;
        //    try
        //    {
        //        MySqlParameter sp1 = new MySqlParameter("@ddBICLayerCustType", MySqlDbType.VarChar);
        //        sp1.Value = ddBICLayerCustType.SelectedValue;

        //        MySqlParameter sp2 = new MySqlParameter("@ddBICLayerBillCycleMajor", MySqlDbType.VarChar);
        //        sp2.Value = ddBICLayerBillCycleMajor.SelectedValue;

        //        MySqlParameter sp3 = new MySqlParameter("@ddBICLayerCorpPrivate", MySqlDbType.VarChar);
        //        sp3.Value = ddBICLayerCorpPrivate.SelectedValue;

        //        MySqlParameter sp4 = new MySqlParameter("@ddBICLayerEVCenterType", MySqlDbType.VarChar);
        //        //sp4.Value = ddBICLayerEVCenterType.SelectedValue;
        //        sp4.Value = null;   //Sending null as control is disabled.

        //        MySqlParameter sp5 = new MySqlParameter("@ddBICLayerTimSplit", MySqlDbType.VarChar);
        //        sp5.Value = ddBICLayerTimSplit.SelectedValue;

        //        MySqlParameter sp6 = new MySqlParameter("@txtBICLayerOrganisationNo", MySqlDbType.VarChar);
        //        sp6.Value = txtBICLayerOrganisationNo.Text.Trim();

        //        MySqlParameter sp7 = new MySqlParameter("@txtBICLayerBillCycleLen", MySqlDbType.VarChar);
        //        sp7.Value = txtBICLayerBillCycleLen.Text.Trim();

        //        MySqlParameter sp8 = new MySqlParameter("@txtBICLayerBillCycleCode", MySqlDbType.VarChar);
        //        sp8.Value = txtBICLayerBillCycleCode.Text.Trim();

        //        MySqlParameter sp9 = new MySqlParameter("@txtBICLayerBillInd", MySqlDbType.VarChar);
        //        sp9.Value = txtBICLayerBillInd.Text.Trim();

        //        MySqlParameter sp10 = new MySqlParameter("@txtBICLayerANumber", MySqlDbType.VarChar);
        //        sp10.Value = txtBICLayerANumber.Text.Trim();

        //        MySqlParameter sp11 = new MySqlParameter("@txtBICLayerStreetAddress", MySqlDbType.VarChar);
        //        sp11.Value = txtBICLayerStreetAddress.Text.Trim();

        //        MySqlParameter sp12 = new MySqlParameter("@txtBICLayerMailingAddress", MySqlDbType.VarChar);
        //        sp12.Value = txtBICLayerMailingAddress.Text.Trim();

        //        MySqlParameter sp13 = new MySqlParameter("@rbBICAgreementOne", MySqlDbType.VarChar);
        //        if (rbBICAgreementOne.Checked)
        //            sp13.Value = 1;
        //        else
        //            sp13.Value = 2;

        //        MySqlParameter sp14 = new MySqlParameter("@txtBICLayerOptionalText", MySqlDbType.VarChar);
        //        sp14.Value = txtBICLayerOptionalText.Text.Trim();

        //        MySqlParameter sp15 = new MySqlParameter("@CurrentUser", MySqlDbType.VarChar);
        //        sp15.Value = Session["CurrentUser"];

        //        MySqlParameter sp16 = new MySqlParameter("@CurrentDateTime", MySqlDbType.DateTime);
        //        sp16.Value = DateTime.Now.Date;

        //        cmd.Parameters.Add(sp1);
        //        cmd.Parameters.Add(sp2);
        //        cmd.Parameters.Add(sp3);
        //        cmd.Parameters.Add(sp4);
        //        cmd.Parameters.Add(sp5);
        //        cmd.Parameters.Add(sp6);
        //        cmd.Parameters.Add(sp7);
        //        cmd.Parameters.Add(sp8);
        //        cmd.Parameters.Add(sp9);
        //        cmd.Parameters.Add(sp10);
        //        cmd.Parameters.Add(sp11);
        //        cmd.Parameters.Add(sp12);
        //        cmd.Parameters.Add(sp13);
        //        cmd.Parameters.Add(sp14);
        //        cmd.Parameters.Add(sp15);
        //        cmd.Parameters.Add(sp16);

        //        cmd.CommandText = "";
        //        cmd.CommandText = cmd.CommandText + "INSERT INTO `Lager-BIC`";
        //        cmd.CommandText = cmd.CommandText + "(`Kund-typ`, `Agree-typ`, `Prson-org-nm`,";
        //        cmd.CommandText = cmd.CommandText + " `Bill-cycle-code`, `Bill-cycle-dur`, `Bill-cycle-len`,";
        //        cmd.CommandText = cmd.CommandText + " `Bill-ind`, `Bus-res-type`, `Oper-ind`,";
        //        cmd.CommandText = cmd.CommandText + " `Tims-split`,`Line-2-addr`, `Line-3-addr`,";
        //        cmd.CommandText = cmd.CommandText + " `LAN-userid`, `Datum`, `Ben`)";
        //        cmd.CommandText = cmd.CommandText + " VALUES(@ddBICLayerCustType, @rbBICAgreementOne, @txtBICLayerOrganisationNo,";
        //        cmd.CommandText = cmd.CommandText + " @txtBICLayerBillCycleCode, @ddBICLayerBillCycleMajor, @txtBICLayerBillCycleLen,";
        //        cmd.CommandText = cmd.CommandText + " @txtBICLayerBillInd, @ddBICLayerCorpPrivate, @ddBICLayerEVCenterType,";
        //        cmd.CommandText = cmd.CommandText + " @ddBICLayerTimSplit, @txtBICLayerStreetAddress, @txtBICLayerMailingAddress,";
        //        cmd.CommandText = cmd.CommandText + " @CurrentUser, @CurrentDateTime, @txtBICLayerOptionalText)";

        //        cmd.ExecuteNonQuery();

        //        int v_bic_lager_nr = 0, k, l;

        //        cmd.CommandText = "SELECT max(`BIC-lager-nr`) BICnr FROM `Lager-BIC`";

        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            v_bic_lager_nr = dr.GetInt32("BICnr");
        //            dr.Close();
        //        }

        //        //CUSTOMER//
        //        for (k = 0; k < CustCount; k++)
        //        {
        //            cmd.CommandText = "INSERT INTO `Lager-BIC-cust`(`BIC-lager-nr`) VALUES(" + v_bic_lager_nr + ")";
        //            cmd.ExecuteNonQuery();
        //            cmd.CommandText = "SELECT max(`Cust-id`) CustID FROM `Lager-BIC-cust`";
        //            dr = cmd.ExecuteReader();
        //            if (dr.Read())
        //            {
        //                CustArray[k] = dr.GetInt32("CustID");
        //                dr.Close();
        //            }
        //        }

        //        //BILL ENT ID//
        //        for (k = 0; k < CustCount; k++)
        //        {
        //            for (l = 0; l < BillIDArray[k]; l++)
        //            {
        //                cmd.CommandText = "INSERT INTO `Lager-BIC-bill`(`Cust-id`) VALUES(" + CustArray[k] + ")";
        //                cmd.ExecuteNonQuery();
        //                cmd.CommandText = "SELECT max(`Bill-ent-id`) BillEntID FROM `Lager-BIC-bill`";
        //                dr = cmd.ExecuteReader();
        //                if (dr.Read())
        //                {
        //                    BillEntIDArray[k, l] = dr.GetInt32("BillEntID");
        //                    dr.Close();
        //                }
        //            }
        //        }

        //        //CHARGE NUMBER//
        //        for (k = 0; k < CustCount; k++)
        //        {
        //            for (l = 0; l < BillIDArray[k]; l++)
        //            {
        //                cmd.CommandText = "INSERT INTO `Lager-BIC-chrgb`(`Bill-ent-id`, `Anr`) VALUES(" + BillEntIDArray[k, l] + ", " + txtBICLayerANumber.Text.Trim() + ")";
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        //AGREEMENT//
        //        int agreeCount = 0;

        //        if (rbBICAgreementOne.Checked)
        //            agreeCount = Int32.Parse(ddBICAgreementCount.SelectedValue);
        //        else
        //            agreeCount = CustCount;

        //        for (k = 0; k < agreeCount; k++)
        //        {
        //            if (rbBICAgreementOne.Checked)
        //                cmd.CommandText = "INSERT INTO `Lager-BIC-agree`(`BIC-lager-nr`,`Cust-id`) VALUES(" + v_bic_lager_nr + ", " + CustArray[0] + ")";
        //            else
        //                cmd.CommandText = "INSERT INTO `Lager-BIC-agree`(`BIC-lager-nr`,`Cust-id`) VALUES(" + v_bic_lager_nr + ", " + CustArray[k] + ")";

        //            cmd.ExecuteNonQuery();
        //        }

        //        lblBICSubmitResult.Text = "Information Saved Successfully!";

        //        tran.Commit();
        //        cmd.Dispose();
        //    }
        //    catch(MySqlException ex)
        //    {
        //        tran.Rollback();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //} //

        protected void btnBICFinalSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            btnBICFinalSubmit.ForeColor = System.Drawing.Color.DarkGray;
            btnBICFinalSubmit.Enabled = false;

            string cs1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            MySqlConnection con = new MySqlConnection(cs1);

            MySqlCommand cmd = new MySqlCommand();
            MySqlTransaction tran = null;

            con.Open();
            tran = con.BeginTransaction();

            cmd.Connection = con;
            //cmd.Transaction = tran;
            try
            {
                MySqlParameter sp1 = new MySqlParameter("@ddBICLayerCustType", MySqlDbType.VarChar);
                sp1.Value = ddBICLayerCustType.SelectedValue;

                MySqlParameter sp2 = new MySqlParameter("@ddBICLayerBillCycleMajor", MySqlDbType.VarChar);
                sp2.Value = ddBICLayerBillCycleMajor.SelectedValue;

                MySqlParameter sp3 = new MySqlParameter("@ddBICLayerCorpPrivate", MySqlDbType.VarChar);
                sp3.Value = ddBICLayerCorpPrivate.SelectedValue;

                MySqlParameter sp4 = new MySqlParameter("@ddBICLayerEVCenterType", MySqlDbType.VarChar);
                //sp4.Value = ddBICLayerEVCenterType.SelectedValue;
                sp4.Value = null;   //Sending null as control is disabled.

                MySqlParameter sp5 = new MySqlParameter("@ddBICLayerTimSplit", MySqlDbType.VarChar);
                sp5.Value = ddBICLayerTimSplit.SelectedValue;

                MySqlParameter sp6 = new MySqlParameter("@txtBICLayerOrganisationNo", MySqlDbType.VarChar);
                sp6.Value = txtBICLayerOrganisationNo.Text.Trim();

                MySqlParameter sp7 = new MySqlParameter("@txtBICLayerBillCycleLen", MySqlDbType.VarChar);
                sp7.Value = txtBICLayerBillCycleLen.Text.Trim();

                MySqlParameter sp8 = new MySqlParameter("@txtBICLayerBillCycleCode", MySqlDbType.VarChar);
                sp8.Value = txtBICLayerBillCycleCode.Text.Trim();

                MySqlParameter sp9 = new MySqlParameter("@txtBICLayerBillInd", MySqlDbType.VarChar);
                sp9.Value = txtBICLayerBillInd.Text.Trim();

                MySqlParameter sp10 = new MySqlParameter("@txtBICLayerANumber", MySqlDbType.VarChar);
                sp10.Value = txtBICLayerANumber.Text.Trim();

                MySqlParameter sp11 = new MySqlParameter("@txtBICLayerStreetAddress", MySqlDbType.VarChar);
                sp11.Value = txtBICLayerStreetAddress.Text.Trim();

                MySqlParameter sp12 = new MySqlParameter("@txtBICLayerMailingAddress", MySqlDbType.VarChar);
                sp12.Value = txtBICLayerMailingAddress.Text.Trim();

                MySqlParameter sp13 = new MySqlParameter("@rbBICAgreementOne", MySqlDbType.VarChar);
                if (rbBICAgreementOne.Checked)
                    sp13.Value = 1;
                else
                    sp13.Value = 2;

                MySqlParameter sp14 = new MySqlParameter("@txtBICLayerOptionalText", MySqlDbType.VarChar);
                sp14.Value = txtBICLayerOptionalText.Text.Trim();

                MySqlParameter sp15 = new MySqlParameter("@CurrentUser", MySqlDbType.VarChar);
                sp15.Value = Session["CurrentUser"];

                MySqlParameter sp16 = new MySqlParameter("@CurrentDateTime", MySqlDbType.DateTime);
                sp16.Value = DateTime.Now.Date;

                cmd.Parameters.Add(sp1);
                cmd.Parameters.Add(sp2);
                cmd.Parameters.Add(sp3);
                cmd.Parameters.Add(sp4);
                cmd.Parameters.Add(sp5);
                cmd.Parameters.Add(sp6);
                cmd.Parameters.Add(sp7);
                cmd.Parameters.Add(sp8);
                cmd.Parameters.Add(sp9);
                cmd.Parameters.Add(sp10);
                cmd.Parameters.Add(sp11);
                cmd.Parameters.Add(sp12);
                cmd.Parameters.Add(sp13);
                cmd.Parameters.Add(sp14);
                cmd.Parameters.Add(sp15);
                cmd.Parameters.Add(sp16);

                cmd.CommandText = "";
                cmd.CommandText = cmd.CommandText + "INSERT INTO `Lager-BIC`";
                cmd.CommandText = cmd.CommandText + "(`Kund-typ`, `Agree-typ`, `Prson-org-nm`,";
                cmd.CommandText = cmd.CommandText + " `Bill-cycle-code`, `Bill-cycle-dur`, `Bill-cycle-len`,";
                cmd.CommandText = cmd.CommandText + " `Bill-ind`, `Bus-res-type`, `Oper-ind`,";
                cmd.CommandText = cmd.CommandText + " `Tims-split`,`Line-2-addr`, `Line-3-addr`,";
                cmd.CommandText = cmd.CommandText + " `LAN-userid`, `Datum`, `Ben`)";
                cmd.CommandText = cmd.CommandText + " VALUES(@ddBICLayerCustType, @rbBICAgreementOne, @txtBICLayerOrganisationNo,";
                cmd.CommandText = cmd.CommandText + " @txtBICLayerBillCycleCode, @ddBICLayerBillCycleMajor, @txtBICLayerBillCycleLen,";
                cmd.CommandText = cmd.CommandText + " @txtBICLayerBillInd, @ddBICLayerCorpPrivate, @ddBICLayerEVCenterType,";
                cmd.CommandText = cmd.CommandText + " @ddBICLayerTimSplit, @txtBICLayerStreetAddress, @txtBICLayerMailingAddress,";
                cmd.CommandText = cmd.CommandText + " @CurrentUser, @CurrentDateTime, @txtBICLayerOptionalText);";

                cmd.ExecuteNonQuery();

                int v_bic_lager_nr, k, l;
                v_bic_lager_nr = (Int32)cmd.LastInsertedId;

                //CUSTOMER//
                for (k = 0; k < CustCount; k++)
                {
                    cmd.CommandText = "INSERT INTO `Lager-BIC-cust`(`BIC-lager-nr`) VALUES(" + v_bic_lager_nr + ")";
                    cmd.ExecuteNonQuery();
                    CustArray[k] = (Int32)cmd.LastInsertedId;
                }

                //BILL ENT ID//
                for (k = 0; k < CustCount; k++)
                {
                    for (l = 0; l < BillIDArray[k]; l++)
                    {
                        cmd.CommandText = "INSERT INTO `Lager-BIC-bill`(`Cust-id`) VALUES(" + CustArray[k] + ")";
                        cmd.ExecuteNonQuery();
                        BillEntIDArray[k, l] = (Int32)cmd.LastInsertedId; ;
                    }
                }

                //CHARGE NUMBER//
                for (k = 0; k < CustCount; k++)
                {
                    for (l = 0; l < BillIDArray[k]; l++)
                    {
                        cmd.CommandText = "INSERT INTO `Lager-BIC-chrgb`(`Bill-ent-id`, `Anr`) VALUES(" + BillEntIDArray[k, l] + ", " + txtBICLayerANumber.Text.Trim() + ")";
                        cmd.ExecuteNonQuery();
                    }
                }

                //AGREEMENT//
                int agreeCount = 0;

                if (rbBICAgreementOne.Checked)
                    agreeCount = Int32.Parse(ddBICAgreementCount.SelectedValue);
                else
                    agreeCount = CustCount;

                for (k = 0; k < agreeCount; k++)
                {
                    if (rbBICAgreementOne.Checked)
                        cmd.CommandText = "INSERT INTO `Lager-BIC-agree`(`BIC-lager-nr`,`Cust-id`) VALUES(" + v_bic_lager_nr + ", " + CustArray[0] + ")";
                    else
                        cmd.CommandText = "INSERT INTO `Lager-BIC-agree`(`BIC-lager-nr`,`Cust-id`) VALUES(" + v_bic_lager_nr + ", " + CustArray[k] + ")";

                    cmd.ExecuteNonQuery();
                }

                lblBICSubmitResult.Text = "Information Saved Successfully!";

                tran.Commit();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                tran.Rollback();
            }
            finally
            {
                con.Close();
            }
        }

        protected void rbBICAgreementOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBICAgreementOne.Checked)
            {
                lblBICAgreementCount.Visible = true;
                ddBICAgreementCount.SelectedIndex = 0;
                ddBICAgreementCount.Visible = true;
                //btnBICAgreementOk.Visible = true;
            }
            else
            {
                lblBICAgreementCount.Visible = false;
                ddBICAgreementCount.Visible = false;
                //btnBICAgreementOk.Visible = false;
            }
        }

        protected void rbBICAgreeDifferent_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBICAgreeDifferent.Checked)
            {
                lblBICAgreementCount.Visible = false;
                ddBICAgreementCount.Visible = false;
                //btnBICAgreementOk.Visible = false;
            }
            else
            {
                lblBICAgreementCount.Visible = true;
                ddBICAgreementCount.SelectedIndex = 0;
                ddBICAgreementCount.Visible = true;
                //btnBICAgreementOk.Visible = true;
            }
        }

        //protected void btnBICAgreementOk_Click(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        //rbBICAgreementOne.Enabled = false;
        //        //lblBICAgreementOne.ForeColor = System.Drawing.Color.DarkGray;
        //        //rbBICAgreeDifferent.Enabled = false;
        //        //lblBICAgreementDifferent.ForeColor = System.Drawing.Color.DarkGray;

        //        //ddBICAgreementCount.ForeColor = System.Drawing.Color.DarkGray;
        //        //ddBICAgreementCount.Enabled = false;

        //        //btnBICAgreementOk.ForeColor = System.Drawing.Color.DarkGray;
        //        //btnBICAgreementOk.Enabled = false;

        //        //btnBICFinalSubmit.Visible = true;
        //        //btnBICFinalReset.Visible = true;
        //        btnBICFinalSubmit.Focus();
        //    }
        //}

        protected void btnBICFinalReset_Click(object sender, EventArgs e)
        {
            resetBICAgreementPanel();
            resetBICTree();
            resetBICHome();
            MultiView1.ActiveViewIndex = 0;
            ddBICLayerCustType.Focus();
        }

        protected void CustomValidator9_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rbBICAgreementOne.Checked && ddBICAgreementCount.SelectedIndex == 0)
            {
                args.IsValid = false;
                ddBICAgreementCount.Focus();
            }
            else
                args.IsValid = true;
        }
    }
}

