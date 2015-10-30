using System;
using Newtonsoft.Json;

namespace AutomatedTester.BrowserMob.HAR
{
    public class Cookie
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Path { get; set; }

        public string Domain { get; set; }
       
        public DateTime Expires { get; set; }

        public bool? HttpOnly { get; set; }

        public bool? Secure { get; set; }

        public string Comment { get; set; }

        [JsonConstructor]
        public Cookie(string expires)
        {
            DateTime expireToSet;
            if (DateTime.TryParse(expires, out expireToSet))
            {
                Expires = expireToSet;
            }
            else
                Expires = DateTime.Now.AddYears(10);
        }
    }
}