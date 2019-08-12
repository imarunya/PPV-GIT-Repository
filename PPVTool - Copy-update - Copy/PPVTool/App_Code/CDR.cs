using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CDR
{

    int m_lpnr;
    string m_Rec;
    string m_Chrgno;
    string m_CDRNo;
    string m_Servid_Calltype;
    DateTime m_Date;
    string m_Time;
    string m_CallDuration;
    decimal m_Amount;
    string m_BNo;
    string m_CNo;
    string m_QualifyingInd;
    string m_Ben;
    string m_Asoc;
    string m_ServiceIdentifier;

    public int Lpnr
    {
        get { return this.m_lpnr; }
        set { this.m_lpnr = value; }
    }

    public string Rec
    {
        get { return this.m_Rec; }
        set { this.m_Rec = value; }
    }

    public string Chrgno
    {
        get { return this.m_Chrgno; }
        set { this.m_Chrgno = value; }
    }

    public string CDRNo
    {
        get { return this.m_CDRNo; }
        set { this.m_CDRNo = value; }
    }

    public string Servid_Calltype
    {
        get { return this.m_Servid_Calltype; }
        set { this.m_Servid_Calltype = value; }
    }

    public DateTime Date
    {
        get { return this.m_Date; }
        set { this.m_Date = value; }
    }

    public string Time
    {
        get { return this.m_Time; }
        set { this.m_Time = value; }
    }

    public string CallDuration
    {
        get { return this.m_CallDuration; }
        set { this.m_CallDuration = value; }
    }

    public decimal Amount
    {
        get { return this.m_Amount; }
        set { this.m_Amount = value; }
    }

    public string BNo
    {
        get { return this.m_BNo; }
        set { this.m_BNo = value; }
    }

    public string CNo
    {
        get { return this.m_CNo; }
        set { this.m_CNo = value; }
    }

    public string QualifyingInd
    {
        get { return this.m_QualifyingInd; }
        set { this.m_QualifyingInd = value; }
    }

    public string Ben
    {
        get { return this.m_Ben; }
        set { this.m_Ben = value; }
    }

    public string Asoc
    {
        get { return this.m_Asoc; }
        set { this.m_Asoc = value; }
    }

    public string ServiceIdentifier
    {
        get { return this.m_ServiceIdentifier; }
        set { this.m_ServiceIdentifier = value; }
    }

}
