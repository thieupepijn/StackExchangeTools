using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetStackExchangeAnswersFromUser
{
    public class Site : IStackExchangeItem
    {
        public string Name { get; private set; }

        public Site(JToken jtoken)
        {
          Name = jtoken["name"].Value<string>();
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

        public string Write()
        {
            return Name;
        }
    }
}
