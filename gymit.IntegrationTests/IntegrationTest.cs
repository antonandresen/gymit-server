using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

using gymit.Models.DBContexts;
using gymit.Contracts.V1;
using gymit.Contracts.V1.Requests;
using gymit.Contracts.V1.Responses;

namespace gymit.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => 
                {
                    builder.ConfigureServices(services =>
                    {
                        // Replace real DB with in memory DB (not working rn, it's using real DB).
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task<TestResponse> CreateTestAsync(CreateTestRequest request)
        {
            var response = await TestClient.PostAsJsonAsync($"{ApiRoutes.Base}/test", request);
            return await response.Content.ReadAsAsync<TestResponse>();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync($"{ApiRoutes.BareBase}/identity/register",
                new UserRegistrationRequest 
                {
                    Email = "testing1@integration.com",
                    Password = "SomePassword1234!"
                });
            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }
    }
}
