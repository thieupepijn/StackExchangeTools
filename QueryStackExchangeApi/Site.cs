using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryStackExchangeApi
{
    public class Site : IStackExchangeItem
    {
        public string Name { get; private set; }

        public string ApiName { get; private set; }

        public string IconUrl { get; private set; }

        public Site(JToken jtoken)
        {
            Name = jtoken["name"].Value<string>();
            ApiName = jtoken["api_site_parameter"].Value<string>();
            IconUrl = jtoken["icon_url"].Value<string>();
        }

        public static List<Site> GetSites(IJEnumerable<JToken> jtokens)
        {
            List<Site> sites = new List<Site>();
            foreach (JToken jtoken in jtokens)
            {
                Site site = new Site(jtoken);
                sites.Add(site);
            }
            return sites;
        }

        public string GetAnswersUrl(string userid)
        {
            string url = string.Format("https://api.stackexchange.com/2.2/users/{0}/answers?order=desc&sort=activity&site=[1}&filter=withbody", userid);
            return url;

        }

        public string Write()
        {
            return Name;
        }
    }
}
