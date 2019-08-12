using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
public class AdminDB
{

    string myConn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
    MySqlConnection conn = null;

    public DataTable GetDataByDataTable(string sql)
    {
        DataTable dt = new DataTable();
        try
        {
            conn = new MySqlConnection(myConn);
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = new MySqlCommand(sql, conn);
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        return dt;
    }
}
