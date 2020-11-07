using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GetStackExchangeAnswersFromUser
{
    public class Util
    {

        public static string GetJsonFromUrl(string url)
        {
            string responseText = string.Empty;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                httpWebRequest.Method = "GET";
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseText = streamReader.ReadToEnd();
                }
            }
            catch
            {
                string errorMessage = string.Format("Could not retrieve json-data from url: {0}", url);
                throw new Exception(errorMessage);
            }
            return responseText;
        }


        public static IJEnumerable<JToken> GetJsonTokensFromJsonString(string json, string url)
        {
            IJEnumerable<JToken> jtokens;
            try
            {
                JObject mainobject = JObject.Parse(json);
                jtokens = mainobject.First.Children().Children();
            }
            catch
            {
                string errorMessage = string.Format("Could not parse json-data retrieved from url: {0}", url);
                throw new Exception(errorMessage);
            }
            return jtokens;
        }


      


    }
}
