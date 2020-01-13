using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using ReleaseMusicServerlessApi.Models;
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

        public APIGatewayProxyResponse GetStatus(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Ok",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
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
                    Body = JsonConvert.SerializeObject(new { message = "User register success !" }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
        }

        public APIGatewayProxyResponse GenerateTokenUser(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                UserService userService = new UserService();
                dynamic response = userService.GenerateTokenUser(JsonConvert.DeserializeObject(request.Body));

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(response),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
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
                    Body = JsonConvert.SerializeObject(result),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "x-access-token", "text/plain" }, { "Access-Control-Allow-Origin", "*" } }
                };

                return response;
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "x-access-token", "text/plain" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
        }

        public APIGatewayProxyResponse GetMusicsByQuery(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                UserService userService = new UserService();

                if (!request.Headers.ContainsKey("x-access-token"))
                    throw new Exception("x-access-token Header is mandatory !");

                bool tokenIsValid = userService.CheckToken(request.Headers["x-access-token"]);
                if (!tokenIsValid)
                    throw new Exception("Token invalid !");

                dynamic result = new SpotifyService().GetTracksByQuery(request.PathParameters["key"], request.PathParameters["query"], request.QueryStringParameters["type"]);

                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(result),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "x-access-token", "text/plain" }, { "Access-Control-Allow-Origin", "*" } }
                };

                return response;
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "x-access-token", "text/plain" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
        }

        public APIGatewayProxyResponse GetMusicYouTubeEmbed(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                UserService userService = new UserService();

                if (!request.Headers.ContainsKey("x-access-token"))
                    throw new Exception("x-access-token Header is mandatory !");

                bool tokenIsValid = userService.CheckToken(request.Headers["x-access-token"]);
                if (!tokenIsValid)
                    throw new Exception("Token invalid !");

                dynamic result = new YouTubeService().GetEmbedVideo(request.QueryStringParameters["q"]);

                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(result),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "x-access-token", "text/plain" }, { "Access-Control-Allow-Origin", "*" } }
                };

                return response;
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(ex.Message),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "x-access-token", "text/plain" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
        }
    }
}
