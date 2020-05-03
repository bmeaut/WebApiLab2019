using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiLab.Client.Api;

namespace WebApiLabor.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("ProductId: ");
            var id = Console.ReadLine();
            //await GetProductAsync(int.Parse(id));
            var p = await GetProduct2Async(int.Parse(id));
            Console.WriteLine($"{p.Name}:{p.UnitPrice}.-");
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
                    Console.WriteLine($"{json.RootElement.GetProperty("name")}:" +
                        $"{json.RootElement.GetProperty("unitPrice")}.-");
                }
            }
        }

        public static async Task<Product> GetProduct2Async(int id)
        {
            using (var httpClient = new HttpClient())
            {
                ProductsClient client = new ProductsClient(httpClient);
                return await client.GetAsync(id);
            }
        }

    }
}
