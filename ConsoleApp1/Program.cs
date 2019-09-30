using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static async Task Main()
        {

            //var services = new ServiceCollection().AddHttpClient();
            //services.AddHttpClient<IThrottledHttpClient, ThrottledHttpClient>();
            //var serviceProvider = services.BuildServiceProvider();
            //var client = serviceProvider.GetService<IThrottledHttpClient>();
            //var numbers = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 11, 13, 17, 19, 23, 29, 31, 41, 43, 90, 67, 1763 };
            //var results = await client.GetPrimeNumberResults(numbers);
            //foreach (var result in results)
            //{
            //    Console.WriteLine($"{result.Number} is a prime number? \t {result.IsPrime}.");
            //}
            //ProcessRepositories();

            var repositories = ProcessRepositories().Result;
            foreach (var repo in repositories)
            {
                Console.WriteLine(repo.Name);
                Console.WriteLine(repo.Description);
                Console.WriteLine(repo.GitHubHomeUrl);
                Console.WriteLine(repo.Homepage);
                Console.WriteLine(repo.Watchers);
                Console.WriteLine(repo.LastPush);
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        //private static async Task ProcessRepositories()
        private static async Task<List<Repository>> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            //var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
            //var msg = await stringTask;
            //Console.Write(msg);
            var serializer = new DataContractJsonSerializer(typeof(List<Repository>));
            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = serializer.ReadObject(await streamTask) as List<Repository>;
            return repositories;           
        }
    }

    [DataContract(Name = "repo")]
    public class Repository
    {
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "html_url")]
        public Uri GitHubHomeUrl { get; set; }

        [DataMember(Name = "homepage")]
        public Uri Homepage { get; set; }

        [DataMember(Name = "watchers")]
        public int Watchers { get; set; }

        [DataMember(Name = "pushed_at")]
        private string JsonDate { get; set; }
        [IgnoreDataMember]
        public DateTime LastPush
        {
            get
            {
                return DateTime.ParseExact(JsonDate, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}