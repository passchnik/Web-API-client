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
        static async Task Main(string[] args)
        {
            const string appPath= "http://tester.consimple.pro";

            try
            {
                //This do-While loop here for infinity program work.
                //You may not restart the application to check again.
                do
                {
                    Console.Clear();
                    string comand = null;

                    do
                    {
                        Console.WriteLine("To send request write 's', to close programme write 'c'");
                        comand = Console.ReadLine();

                    } while (comand != "s" && comand!="c");

                    if (comand == "c")
                        Environment.Exit(0);

                    Console.WriteLine("Web API response:\n");

                    string jsonResponse = await GetStringResponseAsync(appPath);

                    ShowResponsOnConsoleAsTable(jsonResponse);

                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                } while (true);               
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine(ex.Message); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //Async method that contains all the logic to get a Json answer in string
        private static async Task<string> GetStringResponseAsync(string webApiPath)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var streamTask = client.GetStringAsync(webApiPath);

                return await streamTask;
            }

        }


        //Method used to output api answer in "table" format
        private static void ShowResponsOnConsoleAsTable(string jsonString)
        {
            if (jsonString == null)
                throw new Exception("Method: 'Program.ShowResponsOnConsoleAsTable(string jsonString)'. Exception: jsonString is null");

            var result = JsonConvert.DeserializeObject<JsonType>(jsonString);

            Console.WriteLine("Pruduct name \t\tCategory name\n");

            foreach (var item in result.Products)
            {
                Console.Write($"{item.Name}");
                int strLength = item.Name.Length;
                int tabLength = 8;

                string category = $"{result.Categories.Where(t => t.Id == item.CategoryId).Select(t => t.Name).SingleOrDefault()}";

                //this code is for cool space in console between first column and second.
                //distance depends on the number of characters in the name of the product.
                if (strLength > tabLength)
                {
                    for (int i = 0; i < tabLength*3 - strLength; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine(category);

                }
                else if (strLength < tabLength)
                {
                    Console.WriteLine($"\t\t\t{category}");
                }
                else
                {
                    Console.WriteLine($"\t\t{category}");
                }
            }
            Console.WriteLine();
        }
       


    }
}
