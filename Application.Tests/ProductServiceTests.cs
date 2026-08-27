using Application.DTOs;
using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Application.Tests;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly IMapper _mapper;
    private readonly ProductService _sut; // System Under Test

    public ProductServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IProductRepository>();
        _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepoMock.Object);

        var config = new MapperConfiguration(
            cfg => cfg.AddProfile<MappingProfile>(),
            NullLoggerFactory.Instance);   // 👈 second argument added
        _mapper = config.CreateMapper();

        _sut = new ProductService(_unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProductDto()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "Laptop", CreatedBy = "admin", CreatedOn = DateTime.UtcNow };
        _productRepoMock.Setup(r => r.GetByIdWithItemsAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.ProductName.Should().Be("Laptop");
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _productRepoMock.Setup(r => r.GetByIdWithItemsAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        // Act
        Func<Task> act = async () => await _sut.GetByIdAsync(99);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ValidDto_AddsProductAndReturnsDto()
    {
        // Arrange
        var dto = new CreateProductDto { ProductName = "Mouse", CreatedBy = "admin" };

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        result.ProductName.Should().Be("Mouse");
        _productRepoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_RemovesProduct()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "Keyboard" };
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        await _sut.DeleteAsync(1);

        // Assert
        _productRepoMock.Verify(r => r.Remove(product), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

        // Act
        Func<Task> act = async () => await _sut.DeleteAsync(1);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}