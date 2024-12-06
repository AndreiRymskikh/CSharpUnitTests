using Application;
using Moq.Protected;
using Moq;
using System.Net.Http.Json;
using System.Net;
using Application.WorldTime;

namespace Tests.Utilities
{
    public static class MockHttpResponseMessage
    {
        public static Mock<HttpMessageHandler> MockSuccessfullHttpResponse(string dateTime)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var expectedResponse = new WorldTimeAPIResponse
            {
                datetime = dateTime
            };

            mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonContent.Create(expectedResponse)
            });

            return mockHttpMessageHandler;
        }

        public static Mock<HttpMessageHandler> MockFallureHttpResponse(HttpStatusCode statusCode)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode
            });

            return mockHttpMessageHandler;
        }
    }
}
