using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class ProductRepository_test : IDisposable
{
    private readonly DbContextOptions<ProductDataContext> _options;

    public ProductRepository_test()
    {
        _options = new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }


    [Fact]
    public void Create_Product_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new ProductRepository(context);
        var product = new Product
        {
            ProductName = "TestProduct",
            ProductDescription = "TestDescription",
            QuantityInStock = 10
        };
        // Act
        var result = repository.Create(product).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestProduct", result.ProductName);
        Assert.Equal("TestDescription", result.ProductDescription);
        Assert.Equal(10, result.QuantityInStock);
    }

    [Fact]
    public void GetAll_ShouldReturnAllProducts()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Products.AddRange(
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Product1",
                ProductDescription = "Description1",
                QuantityInStock = 10,
                Category = new Category { CategoryName = "Category1" },
                Manufacturer = new Manufacturer { ManufacturerName = "Manufacturer1" },
                Price = new Price { ProductPrice = 15.99m, PriceDate = new DateOnly(2022, 1, 27) },
                Production = new ProductionInformation { ProductionDate = new DateOnly(2022, 1, 28) }
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Product2",
                ProductDescription = "Description2",
                QuantityInStock = 15,
                Category = new Category { CategoryName = "Category2" },
                Manufacturer = new Manufacturer { ManufacturerName = "Manufacturer2" },
                Price = new Price { ProductPrice = 20.99m, PriceDate = new DateOnly(2022, 1, 29) },
                Production = new ProductionInformation { ProductionDate = new DateOnly(2022, 1, 30) }
            }
        );
        context.SaveChanges();
        // Act
        var repository = new ProductRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSingleProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productToAdd = new Product { ProductName = "TestProduct", ProductDescription = "TestDescription", QuantityInStock = 10 };
        context.Products.Add(productToAdd);
        context.SaveChanges();
        // Act
        var repository = new ProductRepository(context);
        var result = repository.GetOne(p => p.ProductName == "TestProduct").Result;
        // assert
        Assert.NotNull(result);
        Assert.Equal("TestProduct", result.ProductName);
        Assert.Equal("TestDescription", result.ProductDescription);
        Assert.Equal(10, result.QuantityInStock);
    }

    [Fact]
    public void Update_ShouldUpdateProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productToAdd = new Product { ProductName = "OldProduct", ProductDescription = "OldDesc", QuantityInStock = 15 };
        context.Products.Add(productToAdd);
        context.SaveChanges();

        var repository = new ProductRepository(context);
        var updatedProduct = new Product { ProductName = "NewProduct", ProductDescription = "NewDesc", QuantityInStock = 20 };
        productToAdd.ProductName = updatedProduct.ProductName;
        productToAdd.ProductDescription = updatedProduct.ProductDescription;
        productToAdd.QuantityInStock = updatedProduct.QuantityInStock;
        // Act
        var result = repository.Update(p => p.ProductId == productToAdd.ProductId, productToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewProduct", result.ProductName);
        Assert.Equal("NewDesc", result.ProductDescription);
        Assert.Equal(20, result.QuantityInStock);
    }

    [Fact]
    public void Delete_ShouldDeleteProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productToAdd = new Product { ProductName = "ToDelete", ProductDescription = "Desc", QuantityInStock = 12 };
        context.Products.Add(productToAdd);
        context.SaveChanges();

        var repository = new ProductRepository(context);
        // Act
        var result = repository.Delete(p => p.ProductName == "ToDelete").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Products.FirstOrDefault(p => p.ProductName == "ToDelete"));
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productToAdd = new Product { ProductName = "Product", ProductDescription = "Desc", QuantityInStock = 8 };
        context.Products.Add(productToAdd);
        context.SaveChanges();
        // Act
        var repository = new ProductRepository(context);
        var result = repository.Exists(p => p.ProductName == "Product").Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseForNonExistingProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new ProductRepository(context);
        // Act
        var result = repository.Exists(p => p.ProductName == "Product").Result;
        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        using var context = new ProductDataContext(_options);
        context.Database.EnsureDeleted();
    }
}
