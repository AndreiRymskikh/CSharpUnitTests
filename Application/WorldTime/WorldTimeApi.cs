using Application.Constants;
using System.Globalization;
using System.Net.Http.Json;
using System.Net;

namespace Application.WorldTime
{
    internal class WorldTimeApi
    {
        private readonly HttpClient _client;
        private string dateTimeStr;

        public WorldTimeApi(HttpClient client)
        {
            _client = client;
        }

        public DateTimeOffset GetDateTime(string url)
        {
            try
            {
                var response = _client.GetFromJsonAsync<WorldTimeAPIResponse>(url).Result;

                return DateTimeOffset.ParseExact(
                    response.datetime,
                    DateTimeFormats.DateTimeFormatToParse,
                    CultureInfo.InvariantCulture
                    );
            }
            catch (Exception ex) when (ex.InnerException is HttpRequestException httpRequestException &&
            httpRequestException.StatusCode == HttpStatusCode.BadGateway)
            {
                throw new Exception(ErrorMessages.BadGatewayErrorMsg);
            }
            catch (Exception ex) when (ex.InnerException is HttpRequestException httpRequestException &&
            httpRequestException.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception(ErrorMessages.NotFoundErrorMsg);
            }
        }

        public void DisplayDateTime(string label, DateTimeOffset dateTime)
        {
            dateTimeStr = dateTime.ToString(DateTimeFormats.DateTimeFormatter);

            Console.WriteLine($"{label}: {dateTimeStr}");
        }

        public void DisplayTimeDifference(string location, DateTime ukTime, DateTime canadaTime)
        {
            double timeDifference = ukTime.Subtract(canadaTime).TotalHours;

            switch (location)
            {
                case "UK":
                    Console.WriteLine($"You are {timeDifference}h ahead of Canada");
                    break;
                case "Canada":
                    Console.WriteLine($"You are {timeDifference}h behind UK");
                    break;
                default:
                    Console.WriteLine(ErrorMessages.WrongLocationErrorMsg);
                    break;
            }
        }

        public void OutputWorldTimeData(string location)
        {
            var canadaDateTime = GetDateTime(LocationUrls.WorldTimeTorontoUrl);
            var ukDateTime = GetDateTime(LocationUrls.WorldTimeLondonUrl);

            DisplayDateTime("UK Time", ukDateTime);
            DisplayDateTime("Canada Time", canadaDateTime);

            DisplayTimeDifference(location, ukDateTime.DateTime, canadaDateTime.DateTime);
        }
    }
}
