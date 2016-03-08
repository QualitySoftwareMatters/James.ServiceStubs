using System;

using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses.Negotiation;

namespace James.ServiceStubs
{
    public class NotFoundErrorHandler : IStatusCodeHandler
    {
        private readonly ILogger _logger;
        private readonly IStatusCodeHandler _defaultStatusCodeHandler;

        public NotFoundErrorHandler(ILogger logger, IResponseNegotiator responseNegotiator)
        {
            _logger = logger;
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
                _logger.Info($"(FAILURE) {context.ResolvedRoute.Description.Path}");
            }

            _defaultStatusCodeHandler.Handle(statusCode, context);
        }
    }
}