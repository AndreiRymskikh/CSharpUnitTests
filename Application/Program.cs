using Application;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

public class Program
{
    private readonly HttpClient _client;

    public Program(HttpClient client)
    {
        _client = client;
    }

    public void Run()
    {
        var canadaDateTime = GetDateTime("http://worldtimeapi.org/api/timezone/America/Toronto");
        var ukDateTime = GetDateTime("http://worldtimeapi.org/api/timezone/Europe/London");

        Console.WriteLine("Please, specify your location (UK or Canada):");
        string location = Console.ReadLine();

        DisplayDateTime("UK Time", ukDateTime);
        DisplayDateTime("Canada Time", canadaDateTime);

        DisplayTimeDifference(location, ukDateTime.DateTime, canadaDateTime.DateTime);
    }

    //What will happen in case of empty response? Or responce will be 500 error
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
            throw new Exception("Bad Gateway error occurred while fetching the date and time.");
        }

    }

    public void DisplayDateTime(string label, DateTimeOffset dateTime)
    {
        Console.WriteLine($"{label}: {dateTime.ToString(DateTimeFormats.DateTimeFormatter)}");
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
                Console.WriteLine("Wrong location value. It can be UK or Canada");
                break;
        }
    }

    public static void Main(string[] args)
    {
        using HttpClient client = new HttpClient();
        var program = new Program(client);
        program.Run();
    }
}