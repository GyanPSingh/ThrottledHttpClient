using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Core3
{
    public interface ICatalogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numbers">A list of integers</param>
        /// <param name="requestLimit">Max number of concurrent requests</param>
        /// <param name="limitingPeriodInSeconds">per second or per n seconds</param>
        /// <returns></returns>
        Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type);
    }
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Catalog> GetCatalogItems(int page, int take,
                                                   int? brand, int? type)
        {
            var uri = API.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl,
                                                     page, take, brand, type);

            var responseString = await _httpClient.GetStringAsync(uri);

            var catalog = JsonSerializer.Deserialize<Catalog>(responseString);
            // var catalog =JsonConverter.DeserializeObject<Catalog>(responseString);
            return catalog;
        }
    }
    public class Catalog
    {
    }
}
