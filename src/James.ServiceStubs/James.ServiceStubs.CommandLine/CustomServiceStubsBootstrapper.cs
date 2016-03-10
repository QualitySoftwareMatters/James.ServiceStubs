using James.ServiceStubs;

using Nancy.TinyIoc;

namespace ServiceStubs
{
    public class CustomServiceStubsBootstrapper : ServiceStubsBootstrapper
    {
        private readonly string _filePath;

        public CustomServiceStubsBootstrapper(string filePath)
        {
            _filePath = filePath;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<IRouteProvider>((c, p) => new FileRouteProvider(c.Resolve<ILogger>(), c.Resolve<IFileProvider>(), _filePath));
            container.Register<ITemplateProvider>((c, p) => new FileTemplateProvider(c.Resolve<ILogger>(), c.Resolve<IFileProvider>(), _filePath));
        }
    }
}