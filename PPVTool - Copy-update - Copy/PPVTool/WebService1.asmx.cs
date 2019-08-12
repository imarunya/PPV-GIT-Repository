using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data.MySqlClient;

namespace PPVTool
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<string> PlanOptionAutoComplete(string prefixText, int count)
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand com = new MySqlCommand())
                {
                    com.CommandText = "SELECT `Plan-Option` FROM `lager-planoption` WHERE `Plan-Option` like '%" + prefixText + "%'";

                    //com.Parameters.AddWithValue("@Search", prefixText);
                    com.Connection = con;
                    con.Open();
                    List<string> planoption = new List<string>();
                    using (MySqlDataReader sdr = com.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            planoption.Add(sdr["Plan-Option"].ToString());
                        }
                    }
                    con.Close();
                    return planoption;
                }
            }
        }

        [WebMethod]
        public List<string> CallCategoryAutoComplete(string prefixText, int count)
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand com = new MySqlCommand())
                {
                    com.CommandText = "SELECT `CAT-cod` FROM `Lager-CCTB` WHERE `CAT-cod` like '%" + prefixText + "%'";

                    //com.Parameters.AddWithValue("@Search", prefixText);
                    com.Connection = con;
                    con.Open();
                    List<string> catcod = new List<string>();
                    using (MySqlDataReader sdr = com.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            catcod.Add(sdr["CAT-cod"].ToString());
                        }
                    }
                    con.Close();
                    return catcod;
                }
            }
        }

        [WebMethod]
        public List<string> ShowDatabaseAutoComplete(string prefixText, int count)
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand com = new MySqlCommand())
                {
                    if (prefixText.EndsWith("?"))
                        com.CommandText = "SELECT table_name FROM information_schema.tables where table_schema='ppvdataprod'";
                    else
                        com.CommandText = "SELECT table_name FROM information_schema.tables where table_schema='ppvdataprod' and table_name like '%" + prefixText + "%'";

                    com.Connection = con;
                    con.Open();
                    List<string> tablename = new List<string>();
                    using (MySqlDataReader sdr = com.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            tablename.Add(sdr["table_name"].ToString() + "`");
                        }
                    }
                    con.Close();
                    return tablename;
                }
            }
        }

    }
}
