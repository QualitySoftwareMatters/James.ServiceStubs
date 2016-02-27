using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace James.ServiceStubs.Sample
{
    public class SystemGateway
    {
        private readonly Uri _baseUri;
        public const string GeneralError = "Something is wrong with the service.  Please try again.";
        public const string NotFoundErrort = "The resource was not found.";

        public SystemGateway(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public SystemInfo Get()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = client.GetAsync("api/System").Result;

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new ApplicationException(NotFoundErrort);
                    }

                    return response.Content.ReadAsAsync<SystemInfo>().Result;
                }
                catch (AggregateException ex)
                {
                    throw new ApplicationException(GeneralError, ex);
                }
            }
        }
    }
}