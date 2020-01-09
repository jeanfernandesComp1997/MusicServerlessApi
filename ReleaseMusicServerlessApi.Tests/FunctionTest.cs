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
        public void TestCreateUser()
        {
            TestLambdaContext context = new TestLambdaContext();
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            dynamic body = new
            {
                email = "",
                password = ""
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
                email = "",
                password = ""
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
            headers.Add("x-access-token", "");

            request = new APIGatewayProxyRequest() { Headers =  headers};
            response = this.functions.GetMusicReleases(request, context);

            output.WriteLine(response.Body);

            Assert.Equal(200, response.StatusCode);

            return response;
        }
    }
}
