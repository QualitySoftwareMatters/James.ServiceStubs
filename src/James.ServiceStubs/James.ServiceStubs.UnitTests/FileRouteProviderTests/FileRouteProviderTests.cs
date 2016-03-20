using System;
using System.IO;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

namespace James.ServiceStubs.UnitTests.FileRouteProviderTests
{
    [TestFixture]
    public class FileRouteProviderTests
    {
        [Test]
        public void given_routes_file_exists_when_getting_routes_should_return_routes()
        {
            var logger = Substitute.For<ILogger>();
            var fileProvider = new FileProvider();
            var provider = new FileRouteProvider(logger, fileProvider);

            var routes = provider.GetRoutes();

            routes.Count.Should().Be(4);
        }

        [Test]
        public void given_routes_file_exists_and_custom_file_path_when_getting_routes_should_return_routes()
        {
            var customFilePath = Path.Combine(Environment.CurrentDirectory, "CustomFilePath");
            var logger = Substitute.For<ILogger>();
            var fileProvider = new FileProvider();
            var provider = new FileRouteProvider(logger, fileProvider, customFilePath);

            var routes = provider.GetRoutes();

            routes.Count.Should().Be(1);
        }
    }
}
