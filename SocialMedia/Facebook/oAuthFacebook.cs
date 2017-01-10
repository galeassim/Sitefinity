﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Script.Serialization;
using JXTPortal.Entities.Models;
using System.Xml;
using System.Globalization;
using log4net;

public class oAuthFacebook
{
    private ILog _logger;
    private string _clientId;
    private string _clientSecret;
    private string _redirectUri;
    private string _code;
    private string _jobapplyurl;
    private string _permissions = "email"; //user_work_history,user_education_history, user_website

    private string authorize_url = SocialMedia.Resource1.fb_authroize_with_permissions;
    private string access_token_url = SocialMedia.Resource1.fb_access_token;
    private string userinfo_url = SocialMedia.Resource1.fb_userinfo;
    private string checkpermission_url = SocialMedia.Resource1.fb_checkpermission;
    

    public string ClientID
    {
        get { return _clientId; }
        set { _clientId = value; }
    }

    public string ClientSecret
    {
        get { return _clientSecret; }
        set { _clientSecret = value; }
    }

    public string RedirectURI
    {
        get { return _redirectUri; }
        set { _redirectUri = value; }
    }

    public string Code
    {
        get { return _code; }
        set { _code = value; }
    }
    public oAuthFacebook()
    {
        _logger = LogManager.GetLogger(typeof(oAuthFacebook));
    }

    public string Permissions
    {
        get { return _permissions; }
        set { _permissions = value; }
    }

    public string Authorize()
    {
        if (string.IsNullOrEmpty(this.ClientID) || string.IsNullOrEmpty(this.RedirectURI) || string.IsNullOrEmpty(this.Permissions))
        {
            return "Error: Please provide ClientID, Redirect URI & Permissions required";
        }

        string url = string.Format(authorize_url, this.ClientID, System.Web.HttpUtility.UrlEncode(this.RedirectURI), System.Web.HttpUtility.UrlEncode(this.Permissions));
        return url;
    }

    public string AuthorizeWithoutURLEncode()
    {
        _logger.InfoFormat("AuthorizeWithoutURLEncode() Started");

        if (string.IsNullOrEmpty(this.ClientID) || string.IsNullOrEmpty(this.RedirectURI) || string.IsNullOrEmpty(this.Permissions))
        {
            return "Error: Please provide ClientID, Redirect URI & Permissions required";
        }

        string url = string.Format(authorize_url, this.ClientID, this.RedirectURI, System.Web.HttpUtility.UrlEncode(this.Permissions));
        _logger.InfoFormat("AutoriseWithout URL: {0}", url);
        return url;
    }

    public string HasFullPermission(string userid, string accesstoken)
    {
        _logger.Info("Has Full Permission Started");

        string url = string.Format(checkpermission_url, userid, accesstoken);
        _logger.InfoFormat("Permission URL: {0}", url);

        try
        {
            HttpWebRequest webrequest = WebRequest.Create(url) as HttpWebRequest;
            webrequest.Method = "GET";
            HttpWebResponse webresponse = webrequest.GetResponse() as HttpWebResponse;
            _logger.InfoFormat("Webresponse Status Code: {0}", webresponse.StatusDescription);

            if (webresponse.StatusCode.ToString() == "OK")
            {
                Stream stream = webresponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                FBDataPermission permissions = jss.Deserialize<FBDataPermission>(result);
                _logger.DebugFormat("FB data Permission: {0}", permissions);

                bool hasEducation = false, hasWork = false, hasWebsite = false, hasEmail = false, hasPublicProfile = false;

                foreach (FBPermission permission in permissions.data)
                {
                    if (permission.permission == "user_education_history" && permission.status == "granted")
                    {
                        hasEducation = true;
                    }
                    if (permission.permission == "user_work_history" && permission.status == "granted")
                    {
                        hasWork = true;
                    }
                    if (permission.permission == "user_website" && permission.status == "granted")
                    {
                        hasWebsite = true;
                    }
                    if (permission.permission == "email" && permission.status == "granted")
                    {
                        hasEmail = true;
                    }
                    if (permission.permission == "public_profile" && permission.status == "granted")
                    {
                        hasPublicProfile = true;
                    }

                    _logger.DebugFormat("Permission Type: {0} and Permission Status: {1}", permission.permission, permission.status);
                }

                if (hasEducation && hasWork && hasWebsite && hasEmail && hasPublicProfile)
                {
                    return "";
                }
                else
                {
                    return "missing";
                }
                
            }
            else
            {
                return "Facebook Error";
            }
        }
        catch (Exception ex)
        {
            _logger.ErrorFormat("HasFullPermission Exeption: {0}", ex);
            return "Facebook Error: " + ex.Message + "<br />";
        }
    }

