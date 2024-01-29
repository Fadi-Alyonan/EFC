using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Dtos;
using Shared.Entities;
using Shared.Repositories;
using Shared.Services;

namespace Shared_tests.Services_tests;

public class ProductService_test : IDisposable
{
    private readonly DbContextOptions<ProductDataContext> _options;

    public ProductService_test()
    {
        _options = new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void CreateProduct_ShouldAddProductToDatabase()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var categoryRepository = new CategoryRepository(context);
        var manufacturerRepository = new ManufacturerRepository(context);
        var priceRepository = new PriceRepository(context);
        var productionInformationRepository = new ProductionInformationRepository(context);
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, productionInformationRepository, priceRepository, manufacturerRepository, categoryRepository);

        var productDto = new ProductDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            ProductDescription = "Test Description",
            QuantityInStock = 10,
            ProductionDate = new DateOnly(2024, 1, 27),
            CategoryName = "Test Category",
            ManufacturerName = "Test Manufacturer",
            ProductPrice = 99.99m
        };
        // Act
        var result = productService.CreateProduct(productDto).Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetAllProducts_ShouldReturnAllProducts()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Products.AddRange(
            new Product
            {
                Production = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 27) },
                Category = new Category { CategoryName = "Test Category 1" },
                Manufacturer = new Manufacturer { ManufacturerName = "Test Manufacturer 1" },
                Price = new Price { ProductPrice = 99.99m, PriceDate = new DateOnly(2024, 1, 27) },
                ProductId = Guid.NewGuid(),
                ProductName = "Test Product 1",
                ProductDescription = "Test ProductDis 1",
                QuantityInStock = 1,
                
            },
            new Product
            {
                Production = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 27) },
                Category = new Category { CategoryName = "Test Category 2" },
                Manufacturer = new Manufacturer { ManufacturerName = "Test Manufacturer 2" },
                Price = new Price { ProductPrice = 99.99m, PriceDate = new DateOnly(2024, 1, 27) },
                ProductId = Guid.NewGuid(),
                ProductName = "Test Product 2",
                ProductDescription = "Test ProductDis 2",
                QuantityInStock = 2,
                
            }
        );
        context.SaveChanges();

        var categoryRepository = new CategoryRepository(context);
        var manufacturerRepository = new ManufacturerRepository(context);
        var priceRepository = new PriceRepository(context);
        var productionInformationRepository = new ProductionInformationRepository(context);
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, productionInformationRepository, priceRepository, manufacturerRepository, categoryRepository);
        // Act
        var result = productService.GetAllProducts().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOneProduct_ShouldReturnSingleProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Products.Add(new Product
        {
            Production = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 27) },
            Category = new Category { CategoryName = "Test Category" },
            Manufacturer = new Manufacturer { ManufacturerName = "Test Manufacturer" },
            Price = new Price { ProductPrice = 99.99m },
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            ProductDescription = "Test ProductDis",
            QuantityInStock = 10,
            
        });
        context.SaveChanges();

        var categoryRepository = new CategoryRepository(context);
        var manufacturerRepository = new ManufacturerRepository(context);
        var priceRepository = new PriceRepository(context);
        var productionInformationRepository = new ProductionInformationRepository(context);
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, productionInformationRepository, priceRepository, manufacturerRepository, categoryRepository);
        // Act
        var product = new Product { ProductName = "Test Product" };
        var result = productService.GetOneProduct(product).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Product", result.ProductName);
    }

    [Fact]
    public void UpdateProduct_ShouldUpdateProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Products.Add(new Product
        {
            Production = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 27) },
            Category = new Category { CategoryName = "Old Category" },
            Manufacturer = new Manufacturer { ManufacturerName = "Old Manufacturer" },
            Price = new Price { ProductPrice = 49.99m },
            ProductName = "Old Product",
            ProductDescription = "Old ProductDis",
            QuantityInStock = 5
            
        });
        context.SaveChanges();

        var categoryRepository = new CategoryRepository(context);
        var manufacturerRepository = new ManufacturerRepository(context);
        var priceRepository = new PriceRepository(context);
        var productionInformationRepository = new ProductionInformationRepository(context);
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, productionInformationRepository, priceRepository, manufacturerRepository, categoryRepository);

        var updatedProductDto = new ProductDto
        {
            ProductName = "New Product",
            ProductDescription = "New ProductDis",
            QuantityInStock = 10,
            ProductionDate = new DateOnly(2024, 1, 28),
            CategoryName = "New Category",
            ManufacturerName = "New Manufacturer",
            ProductPrice = 59.99m
        };
       
        // Act
        var result = productService.UpdateProduct(updatedProductDto).Result;
        // Assert
        Assert.True(result);
        var updatedProduct = context.Products.Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.Production).Include(p => p.Price).FirstOrDefault(p => p.ProductName == "New Product");
        Assert.NotNull(updatedProduct);
        Assert.Equal(10, updatedProduct.QuantityInStock);
        Assert.Equal(new DateOnly(2024, 1, 28), updatedProduct.Production!.ProductionDate);
        Assert.Equal("New Category", updatedProduct.Category!.CategoryName);
        Assert.Equal("New Manufacturer", updatedProduct.Manufacturer!.ManufacturerName);
        Assert.Equal(59.99m, updatedProduct.Price!.ProductPrice);
    }

    [Fact]
    public void DeleteProduct_ShouldDeleteProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Products.Add(new Product
        {
            Production = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 27) },
            Category = new Category { CategoryName = "Category To Delete" },
            Manufacturer = new Manufacturer { ManufacturerName = "Manufacturer To Delete" },
            Price = new Price { ProductPrice = 79.99m },
            ProductName = "Product To Delete",
            ProductDescription = "Product To DeleteDis",
            QuantityInStock = 8,
            
        });
        context.SaveChanges();

        var categoryRepository = new CategoryRepository(context);
        var manufacturerRepository = new ManufacturerRepository(context);
        var priceRepository = new PriceRepository(context);
        var productionInformationRepository = new ProductionInformationRepository(context);
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, productionInformationRepository, priceRepository, manufacturerRepository, categoryRepository);
        // Act
        var productDtoToDelete = new ProductDto { ProductName = "Product To Delete" };
        var result = productService.DeleteProduct(productDtoToDelete).Result;
        //Assert
        Assert.True(result);
        Assert.Null(context.Products.FirstOrDefault(p => p.ProductName == "Product To Delete"));
    }

    [Fact]
    public void CheckIfProductExistsAsync_ShouldReturnTrueForExistingProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Products.Add(new Product
        {
            Production = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 27) },
            Category = new Category { CategoryName = "Existing Category" },
            Manufacturer = new Manufacturer { ManufacturerName = "Existing Manufacturer" },
            Price = new Price { ProductPrice = 69.99m },
            ProductName = "Existing Product",
            ProductDescription = "Existing ProductDis",
            QuantityInStock = 5
        });
        context.SaveChanges();

        var categoryRepository = new CategoryRepository(context);
        var manufacturerRepository = new ManufacturerRepository(context);
        var priceRepository = new PriceRepository(context);
        var productionInformationRepository = new ProductionInformationRepository(context);
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, productionInformationRepository, priceRepository, manufacturerRepository, categoryRepository);
        // Act
        var result = productService.CheckIfProductExistsAsync("Existing Product").Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckIfProductExistsAsync_ShouldReturnFalseForNonExistingProduct()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var categoryRepository = new CategoryRepository(context);
        var manufacturerRepository = new ManufacturerRepository(context);
        var priceRepository = new PriceRepository(context);
        var productionInformationRepository = new ProductionInformationRepository(context);
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, productionInformationRepository, priceRepository, manufacturerRepository, categoryRepository);
        // Act
        var result = productService.CheckIfProductExistsAsync("No Existing Product").Result;
        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        using var context = new ProductDataContext(_options);
        context.Database.EnsureDeleted();
    }
}
