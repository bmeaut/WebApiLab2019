using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLabor.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("ProductId: ");
            var id = Console.ReadLine();
            await GetProductAsync(Int32.Parse(id));

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
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                }
            }
        }
    }
}
