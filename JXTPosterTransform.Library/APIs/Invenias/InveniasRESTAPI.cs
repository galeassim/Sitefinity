﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;

namespace JXTPosterTransform.Library.APIs.Invenias
{
    public class InveniasRESTAPI
    {
        InveniasSettings _settings;

        public InveniasRESTAPI(string clientID, string username, string password)
        {
            _settings = new InveniasSettings { ClientID = clientID, Username = username, Password = password };
        }

        public InveniaTokenResponse Authenticate()
        {
            string tokenURL = "https://publicapi.invenias.com/token";

            string postData = string.Format("grant_type=password&username={0}&password={1}&client_id={2}", _settings.Username, _settings.Password, _settings.ClientID);
            string tokenResponse = HttpPost(tokenURL, postData, false);

            if (!string.IsNullOrEmpty(tokenResponse))
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                InveniaTokenResponse token = (InveniaTokenResponse)ser.Deserialize(tokenResponse, typeof(InveniaTokenResponse));

                _settings.RESTAuthToken = token.access_token;
                return token;
            }

            return null;
        }

        public List<InveniasAdvertisementsValue> AdvertisementsGet()
        {
            string targetURL = "https://publicapi.invenias.com/v1/Advertisements";

            string advertisements = HttpGet(targetURL, true);

            if (!string.IsNullOrEmpty(advertisements))
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                InveniasAdvertisementsRoot token = (InveniasAdvertisementsRoot)ser.Deserialize(advertisements, typeof(InveniasAdvertisementsRoot));

                return token.value;
            }
            return null;
        }

        public bool AdvertisementCreate(InveniasAdvertisementsValue ad)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string adJson = ser.Serialize(ad);

            HttpPost("https://publicapi.invenias.com/v1/Advertisements", adJson, true);

            return true;
        }


        #region HTTP call methods

        public string HttpPost(string URI, string Parameters, bool AuthHeader)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.ContentType = "text/plain";
            req.Method = "POST";

            if (AuthHeader)
                req.Headers.Add("Authorization", "Bearer " + _settings.RESTAuthToken);

            // Add parameters to post
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = data.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(data, 0, data.Length);
            os.Close();

            // Do the post and get the response.
            System.Net.WebResponse response = null;

            try
            {
                response = req.GetResponse();
            }
            catch (WebException ex)
            {
                string msg = "";

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    //throw ex;
                    response = ex.Response;
                    msg = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd().Trim();
                }
            }

            if (response == null) return null;

            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());

            return sr.ReadToEnd().Trim();
        }

        private string HttpGet(string URI, bool AuthHeader)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Method = "GET";

            if( AuthHeader )
                req.Headers.Add("Authorization", "Bearer " + _settings.RESTAuthToken);

            System.Net.WebResponse response = null;
            try
            {
                response = req.GetResponse();
            }
            catch (WebException ex)
            {
                string msg = "";

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    //throw ex;
                    response = ex.Response;
                    msg = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd().Trim();
                }
                response.Close();
                return msg;
            }

            if (response == null)
            {
                response.Close();
                return null;
            }

            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());

            return sr.ReadToEnd().Trim();
        }


        #endregion

    }
}
