using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public class DatabaseConnection
{
    string myConn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
    MySqlConnection conn = null;

    public bool ValidateUser(string userName, string password)
    {

        string sql = "SELECT `LAN-userid` FROM `lan-tabell` Where `LAN-userid` = '" + userName + "' and `LAN-pwd` = '" + password + "';";
        string sql1 = "SELECT `LAN-pwd` FROM `lan-tabell` Where `LAN-userid` = '" + userName + "' and `LAN-pwd` = '" + password + "';";
        string userId = string.Empty;
        string PwdId = string.Empty;
        bool result = false;

        try
        {
            conn = new MySqlConnection(myConn);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            MySqlCommand cmd1 = new MySqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = sql1;
            userId = Convert.ToString(cmd.ExecuteScalar());
            PwdId = Convert.ToString(cmd1.ExecuteScalar());
            int i = (PwdId.CompareTo(password));

            if ((userId != "") && (i != -1))
            {
                result = true;
            }
           
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

        return result;
    }

    public DataTable GetByDataTable(string sql)
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

    public DataTable GetByDataTable(string sql, int param, string variable)
    {
        var dt = new DataTable();

        try
        {
            conn = new MySqlConnection(myConn);
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = new MySqlCommand(sql, conn);
            da.SelectCommand.Parameters.AddWithValue(variable, param);
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

    public int Insert_Styrtabell_Table(InitiateTest it, List<PricePlan> pp)
    {

        MySqlTransaction trans = null;

        StringBuilder Insert_Sql = new StringBuilder();
        Insert_Sql.Append("Insert into `Styrtabell` (`Styr-lpnr`,`UP-lpnr`,`DB2-lpnr`,Regrkod,`Regr-test`,Status1,Status2,Init,`Antal-fel`,`Word-dok`,`LAN-userid`,Datum,Ben) Values(@Styrlpnr, @UPlpnr, @DB2lpnr, @Regrkod, @Regrtest, @Status1, @Status2, @Init, @Antalfel, @Worddok, @LANuserid, Now(), @Ben); ");

        if (pp.Count > 0)
        {
            for (int i = 0; i < pp.Count; i++)
            {
                Insert_Sql.Append("Insert into `Prisplan-urval` Values (" + pp[i].Lpnr + "," + pp[i].PPLpnr + ");");
            }
        }

        int testCase = 0;

        try
        {
            conn = new MySqlConnection(myConn);
            conn.Open();
            trans = conn.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = Insert_Sql.ToString();
            cmd.Parameters.AddWithValue("@Styrlpnr", it.Styrlpnr);
            cmd.Parameters.AddWithValue("@UPlpnr", it.UPlpnr);
            cmd.Parameters.AddWithValue("@DB2lpnr", it.DB2lpnr);
            cmd.Parameters.AddWithValue("@Regrkod", "N");
            cmd.Parameters.AddWithValue("@Regrtest", 0);
            cmd.Parameters.AddWithValue("@Status1", "0");
            cmd.Parameters.AddWithValue("@Status2", "0");
            cmd.Parameters.AddWithValue("@Init", "");
            cmd.Parameters.AddWithValue("@Antalfel", "0");
            cmd.Parameters.AddWithValue("@Worddok", "");
            cmd.Parameters.AddWithValue("@LANuserid", it.UserId);
            cmd.Parameters.AddWithValue("@Ben", it.Ben);

            if (cmd.ExecuteNonQuery() > 0)
            {
                testCase = it.Styrlpnr;
                trans.Commit();
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            throw ex;
        }

        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        return testCase;
    }

    public string ExceuteSclare(string sql)
    {
        string result = string.Empty;

        try
        {
            conn = new MySqlConnection(myConn);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            result = Convert.ToString(cmd.ExecuteScalar());
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

        return result;
    }

    public int Insert_NyttBICnr(string typ, int customer, int BICnr, string username, ref MySqlTransaction tx, ref MySqlConnection con)
    {
        StringBuilder insert = new StringBuilder();

        int bicnr = 0;

        try
        {
            insert.Append("Insert into `Nytt-BICnr` (BICnr, `BIC-lager-nr`, Typ, Status, `LAN-userid`, Datum) Values (@BICnr,@BIClager,@Typ,'1',@userid,Now()); ");
            con = new MySqlConnection(myConn);
            con.Open();
            tx = con.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = insert.ToString();
            cmd.Parameters.AddWithValue("@BIClager", customer);
            cmd.Parameters.AddWithValue("@Typ", typ);
            cmd.Parameters.AddWithValue("@userid", username);
            cmd.Parameters.AddWithValue("@BICnr", BICnr);

            if (cmd.ExecuteNonQuery() > 0)
            {
                bicnr = BICnr;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

        return bicnr;
    }

    public bool Kopplad_BIC(int Styrlpnr, int BICnr, string Owner, MySqlTransaction tx, MySqlConnection con)
    {
        StringBuilder insert = new StringBuilder();

        bool result = false;

        try
        {
            insert.Append("Insert into `Kopplad-BIC` (`Styr-lpnr`, BICnr, Owner) Values (@Styrlpnr,@BICnr,@Owner); ");

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = insert.ToString();
            cmd.Parameters.AddWithValue("@Styrlpnr", Styrlpnr);
            cmd.Parameters.AddWithValue("@BICnr", BICnr);
            cmd.Parameters.AddWithValue("@Owner", Owner);

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }


        return result;
    }

    public bool Insert_BICAgree(ClsBICAccess bicaccess, MySqlTransaction tx, MySqlConnection con)
    {
        StringBuilder insert = new StringBuilder();

        bool result = false;

        try
        {
            insert.Append(" Insert into `Indata-BIC-agree` (BICnr, Lpnr, `Agree-id`, `Cust-id`, Prisplan, `Plan-unit-id`, `Dur-allw-sec`, `PP-lpnr`) ");
            insert.Append(" Values (@BICnr, @Lpnr, @Agreeid, @Custid, @Prisplan, @Planunit, null, @PPlpnr)");

            //conn = new MySqlConnection(myConn);
            //conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = insert.ToString();
            cmd.Parameters.AddWithValue("@BICnr", bicaccess.BICnr);
            cmd.Parameters.AddWithValue("@Lpnr", bicaccess.Lpnr);
            cmd.Parameters.AddWithValue("@Agreeid", bicaccess.AgreementId);
            cmd.Parameters.AddWithValue("@Custid", bicaccess.CustId);
            cmd.Parameters.AddWithValue("@Prisplan", bicaccess.Priceplan);
            cmd.Parameters.AddWithValue("@Planunit", bicaccess.Planunit);
            cmd.Parameters.AddWithValue("@PPlpnr", bicaccess.PPLpnr);

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }


        return result;
    }

    public bool Insert_BIC05(int bicnr, int lpnr, DateTime custStartDt, DateTime custEndDt, MySqlTransaction tx, MySqlConnection con)
    {
        StringBuilder insert = new StringBuilder();

        bool result = false;

        try
        {
            insert.Append(" Insert into `Indata-BIC-05` (BICnr, Lpnr,  `Cust-id`, `Start-datum`, `Stopp-datum`) ");
            insert.Append(" Values (@BICnr,@Lpnr,0,@startdt,@enddt)");

            //conn = new MySqlConnection(myConn);
            //conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = insert.ToString();
            cmd.Parameters.AddWithValue("@BICnr", bicnr);
            cmd.Parameters.AddWithValue("@Lpnr", lpnr);
            cmd.Parameters.AddWithValue("@startdt", custStartDt);
            cmd.Parameters.AddWithValue("@enddt", custEndDt);

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

        return result;
    }

    public bool Insert_BIC10(int bicnr, int lpnr, DateTime billStartDt, DateTime billEndDt, MySqlTransaction tx, MySqlConnection con)
    {
        StringBuilder insert = new StringBuilder();

        bool result = false;

        try
        {
            insert.Append(" Insert into `Indata-BIC-10` (BICnr, Lpnr, `Bill-ent-id`, `Start-datum`, `Stopp-datum`) ");
            insert.Append(" Values (@BICnr,@Lpnr,0,@startdt,@enddt)");

            //conn = new MySqlConnection(myConn);
            //conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = insert.ToString();
            cmd.Parameters.AddWithValue("@BICnr", bicnr);
            cmd.Parameters.AddWithValue("@Lpnr", lpnr);
            cmd.Parameters.AddWithValue("@startdt", billStartDt);
            cmd.Parameters.AddWithValue("@enddt", billEndDt);

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }


        return result;
    }

    public bool InsertCountryData(GridView grd, DateTime startDt, DateTime endDt, string post, string option)
    {
       

        bool result = false;

        try
        {         

            foreach (GridViewRow rw in grd.Rows)
            {
                StringBuilder insert = new StringBuilder();
                StringBuilder subIns = new StringBuilder();

                insert.Append(" Insert into `Indata-BIC-20-ny` (Lpnr,Post, `Start-datum`, `Stopp-datum`) ");
                insert.Append(" Values (@lpnr,@post,@stdt,@enddt); SELECT LAST_INSERT_ID();");

                int LstId;

                conn = new MySqlConnection(myConn);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = insert.ToString();
                cmd.Parameters.AddWithValue("@lpnr", Convert.ToInt32(rw.Cells[0].Text));
                cmd.Parameters.AddWithValue("@post", post);
                cmd.Parameters.AddWithValue("@stdt", startDt);
                cmd.Parameters.AddWithValue("@enddt", endDt);

                LstId = Convert.ToInt32(cmd.ExecuteScalar());

                if (option == "P")
                {
                    subIns.Append("Insert into `Indata-BIC-30-ny` (Underlpnr,`Optn-trm-val`) Values (@Underlpnr,@optntrmval);");
                    cmd.CommandText = subIns.ToString();
                    cmd.Parameters.AddWithValue("@Underlpnr", LstId);
                    cmd.Parameters.AddWithValue("@optntrmval", rw.Cells[5].Text);
                }

                if (option == "L")
                {
                    subIns.Append("Insert into `Indata-BIC-35-ny` (Underlpnr,`Sel-area-val`,`Area-type-ind`) Values (@Underlpnr,@Selareaval,'T');");
                    cmd.CommandText = subIns.ToString();
                    cmd.Parameters.AddWithValue("@Underlpnr", LstId);
                    cmd.Parameters.AddWithValue("@Selareaval", rw.Cells[5].Text);
                }

                if (option == "T")
                {
                    subIns.Append("Insert into `Indata-BIC-45-ny` (Underlpnr,`Begin-point-val`,`End-point-val`) Values (@Underlpnr,@beginpoint,@endpoint);");
                    cmd.CommandText = subIns.ToString();
                    cmd.Parameters.AddWithValue("@Underlpnr", LstId);
                    cmd.Parameters.AddWithValue("@beginpoint", rw.Cells[5].Text);
                    cmd.Parameters.AddWithValue("@endpoint", rw.Cells[6].Text);
                }

                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }

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

        return result;
    }

    public bool UpdateBICAgree(GridView grd, int bicnr)
    {
        bool result = false;
        StringBuilder update = new StringBuilder();

        try
        {
            foreach (GridViewRow rw in grd.Rows)
            {
                update.Append("Update `Indata-BIC-agree` SET `Dur-allw-sec`= @DurAllow Where BICnr = @BICnr and Lpnr = @lpnr");

                conn = new MySqlConnection(myConn);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = update.ToString();
                cmd.Parameters.AddWithValue("@BICnr", bicnr);
                cmd.Parameters.AddWithValue("@lpnr", Convert.ToInt32(rw.Cells[0].Text));
                cmd.Parameters.AddWithValue("@DurAllow", rw.Cells[5].Text);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }

            }
        }
        catch
        {

        }

        return result;
    }

    public DataTable GetCustomerFrom(int Customer)
    {
        StringBuilder s = new StringBuilder();

        s.Append(" SELECT DISTINCT `Lager-BIC-cust`.`Cust-id`, `Lager-BIC-bill`.`Bill-ent-id`,  `Lager-BIC-chrgb`.`Chrgb-num-id`, `Lager-BIC-chrgb`.`Anr` ");
        s.Append(" FROM `Lager-BIC-cust` INNER JOIN(`Lager-BIC-bill`  INNER JOIN `Lager-BIC-chrgb`  ON `Lager-BIC-bill`.`Bill-ent-id` = `Lager-BIC-chrgb`.`Bill-ent-id`) ");
        s.Append(" ON `Lager-BIC-cust`.`Cust-id` = `Lager-BIC-bill`.`Cust-id` WHERE(((`Lager-BIC-cust`.`Cust-id`) =`lager-bic-bill`.`cust-id`) ");
        s.Append(" AND((`Lager-BIC-bill`.`Bill-ent-id`) =`lager-bic-chrgb`.`bill-ent-id`) ");
        s.Append(" AND((`Lager-BIC-cust`.`BIC-lager-nr`) = @BIClgrNo)); ");

        DataTable dt = GetByDataTable(s.ToString(), Customer, "@BIClgrNo");

        return dt;

    }


    public DataTable GetAgeementByBICnr(int bicnr)
    {
        StringBuilder s = new StringBuilder();

        s.Append(" SELECT DISTINCT `Indata-BIC-agree`.`Cust-id`,`Indata-BIC-agree`.`Agree-id`, `Indata-BIC-agree`.Prisplan, `Indata-BIC-agree`.`PP-lpnr` ");
        s.Append(" FROM `Nytt-BICnr` INNER JOIN `Indata-BIC-agree` ON `Nytt-BICnr`.BICnr = `Indata-BIC-agree`.BICnr ");
        s.Append(" WHERE(((`Nytt-BICnr`.BICnr) = @BICNo) AND((`Indata-BIC-agree`.BICnr) =`Nytt-BICnr`.`bicnr`)) ");
        DataTable dt = GetByDataTable(s.ToString(), bicnr, "@BICNo");

        return dt;
    }

    public DataTable GetCDRLayer()
    {
        StringBuilder s = new StringBuilder();

        s.Append(" SELECT `CDR-lager-nr`, Servid, Calltype, Asoc, Ben  FROM `Lager-CDR`  ORDER BY `Lager-CDR`.Servid, `Lager-CDR`.Calltype ");

        DataTable dt = GetByDataTable(s.ToString());

        return dt;
    }


    public bool InsertCDRData(List<CDR> cdr)
    {

        bool result = false;

        try
        {

            string delete = "delete from `cdr-temp`;";

            int counter = 0;

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = delete;

            foreach (CDR cd in cdr)
            {
                StringBuilder insert = new StringBuilder();

                insert.Append(" Insert into `cdr-temp` (Lpnr,`Rec-ind`, `Chrgb-num-id`,`CDR-lager-nr`,Calltype,Datum,Tid,Samtalslngd,Belopp,`B-nr`,`C-nr`,`Intwk-serv-id`,Kval,`CDR-Asoc`,Ben) ");
                insert.Append(" Values (@Lpnr" + counter.ToString() + ", @Recind" + counter.ToString() + ", @Chrgbnumid" + counter.ToString() + ",@CDRlagernr" + counter.ToString() + ",");
                insert.Append(" @Calltype" + counter.ToString() + ",@Datum" + counter.ToString() + ",@Tid" + counter.ToString() + ",@Samtalslngd" + counter.ToString() + ",@Belopp" + counter.ToString() + ",");
                insert.Append(" @Bnr" + counter.ToString() + ",@Cnr" + counter.ToString() + ",@Intwkservid" + counter.ToString() + ",@Kval" + counter.ToString() + ",@CDRAsoc" + counter.ToString() + ",@Ben" + counter.ToString() + "); ");

                cmd.CommandText += insert.ToString();
                cmd.Parameters.AddWithValue("@lpnr" + counter.ToString(), cd.Lpnr);
                cmd.Parameters.AddWithValue("@Recind" + counter.ToString(), cd.Rec);
                cmd.Parameters.AddWithValue("@Chrgbnumid" + counter.ToString(), cd.Chrgno);
                cmd.Parameters.AddWithValue("@CDRlagernr" + counter.ToString(), cd.CDRNo);
                cmd.Parameters.AddWithValue("@Calltype" + counter.ToString(), cd.Servid_Calltype);
                cmd.Parameters.AddWithValue("@Datum" + counter.ToString(), cd.Date);
                cmd.Parameters.AddWithValue("@Tid" + counter.ToString(), cd.Time);
                cmd.Parameters.AddWithValue("@Samtalslngd" + counter.ToString(), cd.CallDuration);
                cmd.Parameters.AddWithValue("@Belopp" + counter.ToString(), cd.Amount);
                cmd.Parameters.AddWithValue("@Bnr" + counter.ToString(), cd.BNo);
                cmd.Parameters.AddWithValue("@Cnr" + counter.ToString(), cd.CNo);
                cmd.Parameters.AddWithValue("@Intwkservid" + counter.ToString(), cd.ServiceIdentifier);
                cmd.Parameters.AddWithValue("@Kval" + counter.ToString(), cd.QualifyingInd);
                cmd.Parameters.AddWithValue("@CDRAsoc" + counter.ToString(), cd.Asoc);
                cmd.Parameters.AddWithValue("@Ben" + counter.ToString(), cd.Ben);

                counter = counter + 1;

            }

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;
            if (cmd.ExecuteNonQuery() > 0)
            {
                return result = true;
            }
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

        return result;
    }


    public int upd_CDRnr(string wtyp, string username)
    {
        int p_CDRnr = 0;

        try
        {
            string insrt = "Insert into `Nytt-CDRnr`(Typ,`Status`,`LAN-userid`,Datum) Values (@typ,'1',@username,Now());SELECT LAST_INSERT_ID();";

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = insrt;

            cmd.Parameters.AddWithValue("@typ", wtyp);
            cmd.Parameters.AddWithValue("@username", username);

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            p_CDRnr = Convert.ToInt32(cmd.ExecuteScalar());

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

        return p_CDRnr;
    }

    public bool upd_koppladCDR(int p_CDRnr, int p_styr_lpnr, string wowner)
    {
        bool result = false;
        try
        {
            string insrt = "Insert into `Kopplad-CDR`(`Styr-lpnr`,CDRnr,Owner) Values (@Styrlpnr,@CDRNo,@Owner);";

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = insrt;

            cmd.Parameters.AddWithValue("@Styrlpnr", p_styr_lpnr);
            cmd.Parameters.AddWithValue("@CDRNo", p_CDRnr);
            cmd.Parameters.AddWithValue("@Owner", wowner);

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                return result = true;
            }
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

        return result;
    }

    public bool Insert_CDR_Temp_Details(int lpnr, int BURNo, double Sectbelopp, decimal Sectbeloppack, double Sectprocent)
    {
        bool result = false;
        try
        {
            string insrt = "Insert into `CDR-temp-detalj` (Lpnr,BURCnr,Sectbelopp,`Sectbelopp-ack`,Sectprocent,`Dtail-price-type`,`Sect-chrge-appld`,`Sumry-appld-ind`) Values (@Lpnr,@BURNo,@Sectbelopp,@Sectbeloppack,@Sectprocent,'?','?','?')";

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = insrt;

            cmd.Parameters.AddWithValue("@Lpnr", lpnr);
            cmd.Parameters.AddWithValue("@BURNo", BURNo);
            cmd.Parameters.AddWithValue("@Sectbelopp", Sectbelopp);
            cmd.Parameters.AddWithValue("@Sectbeloppack", Sectbeloppack);
            cmd.Parameters.AddWithValue("@Sectprocent", Sectprocent);

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                return result = true;
            }
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

        return result;
    }

    public int upd_CDR_indata(string p_CDRmeny, string username, int p_styr_lpnr)
    {

        int upd_CDR_indata = 0;
        int wlpnr = 0;
        int p_CDRnr = 0;

        MySqlCommand cmd = new MySqlCommand();

        try
        {
            if (p_CDRmeny == "NY")
            {
                p_CDRnr = upd_CDRnr("E", username);

                if (p_CDRnr > 0)
                {
                    bool b = upd_koppladCDR(p_CDRnr, p_styr_lpnr, "J");
                    if (!b)
                    {
                        throw new Exception("CDR record not updated.");
                    }
                }
                else
                {
                    throw new Exception("CDR record not updated.");
                }
            }

            string selCDRTemp = "SELECT Belopp,`Burc-belopp` FROM `CDR-temp`  WHERE Kval = 'N' ";

            DataTable dtCDRTemp = GetByDataTable(selCDRTemp);

            if (dtCDRTemp.Rows.Count > 0)
            {

                string updateCDR = "  SET SQL_SAFE_UPDATES = 0;   Update `CDR-temp` Set `Burc-belopp` = Belopp;  SET SQL_SAFE_UPDATES = 1; ";
                 // string updateCDR = "  SET SQL_SAFE_UPDATES = 0;   Update `CDR-temp` Set `Burc-belopp` = 20000;  SET SQL_SAFE_UPDATES = 1; ";
                cmd.CommandText = updateCDR;
                conn = new MySqlConnection(myConn);
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }


            string selCDRTempDetails = " SELECT Lpnr,BURCnr,`Sectbelopp-ack` FROM `CDR-temp-detalj` ORDER BY Lpnr ASC,BURCnr DESC ";
            DataTable dtCDRTempdtls = GetByDataTable(selCDRTempDetails);

            if (dtCDRTempdtls.Rows.Count > 0)
            {
                for (int i = 0; i < dtCDRTempdtls.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtCDRTempdtls.Rows[i]["Lpnr"]) > wlpnr)
                    {
                        wlpnr = Convert.ToInt32(dtCDRTempdtls.Rows[i]["Lpnr"]);

                        string updtCDRTemp = "Update `CDR-temp` Set `Burc-belopp` = " + Convert.ToDecimal(dtCDRTempdtls.Rows[i]["Sectbelopp-ack"]) + " Where Lpnr = " + wlpnr + "";

                        cmd.CommandText = updtCDRTemp;
                        if (conn.State != ConnectionState.Open)
                        {
                            conn = new MySqlConnection(myConn);
                            conn.Open();
                            cmd.Connection = conn;
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            StringBuilder insertDataIntoMainCDR = new StringBuilder();

            insertDataIntoMainCDR.Append(" INSERT INTO `Indata-CDR` ");
            insertDataIntoMainCDR.Append("(CDRnr, Lpnr,`Rec-ind`,`Chrgb-num-id`,`CDR-lager-nr`,");
            insertDataIntoMainCDR.Append("Datum, Tid, Samtalslngd, Belopp,`B-nr`,`C-nr`,`Intwk-serv-id`,`Kval`,");
            insertDataIntoMainCDR.Append("`Burc-belopp`,`Burc-ASOC`,`Burc-ind`)");

            insertDataIntoMainCDR.Append(" SELECT " + p_CDRnr + ",Lpnr,`Rec-ind`,`Chrgb-num-id`,");
            insertDataIntoMainCDR.Append(" `CDR-lager-nr`,Datum,Tid,Samtalslngd,Belopp,`B-nr`,`C-nr`,`Intwk-serv-id`,`Kval`,");
            insertDataIntoMainCDR.Append(" `Burc-belopp`,`Burc-ASOC`,`Burc-ind` ");
            insertDataIntoMainCDR.Append(" FROM `CDR-temp`");

            cmd.CommandText = insertDataIntoMainCDR.ToString();

            if (conn.State != ConnectionState.Open)
            {
                conn = new MySqlConnection(myConn);
                conn.Open();
                cmd.Connection = conn;
            }

            if (cmd.ExecuteNonQuery() > 0)
            {
                string delCDRTemp = "DELETE FROM `CDR-temp`";
                cmd.CommandText = delCDRTemp;
                cmd.ExecuteNonQuery();
            }


            StringBuilder insertDataIntoCDRDetails = new StringBuilder();

            insertDataIntoCDRDetails.Append(" INSERT INTO `Indata-CDR-detalj`  (CDRnr, Lpnr, BURCnr, Sectbelopp,`Sectbelopp-ack`, Sectprocent,  `Dtail-price-type`,`Sect-chrge-appld`,`Sumry-appld-ind` ) ");
            insertDataIntoCDRDetails.Append(" SELECT " + p_CDRnr + ", Lpnr, BURCnr, Sectbelopp,`Sectbelopp-ack`, Sectprocent, `Dtail-price-type`,`Sect-chrge-appld`,`Sumry-appld-ind`  FROM `CDR-temp-detalj` ");

            cmd.CommandText = insertDataIntoCDRDetails.ToString();

            if (conn.State != ConnectionState.Open)
            {
                conn = new MySqlConnection(myConn);
                conn.Open();
                cmd.Connection = conn;
            }

            if (cmd.ExecuteNonQuery() > 0)
            {
                string delCDRTempDtls = "DELETE FROM `CDR-temp-detalj`";
                cmd.CommandText = delCDRTempDtls;
                cmd.ExecuteNonQuery();
            }

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

        return upd_CDR_indata;
    }

    public bool Update_Styrtabell(int p_styr_lpnr, string winit)
    {
        bool result = false;

        try
        {

            string sql = "Select * from Styrtabell Where `Styr-lpnr` =  @Styrlpnr";

            DataTable dt = GetByDataTable(sql, p_styr_lpnr, "@Styrlpnr");

            if (dt.Rows.Count > 0)
            {
                string UPDATE_STYRTABELL = "Update Styrtabell Set Init = @init Where `Styr-lpnr` = @Styrlpnr";

                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = UPDATE_STYRTABELL;

                cmd.Parameters.AddWithValue("@init", winit);
                cmd.Parameters.AddWithValue("@Styrlpnr", p_styr_lpnr);

                conn = new MySqlConnection(myConn);
                conn.Open();
                cmd.Connection = conn;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }
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

        return result;

    }

    public bool Delete_CDR_Temp_Details(int lpnr = 0)
    {
        bool b = false;

        try
        {
            string delstr = "delete from `CDR-temp-detalj` ";

            MySqlCommand cmd = new MySqlCommand();

            if (lpnr > 0)
            {
                delstr = delstr + " Where lpnr = " + lpnr + "";
            }

            cmd.CommandText = delstr;
            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                b = true;
            }
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

        return b;
    }

    public bool UpdateTables(int p_db2_lpnr, int p_styr_lpnr)
    {
        bool b = false;
        MySqlTransaction trans = null;
        try
        {
            string update1 = "UPDATE `DB2-environment` SET `Status` = '1' WHERE `DB2-lpnr`= @p_db2_lpnr";

            string update2 = "UPDATE `Styrtabell` SET `Status1` = '1' WHERE `Styr-lpnr`= @p_styr_lpnr";

            conn = new MySqlConnection(myConn);
            conn.Open();
            trans = conn.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = update1;
            cmd.Parameters.AddWithValue("@p_db2_lpnr", p_db2_lpnr);

            if (cmd.ExecuteNonQuery() > 0)
            {
                cmd.CommandText = string.Empty;
                cmd.Parameters.Clear();
                cmd.CommandText = update2;
                cmd.Parameters.AddWithValue("@p_styr_lpnr", p_styr_lpnr);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    trans.Commit();
                    b = true;
                }
                else
                {
                    trans.Rollback();
                }
            }
            else
            {
                trans.Rollback();
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            throw ex;
        }

        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        return b;
    }

    public CompareResult Upd_Inline_Test(string wtest)
    {
        int p_styr_lpnr = Convert.ToInt32(wtest);
        string p_db2_lpnr = string.Empty;
        //string p_CDRnr = string.Empty;
        bool result = false;

        CompareResult cr = new CompareResult();

        cr.p_styr_lpnr = p_styr_lpnr;

        MySqlTransaction trans = null;
        try
        {
            string select = "SELECT `DB2-lpnr`, `Status1` FROM `Styrtabell` WHERE `Styr-lpnr`= " + p_styr_lpnr + "";

            DataTable dt = GetByDataTable(select);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Status1"].ToString() == "9")
                {
                    throw new Exception("Current test is closed !!! Reading is interrupted.");
                }
                else if (dt.Rows[0]["Status1"].ToString() == "2")
                {
                    throw new Exception("Current output is already loaded!Analyze the result please.");
                }
                else if (dt.Rows[0]["Status1"].ToString() == "0")
                {
                    throw new Exception("You must run a test first !!! Reading is interrupted.");
                }
                else
                {
                    p_db2_lpnr = dt.Rows[0]["DB2-lpnr"].ToString();
                    cr.p_db2_lpnr = p_db2_lpnr;
                }
            }
            else
            {
                throw new Exception("Current test is missing ???");
            }

            string sel = "SELECT `CDRnr` FROM `Kopplad-CDR`  WHERE `Styr-lpnr`=" + p_styr_lpnr + "";
            DataTable dt1 = GetByDataTable(sel);

            if (dt1.Rows.Count > 0)
            {
                cr.p_CDRnr = dt1.Rows[0]["CDRnr"].ToString();
            }
            else
            {
                throw new Exception("Current test missing in Linked CDR ???");
            }

            //Update the control table that output is loaded ie analysis is in progress (status1 = 2)
            StringBuilder stb = new StringBuilder();

            stb.Append(" UPDATE `DB2-environment` SET `Status` = '0' WHERE `DB2-lpnr`= @p_db2_lpnr; ");
            stb.Append(" UPDATE `Styrtabell` SET `Status1` = '2' WHERE `Styr-lpnr`= @p_styr_lpnr; ");
            stb.Append(" DELETE FROM `Utdata-BURC-detalj` WHERE `Styr-lpnr`= @p_styr_lpnr; ");
            stb.Append(" DELETE FROM `Utdata-BURC` WHERE `Styr-lpnr`= @p_styr_lpnr; ");

            conn = new MySqlConnection(myConn);
            conn.Open();
            trans = conn.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = stb.ToString();
            cmd.Parameters.AddWithValue("@p_db2_lpnr", p_db2_lpnr);
            cmd.Parameters.AddWithValue("@p_styr_lpnr", p_styr_lpnr);


            if (cmd.ExecuteNonQuery() > 0)
            {
                trans.Commit();
                result = true;
            }
        }
        catch (Exception ex)
        {
            if (trans != null)
            {
                trans.Rollback();
                trans.Dispose();
            }
            throw ex;
        }
        finally
        {
            if (trans != null)
            {
                trans.Dispose();
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        cr.Result = result;

        return cr;
    }

    public bool UploadResultData(List<UpdataBURC> updBURC, List<UpdataBURCDetails> updBURCDtls)
    {

        MySqlTransaction trans = null;
        bool result = false;
        try
        {
            int counter = 0;

            MySqlCommand cmd = new MySqlCommand();

            if (updBURC.Count > 0)
            {
                foreach (UpdataBURC upburc in updBURC)
                {
                    StringBuilder strB = new StringBuilder();

                    strB.Append(" insert into `utdata-burc` (`Styr-lpnr`, Lpnr, Ind ,`Err-code`,`Base-chrge-amt`,`Dtail-net-amt`,`Orig-num`,`Term-num`,`Call-cat-code`,`Usage-cat-code`,`Start-date`,`Start-time`,");
                    strB.Append(" `Stopp-date`,`Stopp-time`,`Array-count`) ");

                    strB.Append(" Values(@Styrlpnr" + counter + ",@Lpnr" + counter + ",@Ind" + counter + " ,@Errcode" + counter + ",@Basechrgeamt" + counter + ",@Dtailnetamt" + counter + ",@Orignum" + counter + ", ");
                    strB.Append(" @Termnum" + counter + ",@Callcatcode" + counter + ",@Usagecatcode" + counter + ",@Startdate" + counter + ",@Starttime" + counter + ",@Stoppdate" + counter + ",@Stopptime" + counter + ",");
                    strB.Append(" @Arraycount" + counter + "); ");

                    cmd.CommandText += strB.ToString();
                    cmd.Parameters.AddWithValue("@Styrlpnr" + counter.ToString(), upburc.wtest);
                    cmd.Parameters.AddWithValue("@Lpnr" + counter.ToString(), upburc.wlpnr);
                    cmd.Parameters.AddWithValue("@Ind" + counter.ToString(), upburc.wind);
                    cmd.Parameters.AddWithValue("@Errcode" + counter.ToString(), upburc.werr);
                    cmd.Parameters.AddWithValue("@Basechrgeamt" + counter.ToString(), upburc.wbase);
                    cmd.Parameters.AddWithValue("@Dtailnetamt" + counter.ToString(), upburc.wdtail);
                    cmd.Parameters.AddWithValue("@Orignum" + counter.ToString(), upburc.worig);
                    cmd.Parameters.AddWithValue("@Termnum" + counter.ToString(), upburc.wterm);
                    cmd.Parameters.AddWithValue("@Callcatcode" + counter.ToString(), upburc.wcat);
                    cmd.Parameters.AddWithValue("@Usagecatcode" + counter.ToString(), upburc.wasoc);
                    cmd.Parameters.AddWithValue("@Startdate" + counter.ToString(), upburc.wdat1);
                    cmd.Parameters.AddWithValue("@Starttime" + counter.ToString(), upburc.wtid1);
                    cmd.Parameters.AddWithValue("@Stoppdate" + counter.ToString(), upburc.wdat2);
                    cmd.Parameters.AddWithValue("@Stopptime" + counter.ToString(), upburc.wtid2);
                    cmd.Parameters.AddWithValue("@Arraycount" + counter.ToString(), upburc.wcount);

                    counter = counter + 1;
                }
            }

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;
            trans = conn.BeginTransaction();

            if (cmd.ExecuteNonQuery() > 0)
            {
                if (updBURCDtls.Count > 0)
                {
                    int counter1 = 0;
                    cmd.CommandText = string.Empty;
                    cmd.Parameters.Clear();

                    foreach (UpdataBURCDetails updataBURCDtls in updBURCDtls)
                    {
                        StringBuilder strBd = new StringBuilder();

                        strBd.Append(" Insert Into `utdata-burc-detalj` (`Styr-lpnr`,Lpnr,`Array-nr`,Prisplan,`Sect-code`,`Sect-net-amt`,`Sect-dscnt-pcnt`,`Dtail-price-type`,`Sect-chrge-appld`,`Sumry-appld-ind`)");
                        strBd.Append(" Values (@Styrlpnr" + counter1 + ",@Lpnr" + counter1 + ", @Arraynr" + counter1 + ",@Prisplan" + counter1 + ",@Sectcode" + counter1 + ",@Sectnetamt" + counter1 + ",@Sectdscntpcnt" + counter1 + ",");
                        strBd.Append(" @Dtailpricetype" + counter1 + ", @Sectchrgeappld" + counter1 + ", @Sumryappldind" + counter1 + ");");

                        cmd.CommandText += strBd.ToString();
                        cmd.Parameters.AddWithValue("@Styrlpnr" + counter1.ToString(), updataBURCDtls.wtest);
                        cmd.Parameters.AddWithValue("@Lpnr" + counter1.ToString(), updataBURCDtls.wlpnr);
                        cmd.Parameters.AddWithValue("@Arraynr" + counter1.ToString(), updataBURCDtls.wnr);
                        cmd.Parameters.AddWithValue("@Prisplan" + counter1.ToString(), updataBURCDtls.wpp);
                        cmd.Parameters.AddWithValue("@Sectcode" + counter1.ToString(), updataBURCDtls.wsect);
                        cmd.Parameters.AddWithValue("@Sectnetamt" + counter1.ToString(), updataBURCDtls.wamt);
                        cmd.Parameters.AddWithValue("@Sectdscntpcnt" + counter1.ToString(), updataBURCDtls.wpcnt);
                        cmd.Parameters.AddWithValue("@Dtailpricetype" + counter1.ToString(), updataBURCDtls.wptyp);
                        cmd.Parameters.AddWithValue("@Sectchrgeappld" + counter1.ToString(), updataBURCDtls.wca);
                        cmd.Parameters.AddWithValue("@Sumryappldind" + counter1.ToString(), updataBURCDtls.wai);

                        counter1 = counter1 + 1;
                    }

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        trans.Commit();
                        result = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (trans != null)
            {
                trans.Rollback();
                trans.Dispose();
            }
            throw ex;
        }
        finally
        {
            if (trans != null)
            {
                trans.Dispose();
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        return result;
    }

    public DataTable FetchInDataCDR(int p_CDRNo)
    {
        string sel = "SELECT `Lpnr`,`Burc-belopp`,`Burc-ASOC`,`Burc-ind`,Status FROM `Indata-CDR` WHERE `CDRnr`= " + p_CDRNo + "";

        DataTable dt = GetByDataTable(sel);

        return dt;

    }

    public DataTable FetchUDataBURD(int p_styr_lpnr, int lpnr)
    {
        string sel = "SELECT `Dtail-net-amt`,`Usage-cat-code`,`Ind` FROM `Utdata-BURC` WHERE `Styr-lpnr`= " + p_styr_lpnr + " AND `Lpnr`= " + lpnr + "";
        return GetByDataTable(sel);
    }

    public bool UpdateIndataCDR(string flag, int p_CDRNo, int lpnr)
    {

        bool result = false;

        try
        {
            string updt = "Update `Indata-CDR` Set Status = '" + flag + "'  WHERE `CDRnr`= " + p_CDRNo + " AND `Lpnr`= " + lpnr + " ";

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = updt;
            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
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

        return result;
    }

    public DataTable FetchInDataCDRDtls(int p_CDRNo)
    {
        string sel = "SELECT `Lpnr`,`BURCnr`,`Sectbelopp-ack`,`Sectprocent`,Status FROM `Indata-CDR-detalj` WHERE `CDRnr` = " + p_CDRNo + "";

        DataTable dt = GetByDataTable(sel);
        return dt;
    }

    public DataTable FetchUDataBURDDtls(int p_styr_lpnr, int lpnr, int BURCNo)
    {
        string sel = " SELECT `Sect-net-amt`,`Sect-dscnt-pcnt` FROM `Utdata-BURC-detalj` WHERE `Styr-lpnr`= " + p_styr_lpnr + " AND `Lpnr`=" + lpnr + " AND `Array-nr`= " + BURCNo + "";
        return GetByDataTable(sel);
    }


    public bool UpdateInDataCDRDtls(string flag, int p_CDRNo, int lpnr, int BURCNo)
    {

        bool result = false;

        try
        {
            string updt = "Update `Indata-CDR-detalj` Set Status = '" + flag + "'  WHERE `CDRnr`= " + p_CDRNo + " AND `Lpnr`= " + lpnr + " and BURCnr = " + BURCNo + "";

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = updt;
            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
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

        return result;
    }


    public bool Update_Styrtabell_Error(int p_styr_lpnr, int error)
    {
        bool result = false;

        try
        {
            string UPDATE_STYRTABELL = "UPDATE Styrtabell SET `Antal-fel` =" + error + " WHERE `Styr-lpnr`=" + p_styr_lpnr + "";

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = UPDATE_STYRTABELL;

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

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

        return result;

    }

    public DataTable CheckAnalysisOfTestData(int p_up_lpnr, int p_db2_lpnr)
    {
        string sel = " SELECT * FROM Styrtabell WHERE `UP-lpnr` = " + p_up_lpnr + "  AND  `DB2-lpnr` = " + p_db2_lpnr + " AND  `Status1` = '2'";
        return GetByDataTable(sel);
    }

    public DataTable GetAnalysisOfTestData(int p_up_lpnr, int p_db2_lpnr)
    {
        string sel = "  SELECT DISTINCTROW " +
                     " `Styrtabell`.`Styr-lpnr`, `DB2-environment`.`Prefix`, `Styrtabell`.`LAN-userid`, `Styrtabell`.`Datum`," +
                     " `Styrtabell`.`Ben`, `DB2-environment`.`DB2-lpnr`, `Styrtabell`.`UP-lpnr`" +
                     " FROM `Styrtabell` INNER JOIN `DB2-environment` ON `Styrtabell`.`DB2-lpnr`=`DB2-environment`.`DB2-lpnr`" +
                     " WHERE `Styrtabell`.`Status1`= '2' AND   `Styrtabell`.`UP-lpnr` = " + p_up_lpnr + " AND   `Styrtabell`.`DB2-lpnr` = " + p_db2_lpnr + "; ";
        return GetByDataTable(sel);
    }


    public bool Update_Control_Table_Status(int p_styr_lpnr, string wstat1, string wstat2)
    {
        bool result = false;

        try
        {
            string update = "UPDATE Styrtabell  SET Status1 = '" + wstat1 + "',  Status2 = '" + wstat2 + "' WHERE `Styr-lpnr` =" + p_styr_lpnr + "";

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = update;

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

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

        return result;

    }



    public DataTable TestAnalysisGenerate(int wcdrnr, int wstyr)

    {
        //  string trg = " SELECT DISTINCTROW `Indata-CDR`.CDRnr, `Indata-CDR`.Lpnr, `Utdata-BURC`.Lpnr, `Utdata-BURC`.`Styr-lpnr`, `Indata-CDR`.Belopp, `Indata-CDR`.`Burc-belopp`, `Indata-CDR`.`Burc-ASOC`, `Indata-CDR`.`Burc-ind`, `Indata-CDR`.Status, `Utdata-BURC`.Ind, `Utdata-BURC`.`Err-code`, `Utdata-BURC`.`Dtail-net-amt`, `Utdata-BURC`.`Usage-cat-code`, Styrtabell.`Styr-lpnr`, Styrtabell.`UP-lpnr`, Styrtabell.`DB2-lpnr`, `UP-release`.`UP-lpnr`, `UP-release`.`UP-release`, `DB2-environment`.`DB2-lpnr`, `DB2-environment`.Maskin, `DB2-environment`.`IMS-system`, `DB2-environment`.Prefix, `Utdata-BURC`.`Call-cat-code`, PPVrel.PPVtxt, PPVrel.PPVdb FROM `Indata-CDR`, `Utdata-BURC`, PPVrel, `UP-release` INNER JOIN(`DB2-environment` INNER JOIN Styrtabell ON `DB2-environment`.`DB2-lpnr` = Styrtabell.`DB2-lpnr`) ON `UP-release`.`UP-lpnr` = Styrtabell.`UP-lpnr` WHERE(((`Utdata-BURC`.Lpnr) =`Indata-CDR`.`Lpnr`) AND((`Utdata-BURC`.`Styr-lpnr`) =`Styrtabell`.`Styr-lpnr`) AND((`UP-release`.`UP-lpnr`) =`Styrtabell`.`UP-lpnr`) AND((`DB2-environment`.`DB2-lpnr`) =`Styrtabell`.`DB2-lpnr`)) and `Styrtabell`.`Styr-lpnr` = " + wstyr + " AND `Indata-CDR`.`CDRnr` = " + wcdrnr + "";

      string trg = " SELECT DISTINCTROW `Indata-CDR`.CDRnr, `Indata-CDR`.Lpnr, `Utdata-BURC`.Lpnr, `Utdata-BURC`.`Styr-lpnr`, Replace(`Indata-CDR`.Belopp,'.',',') Belopp, Replace(`Indata-CDR`.`Burc-belopp`,'.',',') `Burc-belopp`, `Indata-CDR`.`Burc-ASOC`, `Indata-CDR`.`Burc-ind`, `Indata-CDR`.Status, `Utdata-BURC`.Ind, `Utdata-BURC`.`Err-code`, Replace(`Utdata-BURC`.`Dtail-net-amt`,'.',',') `Dtail-net-amt`, `Utdata-BURC`.`Usage-cat-code`, Styrtabell.`Styr-lpnr`, Styrtabell.`UP-lpnr`, Styrtabell.`DB2-lpnr`, `UP-release`.`UP-lpnr`, `UP-release`.`UP-release`, `DB2-environment`.`DB2-lpnr`, `DB2-environment`.Maskin, `DB2-environment`.`IMS-system`, `DB2-environment`.Prefix, `Utdata-BURC`.`Call-cat-code`, PPVrel.PPVtxt, PPVrel.PPVdb FROM `Indata-CDR`, `Utdata-BURC`, PPVrel, `UP-release` INNER JOIN(`DB2-environment` INNER JOIN Styrtabell ON `DB2-environment`.`DB2-lpnr` = Styrtabell.`DB2-lpnr`) ON `UP-release`.`UP-lpnr` = Styrtabell.`UP-lpnr` WHERE(((`Utdata-BURC`.Lpnr) =`Indata-CDR`.`Lpnr`) AND((`Utdata-BURC`.`Styr-lpnr`) =`Styrtabell`.`Styr-lpnr`) AND((`UP-release`.`UP-lpnr`) =`Styrtabell`.`UP-lpnr`) AND((`DB2-environment`.`DB2-lpnr`) =`Styrtabell`.`DB2-lpnr`)) and `Styrtabell`.`Styr-lpnr` = " + wstyr + " AND `Indata-CDR`.`CDRnr` = " + wcdrnr + "";
       // string trg = " SELECT DISTINCTROW `Indata-CDR`.CDRnr, `Indata-CDR`.Lpnr, `Utdata-BURC`.Lpnr, `Utdata-BURC`.`Styr-lpnr`, Replace(`Indata-CDR`.Belopp,'.',',') Belopp, Replace(`Indata-CDR`.`Burc-belopp`,'.',',') `Burc-belopp`, `Indata-CDR`.`Burc-ASOC`, `Indata-CDR`.`Burc-ind`, `Indata-CDR`.Status, `Utdata-BURC`.Ind, `Utdata-BURC`.`Err-code`, Replace(`Utdata-BURC`.`Dtail-net-amt`,'.',',') `Dtail-net-amt`, `Utdata-BURC`.`Usage-cat-code`, Styrtabell.`Styr-lpnr`, Styrtabell.`UP-lpnr`, Styrtabell.`DB2-lpnr`, `UP-release`.`UP-lpnr`, `UP-release`.`UP-release`, `DB2-environment`.`DB2-lpnr`, `DB2-environment`.Maskin, `DB2-environment`.`IMS-system`, `DB2-environment`.Prefix, `Utdata-BURC`.`Call-cat-code`, PPVrel.PPVtxt, PPVrel.PPVdb FROM `Indata-CDR`, `Utdata-BURC`, PPVrel, `UP-release` INNER JOIN(`DB2-environment` INNER JOIN Styrtabell ON `DB2-environment`.`DB2-lpnr` = Styrtabell.`DB2-lpnr`) ON `UP-release`.`UP-lpnr` = Styrtabell.`UP-lpnr` WHERE(((`Utdata-BURC`.Lpnr) =`Indata-CDR`.`Lpnr`) AND((`Utdata-BURC`.`Styr-lpnr`) =`Styrtabell`.`Styr-lpnr`) AND((`UP-release`.`UP-lpnr`) =`Styrtabell`.`UP-lpnr`) AND((`DB2-environment`.`DB2-lpnr`) =`Styrtabell`.`DB2-lpnr`)) and `Styrtabell`.`Styr-lpnr` = " + wstyr + "";

        return GetByDataTable2(trg);

    }
    //BURC
    public DataTable TestBURC(int wstyr)

    {
        string trg = "SELECT DISTINCTROW Styrtabell.`Styr-lpnr`, Styrtabell.`UP-lpnr`, Styrtabell.`DB2-lpnr`, `UP-release`.`UP-lpnr`, `UP-release`.`UP-release`, `DB2-environment`.`DB2-lpnr`, `DB2-environment`.Maskin, `DB2-environment`.`IMS-system`, `DB2-environment`.Prefix, `Utdata-BURC-detalj`.`Styr-lpnr`, `Utdata-BURC-detalj`.Lpnr, `Utdata-BURC-detalj`.`Array-nr`, `Utdata-BURC-detalj`.Prisplan, `Utdata-BURC-detalj`.`Sect-code`, Replace(`Utdata-BURC-detalj`.`Sect-net-amt`,'.',',') `Sect-net-amt`, Replace(`Utdata-BURC-detalj`.`Sect-dscnt-pcnt`,'.',',')`Sect-dscnt-pcnt`, `Utdata-BURC-detalj`.`Dtail-price-type`, `Utdata-BURC-detalj`.`Sect-chrge-appld`, `Utdata-BURC-detalj`.`Sumry-appld-ind`, PPVrel.PPVtxt, PPVrel.PPVdb FROM PPVrel, (`UP-release` INNER JOIN(`DB2-environment` INNER JOIN Styrtabell ON `DB2-environment`.`DB2-lpnr` = Styrtabell.`DB2-lpnr`) ON `UP-release`.`UP-lpnr` = Styrtabell.`UP-lpnr`) INNER JOIN `Utdata-BURC-detalj` ON Styrtabell.`Styr-lpnr` = `Utdata-BURC-detalj`.`Styr-lpnr` WHERE(((`UP-release`.`UP-lpnr`) =`Styrtabell`.`UP-lpnr`)  AND((`DB2-environment`.`DB2-lpnr`) =`Styrtabell`.`DB2-lpnr`)  AND((`Utdata-BURC-detalj`.`Styr-lpnr`) =`Styrtabell`.`Styr-lpnr`)) AND `Styrtabell`.`Styr-lpnr` = " + wstyr + " ";
        return GetByDataTable2(trg);

    }

    public string imsValue(int wcdrnr, int wstyr)
    {
        string str = "SELECT DISTINCTROW `DB2-environment`.`IMS-system` FROM PPVrel, `Indata-CDR` INNER JOIN `Lager-CDR` ON `Indata-CDR`.`CDR-lager-nr` = `Lager-CDR`.`CDR-lager-nr`, `UP-release` INNER JOIN(`DB2-environment` INNER JOIN Styrtabell ON `DB2-environment`.`DB2-lpnr` = Styrtabell.`DB2-lpnr`) ON `UP-release`.`UP-lpnr` = Styrtabell.`UP-lpnr` WHERE(((`UP-release`.`UP-lpnr`) =`Styrtabell`.`UP-lpnr`) AND ((`DB2-environment`.`DB2-lpnr`) =`Styrtabell`.`DB2-lpnr`) AND((`Lager-CDR`.`CDR-lager-nr`) =`Indata-CDR`.`CDR-lager-nr`)) and `Styrtabell`.`Styr-lpnr` = " + wstyr + " AND `Indata-CDR`.`CDRnr` = " + wcdrnr + "";
        string result;
        try
        {
            conn = new MySqlConnection(myConn);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = str;
            result = Convert.ToString(cmd.ExecuteScalar());

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

        return result;

    }

    public DataTable GetByDataTable2(string sql)
    {
        //int p_up_lpnr;
        //p_up_lpnr = Convert.ToInt32(Session["Uplpnr"]);
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
    //Report
    public DataTable TestReportGenerate(int wcdrnr, int wstyr)

    {
        string trg = "SELECT DISTINCTROW Styrtabell.`Styr-lpnr`, Styrtabell.`UP-lpnr`, Styrtabell.`DB2-lpnr`, `UP-release`.`UP-lpnr`, `UP-release`.`UP-release`, `DB2-environment`.`DB2-lpnr`, `DB2-environment`.Maskin, `DB2-environment`.`IMS-system`, `DB2-environment`.Prefix, PPVrel.PPVtxt, PPVrel.PPVdb, `Indata-CDR`.CDRnr, `Indata-CDR`.Lpnr, `Indata-CDR`.`Rec-ind`, `Indata-CDR`.`Chrgb-num-id`, `Indata-CDR`.`CDR-lager-nr`, `Indata-CDR`.Datum, `Indata-CDR`.Tid, `Indata-CDR`.Samtalslngd, Replace(`Indata-CDR`.Belopp,'.',',') `Belopp`, `Indata-CDR`.`B-nr`, `Indata-CDR`.`B-nr`, `Indata-CDR`.Kval, `Indata-CDR`.`Burc-belopp`, `Indata-CDR`.`Burc-ASOC`, `Indata-CDR`.`Burc-ind`, `Indata-CDR`.Status, `Lager-CDR`.`CDR-lager-nr`, `Lager-CDR`.Servid, `Lager-CDR`.Calltype, `Lager-CDR`.Asoc, `Indata-CDR`.`Intwk-serv-id` FROM PPVrel, `Indata-CDR` INNER JOIN `Lager-CDR` ON `Indata-CDR`.`CDR-lager-nr` = `Lager-CDR`.`CDR-lager-nr`, `UP-release` INNER JOIN(`DB2-environment` INNER JOIN Styrtabell ON `DB2-environment`.`DB2-lpnr` = Styrtabell.`DB2-lpnr`) ON `UP-release`.`UP-lpnr` = Styrtabell.`UP-lpnr` WHERE(((`UP-release`.`UP-lpnr`) =`Styrtabell`.`UP-lpnr`) AND ((`DB2-environment`.`DB2-lpnr`) =`Styrtabell`.`DB2-lpnr`) AND((`Lager-CDR`.`CDR-lager-nr`) =`Indata-CDR`.`CDR-lager-nr`)) and `Styrtabell`.`Styr-lpnr` = " + wstyr + " AND `Indata-CDR`.`CDRnr` = " + wcdrnr + "";
        return GetByDataTable2(trg);

    }


    public bool OpenTestForWrite(int p_styr_lpnr, int db_lpnr)
    {
        bool result = false;

        try
        {
            StringBuilder str = new StringBuilder();

            str.Append(" UPDATE `DB2-environment` SET `Status` = '0' WHERE `DB2-lpnr`= " + db_lpnr + ";");
            str.Append(" UPDATE `Styrtabell` SET `Status1` = '0' WHERE `Styr-lpnr`= " + p_styr_lpnr + " ");

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = str.ToString();

            conn = new MySqlConnection(myConn);
            conn.Open();
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

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

        return result;

    }
}
