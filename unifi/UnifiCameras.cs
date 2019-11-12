using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using twilifi.unifi.Models;

namespace twilifi.unifi
{
    public class UnifiCameras
    {

        private HttpClient httpClient;

        private DateTimeOffset authTime = DateTimeOffset.Now.AddHours(-1000);

        private string accessKey;

        private readonly JObject authObject;

        public UnifiCameras()
        {
            var user = System.Environment.GetEnvironmentVariable("UNIFI_USER");
            var pass = System.Environment.GetEnvironmentVariable("UNIFI_PASS");
            this.authObject = new JObject();
            this.authObject.Add("username", user);
            this.authObject.Add("password", pass);

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
            };

            var baseUri = System.Environment.GetEnvironmentVariable("UNIFI_ENDPOINT");
            this.httpClient = new HttpClient(handler)
            {
                BaseAddress = new System.Uri(baseUri),
            };
        }

        public async Task<ICollection<Event>> GetEvents(string type, long startTime, long endTime)
        {
            await this.EnsureAuth();
            var response = await this.httpClient.GetAsync($"api/events?end={endTime}&start={startTime}&type={type}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<ICollection<Event>>(responseBody);

            return events;
        }

        private async Task EnsureAuth()
        {
            // Lets presume the key is good for 10 minutes and only refresh then.
            if (DateTimeOffset.Now - authTime < TimeSpan.FromMinutes(10))
            {
                return;
            }

            var json = JsonConvert.SerializeObject(authObject);
            var response = await this.httpClient.PostAsync(
                "api/auth",
                new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var token = response.Headers.GetValues("Authorization")?.First();
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get an access key for other things - relies on bearer token
            var tokenResponse = await this.httpClient.PostAsync("api/auth/access-key", new StringContent(""));
            tokenResponse.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<dynamic>(responseBody);
            this.accessKey = parsedContent["accessKey"];

            authTime = DateTimeOffset.Now;
        }
    }
}