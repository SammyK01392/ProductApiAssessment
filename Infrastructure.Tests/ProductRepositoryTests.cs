using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests;

public class ProductRepositoryTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task AddAsync_AddsProduct_PersistsToDatabase()
    {
        // Arrange
        await using var context = CreateContext();
        var repo = new ProductRepository(context);
        var product = new Product { ProductName = "Monitor", CreatedBy = "admin", CreatedOn = DateTime.UtcNow };

        // Act
        await repo.AddAsync(product);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.Products.FirstOrDefaultAsync(p => p.ProductName == "Monitor");
        saved.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdWithItemsAsync_ReturnsProductWithItems()
    {
        // Arrange
        await using var context = CreateContext();
        var product = new Product { ProductName = "Desk", CreatedBy = "admin", CreatedOn = DateTime.UtcNow };
        product.Items.Add(new Item { Quantity = 5 });
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);

        // Act
        var result = await repo.GetByIdWithItemsAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        // Arrange
        await using var context = CreateContext();
        context.Products.AddRange(
            new Product { ProductName = "A", CreatedBy = "admin", CreatedOn = DateTime.UtcNow },
            new Product { ProductName = "B", CreatedBy = "admin", CreatedOn = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);

        // Act
        var count = await repo.CountAsync();

        // Assert
        count.Should().Be(2);
    }
}