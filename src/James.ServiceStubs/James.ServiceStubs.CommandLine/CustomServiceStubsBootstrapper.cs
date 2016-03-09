using Nancy.TinyIoc;

namespace James.ServiceStubs.CommandLine
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

            container.Register<IRouteProvider>((c, p) => new FileRouteProvider(c.Resolve<ILogger>(), _filePath));
            //container.Register<ITemplateProvider>((c, p) => new FileTemplateProvider(c.Resolve<ILogger>(), _filePath));
        }
    }
}