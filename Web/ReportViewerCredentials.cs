using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;

[Serializable]
public class ReportViewerCredentials : IReportServerCredentials
{

    public ReportViewerCredentials()
    {
    }

    public ReportViewerCredentials(string username)
    {
        this.Username = username;
    }


    public ReportViewerCredentials(string username, string password)
    {
        this.Username = username;
        this.Password = password;
    }


    public ReportViewerCredentials(string username, string password, string domain)
    {
        this.Username = username;
        this.Password = password;
        this.Domain = domain;
    }


    public string Username
    {
        get
        {
            return this.username;
        }
        set
        {
            string username = value;
            if (username.Contains("\\"))
            {
                this.domain = username.Substring(0, username.IndexOf("\\"));
                this.username = username.Substring(username.IndexOf("\\") + 1);
            }
            else
            {
                this.username = username;
            }
        }
    }
    private string username;



    public string Password
    {
        get
        {
            return this.password;
        }
        set
        {
            this.password = value;
        }
    }
    private string password;


    public string Domain
    {
        get
        {
            return this.domain;
        }
        set
        {
            this.domain = value;
        }
    }
    private string domain;




    #region IReportServerCredentials Members

    public bool GetBasicCredentials(out string basicUser, out string basicPassword, out string basicDomain)
    {
        basicUser = username;
        basicPassword = password;
        basicDomain = domain;
        return username != null && password != null && domain != null;
    }

    public bool GetFormsCredentials(out string formsUser, out string formsPassword, out string formsAuthority)
    {
        formsUser = username;
        formsPassword = password;
        formsAuthority = domain;
        return username != null && password != null && domain != null;

    }

    public WindowsIdentity ImpersonationUser
    {
        get
        {
            return null;
        }
    }

    bool IReportServerCredentials.GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string formsPassword, out string authority)
    {
        authCookie = null;
        userName = username;
        formsPassword = password;
        authority = domain;

        return false;
    }

    WindowsIdentity IReportServerCredentials.ImpersonationUser
    {
        get
        { //throw new NotImplementedException(); 
            return null;
        }
    }

    System.Net.ICredentials IReportServerCredentials.NetworkCredentials
    {
        get
        { //throw new NotImplementedException(); 
            //return null;
            return new System.Net.NetworkCredential(this.Username, this.Password, this.Domain);
        }
    }

    #endregion
}
