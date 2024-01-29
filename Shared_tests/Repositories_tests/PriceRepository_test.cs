using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class PriceRepository_test : IDisposable
{
    private readonly DbContextOptions<ProductDataContext> _options;

    public PriceRepository_test()
    {
        _options = new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }


    [Fact]
    public void Create_Price_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new PriceRepository(context);
        var price = new Price
        {
            ProductPrice = 50.99m,
            PriceDate = new DateOnly(2024, 1, 27)
        };
        // Act
        var result = repository.Create(price).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(50.99m, result.ProductPrice);
        Assert.Equal(new DateOnly(2024, 1, 27), result.PriceDate);
    }

    [Fact]
    public void GetAll_ShouldReturnAllPrices()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Prices.AddRange(
            new Price { ProductPrice = 30.50m, PriceDate = new DateOnly(2024, 1, 25) },
            new Price { ProductPrice = 40.75m, PriceDate = new DateOnly(2024, 1, 26) }
        );
        context.SaveChanges();
        // Act
        var repository = new PriceRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSinglePrice()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var priceToAdd = new Price { ProductPrice = 25.99m, PriceDate = new DateOnly(2024, 1, 24) };
        context.Prices.Add(priceToAdd);
        context.SaveChanges();
        // Act
        var repository = new PriceRepository(context);
        var result = repository.GetOne(p => p.ProductPrice == 25.99m).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(25.99m, result.ProductPrice);
        Assert.Equal(new DateOnly(2024, 1, 24), result.PriceDate);
    }

    [Fact]
    public void Update_ShouldUpdatePrice()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var priceToAdd = new Price { ProductPrice = 20.00m, PriceDate = new DateOnly(2024, 1, 22) };
        context.Prices.Add(priceToAdd);
        context.SaveChanges();

        var repository = new PriceRepository(context);
        var updatedPrice = new Price { ProductPrice = 22.50m, PriceDate = new DateOnly(2024, 1, 23) };
        priceToAdd.ProductPrice = updatedPrice.ProductPrice;
        priceToAdd.PriceDate = updatedPrice.PriceDate;
        // Act
        var result = repository.Update(p => p.PriceId == priceToAdd.PriceId, priceToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(22.50m, result.ProductPrice);
        Assert.Equal(new DateOnly(2024, 1, 23), result.PriceDate);
    }

    [Fact]
    public void Delete_ShouldDeletePrice()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var priceToAdd = new Price { ProductPrice = 15.99m, PriceDate = new DateOnly(2024, 1, 21) };
        context.Prices.Add(priceToAdd);
        context.SaveChanges();

        var repository = new PriceRepository(context);
        // Act
        var result = repository.Delete(p => p.ProductPrice == 15.99m).Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Prices.FirstOrDefault(p => p.ProductPrice == 15.99m));
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingPrice()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var priceToAdd = new Price { ProductPrice = 18.50m, PriceDate = new DateOnly(2024, 1, 20) };
        context.Prices.Add(priceToAdd);
        context.SaveChanges();
        // Act
        var repository = new PriceRepository(context);
        var result = repository.Exists(p => p.ProductPrice == 18.50m).Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseForNonExistingPrice()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new PriceRepository(context);

        // Act
        var result = repository.Exists(p => p.ProductPrice == 16.75m).Result;
        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        using var context = new ProductDataContext(_options);
        context.Database.EnsureDeleted();
    }
}