    public string oAuth2GetProfileHTML(string profile, string logourl)
    {
        if (profile.StartsWith("Error:") || profile.StartsWith("The remote server returned an error:"))
        {
            return profile;
        }

        JavaScriptSerializer jss = new JavaScriptSerializer();
        FBUserInfo userinfo = jss.Deserialize<FBUserInfo>(profile);

        StringBuilder sb = new StringBuilder();
        sb.Append("<html>"); sb.Append(Environment.NewLine);
        sb.Append("<head>"); sb.Append(Environment.NewLine);
        sb.Append("</head>"); sb.Append(Environment.NewLine);
        sb.Append("<body>"); sb.Append(Environment.NewLine);
        sb.Append(string.Format("<div align=\"center\" style=\"width:100%\"><img alt=\"\" src=\"{0}\" style=\"border-style: none;\" height=\"200\"width=\"327\" /><!-- Logo URL --></div>", logourl)); sb.Append(Environment.NewLine);
        sb.Append("<font face=\"Arial\" size=\"1\">"); sb.Append(Environment.NewLine);
        sb.Append("<table width=\"100%\" border=\"0\">"); sb.Append(Environment.NewLine);
        sb.Append("<tr>"); sb.Append(Environment.NewLine);
        sb.Append(string.Format("<td width=\"75%\" align=\"left\" valign=\"top\"><p style=\"font-weight: bold; font-size: 16pt;\">{0}</p><!-- " +
                "Member Name -->", userinfo.first_name + " " + userinfo.last_name)); sb.Append(Environment.NewLine);
        sb.Append(string.Format("<p style=\"font-size: 10pt;\">{0}</p></td><!-- Email -->", userinfo.email)); sb.Append(Environment.NewLine);

        if (!string.IsNullOrWhiteSpace(userinfo.website ))
        {
            sb.Append(string.Format("<p style=\"font-size: 10pt;\">{0}</p></td><!-- Email -->", userinfo.website)); sb.Append(Environment.NewLine);
        }

        sb.Append("</tr>"); sb.Append(Environment.NewLine);
        sb.Append("</table>"); sb.Append(Environment.NewLine);

        if (userinfo.work != null && userinfo.work.Length > 0)
        {
            sb.Append("<p style=\"font-size: 16pt; font-weight:bold; color:#999;\">Experience</p>"); sb.Append(Environment.NewLine);

            foreach (FBWorkType worktype in userinfo.work)
            {
                sb.Append(string.Format("<p style=\"font-weight:bold; font-size: 12pt;\">{0}{1}</p>", (worktype.position != null) ? worktype.position.name + " at " : string.Empty, worktype.employer.name)); sb.Append(Environment.NewLine);

                sb.Append(string.Format("<p>{0}{1}</p>", worktype.start_date, string.IsNullOrWhiteSpace(worktype.end_date) ? string.Empty : " - " + worktype.end_date)); sb.Append(Environment.NewLine);

            }
        }

        if (userinfo.education != null && userinfo.education.Length > 0)
        {
            sb.Append("<p style=\"font-size: 16pt; font-weight:bold; color:#999;\">Education</p>"); sb.Append(Environment.NewLine);

            foreach (FBEducationType education in userinfo.education)
            {
                string schoolname = education.school.name;
                string degree = (education.degree != null) ? education.degree.name : string.Empty;
                string startyear = (education.year != null) ? education.year.name : string.Empty;

                sb.Append(string.Format("<p style=\"font-weight:bold; font-size: 12pt;\">{0}</p>", HttpUtility.HtmlEncode(schoolname))); sb.Append(Environment.NewLine);

                string comma = string.Empty;
                if (!string.IsNullOrEmpty(degree) && !string.IsNullOrEmpty(startyear))
                {
                    comma = ", ";
                }
                sb.Append(string.Format("<p>{0}{1}{2}</p>", degree, comma, startyear)); sb.Append(Environment.NewLine);


                sb.Append("<p>&nbsp;</p>"); sb.Append(Environment.NewLine);
            }
        }

        sb.Append("</font>"); sb.Append(Environment.NewLine);
        sb.Append("</body>"); sb.Append(Environment.NewLine);
        sb.Append("</html>");


        return sb.ToString();
    }

    public string GetUserInfo(out string accesstoken)
    {
        accesstoken = string.Empty;

        if (string.IsNullOrEmpty(this.ClientID) || string.IsNullOrEmpty(this.RedirectURI) || string.IsNullOrEmpty(this.ClientSecret) || string.IsNullOrEmpty(this.Code))
        {
            return "Error: Please provide ClientID, Redirect URI, ClientSecret & Code";
        }

        string url = string.Format(access_token_url, this.ClientID, HttpUtility.UrlEncode(this.RedirectURI), this.ClientSecret, this.Code);

        try
        {
            HttpWebRequest webrequest = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse webresponse = webrequest.GetResponse() as HttpWebResponse;

            if (webresponse.StatusCode.ToString() == "OK")
            {
                Stream stream = webresponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();
                

                if (!string.IsNullOrEmpty(result))
                {
                    string[] strs = result.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in strs)
                    {
                        if (s.StartsWith("access_token="))
                        {
                            accesstoken = s.Replace("access_token=", string.Empty);
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(accesstoken))
                    {
                        return RetrieveUserInfo(accesstoken);
                    }
                    else
                    {
                        return "Error: Request Failed";
                    }
                }
                else
                {
                    return "Error: Request Failed";
                }
            }
            else
            {
                return "Error: Request Failed";
            }
        }
        catch (Exception ex)
        {
            return " GetUserInfoError: " + ex.Message + "<br />";
        }
    }

