using System;

using FluentAssertions;

using NUnit.Framework;

namespace James.ServiceStubs.Core.UnitTests.Sample
{
    [TestFixture]
    public class SystemGatewayTests
    {
        [Test]
        public void given_service_is_down_when_getting_should_throw()
        {
            var baseUri = new Uri("http://localhost:5000");
            var gateway = new SystemGateway(baseUri);

            Action action = () => gateway.Get();

            action
                .ShouldThrow<ApplicationException>()
                .WithMessage(SystemGateway.GeneralError)
                .WithInnerException<AggregateException>();
        }

        [Test]
        public void given_service_is_up_when_getting_should_return_system_info()
        {
            var baseUri = new Uri("http://localhost:5000/");

            using (var host = new ServiceStubsHost(baseUri))
            {
                host.Start();
                var gateway = new SystemGateway(baseUri);

                var info = gateway.Get();

                var expectedMachineName = Environment.MachineName;
                var expectedDateTime = new DateTimeOffset(2012, 01, 01, 11, 24, 15, TimeSpan.FromHours(-5));

                info.ServerName.Should().Be(expectedMachineName);
                info.ServerTimeStamp.Should().Be(expectedDateTime);
            }
        }

        [Test]
        public void given_resource_is_not_found_when_getting_should_throw()
        {
            using (var host = new ServiceStubsHost(new Uri("http://localhost:5000/")))
            {
                host.Start();
                var gateway = new SystemGateway(new Uri("http://localhost:5000/notfound/"));

                Action action = () => gateway.Get();

                action
                    .ShouldThrow<ApplicationException>()
                    .WithMessage(SystemGateway.NotFoundErrort);
            }
        }
    }

    public class SystemInfo
    {
        public string ServerName { get; set; }
        public DateTimeOffset ServerTimeStamp { get; set; }
    }
}
