using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data;

namespace PPVTool.PrintData
{
    public partial class test_BURC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uprls, hfMskn, miljo;
            int wcdrnr, wstyr, p_up_lpnr, p_db2_lpnr;


            if (!Page.IsPostBack)
            {
                miljo = Convert.ToString(Session["miljo"]);
                hfMskn = Convert.ToString(Session["hfMskn"]);
                wcdrnr = Convert.ToInt32(Session["wcdrnr"]);
                wstyr = Convert.ToInt32(Session["wstyr"]);
                p_up_lpnr = Convert.ToInt32(Session["Uplpnr"]);
                p_db2_lpnr = Convert.ToInt32(Session["p_db2_lpnr"]);
                uprls = Convert.ToString(Session["uprls"]);

                DatabaseConnection dc = new DatabaseConnection();
                GridView1.DataSource = dc.TestBURC(wstyr);
                GridView1.DataBind();
                string imsSystem = dc.imsValue(wcdrnr, wstyr);

                imsSystem = dc.imsValue(wcdrnr, wstyr);
                string[,] row1 = { { wstyr.ToString(), uprls.ToString(), hfMskn.ToString(), imsSystem.ToString(), miljo.ToString(), wcdrnr.ToString() } };
                int ik = row1.GetUpperBound(0);
                DataTable table = new DataTable();
                table.Columns.Add("TestNR", typeof(string));
                table.Columns.Add("UP-release", typeof(string));
                table.Columns.Add("Maskin", typeof(string));
                table.Columns.Add("IMS System", typeof(string));
                table.Columns.Add("Miljo", typeof(string));
                table.Columns.Add("CDRpaket", typeof(string));
                for (int i = 0; i <= row1.GetUpperBound(0); i++)
                {
                    table.Rows.Add();
                    table.Rows[i]["TestNR"] = row1[i, 0];
                    table.Rows[i]["UP-release"] = row1[i, 1];
                    table.Rows[i]["Maskin"] = row1[i, 2];
                    table.Rows[i]["IMS System"] = row1[i, 3];
                    table.Rows[i]["Miljo"] = row1[i, 4];
                    table.Rows[i]["CDRpaket"] = row1[i, 5];

                }

                GridView2.DataSource = table;
                GridView2.DataBind();


            }
        }

        //protected void btnPDF_Click(object sender, EventArgs e)
        //{
        //    PdfPTable pdfTable = new PdfPTable(GridView1.Columns.Count);

        //    foreach (TableCell headerCell in GridView1.HeaderRow.Cells)
        //    {
        //        Font font = new Font();
        //        font.Color = new BaseColor(GridView1.HeaderStyle.ForeColor);
        //        PdfPCell pdfCell = new PdfPCell(new Phrase(headerCell.Text, font));
        //        pdfCell.BackgroundColor = new BaseColor(GridView1.HeaderStyle.BackColor);
        //        pdfTable.AddCell(pdfCell);
        //    }


        //    foreach (GridViewRow gridViewRow in GridView1.Rows)
        //    {
        //        foreach (TableCell tableCell in gridViewRow.Cells)
        //        {
        //            Font font = new Font();
        //            font.Color = new BaseColor(GridView1.RowStyle.ForeColor);

        //            PdfPCell pdfCell = new PdfPCell(new Phrase(tableCell.Text));
        //            pdfCell.BackgroundColor = new BaseColor(GridView1.RowStyle.BackColor);
        //            pdfTable.AddCell(pdfCell);
        //        }
        //    }

        //    Document pdfDocument = new Document(PageSize.B4, 10f, 10f, 10f, 10f);
        //    PdfWriter.GetInstance(pdfDocument, Response.OutputStream);

        //    pdfDocument.Open();
        //    pdfDocument.Add(pdfTable);
        //    pdfDocument.Close();

        //    Response.ContentType = "Applcaition/pdf";
        //    Response.AppendHeader("Contetnt-disposition", "attachement;filename= testReport.pdf");
        //    Response.Write(pdfDocument);
        //    Response.Flush();
        //    Response.End();
        //}



    }
}