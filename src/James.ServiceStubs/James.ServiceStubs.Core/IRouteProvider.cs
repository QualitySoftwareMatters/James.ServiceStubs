using System.Collections.Generic;

namespace James.ServiceStubs.Core
{
    public interface IRouteProvider
    {
        List<Route> GetRoutes();
    }
}