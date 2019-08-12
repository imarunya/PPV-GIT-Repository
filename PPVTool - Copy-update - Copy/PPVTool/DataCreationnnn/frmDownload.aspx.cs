using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPVTool.DataCreation
{
    public partial class frmDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string fileName = Request.QueryString["filename"].ToString();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.TransmitFile(Server.MapPath("~/DownloadFiles/" + fileName));
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}