using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using ReleaseMusicServerlessApi.Services;
using System;
using System.Collections.Generic;
using System.Net;



// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ReleaseMusicServerlessApi
{
    public class Functions
    {
        public Functions()
        {

        }

        public APIGatewayProxyResponse CreateUser(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                UserService userService = new UserService();
                userService.CreateUser(JsonConvert.DeserializeObject(request.Body));

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject("User register success !"),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }

        public APIGatewayProxyResponse GenerateTokenUser(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                UserService userService = new UserService();
                string token = userService.GenerateTokenUser(JsonConvert.DeserializeObject(request.Body));

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(token),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }

        public APIGatewayProxyResponse GetMusicReleases(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                UserService userService = new UserService();

                if (!request.Headers.ContainsKey("x-access-token"))
                    throw new Exception("x-access-token Header is mandatory !");

                bool tokenIsValid = userService.CheckToken(request.Headers["x-access-token"]);
                if (!tokenIsValid)
                    throw new Exception("Token invalid !");

                dynamic result = new SpotifyService().GetReleasesSpotify(null);

                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(""),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };

                return response;
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }
    }
}
