using System;

using Nancy.Bootstrapper;
using Nancy.Hosting.Self;

namespace James.ServiceStubs.Core
{
    public class ServiceStubsHost : NancyHost
    {
        public ServiceStubsHost(string baseUri)
            : this(new Uri(baseUri))
        {}

        public ServiceStubsHost(string baseUri, INancyBootstrapper bootstrapper)
            : this(new Uri(baseUri), bootstrapper)
        {}

        public ServiceStubsHost(Uri baseUri)
            : this(baseUri, new ServiceStubsBootstrapper())
        {}

        public ServiceStubsHost(Uri baseUri, INancyBootstrapper bootstrapper)
            : base(baseUri, bootstrapper)
        {}
    }
}