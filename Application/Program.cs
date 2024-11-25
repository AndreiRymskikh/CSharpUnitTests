using Application;
using System;
using System.Globalization;
using System.Net.Http.Json;

public class Program
{
    private readonly HttpClient _client;
    private readonly string _dateTimeFormatter = "dddd dd MMMM yyyy HH:mm:ss";

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

        DisplayTimeDifference(location, ukDateTime, canadaDateTime);
    }

    public DateTimeOffset GetDateTime(string url)
    {
        var response = _client.GetFromJsonAsync<WorldTimeAPIResponse>(url).Result;
        return DateTimeOffset.ParseExact(response.datetime, "yyyy-MM-dd'T'HH:mm:ss.FFFFFFzzz", CultureInfo.InvariantCulture);
    }

    public void DisplayDateTime(string label, DateTimeOffset dateTime)
    {
        Console.WriteLine($"{label}: {dateTime.ToString(_dateTimeFormatter)}");
    }

    public void DisplayTimeDifference(string location, DateTimeOffset ukTime, DateTimeOffset canadaTime)
    {
        double timeDifference = ukTime.Subtract(canadaTime.DateTime).TotalHours;

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