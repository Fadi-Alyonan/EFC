using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class ProductionInformationRepository_test : IDisposable
{
    private readonly DbContextOptions<ProductDataContext> _options;

    public ProductionInformationRepository_test()
    {
        _options = new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }


    [Fact]
    public void Create_ProductionInformation_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new ProductionInformationRepository(context);
        var productionInfo = new ProductionInformation
        {
            ProductionDate = new DateOnly(2024, 1, 27)
        };
        // Act
        var result = repository.Create(productionInfo).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(new DateOnly(2024, 1, 27), result.ProductionDate);
    }

    [Fact]
    public void GetAll_ShouldReturnAllProductionInformation()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.ProductionInformations.AddRange(
            new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 25) },
            new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 26) }
        );
        context.SaveChanges();
        // Act
        var repository = new ProductionInformationRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSingleProductionInformation()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productionInfoToAdd = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 24) };
        context.ProductionInformations.Add(productionInfoToAdd);
        context.SaveChanges();
        // Act
        var repository = new ProductionInformationRepository(context);
        var result = repository.GetOne(p => p.ProductionDate == new DateOnly(2024, 1, 24)).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(new DateOnly(2024, 1, 24), result.ProductionDate);
    }

    [Fact]
    public void Update_ShouldUpdateProductionInformation()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productionInfoToAdd = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 22) };
        context.ProductionInformations.Add(productionInfoToAdd);
        context.SaveChanges();

        var repository = new ProductionInformationRepository(context);
        var updatedProductionInfo = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 23) };
        productionInfoToAdd.ProductionDate = updatedProductionInfo.ProductionDate;
        // Act
        var result = repository.Update(p => p.ProductionId == productionInfoToAdd.ProductionId, productionInfoToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(new DateOnly(2024, 1, 23), result.ProductionDate);
    }

    [Fact]
    public void Delete_ShouldDeleteProductionInformation()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productionInfoToAdd = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 21) };
        context.ProductionInformations.Add(productionInfoToAdd);
        context.SaveChanges();
        // Act
        var repository = new ProductionInformationRepository(context);
        var result = repository.Delete(p => p.ProductionDate == new DateOnly(2024, 1, 21)).Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.ProductionInformations.FirstOrDefault(p => p.ProductionDate == new DateOnly(2024, 1, 21)));
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingProductionInformation()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var productionInfoToAdd = new ProductionInformation { ProductionDate = new DateOnly(2024, 1, 20) };
        context.ProductionInformations.Add(productionInfoToAdd);
        context.SaveChanges();
        // Act
        var repository = new ProductionInformationRepository(context);
        var result = repository.Exists(p => p.ProductionDate == new DateOnly(2024, 1, 20)).Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseForNonExistingProductionInformation()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new ProductionInformationRepository(context);
        // Act
        var result = repository.Exists(p => p.ProductionDate == new DateOnly(2024, 1, 19)).Result;
        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        using var context = new ProductDataContext(_options);
        context.Database.EnsureDeleted();
    }
}
