using System.Collections.Generic;

namespace James.ServiceStubs
{
    public interface IRouteProvider
    {
        List<Route> GetRoutes();
    }
}