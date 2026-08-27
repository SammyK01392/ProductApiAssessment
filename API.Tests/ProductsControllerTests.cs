using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace API.Tests;

public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_WithoutAuth_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAccessToken()
    {
        // Arrange
        var loginDto = new LoginDto { UserName = "admin", Password = "Admin@123" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginDto);
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto { UserName = "wrong", Password = "wrong" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}