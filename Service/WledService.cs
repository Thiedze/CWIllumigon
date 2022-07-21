using System.Text;
using System.Text.Json;
using CWIllumigon.Domain;

namespace CWIllumigon.Service;

public class WledService
{
    private string Ip { get; }

    public WledService(string ip)
    {
        Ip = ip;
    }

    public void Show(Triangle triangle)
    {
        using var client = new HttpClient();

        var url = "http://" + Ip + "/json/state";
        var json = JsonSerializer.Serialize(triangle);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = client.PostAsync(url, data).Result;

        Console.WriteLine(!response.IsSuccessStatusCode ? "Something went wrong" : "Yeah");
    }
}