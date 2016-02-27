using System;
using System.Collections.Generic;

using James.ServiceStubs.Razor;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;
using Nancy.TinyIoc;

namespace James.ServiceStubs
{
    public class ServiceStubsBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<ITemplateEngine, RazorTemplateEngine>().AsMultiInstance();
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(x =>
                {
                    x.StatusCodeHandlers = new List<Type>
                    {
                        typeof (NotFoundErrorHandler),
                        typeof (DefaultStatusCodeHandler)
                    };
                });
            }
        }
    }
}