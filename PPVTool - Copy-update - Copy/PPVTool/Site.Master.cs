using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace PPVTool
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentUser"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        string pc_dir = "~/DownloadFiles/";
        

        protected void uploadMainframe_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.UseShellExecute = false;
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = Server.MapPath(pc_dir + "Upload.bat");
                //proc.StartInfo.FileName = Server.MapPath(pc_dir + "test.bat");
                //proc.StartInfo.Verb = "runas";
                proc.Start();
                proc.Close();
                labelerror.Text = "File(s) uploaded succesfully!";
            }
            catch (Exception ex)
            {
                labelerror.Text = ex.Message;

            }
        }

        protected void DownloadMainframe_Click(object sender, EventArgs e)
        {
            string pc_fil = Server.MapPath(pc_dir + "Download.bat");
            var lines = File.ReadAllLines(pc_fil);

            try
            {
                System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
                proc.FileName = @"C:\DownloadFiles\Download.bat";
               // proc.Arguments = @"C:\DownloadFiles\Download.bat";
                System.Diagnostics.Process.Start(proc);
                labelerror.Text = "File(s) downloaded succesfully!";
            }
            catch (Exception ex)
            {
                labelerror.Text = ex.Message;
            }
        }


    }

}