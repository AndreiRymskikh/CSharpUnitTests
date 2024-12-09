using Application;
using Moq.Protected;
using Moq;
using System.Net.Http.Json;
using System.Net;
using Application.WorldTime;
using System.Collections.Generic;

namespace Tests.Utilities
{
    public static class MockHttpResponseMessage
    {
        public static Mock<HttpMessageHandler> MockSuccessfullHttpResponse<T>(T expectedResponse)
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
