using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace ReleaseMusicServerlessApi.Tests
{
    public class FunctionTest
    {
        private readonly ITestOutputHelper output;
        private readonly Functions functions;

        public FunctionTest(ITestOutputHelper output)
        {
            this.output = output;
            this.functions = new Functions();
        }

        [Fact]
        public void TestGetStatus()
        {
            TestLambdaContext context = new TestLambdaContext();
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            request = new APIGatewayProxyRequest() { };
            response = this.functions.GetStatus(request, context);

            output.WriteLine(response.Body);

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public void TestCreateUser()
        {
            TestLambdaContext context = new TestLambdaContext();
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            dynamic body = new
            {
                email = "gabriel@hotmail.com",
                password = "123"
            };

            request = new APIGatewayProxyRequest() { Body = JsonConvert.SerializeObject(body) };
            response = this.functions.CreateUser(request, context);

            output.WriteLine(response.Body);

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public string TestGenerateTokenUser()
        {
            TestLambdaContext context = new TestLambdaContext();
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            dynamic body = new
            {
                email = "carol@gmail.com",
                password = "123"
            };

            request = new APIGatewayProxyRequest() { Body = JsonConvert.SerializeObject(body) };
            response = this.functions.GenerateTokenUser(request, context);

            output.WriteLine(response.Body);

            Assert.Equal(200, response.StatusCode);

            return response.Body;
        }

        [Fact]
        public dynamic TestGetMusicReleases()
        {
            TestLambdaContext context = new TestLambdaContext();
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var headers = new Dictionary<string, string>();
            headers.Add("x-access-token", "tO1GTUoQFq63YQ/weayAC1ST3g9w+ITkoKosxJB34776Kxq0RUfcOIg4UtJ3rTcVYnIGZETtAEf0tnGnPDNjMA==:w+98CdgLjjWjOvgCiXm0Dw==:NvMw+ilsFeFXGK9mHmAW9b12bLyNH2FvCOob1XmdmwfkjU4lehyo/HDemp0N6+vp");

            request = new APIGatewayProxyRequest() { Headers =  headers};
            response = this.functions.GetMusicReleases(request, context);

            output.WriteLine(JsonConvert.SerializeObject(response));

            Assert.Equal(200, response.StatusCode);

            return response;
        }

        [Fact]
        public dynamic TestGetTracksByQuery()
        {
            TestLambdaContext context = new TestLambdaContext();
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var headers = new Dictionary<string, string>();
            headers.Add("x-access-token", "tO1GTUoQFq63YQ/weayAC1ST3g9w+ITkoKosxJB34776Kxq0RUfcOIg4UtJ3rTcVYnIGZETtAEf0tnGnPDNjMA==:w+98CdgLjjWjOvgCiXm0Dw==:NvMw+ilsFeFXGK9mHmAW9b12bLyNH2FvCOob1XmdmwfkjU4lehyo/HDemp0N6+vp");

            var pathParameters = new Dictionary<string, string>()
            {
                { "key", "genre" },
                { "query", "rock" },
                { "type", "track" }
            };

            request = new APIGatewayProxyRequest() { Headers = headers, PathParameters = pathParameters };
            response = this.functions.GetMusicsByQuery(request, context);

            output.WriteLine(JsonConvert.SerializeObject(response));

            Assert.Equal(200, response.StatusCode);

            return response;
        }

        [Fact]
        public dynamic TestGetMusicYouTubeEmbed()
        {
            TestLambdaContext context = new TestLambdaContext();
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var headers = new Dictionary<string, string>();
            headers.Add("x-access-token", "tO1GTUoQFq63YQ/weayAC1ST3g9w+ITkoKosxJB34776Kxq0RUfcOIg4UtJ3rTcVYnIGZETtAEf0tnGnPDNjMA==:w+98CdgLjjWjOvgCiXm0Dw==:CZV1JT7UJ0M+n9IfVaQSgZshcQCBYdrPWwsA1BoPf/jnKwB09S+E6XymUqqoBHRI");

            var pathParameters = new Dictionary<string, string>()
            {
                { "q", "Justin Bieber Yumi" }
            };

            request = new APIGatewayProxyRequest() { Headers = headers, PathParameters = pathParameters };
            response = this.functions.GetMusicYouTubeEmbed(request, context);

            output.WriteLine(JsonConvert.SerializeObject(response));

            Assert.Equal(200, response.StatusCode);

            return response;
        }
    }
}
