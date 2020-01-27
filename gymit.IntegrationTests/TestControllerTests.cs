using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

using gymit.Contracts.V1;
using gymit.Models;
using gymit.Contracts.V1.Requests;

namespace gymit.IntegrationTests
{
    public class TestControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Get_WithoutAnyTests_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync($"{ApiRoutes.BareBase}/test");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Test>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task GetOne_ReturnsTest_WhenPostExistsInTheDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var createdTest = await CreateTestAsync(new CreateTestRequest { Number = 5, Text = "Very nice" });

            // Act
            var response = await TestClient.GetAsync($"{ApiRoutes.Base}/test/{createdTest.ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedTest = await response.Content.ReadAsAsync<Test>();
            returnedTest.ID.Should().Be(createdTest.ID);
            returnedTest.Text.Should().Be("Very nice");
        }
    }
}
