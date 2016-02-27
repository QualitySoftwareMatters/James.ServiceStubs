using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace James.ServiceStubs.Core
{
    public class TokenPoker : ITokenPoker
    {
        public string PokeData(string value, IDictionary<string, object> data)
        {
            var reg = new Regex(@"{[\w\.]+}");

            foreach (Match match in reg.Matches(value))
            {
                var key = match.Value.Substring(1, match.Value.Length - 2);
                value = value.Replace(match.Value, GetReplacementValue(key, data));
            }

            return value;
        }

        private string GetReplacementValue(string key, IDictionary<string, object> data)
        {
            var segments = key.Split('.');
            var segmentKey = segments.First();
           
            if (segments.Length == 1)
            {
                return data[segmentKey].ToString();
            }
            else
            { 
                var newKey = segments.Where(s => s != segments.First()).ToList().Aggregate((i, j) => i + "." + j);
                return GetReplacementValue(newKey, data[segmentKey] as IDictionary<string, object>);
            }
        }
    }
}