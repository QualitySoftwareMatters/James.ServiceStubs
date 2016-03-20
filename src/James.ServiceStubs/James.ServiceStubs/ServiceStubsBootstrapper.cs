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

            container.Register<ILogger, ConsoleLogger>();
            container.Register<IRouteProvider>((c,p) => new FileRouteProvider(c.Resolve<ILogger>(), c.Resolve<IFileProvider>()));
            container.Register<ITemplateProvider>((c, p) => new FileTemplateProvider(c.Resolve<ILogger>(), c.Resolve<IFileProvider>()));
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

        //protected override IEnumerable<Type> ModelBinders => new[] { typeof(DynamicModelBinder) };
    }
}