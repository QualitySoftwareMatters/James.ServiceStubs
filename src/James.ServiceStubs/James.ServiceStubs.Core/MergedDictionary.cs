using System;
using System.Collections.Generic;
using System.Linq;

namespace James.ServiceStubs.Core
{
    public class MergedDictionary<TValue> : Dictionary<string, TValue>
    {
        public MergedDictionary(params IDictionary<string, TValue>[] dictionaries)
            : base(Merge(dictionaries), StringComparer.InvariantCultureIgnoreCase)
        {}

        private static IDictionary<string, TValue> Merge(params IDictionary<string, TValue>[] dictionaries)
        {
            var listOfDictionaries = new List<IDictionary<string, TValue>>();
            listOfDictionaries.AddRange(dictionaries);

            dictionaries = listOfDictionaries.Where(dict => dict != null).ToArray();

            var pairs = dictionaries
                .SelectMany(dict => dict);

            var output = new Dictionary<string, TValue>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var pair in pairs)
            {
                if (!output.ContainsKey(pair.Key))
                {
                    output.Add(pair.Key, pair.Value);
                }
            }

            return output;
        }
    }
}