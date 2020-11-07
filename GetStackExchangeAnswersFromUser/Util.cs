using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GetStackExchangeAnswersFromUser
{
    public class Util
    {

        public static string GetJsonFromUrl(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            httpWebRequest.Method = "GET";
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string responseText = string.Empty;
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                responseText = streamReader.ReadToEnd();
            }
            return responseText;
        }


      


    }
}
