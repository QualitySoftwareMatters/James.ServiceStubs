using System.Collections.Generic;

namespace James.ServiceStubs.Core
{
    public interface ITemplateProvider
    {
        string GetContentsFor(string templateKey, IDictionary<string, object> parameters);
    }
}