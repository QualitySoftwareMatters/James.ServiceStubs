using System;
using System.Collections.Generic;
using System.IO;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

namespace James.ServiceStubs.UnitTests.FileRouteProviderTests
{
    public class given_routes_file_does_not_exist_when_getting_routes
    {
        private ILogger _logger;
        private List<Route> _routes;

        [OneTimeSetUp]
        public void SetUp()
        {
            var customFilePath = Path.Combine(Environment.CurrentDirectory, "hidden");
            _logger = Substitute.For<ILogger>();
            var fileProvider = Substitute.For<IFileProvider>();
            var provider = new FileRouteProvider(_logger, fileProvider, customFilePath);

            fileProvider.Exists(Path.Combine(customFilePath, "routes.json")).Returns(false);
            _routes = provider.GetRoutes();
        }
   
        [Test]
        public void given_routes_file_does_not_exist_when_getting_routes_should_log_warning()
        {
            _logger.Received(1).Warn(FileRouteProvider.RoutesFileDoesNotExistMessage);
        }

        [Test]
        public void given_routes_file_does_not_exist_when_getting_routes_should_return_empty_list()
        {
            _routes.Should().BeEmpty();
        }
    }
}