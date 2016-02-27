using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nancy;

using Newtonsoft.Json;

namespace James.ServiceStubs
{
    public static class NancyContextExtensions
    {
        public static IDictionary<string, object> GetParameters(this NancyContext context)
        {
            var body = GetBody(context);
            var cookies = context.Request.Cookies.ToDictionary(k => k.Key, k => (object) k.Value);
            var form = ConvertDynamicDictionary(context.Request.Form);
            var headers = context.Request.Headers.ToDictionary(k => k.Key.Replace("-", ""), k => (object) k.Value.FirstOrDefault());
            var parameters = ConvertDynamicDictionary(context.Parameters);
            var query = ConvertDynamicDictionary(context.Request.Query);

            var output = new Dictionary<string, object>(parameters, StringComparer.InvariantCultureIgnoreCase)
            {
                {"Body", body},
                {"Cookies", cookies},
                {"Form", form},
                {"Headers", headers},
                {"Query", query}
            };

            return output;
        }

        private static IDictionary<string, object> GetBody(NancyContext context)
        {
            if (context.Request.Body == null || context.Request.Form.Count > 0) return new Dictionary<string, object>();

            using (TextReader te = new StreamReader(context.Request.Body))
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new ObjectDictionaryConverter());

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(te.ReadToEnd(), settings);
            }
        }

        private static IDictionary<string, object> ConvertDynamicDictionary(DynamicDictionary dictionary)
        {
            return dictionary.GetDynamicMemberNames().ToDictionary(
                    memberName => memberName,
                    memberName => dictionary[memberName],
                    StringComparer.InvariantCultureIgnoreCase);
        }
    }
}