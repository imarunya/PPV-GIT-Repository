using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class InitiateTest
{
    int m_Styrlpnr;
    int m_UPlpnr;
    int m_DB2lpnr;
    string m_Ben;
    string m_userId;

    public int Styrlpnr
    {
        get
        {
            return this.m_Styrlpnr;
        }
        set
        {
            this.m_Styrlpnr = value;
        }
    }

    public int UPlpnr
    {
        get
        {
            return this.m_UPlpnr;
        }
        set
        {
            this.m_UPlpnr = value;
        }
    }

    public int DB2lpnr
    {
        get
        {
            return this.m_DB2lpnr;
        }
        set
        {
            this.m_DB2lpnr = value;
        }
    }

    public string Ben
    {
        get
        {
            return this.m_Ben;
        }
        set
        {
            this.m_Ben = value;
        }
    }


    public string UserId
    {
        get
        {
            return this.m_userId;
        }
        set
        {
            this.m_userId = value;
        }
    }
}


public class PricePlan
{
    int m_lpnr;
    int m_pplpnr;

    public int Lpnr
    {
        get { return this.m_lpnr; }
        set { this.m_lpnr = value; }
    }

    public int PPLpnr
    {
        get { return this.m_pplpnr; }
        set { this.m_pplpnr = value; }
    }
}

public class DB2EnviornmentVariable
{
    string _wmaskin;
    string _wIMS;
    string _wDSN;
    string _wPSB;
    string _walias;
    string _wcreator;
    string _wvol;

    public string wmaskin
    {
        get { return this._wmaskin; }
        set { this._wmaskin = value; }
    }

    public string wIMS
    {
        get { return this._wIMS; }
        set { this._wIMS = value; }
    }

    public string wDSN
    {
        get { return this._wDSN; }
        set { this._wDSN = value; }
    }

    public string wPSB
    {
        get { return this._wPSB; }
        set { this._wPSB = value; }
    }
    public string walias
    {
        get { return this._walias; }
        set { this._walias = value; }
    }

    public string wcreator
    {
        get { return this._wcreator; }
        set { this._wcreator = value; }
    }

    public string wvol
    {
        get { return this._wvol; }
        set { this._wvol = value; }
    }
}


public class BICData
{
    string _p_BICnr;
    string _p_BIC_lager_nr;
    string _wtyp;

    public string p_BICnr
    {
        get { return this._p_BICnr; }
        set { this._p_BICnr = value; }
    }

    public string p_BIC_lager_nr
    {
        get { return this._p_BIC_lager_nr; }
        set { this._p_BIC_lager_nr = value; }
    }

    public string wtyp
    {
        get { return this._wtyp; }
        set { this._wtyp = value; }
    }
}


public class UserDetails
{
    string _wemail;
    string _p_mvs_user;    

    public string wemail
    {
        get { return this._wemail; }
        set { this._wemail = value; }
    }

    public string p_mvs_user
    {
        get { return this._p_mvs_user; }
        set { this._p_mvs_user = value; }
    }
}


public class CompareResult
{
    int _p_styr_lpnr;
    string _p_db2_lpnr;
    string _p_CDRnr;
    bool _result;


    public int  p_styr_lpnr
    {
        get { return this._p_styr_lpnr; }
        set { this._p_styr_lpnr = value; }
    }

    public string p_db2_lpnr
    {
        get { return this._p_db2_lpnr; }
        set { this._p_db2_lpnr = value; }
    }

    public string p_CDRnr
    {
        get { return this._p_CDRnr; }
        set { this._p_CDRnr = value; }
    }

    public bool Result
    {
        get { return this._result; }
        set { this._result = value; }
    }
}

