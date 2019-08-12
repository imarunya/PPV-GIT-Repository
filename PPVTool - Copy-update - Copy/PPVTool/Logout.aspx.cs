using System;

namespace PPVTool
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session.Abandon();
            Response.Redirect("Login.aspx");
            //DataTable dt = new DataTable();
            //String myConn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            //MySqlConnection conn = new MySqlConnection(myConn);
            //try
            //{
            //    string sel = "SELECT * FROM ppvdataprod.`indata-bic-05` LIMIT 50;";
            //    conn.Open();
            //    MySqlDataAdapter mda = new MySqlDataAdapter(sel, conn);
            //    mda.Fill(dt);
            //    conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }
            //}
        }
    }
}