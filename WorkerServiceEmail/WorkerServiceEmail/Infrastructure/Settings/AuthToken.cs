using Marvelous.Contracts.Enums;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace WorkerServiceEmail.Infrastructure
{
    public class AuthToken
    {
        public async Task<RestResponse<T>> SendRequestAsync<T>(string url, string path, string jwtToken = "null")
        {   
            var request = new RestRequest(path);
            var client = new RestClient(new RestClientOptions(url)
            {
                Timeout = 300000
            });
            client.Authenticator = new JwtAuthenticator(jwtToken);
            client.AddDefaultHeader(nameof(Microservice), Microservice.MarvelousEmailSender.ToString());
            var response = await client.ExecuteAsync<T>(request);
            Console.WriteLine(response.GetType());
            //CheckTransactionError(response);
            return response;
        }

        private static void CheckTransactionError<T>(RestResponse<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                //case HttpStatusCode.RequestTimeout:
                //    throw new RequestTimeoutException($"Request Timeout {response.ErrorException!.Message}");
                //case HttpStatusCode.ServiceUnavailable:
                //    throw new ServiceUnavailableException($"Service Unavailable {response.ErrorException!.Message}");
                //default:
                //    throw new BadGatewayException($"Error on {service}. {response.ErrorException!.Message}");
            }
            //if (response.Data is null)
            //    throw new BadGatewayException($"Content equal's null {response.ErrorException!.Message}");
        }
    }
}
