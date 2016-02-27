using System.Collections.Generic;

namespace James.ServiceStubs.Core
{
    public interface ITemplateEngine
    {
        string Parse(string templateKey, IDictionary<string, object> model);
    }
}
