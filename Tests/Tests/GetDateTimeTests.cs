using System.Globalization;
using Tests.Utilities;
using System.Net;
using Application.Constants;


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

            var result = program.GetDateTime(LocationUrls.WorldTimeTorontoUrl);

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
                program.GetDateTime(LocationUrls.WorldTimeTorontoUrl);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            var expectedErrorMessage = "Bad Gateway error occurred while fetching the date and time.";
            Assert.AreEqual(expectedErrorMessage, result);
        }

        [TestMethod]
        public void GetDateTimeReturnsCorrectNotFoundError()
        {
            var mockHttpMessageHandler = MockHttpResponseMessage.
                MockFallureHttpResponse(HttpStatusCode.NotFound);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var program = new Program(httpClient);
            string result = null;

            try
            {
                program.GetDateTime(LocationUrls.WorldTimeTorontoUrl);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            var expectedErrorMessage = "The endpoint is Not found. Please, try later.";
            Assert.AreEqual(expectedErrorMessage, result);
        }
    }
}
