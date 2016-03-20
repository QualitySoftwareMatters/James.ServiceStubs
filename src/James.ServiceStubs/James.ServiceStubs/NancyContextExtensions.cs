using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using Nancy;

using Newtonsoft.Json;

using Formatting = Newtonsoft.Json.Formatting;

namespace James.ServiceStubs
{
    public static class NancyContextExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = GetSettings();

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectDictionaryConverter());

            return settings;
        }

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

            using (TextReader reader = new StreamReader(context.Request.Body))
            {

                Dictionary<string, object> body;

                if (context.Request.Headers.Accept.First().Item1 == "application/json")
                {
                    body = JsonConvert.DeserializeObject<Dictionary<string, object>>(reader.ReadToEnd(), SerializerSettings);
                }
                else
                {
                    var doc = new XmlDocument();
                    doc.Load(reader);

                    var json = JsonConvert.SerializeXmlNode(doc, Formatting.None, true);
                    body = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, SerializerSettings);
                }

                return body;
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