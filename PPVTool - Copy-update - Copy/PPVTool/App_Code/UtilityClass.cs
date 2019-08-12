using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;


public class UtilityClass
{

    public static string GetTypeFrom(int bicno)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = " SELECT `Typ` FROM `Nytt-BICnr` WHERE BICnr=" + bicno;
        return dc.ExceuteSclare(sql);
    }


    public static string GetBillDate(int bicNo, int billNo)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "SELECT DATE_FORMAT(`Stopp-datum`, '%m/%d/%Y') FROM `Indata-BIC-10` WHERE BICnr=" + bicNo + " AND `Bill-ent-id` = " + billNo;
        return dc.ExceuteSclare(sql);
    }


    public static DataTable FillServiceIdetifier()
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "SELECT `Servid-nr`, Beskrivning FROM ServID";
        return dc.GetByDataTable(sql);
    }

    public static DataTable FillBNo(int largeCDRNo)
    {
        DatabaseConnection dc = new DatabaseConnection();
        StringBuilder sql = new StringBuilder();
        sql.Append(" SELECT `Lager-CDR-Bnr`.`B-nr`,`Lager-CDR-Bnr`.Ben,`Lager-CDR-Bnr`.`CDR-lager-nr`  FROM `Lager-CDR-Bnr`");
        sql.Append(" WHERE(((`Lager-CDR-Bnr`.`CDR-lager-nr`) = @LargeCDRNo)) ");
        sql.Append(" ORDER BY `Lager-CDR-Bnr`.`B-nr` ");

        return dc.GetByDataTable(sql.ToString(), largeCDRNo, "@LargeCDRNo");
    }

    public static DataTable FillCNo()
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = " SELECT `B-nr`, Ben FROM `Lager-CDR-Bnr` ORDER BY `B-nr`";
        return dc.GetByDataTable(sql);
    }

    public static string GetSelectedCDR(int largeCDRNo)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "SELECT CONCAT_WS(' - ', Servid, Calltype, Ben) as CDR  FROM `Lager-CDR` Where `CDR-lager-nr` = " + largeCDRNo;
        return dc.ExceuteSclare(sql);
    }

    public static string GetSelectedCDR(int largeCDRNo, string saperator)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "SELECT CONCAT_WS('" + saperator + "', Servid, Calltype) as CDR  FROM `Lager-CDR` Where `CDR-lager-nr` = " + largeCDRNo;
        return dc.ExceuteSclare(sql);
    }

    public static string GetSelectedCDRBen(int largeCDRNo)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "SELECT Ben  FROM `Lager-CDR` Where `CDR-lager-nr` = " + largeCDRNo;
        return dc.ExceuteSclare(sql);
    }

    public static string GetSelectedCDRAsoc(int largeCDRNo)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "SELECT Asoc  FROM `Lager-CDR` Where `CDR-lager-nr` = " + largeCDRNo;
        return dc.ExceuteSclare(sql);
    }


    public static DataTable GetTempCDRData()
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = " SELECT * FROM `CDR-temp` Where Kval='J' ";
        return dc.GetByDataTable(sql);
    }


    public static DataTable CheckDataExists(int p_up_lpnr, int p_db2_lpnr)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "  SELECT * FROM Styrtabell  WHERE `UP-lpnr` = " + p_up_lpnr + "  AND  `DB2-lpnr` = " + p_db2_lpnr + "  AND `Status1` = '0' ";
        return dc.GetByDataTable(sql);
    }

    public static DataTable GetTestCases(int p_up_lpnr, int p_db2_lpnr)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT distinct ");
        sql.Append("`Styrtabell`.`Styr-lpnr`,");
        sql.Append("`DB2-environment`.`Prefix`,");
        sql.Append("`Styrtabell`.`LAN-userid`,");
        sql.Append("DATE_FORMAT(`Styrtabell`.`Datum`, '%Y-%m-%d') as Datum,");
        sql.Append("`Styrtabell`.`Ben`,");
        sql.Append("`DB2-environment`.`DB2-lpnr`,");
        sql.Append("`Styrtabell`.`UP-lpnr`,");
        sql.Append("`Styrtabell`.`Init`");

        sql.Append(" FROM `Styrtabell` INNER JOIN `DB2-environment` ON");
        sql.Append(" `Styrtabell`.`DB2-lpnr`=`DB2-environment`.`DB2-lpnr`");
        sql.Append(" WHERE `Styrtabell`.`Status1`= '0'");
        sql.Append(" AND `Styrtabell`.`UP-lpnr` = " + p_up_lpnr + "");
        sql.Append(" AND `Styrtabell`.`DB2-lpnr` = " + p_db2_lpnr + "");
        sql.Append(" ORDER BY `Styrtabell`.`Styr-lpnr` DESC; ");

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(sql.ToString());
    }


    public static DataTable CheckMachineStatus(int p_up_lpnr)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(" SELECT DISTINCT `IP-adress`.`Status`,`IP-adress`.`IP-adress2` FROM `UP-release` ");
        sql.Append(" INNER JOIN `IP-adress` ON `UP-release`.`IP-lpnr`=`IP-adress`.`IP-lpnr` ");
        sql.Append(" WHERE `UP-release`.`UP-lpnr`= " + p_up_lpnr + " ");

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(sql.ToString());
    }


    public static DataTable CheckDB2EnviornmenrVariables(int p_db2_lpnr)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(" SELECT `Maskin`,`IMS-system`,`DSN-system`,`PSB`,`Alias`,`Status`,`Creator`,`Volcreator` ");
        sql.Append(" FROM `DB2-environment` ");
        sql.Append(" WHERE `DB2-lpnr` = " + p_db2_lpnr + "");

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(sql.ToString());
    }

    public static DataTable CheckBICData(int p_styr_lpnr)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(" SELECT DISTINCT `Nytt-BICnr`.`BIC-lager-nr`,`Nytt-BICnr`.`Typ`,`Nytt-BICnr`.`BICnr` ");
        sql.Append(" FROM `Kopplad-BIC` INNER JOIN `Nytt-BICnr`");
        sql.Append(" ON `Kopplad-BIC`.`BICnr` = `Nytt-BICnr`.`BICnr`");
        sql.Append(" WHERE `Kopplad-BIC`.`Styr-lpnr`= " + p_styr_lpnr + "");

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(sql.ToString());
    }


    public static int GetCDRNo(int p_styr_lpnr)
    {
        string sel = " SELECT CDRnr FROM `Kopplad-CDR`  WHERE `Styr-lpnr` = " + p_styr_lpnr + " ";
        DatabaseConnection dc = new DatabaseConnection();

        int cdrNo = 0;
        DataTable dt = dc.GetByDataTable(sel);

        if (dt.Rows.Count > 0)
        {
            cdrNo = Convert.ToInt32(dt.Rows[0]["CDRnr"]);
        }

        return cdrNo;
    }

    public static int GetLOCNo(int p_styr_lpnr)
    {
        string sel = " SELECT  `LOKnr` FROM `Kopplad-LOK` WHERE `Styr-lpnr`= " + p_styr_lpnr + " ";
        DatabaseConnection dc = new DatabaseConnection();
        int locno = 0;
        DataTable dt = dc.GetByDataTable(sel);

        if (dt.Rows.Count > 0)
        {
            locno = Convert.ToInt32(dt.Rows[0]["LOKnr"]);
        }

        return locno;
    }

    public static int GetCountryType(int p_BIC_lager_nr)
    {
        string sel = "  SELECT `Agree-typ` FROM `Lager-BIC` WHERE `BIC-lager-nr`= " + p_BIC_lager_nr + " ";
        DatabaseConnection dc = new DatabaseConnection();
        int countryTyp = 0;
        DataTable dt = dc.GetByDataTable(sel);

        if (dt.Rows.Count > 0)
        {
            countryTyp = Convert.ToInt32(dt.Rows[0]["Agree-typ"]);
        }

        return countryTyp;
    }

    public static DataTable GetEmailandUser(string lan_uid)
    {
        string sel = " SELECT `Email`, `IBM-userid` FROM `LAN-tabell` WHERE `LAN-userid`='" + lan_uid + "'";
        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(sel);
    }

    public static string GetDriveType(int p_styr_lpnr)
    {
        string sel = "  SELECT `Init` FROM `Styrtabell` WHERE `Styr-lpnr`=" + p_styr_lpnr + " ";
        DatabaseConnection dc = new DatabaseConnection();

        string driveType = string.Empty;
        DataTable dt = dc.GetByDataTable(sel);

        if (dt.Rows.Count > 0)
        {
            driveType = dt.Rows[0]["Init"].ToString();
        }

        return driveType;
    }

    //Obtain PC database version
    public static string GetDatabaseVersion()
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sql = "SELECT PPVdb FROM PPVrel";
        return dc.ExceuteSclare(sql);
    }

    public static DataTable GetBICData(int p_BIC_lager_nr)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sel = " SELECT * FROM `Lager-BIC` WHERE `BIC-lager-nr`=" + p_BIC_lager_nr + "";
        return dc.GetByDataTable(sel);
    }


    public static DataTable GetBICAgreeData(int p_BICnr)
    {
        StringBuilder str = new StringBuilder();
        str.Append("SELECT `Indata-BIC-agree`.`Cust-id`, ");
        str.Append("`Indata-BIC-agree`.`Agree-id`, ");
        str.Append("`Indata-BIC-agree`.`Prisplan`, ");
        str.Append("`Indata-BIC-agree`.`Plan-unit-id`, ");
        str.Append("`Indata-BIC-agree`.`Dur-allw-sec`, ");
        str.Append("`Lager-prisplan`.`Plan-type-ind` ");
        str.Append(" FROM `Indata-BIC-agree` ");
        str.Append(" INNER JOIN `Lager-prisplan` ");
        str.Append(" ON `Indata-BIC-agree`.`PP-lpnr`= `Lager-prisplan`.`PP-lpnr`");
        str.Append(" WHERE `BICnr`=" + p_BICnr + "");
        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());

    }

    public static DataTable GetBICKSCustomerStructure(int p_BIC_lager_nr)
    {

        DatabaseConnection dc = new DatabaseConnection();
        StringBuilder str = new StringBuilder();
        str.Append("SELECT DISTINCT `Lager-BIC-cust`.`Cust-id`,`Lager-BIC-bill`.`Bill-ent-id`,`Lager-BIC-chrgb`.`Chrgb-num-id` ");
        str.Append(" FROM `Lager-BIC-cust` ");
        str.Append(" INNER JOIN(`Lager-BIC-bill`  ");
        str.Append(" INNER JOIN `Lager-BIC-chrgb` ");
        str.Append(" ON `Lager-BIC-bill`.`Bill-ent-id` = `Lager-BIC-chrgb`.`Bill-ent-id`) ");
        str.Append(" ON `Lager-BIC-cust`.`Cust-id` = `Lager-BIC-bill`.`Cust-id` ");
        str.Append(" WHERE `Lager-BIC-cust`.`BIC-lager-nr`= " + p_BIC_lager_nr + " ");

        return dc.GetByDataTable(str.ToString());

    }

    public static string GetBICKSChargeNoId(int Chrgbnumid)
    {
        string str = "SELECT `Ext-chrgb-num-id` FROM `Lager-BIC-chrgb-ext` WHERE `Chrgb-num-id`= " + Chrgbnumid + "";
        DatabaseConnection dc = new DatabaseConnection();

        DataTable dt = dc.GetByDataTable(str);

        string chargeNumId = "";

        if (dt.Rows.Count > 0)
        {
            chargeNumId = dt.Rows[0]["Ext-chrgb-num-id"].ToString();
        }

        return chargeNumId.PadRight(21 - chargeNumId.Length, ' ');
    }

    public static DataTable GetBIC05Data(int p_BICnr)
    {
        string str = "SELECT `Cust-id`, DATE_FORMAT(`Start-datum`, '%Y-%m-%d') as `Start-datum` , DATE_FORMAT(`Stopp-datum`, '%Y-%m-%d') as `Stopp-datum` FROM `Indata-BIC-05` WHERE `BICnr`= " + p_BICnr + "";
        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str);
    }

    public static DataTable GetBIC10Data(int p_BICnr)
    {
        StringBuilder str = new StringBuilder();

        str.Append(" SELECT `Bill-ent-id`, DATE_FORMAT(`Start-datum`, '%Y-%m-%d') as `Start-datum` ,DATE_FORMAT(subdate(`Start-datum` , 1), '%Y-%m-%d') as startdt , ");
        str.Append(" DATE_FORMAT(`Stopp-datum`, '%Y-%m-%d') `Stopp-datum` ,DATE_FORMAT(adddate(`Stopp-datum` , 1), '%Y-%m-%d') as stopdtadd, ");
        str.Append(" DATE_FORMAT(subdate(`Stopp-datum` , 1) ,'%Y-%m-%d') as startdtminus ");
        str.Append(" FROM `Indata-BIC-10` WHERE `BICnr`= " + p_BICnr + "");
        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());
    }


    public static DataTable GetBIC15Data(int p_BICnr)
    {
        StringBuilder str = new StringBuilder();
        str.Append("SELECT `Chrgb-num-id`,DATE_FORMAT(`Start-datum`, '%Y-%m-%d') as `Start-datum`, ");
        str.Append("DATE_FORMAT(`Stopp-datum`, '%Y-%m-%d') as `Stopp-datum` ");
        str.Append("FROM `Indata-BIC-15` ");
        str.Append("WHERE `BICnr`= " + p_BICnr + "");
        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());
    }


    public static DataTable GetBIC20Data(int p_BICnr)
    {
        StringBuilder str = new StringBuilder();

        str.Append("SELECT ");
        str.Append(" `Indata-BIC-20-ny`.`Lpnr`,");
        str.Append(" `Indata-BIC-20-ny`.`Post`,");
        str.Append(" `Indata-BIC-20-ny`.`Underlpnr`,");
        str.Append(" DATE_FORMAT(`Indata-BIC-20-ny`.`Start-datum`, '%Y-%m-%d') as `Start-datum`,");
        str.Append(" DATE_FORMAT(`Indata-BIC-20-ny`.`Stopp-datum`, '%Y-%m-%d') as `Stopp-datum`,");
        str.Append(" `Indata-BIC-agree`.`Agree-id`,");
        str.Append(" `Indata-BIC-agree`.`Cust-id`,");
        str.Append(" `Indata-BIC-agree`.`Prisplan`,");
        str.Append(" `Indata-BIC-agree`.`Plan-unit-id`");
        str.Append(" FROM `Indata-BIC-20-ny`,`Indata-BIC-agree`");
        str.Append(" WHERE `Indata-BIC-20-ny`.`Lpnr` = `Indata-BIC-agree`.`Lpnr`");
        str.Append(" AND   `Indata-BIC-agree`.`BICnr`= " + p_BICnr + "");
        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());
    }

    public static DataTable GetBIC30Data(int underLpnr)
    {
        StringBuilder str = new StringBuilder();
        str.Append(" SELECT `Optn-trm-val` ");
        str.Append(" FROM `Indata-BIC-30-ny` ");
        str.Append(" WHERE `Underlpnr`= " + underLpnr + " ");

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());
    }


    public static DataTable GetBIC35Data(int underLpnr)
    {
        StringBuilder str = new StringBuilder();
        str.Append("SELECT `Area-type-ind`,`Sel-area-val`");
        str.Append(" FROM `Indata-BIC-35-ny`");
        str.Append(" WHERE `Underlpnr`=" + underLpnr + "");
        
        StringBuilder str1 = str;
        if(str1==str)
        { }
        else
        {

        }

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());
    }


    public static DataTable GetBIC45Data(int underLpnr)
    {
        StringBuilder str = new StringBuilder();
        str.Append("SELECT `Begin-point-val`,`End-point-val`");
        str.Append(" FROM `Indata-BIC-45-ny`");
        str.Append(" WHERE `Underlpnr`=" + underLpnr + "");

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());
    }



    public static DataTable GetCDRData(int p_CDRNo)
    {
        StringBuilder str = new StringBuilder();

        str.Append(" SELECT `Indata-CDR`.`Lpnr`,`Rec-ind`,`Indata-CDR`.`Chrgb-num-id`, ");
        str.Append(" `Indata-CDR`.`CDR-lager-nr`, DATE_FORMAT(`Indata-CDR`.`Datum`,'%Y-%m-%d') Datum,");
        str.Append(" `Indata-CDR`.`Tid`,`Indata-CDR`.`Samtalslngd`,");
        str.Append(" `Indata-CDR`.`Belopp`, `Indata-CDR`.`B-nr`, `Indata-CDR`.`C-nr`, `Indata-CDR`.`Intwk-serv-id`,");
        str.Append(" `Lager-CDR`.`Servid`, `Lager-CDR`.`Calltype`,");
        str.Append(" `Lager-CDR`.`Asoc`");
        str.Append(" FROM `Indata-CDR` INNER JOIN `Lager-CDR`");
        str.Append(" ON `Indata-CDR`.`CDR-lager-nr`=`Lager-CDR`.`CDR-lager-nr`");
        str.Append(" WHERE `CDRnr`= " + p_CDRNo + "");

        DatabaseConnection dc = new DatabaseConnection();

        return dc.GetByDataTable(str.ToString());
    }

    public static DataTable GetCustIdandBillId(int chrgbNumId)
    {
        StringBuilder str = new StringBuilder();
        str.Append("SELECT DISTINCT `Lager-BIC-bill`.`Cust-id`, ");
        str.Append(" `Lager-BIC-bill`.`Bill-ent-id`,");
        str.Append(" `Lager-BIC-chrgb`.`Anr`");
        str.Append(" FROM `Lager-BIC-bill`");
        str.Append(" INNER JOIN `Lager-BIC-chrgb`");
        str.Append(" ON `Lager-BIC-bill`.`Bill-ent-id` = `Lager-BIC-chrgb`.`Bill-ent-id`");
        str.Append(" WHERE `Lager-BIC-chrgb`.`Chrgb-num-id`= " + chrgbNumId + "");

        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(str.ToString());
    }

    public static string GetLOKNo(int p_LOKnr)
    {
        string str = "SELECT Belopp FROM `Indata-LOK` WHERE `LOKnr`= " + p_LOKnr + "";
        DatabaseConnection dc = new DatabaseConnection();

        DataTable dt = dc.GetByDataTable(str);

        string lokNo = "";

        if (dt.Rows.Count > 0)
        {
            lokNo = dt.Rows[0]["Belopp"].ToString();
        }

        return lokNo;
    }

    public static DataTable GetDetailsofTempCDR(int lpnr)
    {
        DatabaseConnection dc = new DatabaseConnection();
        string sel = " Select Lpnr,BURCnr as section,Sectbelopp as AmtPro,`Sectbelopp-ack` as netchrge,Sectprocent as  dscntpcnt from `CDR-temp-detalj` where Lpnr = " + lpnr + "";
        return dc.GetByDataTable(sel);
    }


    public static DataTable CheckCurrentTestStatus(int p_styr_lpnr)
    {
        string sel = " SELECT `Status1` FROM `Styrtabell`  WHERE `Styr-lpnr`=" + p_styr_lpnr + " ";
        DatabaseConnection dc = new DatabaseConnection();
        return dc.GetByDataTable(sel);
    }


   
}
