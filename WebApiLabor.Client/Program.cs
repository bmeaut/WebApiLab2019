using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiLabor.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("ProductId: ");
            var id = Console.ReadLine();
            await GetProductAsync(int.Parse(id));

            Console.ReadKey();
        }

        public static async Task GetProductAsync(int id)
        {
            using (var client = new HttpClient())
            {
                /*A portot írjuk át a szervernek megfelelően*/
                var response = await client.
                    GetAsync(new Uri($"http://localhost:5000/api/Products/{id}"));
                if (response.IsSuccessStatusCode)
                {
                    var jsonStream = await response.Content.ReadAsStreamAsync();
                    var json = await JsonDocument.ParseAsync(jsonStream);
                    Console.WriteLine($"{json.RootElement.GetProperty("name")}:{json.RootElement.GetProperty("unitPrice")}.-");
                }
            }
        }
    }
}
