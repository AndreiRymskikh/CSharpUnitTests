using System.Globalization;
using Tests.Utilities;
using System.Net;
using Application.Constants;
using Application.WorldTime;
using Application.Weather;
using System;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


namespace Tests.Tests
{
    [TestClass]
    public class GetTemperatureByLocationTests
    {
        private const string weatherData = "{\"latitude\":34,\"longitude\":-79,\"generationtime_ms\":0.04,\"utc_offset_seconds\":0," +
            "\"timezone\":\"GMT\",\"timezone_abbreviation\":\"GMT\",\"elevation\":55.1,\"hourly_units\":{\"time\":\"iso8601\"," +
            "\"temperature_2m\":\"°C\"},\"hourly\":{\"time\":[\"2024-12-09T12:00\",\"2024-12-09T13:00\"]," +
            "\"temperature_2m\":[13.1,12.9]}}";
        private readonly OpenMeteoApiResponse expectedResponse = new OpenMeteoApiResponse
        {
            Latitude = 34,
            Longitude = -79,
            Generationtime_ms = 0.04,
            Utc_offset_seconds = 0,
            Timezone = "GMT",
            Timezone_abbreviation = "GMT",
            Elevation = 55.1,
            Hourly_units = new HourlyUnits
            {
                Time = "iso8601",
                Temperature_2m = "°C",
            },
            Hourly = new Hourly
            {
                Time = new List<string> { "2024-12-09T12:00", "2024-12-09T13:00" },
                Temperature_2m = new List<double> { 13.1, 12.9 }
            }
        };

        [TestMethod]
        public void GetTemperatureReturnsExpectedData()
        {
            var mockHttpMessageHandler = MockHttpResponseMessage.
                MockSuccessfullHttpResponse(expectedResponse);
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var openMeteoApi = new OpenMeteoApi(httpClient);

            string jsonString = openMeteoApi.GetTemperatureByLocation("UK").ToString();
            // Remove all whitespace characters from the string
            string resultStr = Regex.Replace(jsonString, @"\s+", "");

            Assert.AreEqual(weatherData, resultStr);
        }

        [TestMethod]
        public void GetDateTimeReturnsCorrectBadGatewayError()
        {
            var mockHttpMessageHandler = MockHttpResponseMessage.
                MockFallureHttpResponse(HttpStatusCode.BadGateway);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var worldTimeApi = new WorldTimeApi(httpClient);
            string result = null;

            try
            {
                worldTimeApi.GetDateTime(LocationUrls.WorldTimeTorontoUrl);
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
            var worldTimeApi = new WorldTimeApi(httpClient);
            string result = null;

            try
            {
                worldTimeApi.GetDateTime(LocationUrls.WorldTimeTorontoUrl);
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
