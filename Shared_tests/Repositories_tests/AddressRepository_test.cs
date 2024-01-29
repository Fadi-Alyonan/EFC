using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class AddressRepository_test : IDisposable
{
    private readonly DbContextOptions<DataContext> _options;

    public AddressRepository_test()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    } 

    [Fact]
    public void Create_AddressEntity_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new DataContext(_options);
        var repository = new AddressRepository(context);
        var address = new AddressEntity
        {
            StreetName = "TestStreet",
            PostalCode = "12345",
            City = "TestCity"
        };
        // Act
        var result = repository.Create(address).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestStreet", result.StreetName);
        Assert.Equal("12345", result.PostalCode);
        Assert.Equal("TestCity", result.City);
    }

    [Fact]
    public void GetAll_ShouldReturnAllAddresses()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Addresses.AddRange(
            new AddressEntity { StreetName = "Street1" },
            new AddressEntity { StreetName = "Street2" }
        );
        context.SaveChanges();

        var repository = new AddressRepository(context);
        // Act
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSingleAddress()
    {
        // Arrange
        using var context = new DataContext(_options);
        var addressToAdd = new AddressEntity { StreetName = "TestStreet" };
        context.Addresses.Add(addressToAdd);
        context.SaveChanges();

        var repository = new AddressRepository(context);
        // Act
        var result = repository.GetOne(a => a.StreetName == addressToAdd.StreetName).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestStreet", result.StreetName);
    }

    [Fact]
    public void Update_ShouldUpdateAddress()
    {
        // Arrange
        using var context = new DataContext(_options);
        var addressToAdd = new AddressEntity { StreetName = "OldStreet" };
        context.Addresses.Add(addressToAdd);
        context.SaveChanges();

        var repository = new AddressRepository(context);
        addressToAdd.StreetName = "NewStreet";
        // Act
        var result = repository.Update(a => a.AddressId == addressToAdd.AddressId, addressToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewStreet", result.StreetName);
    }

    [Fact]
    public void Delete_ShouldDeleteAddress()
    {
        // Arrange
        using var context = new DataContext(_options);
        var addressToAdd = new AddressEntity { StreetName = "Street" };
        context.Addresses.Add(addressToAdd);
        context.SaveChanges();

        var repository = new AddressRepository(context);
        // Act
        var result = repository.Delete(a => a.StreetName == "Street").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Addresses.FirstOrDefault(a => a.StreetName == "Street"));
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingAddress()
    {
        // Arrange
        using var context = new DataContext(_options);
        var addressToAdd = new AddressEntity { StreetName = "Street" };
        context.Addresses.Add(addressToAdd);
        context.SaveChanges();

        var repository = new AddressRepository(context);
        // Act
        var result = repository.Exists(a => a.StreetName == "Street").Result;
        // Assert
        Assert.True(result);
    }

    public void Dispose()
    {
        using var context = new DataContext(_options);
        context.Database.EnsureDeleted();
    }
}

