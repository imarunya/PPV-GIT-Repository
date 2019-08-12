using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                DatabaseConnection dc = new DatabaseConnection();

                FriendGridView.DataSource = dc.GetCDRLayer(); ;
                FriendGridView.DataBind();
            }
        }

        //protected void FriendGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType != DataControlRowType.DataRow)
        //        return;

        //    e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';"
        //    + "this.originalBackgroundColor=this.style.backgroundColor;"
        //    + "this.style.backgroundColor='#bbbbbb';";

        //    e.Row.Attributes["onmouseout"] =
        //    "this.style.backgroundColor=this.originalBackgroundColor;";

        //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(FriendGridView, String.Concat("Select$", e.Row.RowIndex));
        //}

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gvr in FriendGridView.Rows)
            {
                gvr.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(FriendGridView, String.Concat("Select$", gvr.RowIndex),true);
            }

            base.Render(writer);
        }

        protected void FriendGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // assign firstname
            if (FriendGridView.SelectedRow != null)
                txtFriend.Text = Server.HtmlDecode(
                  FriendGridView.SelectedRow.Cells[1].Text);
            else
                txtFriend.Text = "";
        }

        //protected void FriendGridView_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // assign firstname
        //    if (FriendGridView.SelectedRow != null)
        //        txtFriend.Text = Server.HtmlDecode(
        //          FriendGridView.SelectedRow.Cells[1].Text);
        //    else
        //        txtFriend.Text = "";
        //}
    }
}