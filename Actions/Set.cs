﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SharpFireStarter.Activity
{
    public static class Set
    {

        public static async void WriteToDB(string appID, string node, string oAuth, string data)
        {
            string endpoint = string.Format("{0}/{1}.json?access_token={2}", appID, node, oAuth); 

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            //httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.ContentType = "application/json-patch+json; charset=utf-8";
            httpWebRequest.Method = "PATCH";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + oAuth);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "[  { \"ReferenceId\": \"a123\"  } ]";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = reader.ReadToEnd();
                            Logger.Log(result);
                        }
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            Logger.Log(error);
                        }
                    }

                }
            }
        }
    }
}