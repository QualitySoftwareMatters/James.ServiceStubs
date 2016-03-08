using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace James.ServiceStubs
{
    public class FileRouteProvider : IRouteProvider
    {
        private readonly ILogger _logger;
        private readonly string _routeConfigPath;
         
        public FileRouteProvider(ILogger logger)
            : this(logger, Path.Combine(Environment.CurrentDirectory, "routes.json"))
        { }

        public FileRouteProvider(ILogger logger, string path)
        {
            _logger = logger;
            _routeConfigPath = path;
        }

        private List<Route> _cachedRoutes;
         
        public List<Route> GetRoutes()
        {
            if (_cachedRoutes == null)
            {
                if (File.Exists(_routeConfigPath))
                {
                    var json = File.ReadAllText(_routeConfigPath);
                    _cachedRoutes = JsonConvert.DeserializeObject<List<Route>>(json);
                }
                else
                {
                    _logger.Warn("routes.json file does not exist.  No routes will be loaded.");
                }
            }

            _cachedRoutes = _cachedRoutes ?? new List<Route>();
            return _cachedRoutes;
        } 
    }
}