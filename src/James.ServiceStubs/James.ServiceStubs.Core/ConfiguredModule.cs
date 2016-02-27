using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Nancy;

namespace James.ServiceStubs.Core
{
    public class ConfiguredModule : NancyModule
    {
        private readonly ITemplateEngine _engine;
        public const string RouteResolvedKey = "RouteResolved";

        public ConfiguredModule(IRouteProvider provider, ITemplateEngine engine)
        {
            _engine = engine;

            var routes = provider.GetRoutes();

            foreach (var route in routes)
            {
                switch (route.Type)
                {
                    case RequestType.Get:
                        Get[route.Template] = _ =>
                        {   
                            Func<Response> function = () => GetResponse(route, Context);
                            return ExecuteWithDelay(function, route.CurrentDelayInMilliseconds);
                        };
                        break;
                    case RequestType.Post:
                        Post[route.Template] = _ =>
                        {
                            Func<Response> function = () => GetResponse(route, Context);
                            return ExecuteWithDelay(function, route.CurrentDelayInMilliseconds);
                        };
                        break;
                    case RequestType.Put:
                        Put[route.Template] = _ =>
                        {
                            Func<Response> function = () => GetResponse(route, Context);
                            return ExecuteWithDelay(function, route.CurrentDelayInMilliseconds);
                        };
                        break;
                    case RequestType.Delete:
                        Put[route.Template] = _ =>
                        {
                            Func<Response> function = () => GetResponse(route, Context);
                            return ExecuteWithDelay(function, route.CurrentDelayInMilliseconds);
                        };
                        break;
                    default:
                        break;
                }
            }
        }

        private Response GetResponse(Route route, NancyContext context)
        {
            Console.WriteLine($"SUCCESS:  {Context.ResolvedRoute.Description.Path}");
            context.Items.Add(RouteResolvedKey, true);

            var contentType = context.Request.Headers.Accept.FirstOrDefault();
            var contentTypeString = contentType == null
                ? "application/json"
                : contentType.Item1;

            Response response = _engine.Parse(route.Path, Context.GetParameters());
            response
                .WithContentType($"{contentTypeString}")
                .WithStatusCode(route.Status);

            return response;
        }

        private Response ExecuteWithDelay(Func<Response> function, int delayInMilliseconds)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = function();

            stopwatch.Stop();
            var left = TimeSpan.FromMilliseconds(delayInMilliseconds) - stopwatch.Elapsed;
            Thread.Sleep(left > TimeSpan.Zero ? left : TimeSpan.Zero);

            return response;
        }
    }
}
