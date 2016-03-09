using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace James.ServiceStubs
{
    public class FileRouteProvider : IRouteProvider
    {
        private readonly ILogger _logger;
        private readonly IFileProvider _fileProvider;
        private readonly string _routeConfigPath;
        public const string RoutesFileDoesNotExistMessage = "routes.json file does not exist.No routes will be loaded.";
         
        public FileRouteProvider(ILogger logger, IFileProvider fileProvider)
            : this(logger, fileProvider, Environment.CurrentDirectory)
        { }

        public FileRouteProvider(ILogger logger, IFileProvider fileProvider, string filePath)
        {
            _logger = logger;
            _fileProvider = fileProvider;
            _routeConfigPath = Path.Combine(filePath, "routes.json");
        }

        private List<Route> _cachedRoutes;
         
        public List<Route> GetRoutes()
        {
            if (_cachedRoutes == null)
            {
                if (_fileProvider.Exists(_routeConfigPath))
                {
                    var json = _fileProvider.ReadAllText(_routeConfigPath);
                    _cachedRoutes = JsonConvert.DeserializeObject<List<Route>>(json);
                }
                else
                {
                    _logger.Warn(RoutesFileDoesNotExistMessage);
                }
            }

            _cachedRoutes = _cachedRoutes ?? new List<Route>();
            return _cachedRoutes;
        } 
    }
}