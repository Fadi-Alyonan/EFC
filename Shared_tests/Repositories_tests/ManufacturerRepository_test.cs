
using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class ManufacturerRepository_test : IDisposable
{
    private readonly DbContextOptions<ProductDataContext> _options;

    public ManufacturerRepository_test()
    {
        _options = new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }


    [Fact]
    public void Create_Manufacturer_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new ManufacturerRepository(context);
        var manufacturer = new Manufacturer
        {
            ManufacturerName = "ABC Electronics"
        };
        // Act
        var result = repository.Create(manufacturer).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("ABC Electronics", result.ManufacturerName);
    }

    [Fact]
    public void GetAll_ShouldReturnAllManufacturers()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Manufacturers.AddRange(
            new Manufacturer { ManufacturerName = "ABC Electronics" },
            new Manufacturer { ManufacturerName = "XYZ Furniture" }
        );
        context.SaveChanges();
        // Act
        var repository = new ManufacturerRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSingleManufacturer()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var manufacturerToAdd = new Manufacturer { ManufacturerName = "XYZ Electronics" };
        context.Manufacturers.Add(manufacturerToAdd);
        context.SaveChanges();
        // Act
        var repository = new ManufacturerRepository(context);
        var result = repository.GetOne(m => m.ManufacturerName == "XYZ Electronics").Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("XYZ Electronics", result.ManufacturerName);
    }

    [Fact]
    public void Update_ShouldUpdateManufacturer()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var manufacturerToAdd = new Manufacturer { ManufacturerName = "OldManufacturer" };
        context.Manufacturers.Add(manufacturerToAdd);
        context.SaveChanges();

        var repository = new ManufacturerRepository(context);
        var updatedManufacturer = new Manufacturer { ManufacturerName = "NewManufacturer" };
        manufacturerToAdd.ManufacturerName = updatedManufacturer.ManufacturerName;
        // Act
        var result = repository.Update(m => m.ManufacturerId == manufacturerToAdd.ManufacturerId, manufacturerToAdd).Result;
        //Assert
        Assert.NotNull(result);
        Assert.Equal("NewManufacturer", result.ManufacturerName);
    }

    [Fact]
    public void Delete_ShouldDeleteManufacturer()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var manufacturerToAdd = new Manufacturer { ManufacturerName = "ManufacturerToDelete" };
        context.Manufacturers.Add(manufacturerToAdd);
        context.SaveChanges();
        
        var repository = new ManufacturerRepository(context);
        //Act
        var result = repository.Delete(m => m.ManufacturerName == "ManufacturerToDelete").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Manufacturers.FirstOrDefault(m => m.ManufacturerName == "ManufacturerToDelete"));
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingManufacturer()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var manufacturerToAdd = new Manufacturer { ManufacturerName = "ExistingManufacturer" };
        context.Manufacturers.Add(manufacturerToAdd);
        context.SaveChanges();
        // Act
        var repository = new ManufacturerRepository(context);
        var result = repository.Exists(m => m.ManufacturerName == "ExistingManufacturer").Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseForNonExistingManufacturer()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new ManufacturerRepository(context);

        // Act
        var result = repository.Exists(m => m.ManufacturerName == "ExistingManufacturer").Result;
        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        using var context = new ProductDataContext(_options);
        context.Database.EnsureDeleted();
    }
}
