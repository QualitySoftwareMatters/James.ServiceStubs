using System;
using System.Collections.Generic;
using System.IO;

namespace James.ServiceStubs.Core
{
    public class TemplateProvider : ITemplateProvider
    {
        public string GetContentsFor(string templateKey, IDictionary<string, object> parameters)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, templateKey);
            return File.ReadAllText(filePath);
        }
    }
}