using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool
{
    public partial class PrintMaintRefData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //    txtQueryInput.Focus();
            //}
        }

        protected void btnQueryRun_Click(object sender, EventArgs e)
        {
            AdminDB db = new AdminDB();
            //string sql = "SELECT Prisplan, Ben, `PP-lpnr` as PPlpnr FROM `lager-prisplan` ORDER BY Prisplan";
            string sql = txtQueryInput.Text;
            if (sql.Trim() != "")
            {
                if (db.GetDataByDataTable(sql) != null)
                {
                    gvQueryResult.Visible = true;
                    gvQueryResult.DataSource = db.GetDataByDataTable(sql);
                    gvQueryResult.DataBind();
                    lblQueryInvalid.Text = "";

                    // Header
                    //gvQueryResultHeader.Visible = true;
                    //gvQueryResultHeader.DataSource = db.GetDataByDataTable(sql);
                    //gvQueryResultHeader.DataBind();
                    
                }
                else
                {
                    gvQueryResult.Visible = false;
                    lblQueryInvalid.Text = "Invalid Query";
                    txtQueryInput.Focus();
                }
            }
        }

        protected void btnQueryReset_Click(object sender, EventArgs e)
        {
            txtQueryInput.Text = "";
            gvQueryResult.Visible = false;
            lblQueryInvalid.Text = "";
            txtQueryInput.Focus();
        }

        //protected void txtQueryInput_CompleteQuote()
        //{
        //    txtQueryInput.Text = txtQueryInput.Text.Trim() + "`";
        //}
    }
}