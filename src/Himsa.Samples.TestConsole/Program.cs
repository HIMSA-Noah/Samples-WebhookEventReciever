using Himsa.Samples.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Samples.TestConsole
{
    class Program
    {
        static HttpClient client = new HttpClient();

        private static string sharedSecret = "keepmesafe";

        static void Main(string[] args)
        {
            

            RunAsync(sharedSecret).GetAwaiter().GetResult();
        }

        static async Task RunAsync(string sharedSecret)
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Console.WriteLine("Sending a test event");

                NoahEvent noahEvent = new NoahEvent()
                {
                    TenantId = Guid.NewGuid(),
                    NotificationEventId = Guid.NewGuid(),
                    NotificationEventSubscriptionId = Guid.NewGuid()
                };

                var result = await CreateEventAsync(noahEvent, sharedSecret);

                Console.WriteLine("NotificationEventId: {0}, Result: {1}", noahEvent.NotificationEventId, result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            Console.ReadLine();
        }

        static async Task<bool> CreateEventAsync(NoahEvent noahEvent, string sharedSecret)
        {
            var jsonAsString = JsonConvert.SerializeObject(noahEvent);

            var httpContent = new StringContent(jsonAsString, Encoding.UTF8, "application/json");

            var bodyForSignature = httpContent.ReadAsStringAsync().Result;

            // Create a Signatur of the payload
            string hubSignature = CreateSignature(bodyForSignature, sharedSecret); // "xxx";
            client.DefaultRequestHeaders.Add("X-Hub-Signature", hubSignature);
            client.DefaultRequestHeaders.Add("X-Hub-Origin", "himsacloud.com");
            client.DefaultRequestHeaders.Add("X-Hub-TransmissionAtempt", "1");

            HttpResponseMessage response = await client.PostAsync("api/noahevents", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else {
                return false;
            }
        }

        private static string CreateSignature(string body, string secret)
        {
            var hash = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var computedSignature = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(body)));
            return computedSignature;
        }

        
    }
}
