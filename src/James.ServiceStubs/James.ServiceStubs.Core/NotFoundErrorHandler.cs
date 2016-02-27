using System;

using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses.Negotiation;

namespace James.ServiceStubs.Core
{
    public class NotFoundErrorHandler : IStatusCodeHandler
    {
        private readonly IStatusCodeHandler _defaultStatusCodeHandler;

        public NotFoundErrorHandler(IResponseNegotiator responseNegotiator)
        {
            _defaultStatusCodeHandler = new DefaultStatusCodeHandler(responseNegotiator);
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            if (context.Items.ContainsKey(ConfiguredModule.RouteResolvedKey) == false)
            {
                Console.WriteLine($"FAILURE:  {context.ResolvedRoute.Description.Path}");
            }

            _defaultStatusCodeHandler.Handle(statusCode, context);
        }
    }
}