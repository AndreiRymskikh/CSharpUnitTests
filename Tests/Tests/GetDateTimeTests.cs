﻿using Application;
using Moq.Protected;
using Moq;
using System.Globalization;
using Tests.Utilities;
using System.Net;


namespace Tests.Tests
{
    [TestClass]
    public class GetDateTimeTests
    {
        private const string dateTime = "2024-11-20T08:16:14.857403+01:00";

        [TestMethod]
        public void GetDateTimeReturnsExpectedDateTime()
        {
            var mockHttpMessageHandler = MockHttpResponseMessage.
                MockSuccessfullHttpResponse(dateTime);
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var program = new Program(httpClient);

            var result = program.GetDateTime("http://worldtimeapi.org/api/timezone/America/Toronto");

            var expectedDateTimeOffset = DateTimeOffset.ParseExact(
            dateTime,
            DateTimeFormats.DateTimeFormatToParse,
            CultureInfo.InvariantCulture
            );

            Assert.AreEqual(expectedDateTimeOffset, result);
        }

        [TestMethod]
        public void GetDateTimeReturnsCorrectBadGatewayError()
        {
            var mockHttpMessageHandler = MockHttpResponseMessage.
                MockFallureHttpResponse(HttpStatusCode.BadGateway);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var program = new Program(httpClient);
            string result = null;

            try
            {
                program.GetDateTime("http://worldtimeapi.org/api/timezone/America/Toronto");
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            var expectedErrorMessage = "Bad Gateway error occurred while fetching the date and time.";
            Assert.AreEqual(expectedErrorMessage, result);
        }
    }
}
