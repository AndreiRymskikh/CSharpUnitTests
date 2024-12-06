using Application.Weather;
using Application.WorldTime;


public class Program
{
    public static void Main(string[] args)
    {
        using HttpClient client = new HttpClient();
        var worldTimeApi = new WorldTimeApi(client);
        var weatherApi = new OpenMeteoApi(client);

        Console.WriteLine("Please, specify your location (UK or Canada):");

        string location = Console.ReadLine();

        weatherApi.GetTemperatureByLocation(location);
        worldTimeApi.OutputWorldTimeData(location);
    }
}