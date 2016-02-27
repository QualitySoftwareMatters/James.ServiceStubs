using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace James.ServiceStubs.Core
{
    public class RouteProvider : IRouteProvider
    {
        private readonly string _routeConfigPath;
         
        public RouteProvider()
        {
            _routeConfigPath = Path.Combine(Environment.CurrentDirectory, "routes.json");
        }

        private List<Route> _cachedRoutes = null;
         
        public List<Route> GetRoutes()
        {
            if (_cachedRoutes == null)
            {
                var json = File.ReadAllText(_routeConfigPath);
                _cachedRoutes = JsonConvert.DeserializeObject<List<Route>>(json);
            }

            return _cachedRoutes;
        } 
    }
}