    private string RetrieveUserInfo(string access_token)
    {
        _logger.Info("Retrieving User Info\n");

        string url = string.Format(userinfo_url, access_token);
        _logger.InfoFormat("Request URL: {0}", url);

        HttpWebRequest webrequest = WebRequest.Create(url) as HttpWebRequest;
        _logger.DebugFormat("Http WebRequet: {0}", webrequest);

        HttpWebResponse webresponse = webrequest.GetResponse() as HttpWebResponse;
        _logger.DebugFormat("Http Response: {0}", webresponse);

        string result = string.Empty;

        if (webresponse.StatusCode.ToString() == "OK")
        {
            Stream stream = webresponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            _logger.DebugFormat("Webresponse Stream: (0}", reader);

            result = reader.ReadToEnd();
            result = result.Replace(@"u0040", "@");
            _logger.DebugFormat("Webresponse Result: (0}", result);

            result = Regex.Unescape(result);
        }

        return (string.IsNullOrEmpty(result) ? "Error: Request Failed" : result);
    }

    public string RetreiveAccessTokenWithFBCode()
    {
        _logger.Info("Retrieving facebook Access Token");
        if (!string.IsNullOrEmpty(this.Code) && !string.IsNullOrEmpty(this.ClientID) && !string.IsNullOrEmpty(this.RedirectURI) && !string.IsNullOrEmpty(this.ClientSecret))
        {
            _logger.DebugFormat("FBcode: {0}, clientId: {1}, RedirectUri: {2}", Code, ClientID, RedirectURI);
            
            WebResponse response;

            try
            {
                string tokenGetURL = "https://graph.facebook.com/v2.3/oauth/access_token?client_id=" + this.ClientID + "&redirect_uri=" + this.RedirectURI + "&client_secret=" + this.ClientSecret + "&code=" + this.Code;

                _logger.InfoFormat("oAuth Token URL {0}", tokenGetURL);

                WebRequest request = WebRequest.Create(tokenGetURL);
                response = request.GetResponse();

                _logger.DebugFormat("Web Request Response: {0}", response);

                StreamReader sr = new StreamReader(response.GetResponseStream());
                               
                string tokenJsonObj = sr.ReadToEnd();

                _logger.DebugFormat("Response Stream: {0}", tokenJsonObj);

                FacebookToken tokenObj = new JavaScriptSerializer().Deserialize<FacebookToken>(tokenJsonObj);

                string token = tokenObj.access_token;

                _logger.InfoFormat("Token Received from Response: {0}", token);

                sr.Close();
                response.Close();

                return token;
            }
            catch (WebException ex)
            {
                _logger.Error(ex);
                string msg = "";

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    //throw ex;
                    response = ex.Response;
                    msg = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd().Trim();
                    _logger.ErrorFormat("Exeption Response Stream: {0}", msg);
                    response.Close();
                }
            }
        }
        return null;
    }

    public FacebookUserDetails RetreiveUserDetails(string token)
    {
        _logger.Info("Retrieving Facebook User Details\n");

        string url = "https://graph.facebook.com/me?access_token=" + token;
        _logger.InfoFormat("Request url: {0}", url);

        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        _logger.DebugFormat("Request Response: {0}", response);

        StreamReader sr = new StreamReader(response.GetResponseStream());

        string userJsonObj = sr.ReadToEnd();
        _logger.DebugFormat("Request JSON Object: {0}", userJsonObj);

        FacebookUserDetails userObj = new JavaScriptSerializer().Deserialize<FacebookUserDetails>(userJsonObj);
        _logger.InfoFormat("Retrieved USer Object: {0}", userObj);

        sr.Close();
        response.Close();

        return userObj;
    }

    class FBUserInfo
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public FBWorkType[] work { get; set; }
        public FBEducationType[] education { get; set; }
        public string id { get; set; }
        public string website { get; set; } 
    }

    class FBWorkType
    {
        public string end_date { get; set; }
        public FBPageType employer { get; set; }
        public string start_date { get; set; }
        public FBPageType position { get; set; }
    }

    class FBEducationType
    {
        public FBPageType school { get; set; }
        public string type { get; set; }
        public FBPageType year { get; set; }
        public FBPageType degree { get; set; }
    }

    class FBPageType
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    class FBDataPermission
    {
        public FBPermission[] data { get; set; }
    }

    class FBPermission
    {
        public string permission { get; set; }
        public string status { get; set; }
    }

}


