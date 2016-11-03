﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using JXTPosterTransform.Library.Common;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace JXTPosterTransform.Library.Methods
{
    public class PullJsonFromURL
    {
        public static ResponseClass ProcessJsonToXML(string sourceURL, string fileName)
        {
            ResponseClass responseClass = new ResponseClass();

            Console.WriteLine("Loading the Json - " + sourceURL);

            try
            {
                string jsonFromURL = Utils.GetJsonFromUrl(sourceURL);

                //do this to prevent any json comes from the URL does not have a single root node
                jsonFromURL = "{ root: " + jsonFromURL + "}";

                // To convert JSON text contained in string json into an XML node
                XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(jsonFromURL);

                string xmlString = string.Empty;
                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmlDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    xmlString = stringWriter.GetStringBuilder().ToString();
                }


                // Save the Raw XML
                System.IO.File.WriteAllText(ConfigurationManager.AppSettings["FTPTempStorage"] + fileName + "_Raw.xml", xmlString);

                responseClass.ResponseXML = xmlString;             

                // Success to get the XML.
                responseClass.blnSuccess = true;
            }
            catch(Exception ex)
            {
                responseClass.strMessage = "Can't find the downloaded file from URL - " + sourceURL;
                Console.WriteLine(responseClass.strMessage);
                Console.WriteLine(ex.Message + " - " + ex.InnerException);
            }

            return responseClass;



            //XDocument xDocument = XDocument.Load(XmlReader.Create(source));

            //return xDocument.ToString();
            //get the root namespace
            //var ns = xDocument.Root.Name.Namespace;

            //Filter active jobs
            //var activeJobs = xDocument.Descendants(ns + "JobRecord");
        }


    }
}