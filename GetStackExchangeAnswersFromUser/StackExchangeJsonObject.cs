using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;


namespace GetStackExchangeAnswersFromUser
{
    public class StackExchangeJsonObject
    {

        public StackExchangeJsonObject(string jsonLine)
        {
            JObject jobject = JObject.Parse(jsonLine);
            
        }

    }
}
