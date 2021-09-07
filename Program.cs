using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_Client
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            //http://tester.consimple.pro

            await ProcessRepositories();
        }

        private static async Task ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStringAsync("http://tester.consimple.pro");

            var str = await streamTask;

            var result = JsonConvert.DeserializeObject<JsonType>(str);

            Console.WriteLine("Pruducs \t\tCategories\n");

            foreach (var item in result.Products)
            {
                Console.Write($"{item.Name}");
                int strLength = item.Name.Length;

                string category = $"{result.Categories.Where(t => t.Id == item.CategoryId).Select(t => t.Name).SingleOrDefault()}";

                if (strLength > 8)
                {
                    for (int i = 0; i < 24- strLength; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine(category);

                }
                else if(strLength < 8)
                {
                    Console.WriteLine($"\t\t\t{category}");    
                }
                else
                {
                    Console.WriteLine($"\t\t{category}");
                }
            }

            

            Console.ReadLine();
        }


    }
}
