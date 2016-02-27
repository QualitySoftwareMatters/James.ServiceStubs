using System.Collections.Generic;

namespace James.ServiceStubs.Core
{
    public interface ITokenPoker
    {
        string PokeData(string value, IDictionary<string, object> data);
    }
}