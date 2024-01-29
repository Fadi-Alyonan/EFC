using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class PhoneNumberRepository_test : IDisposable
{
    private readonly DbContextOptions<DataContext> _options;

    public PhoneNumberRepository_test()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }


    [Fact]
    public void Create_PhoneNumberEntity_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new DataContext(_options);
        var repository = new PhoneNumberRepository(context);
        var phoneNumber = new PhoneNumberEntity
        {
            PhoneNumber = "123456789"
        };
        // Act
        var result = repository.Create(phoneNumber).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("123456789", result.PhoneNumber);
    }

    [Fact]
    public void GetAll_ShouldReturnAllPhoneNumbers()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.PhoneNumbers.AddRange(
            new PhoneNumberEntity { PhoneNumber = "123" },
            new PhoneNumberEntity { PhoneNumber = "456" }
        );
        context.SaveChanges();
        // Act
        var repository = new PhoneNumberRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSinglePhoneNumber()
    {
        // Arrange
        using var context = new DataContext(_options);
        var phoneNumberToAdd = new PhoneNumberEntity { PhoneNumber = "789" };
        context.PhoneNumbers.Add(phoneNumberToAdd);
        context.SaveChanges();
        // Act
        var repository = new PhoneNumberRepository(context);
        var result = repository.GetOne(p => p.PhoneNumber == "789").Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("789", result.PhoneNumber);
    }

    [Fact]
    public void Update_ShouldUpdatePhoneNumber()
    {
        // Arrange
        using var context = new DataContext(_options);
        var phoneNumberToAdd = new PhoneNumberEntity { PhoneNumber = "123456789" };
        context.PhoneNumbers.Add(phoneNumberToAdd);
        context.SaveChanges();

        var repository = new PhoneNumberRepository(context);
        phoneNumberToAdd.PhoneNumber = "987654321";
        // Act
        var result = repository.Update(p => p.PhoneNumberId == phoneNumberToAdd.PhoneNumberId, phoneNumberToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("987654321", result.PhoneNumber);
    }

    [Fact]
    public void Delete_ShouldDeletePhoneNumber()
    {
        // Arrange
        using var context = new DataContext(_options);
        var phoneNumberToAdd = new PhoneNumberEntity { PhoneNumber = "123456789" };
        context.PhoneNumbers.Add(phoneNumberToAdd);
        context.SaveChanges();
        var repository = new PhoneNumberRepository(context);
        // Act
        var result = repository.Delete(p => p.PhoneNumber == "123456789").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.PhoneNumbers.FirstOrDefault(p => p.PhoneNumber == "123456789"));
    }

    public void Dispose()
    {
        using var context = new DataContext(_options);
        context.Database.EnsureDeleted();
    }

}
