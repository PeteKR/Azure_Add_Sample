using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WWSI.PK105_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Autentykacja service_to_service = dwie usługi nad którymi mam kontrolę łączą się ze sobą
            //var app = ConfidentialClientApplicationBuilder.Create("")
            //    .WithAuthority("")
            //    .WithClientSecret("").Build();

            //var token = app.AcquireTokenForClient(new[] { "" });
            //var result = await token.ExecuteAsync();

            var app = PublicClientApplicationBuilder.Create("")
                    .WithAuthority("")
                    .WithDefaultRedirectUri().Build();

            var token = app.AcquireTokenInteractive(new[] { "" });

            var result = await token.ExecuteAsync();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {result.AccessToken}");

            ClientService client = new ClientService("https://localhost:5001/", httpClient);
        
            await client.CreatePatientAsync(new Patient() 
                { FirstName = "Grzegorz", LastName="Brzęczyszczykiewicz", Id = 0, TestDate = DateTimeOffset.Now });

            var patients = await client.GetAllPatientsAsync();

            foreach(var p in patients)
            {
                Console.WriteLine(p.FirstName + " " + p.LastName + " " + p.TestDate);
            }

            await client.InvalidAsync();

        }
    }
}
