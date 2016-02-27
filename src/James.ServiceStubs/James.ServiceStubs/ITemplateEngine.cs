using System.Collections.Generic;

namespace James.ServiceStubs
{
    public interface ITemplateEngine
    {
        string Parse(string templateKey, IDictionary<string, object> model);
    }
}
