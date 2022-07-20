using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using CWIllumigon.Domain;

namespace CWIllumigon
{
    public class WledService
    {
        public WledService(string ip)
        {
            Ip = ip;
        }

        private string Ip { get; }

        public void StartDetection(Triangle triangle)
        {
            using var client = new HttpClient();

            var url = "http://" + Ip + "/json/state";
            var json = JsonSerializer.Serialize(triangle);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync(url, data).Result;

            Console.WriteLine(!response.IsSuccessStatusCode ? "Something went wrong" : "Yeah");
        }
    }
}