using Application.Constants;
using Application.Weather;
using Newtonsoft.Json.Linq;
using System.Reflection;


namespace Tests.Tests
{
    [TestClass]
    public class FindTemperatureForCurrentTimeTests
    {
        private const string weatherData = "{\"latitude\":34,\"longitude\":-79,\"generationtime_ms\":0.04,\"utc_offset_seconds\":0," +
            "\"timezone\":\"GMT\",\"timezone_abbreviation\":\"GMT\",\"elevation\":55.1,\"hourly_units\":{\"time\":\"iso8601\"," +
            "\"temperature_2m\":\"°C\"},\"hourly\":{\"time\":[\"2024-12-09T12:00\",\"2024-12-09T13:00\"]," +
            "\"temperature_2m\":[13.1,12.9]}}";

        [TestMethod]
        public void FindTemperatureForCurrentTimeReturnsExpectedData()
        {
            var jsonObjectData = JObject.Parse(weatherData);
            var openMeteoApi = new OpenMeteoApi(null);

            var fieldInfoCurrentTime = typeof(OpenMeteoApi).GetField(
                "currentTime",
                BindingFlags.NonPublic |
                BindingFlags.Instance);
            fieldInfoCurrentTime.SetValue(openMeteoApi, new DateTime(2024, 12, 9, 12, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm"));


            openMeteoApi.FindTemperatureForCurrentTime(LocationInput.Canada.ToString(), jsonObjectData);

            var fieldInfoCurrentTemperature = typeof(OpenMeteoApi).GetField(
                "currentTemperature",
                BindingFlags.NonPublic |
                BindingFlags.Instance);
            var actualTemperature = (double)fieldInfoCurrentTemperature.GetValue(openMeteoApi);

            Assert.AreEqual(13.1, actualTemperature);
        }

        [TestMethod]
        public void FindTemperatureForCurrentTimeReturnsExpectedError()
        {
            var jsonObjectData = JObject.Parse(weatherData);
            var openMeteoApi = new OpenMeteoApi(null);
            string actualErrorMsg = null;

            try
            {
                openMeteoApi.FindTemperatureForCurrentTime(LocationInput.Canada.ToString(), jsonObjectData);
            }
            catch (Exception ex)
            {
                actualErrorMsg = ex.Message;
            }

            Assert.AreEqual(ErrorMessages.CurrentTimeNotFound, actualErrorMsg);
        }
    }
}
