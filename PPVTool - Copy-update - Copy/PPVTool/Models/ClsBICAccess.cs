using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class ClsBICAccess
{
    int m_lpnr;
    int m_bicnr;
    int m_agreementid;
    int m_custid;
    string m_priceplan;
    int m_planunit;
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

    public int BICnr
    {
        get { return this.m_bicnr; }
        set { this.m_bicnr = value; }
    }

    public int AgreementId
    {
        get { return this.m_agreementid; }
        set { this.m_agreementid = value; }
    }

    public int CustId
    {
        get { return this.m_custid; }
        set { this.m_custid = value; }
    }

    public string Priceplan
    {
        get { return this.m_priceplan; }
        set { this.m_priceplan = value; }
    }

    public int Planunit
    {
        get { return this.m_planunit; }
        set { this.m_planunit = value; }
    }
}
