using Application.Constants;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Application.Weather
{
    internal class OpenMeteoApi
    {
        private readonly HttpClient _client;
        private string currentTime;
        private double currentTemperature;

        public OpenMeteoApi(HttpClient client)
        {
            _client = client;
            currentTime = new DateTime(
                DateTime.UtcNow.Year, 
                DateTime.UtcNow.Month, 
                DateTime.UtcNow.Day, 
                DateTime.UtcNow.Hour, 
                0, 
                0).
                  ToString(DateTimeFormats.OpenMeteoDateTimeFormat);
        }

        public JObject GetTemperatureByLocation(string location)
        {
            HttpResponseMessage response = null;
            JObject data;

            try
            {
                if (location == LocationInput.UK.ToString())
                {
                    response = _client.GetAsync(LocationUrls.OpenMeteoLondonUrl).Result;
                }
                else if (location == LocationInput.Canada.ToString())
                {
                    response = _client.GetAsync(LocationUrls.OpenMeteoTorontoUrl).Result;
                }
                else
                {
                    throw new Exception(ErrorMessages.WrongLocationErrorMsg);
                }

                string responseBody = response.Content.ReadAsStringAsync().Result;
                data = JObject.Parse(responseBody);

                return data;
            }
            catch (Exception ex) when ((ex.InnerException is HttpRequestException httpRequestException &&
            httpRequestException.StatusCode == HttpStatusCode.BadGateway) || response.StatusCode == HttpStatusCode.BadGateway)
            {
                throw new Exception(ErrorMessages.BadGatewayErrorMsg);
            }
            catch (Exception ex) when ((ex.InnerException is HttpRequestException httpRequestException &&
            httpRequestException.StatusCode == HttpStatusCode.NotFound) || response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception(ErrorMessages.NotFoundErrorMsg);
            }
        }

        public void FindTemperatureForCurrentTime(string location, JObject data) 
        {
            // Find the temperature for the current time
            var times = data["hourly"]["time"];
            var temperatures = data["hourly"]["temperature_2m"];
            int timeIndex = -1;

            for (int i = 0; i < times.Count(); i++)
            {
                if (times[i].ToString() == currentTime)
                {
                    timeIndex = i;
                    break;
                }
            }

            if (timeIndex != -1)
            {
                currentTemperature = temperatures[timeIndex].ToObject<double>();
                Console.WriteLine($"The current temperature in {location} at {currentTime} is {currentTemperature}°C");
            }
            else
            {
                throw new Exception(ErrorMessages.CurrentTimeNotFound);
            }
        }

        public void OutputTemperatureForCurrentLocation(string location)
        {
            var data = GetTemperatureByLocation(location);
            FindTemperatureForCurrentTime(location, data);
        }
    }
}
