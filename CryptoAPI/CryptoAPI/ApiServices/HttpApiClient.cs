using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoAPI.ApiServices
{
    public class HttpAPIClient
    {
        private HttpClient GetHttpClient(string url)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private async Task<T> GetAsync<T>(string url, string urlParameters)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {

                    HttpResponseMessage response = await client.GetAsync(urlParameters);

                    Console.WriteLine("Primio sam status: " + response.StatusCode);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }


                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public async Task<T> RunAsync<T>(string url, string urlParameters)
        {
            return await GetAsync<T>(url, urlParameters);
        }


        private async Task<List<T>> GetAsyncList<T>(string url, List<string> urlParameters)
        {
            try
            {

                List<T> listOfResults = new List<T>();
                using (var client = GetHttpClient(url))
                {
                    int brojac = 0;
                    foreach (var item in urlParameters)
                    {
                        HttpResponseMessage response = await client.GetAsync(item);

                        Console.WriteLine("Primio sam status: " + response.StatusCode);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<T>(json);
                            listOfResults.Add(result);
                        }

                        Console.WriteLine("Preuzeo: " + brojac++);
                    }

                    return listOfResults;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<T>> RunAsyncList<T>(string url, List<string> urlParameters)
        {
            return await GetAsyncList<T>(url, urlParameters);
        }
    }
}
