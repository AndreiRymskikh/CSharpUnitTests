using Application.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weather
{
    internal class OpenMeteoApi
    {
        private readonly HttpClient _client;

        public OpenMeteoApi(HttpClient client)
        {
            _client = client;
        }

        public async void GetTemperatureByLocation(string location)
        {
            HttpResponseMessage response;

              if (location == "UK")
            {
                response = await _client.GetAsync(LocationUrls.OpenMeteoLondonUrl);
            }
              else if(location == "Canada")
            {
                response = await _client.GetAsync(LocationUrls.OpenMeteoTorontoUrl);
            }
              else
            {
                throw new Exception("Wrong location was specified");
            }
               
              string responseBody = await response.Content.ReadAsStringAsync();
              JObject data = JObject.Parse(responseBody);

              // Get the current time rounded down to the nearest hour in ISO 8601 format
              DateTime now = DateTime.UtcNow;
              string currentTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).
                    ToString(DateTimeFormats.OpenMeteoDateTimeFormat);
              double currentTemperature;

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
                  Console.WriteLine($"The current temperature at {currentTime} is {currentTemperature}°C");
              }
              else
              {
                  Console.WriteLine("Current time not found in the data.");
              }        
        }
    }
}
