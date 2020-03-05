using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace blazor.wa.aadauth.sample
{
    public class Client : IClient
    {
        private readonly HttpClient _httpClient;

        // TODO get from app settings
        private const string FunctionsHost = "http://localhost:7071/api"; 
        public string Token { get; set; }
        public string UserName { get; set; }
        public string TenantId { get; set; }
        public string Hostname { get; set; }

        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;            
        }

        private string GetQueryParameters()
        {
            return $"username={UserName}&tenantid={TenantId}";
        }
        public async Task<List<Class1>> DoWork()
        {
            if(string.IsNullOrEmpty(Token))
            {
                throw new ArgumentNullException("Token");
            }      
            if (string.IsNullOrEmpty(Token))
            {   
                throw new ArgumentNullException(Token);
            }
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            List<Class1> results = new List<Class1>();
            try
            {
                string queryParams = GetQueryParameters(), url = "";
                url = $"{FunctionsHost}/funcapp?{queryParams}";
                Log.Information($"Url is {url}");
                Log.Information(await _httpClient.GetStringAsync(url));
                System.Threading.Thread.Sleep(5000);
                results = await _httpClient.GetJsonAsync<List<Class1>>(url);
            }
            catch (System.Exception e)
            {
                Log.Error($"Failed to get stuff, error was {e}");
                throw e;
            }

            return results;
        }
    }
}