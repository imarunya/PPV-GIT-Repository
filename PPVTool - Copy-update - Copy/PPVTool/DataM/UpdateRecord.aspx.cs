using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool.DataM
{
    public partial class UpdateRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUpRelease();
            }
        }

        private void BindUpRelease()
        {
            string sql = " Select * from styrtabell where Status1= 1 ";
            DatabaseConnection dc = new DatabaseConnection();
            grdTest.DataSource = dc.GetByDataTable(sql);
            grdTest.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {

            foreach (GridViewRow gvr in grdTest.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdTest, String.Concat("Select$", gvr.RowIndex), true);
            }

            base.Render(writer);
        }

        protected void grdTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdTest.SelectedRow != null)
            {
                hfTestNo.Value = Server.HtmlDecode(grdTest.SelectedRow.Cells[0].Text);
                txtTestNo.Text = Server.HtmlDecode(grdTest.SelectedRow.Cells[0].Text);
                hfDBLpnr.Value = Server.HtmlDecode(grdTest.SelectedRow.Cells[1].Text);
            }
            else
            {
                txtTestNo.Text = "";
                hfDBLpnr.Value = "";
                hfTestNo.Value = "";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnection dc = new DatabaseConnection();

                if (dc.OpenTestForWrite(Convert.ToInt32(hfTestNo.Value), Convert.ToInt32(hfDBLpnr.Value)))
                {
                    throw new Exception("Test no open for file creation.");
                }
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
            }
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
                if (fuFileUpload.HasFile)
                {

                    string filepath = "C:\\PPV2\\" + fuFileUpload.FileName;
                    fuFileUpload.SaveAs(filepath);
                }
                else
                {
                    throw new Exception("Please select a file.");
                }
            }
            catch (Exception ex)
            {
                Show(ex.Message, this);
            }
        }
    }
}