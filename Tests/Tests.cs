using Application;
using Moq.Protected;
using Moq;
using System.Globalization;
using System.Net.Http.Json;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GetDateTimeReturnsExpectedDateTime()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var expectedResponse = new WorldTimeAPIResponse
            {
                datetime = "2024-11-20T08:16:14.857403+01:00"
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

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var program = new Program(httpClient);

            var result = program.GetDateTime("http://worldtimeapi.org/api/timezone/America/Toronto");

            var expectedDateTimeOffset = DateTimeOffset.ParseExact(
            "2024-11-20T08:16:14.857403+01:00",
            "yyyy-MM-dd'T'HH:mm:ss.FFFFFFzzz",
            CultureInfo.InvariantCulture
            );
            Assert.AreEqual(expectedDateTimeOffset, result);
        }

        [TestMethod]
        public void GetTimeDifferenceReturnlsCorrectTimeDifferenceUkLocation()
        {
            var ukDateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(0));
            var canadaDateTime = new DateTimeOffset(2024, 11, 20, 03, 16, 14, TimeSpan.FromHours(-5));
            var program = new Program(null);

            var sw = new StringWriter();
        
            Console.SetOut(sw);
            program.DisplayTimeDifference("UK", ukDateTime, canadaDateTime);
            var result = sw.ToString().Trim();

            var expectedOutput = "You are 6h ahead of Canada";
            Assert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetTimeDifferenceReturnlsCorrectTimeDifferenceCanadaLocation()
        {
            var ukDateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(0));
            var canadaDateTime = new DateTimeOffset(2024, 11, 20, 03, 16, 14, TimeSpan.FromHours(-5));
            var program = new Program(null);

            var sw = new StringWriter();

            Console.SetOut(sw);
            program.DisplayTimeDifference("Canada", ukDateTime, canadaDateTime);
            var result = sw.ToString().Trim();

            var expectedOutput = "You are 6h behind UK";
            Assert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetTimeDifferenceReturnlsCorrectMessageWrongLocation()
        {
            var ukDateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(0));
            var canadaDateTime = new DateTimeOffset(2024, 11, 20, 03, 16, 14, TimeSpan.FromHours(-5));
            var program = new Program(null);

            var sw = new StringWriter();

            Console.SetOut(sw);
            program.DisplayTimeDifference("WrongLocation", ukDateTime, canadaDateTime);
            var result = sw.ToString().Trim();

            var expectedOutput = "Wrong location value. It can be UK or Canada";
            Assert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void DisplayDateTimeOutputCorrectFormat()
        {
            var dateTime = new DateTimeOffset(2024, 11, 20, 08, 16, 14, TimeSpan.FromHours(1));
            var program = new Program(null);
            var expectedOutput = "Test Label: Wednesday 20 November 2024 08:16:14";

            var sw = new StringWriter();
            
            Console.SetOut(sw);
            program.DisplayDateTime("Test Label", dateTime);
            var result = sw.ToString().Trim();

            Assert.AreEqual(expectedOutput, result);
        }
    }
}