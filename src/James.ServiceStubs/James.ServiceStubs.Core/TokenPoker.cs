using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using James.ServiceStubs.Core.Rz;

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

                value = value.Replace(match.Value, GetReplacementValue(key, data.ToExpando()));
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

            var newKey = segments.Where(s => s != segments.First()).ToList().Aggregate((i, j) => i + "." + j);
            var newData = data[segmentKey] as IDictionary<string, object>;

            if (newData == null == false) return GetReplacementValue(newKey, newData);

            var type = data[segmentKey].GetType();
            var properties = type.GetProperties();

            newData = properties.Select(x => new KeyValuePair<string, object>(x.Name, x.GetValue(data[segmentKey], null)))
                .ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.InvariantCultureIgnoreCase);
            return GetReplacementValue(newKey, newData);
        }
    }
}