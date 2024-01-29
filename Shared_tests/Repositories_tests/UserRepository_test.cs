using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class UserRepository_test : IDisposable
{
    private readonly DbContextOptions<DataContext> _options;

    public UserRepository_test()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    

    [Fact]
    public void Create_UserEntity_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new DataContext(_options);
        var repository = new UserRepository(context);
        var user = new UserEntity
        {
            Email = "test@example.com",
            Password = "password",
            Profile = new ProfileEntity { FirstName = "Test", LastName = "Test" },
            Address = new AddressEntity { StreetName = "Street 1", PostalCode = "12345", City = "City" },
            Role = new RoleEntity { RoleName = "User" },
            PhoneNumber = new PhoneNumberEntity { PhoneNumber = "123456789" }
        };
        // Act
        var result = repository.Create(user).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public void GetAll_ShouldReturnAllUsers()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Users.AddRange(
            new UserEntity
            {
                Email = "user1@example.com",
                Password = "password1",
                Profile = new ProfileEntity { FirstName = "User", LastName = "One" },
                Address = new AddressEntity { StreetName = "Street 1", PostalCode = "12345", City = "City" },
                Role = new RoleEntity { RoleName = "User" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "123456789" }
            },
            new UserEntity
            {
                Email = "user2@example.com",
                Password = "password2",
                Profile = new ProfileEntity { FirstName = "User", LastName = "Two" },
                Address = new AddressEntity { StreetName = "Street 2", PostalCode = "67890", City = "City" },
                Role = new RoleEntity { RoleName = "User" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "987654321" }
            }
        );
        context.SaveChanges();
        // Act
        var repository = new UserRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnOneSingleUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        var userToAdd = new UserEntity
        {
            Email = "Test@example.com",
            Password = "password",
            Profile = new ProfileEntity { FirstName = "Test", LastName = "Test" },
            Address = new AddressEntity { StreetName = "Street", PostalCode = "13579", City = "City" },
            Role = new RoleEntity { RoleName = "User" },
            PhoneNumber = new PhoneNumberEntity { PhoneNumber = "111222333" }
        };
        context.Users.Add(userToAdd);
        context.SaveChanges();
        // Act
        var repository = new UserRepository(context);
        var result = repository.GetOne(u => u.Email == "Test@example.com").Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test@example.com", result.Email);
    }

    [Fact]
    public void Update_ShouldUpdateUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        var userToAdd = new UserEntity
        {
            Email = "old@example.com",
            Password = "oldpassword",
            Profile = new ProfileEntity { FirstName = "Old", LastName = "User" },
            Address = new AddressEntity { StreetName = "Old Street", PostalCode = "98765", City = "Old City" },
            Role = new RoleEntity { RoleName = "OldRole" },
            PhoneNumber = new PhoneNumberEntity { PhoneNumber = "111222333" }
        };
        context.Users.Add(userToAdd);
        context.SaveChanges();

        var repository = new UserRepository(context);
        var updatedUser = new UserEntity
        {
            Email = "new@example.com",
            Password = "newpassword",
            Profile = new ProfileEntity { FirstName = "New", LastName = "User" },
            Address = new AddressEntity { StreetName = "New Street", PostalCode = "54321", City = "New City" },
            Role = new RoleEntity { RoleName = "NewRole" },
            PhoneNumber = new PhoneNumberEntity { PhoneNumber = "444555666" }
        };
        userToAdd.Email = updatedUser.Email;
        userToAdd.Password = updatedUser.Password;
        userToAdd.Profile = updatedUser.Profile;
        userToAdd.Address = updatedUser.Address;
        userToAdd.Role = updatedUser.Role;
        userToAdd.PhoneNumber = updatedUser.PhoneNumber;
        // Act
        var result = repository.Update(u => u.UserId == userToAdd.UserId, userToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("new@example.com", result.Email);
    }

    [Fact]
    public void Delete_ShouldDeleteUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        var userToAdd = new UserEntity
        {
            Email = "Test@example.com",
            Password = "password",
            Profile = new ProfileEntity { FirstName = "Test", LastName = "Test" },
            Address = new AddressEntity { StreetName = "Street", PostalCode = "54321", City = "City" },
            Role = new RoleEntity { RoleName = "User" },
            PhoneNumber = new PhoneNumberEntity { PhoneNumber = "999888777" }
        };
        context.Users.Add(userToAdd);
        context.SaveChanges();
        // Act
        var repository = new UserRepository(context);
        var result = repository.Delete(u => u.Email == "Test@example.com").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Users.FirstOrDefault(u => u.Email == "Test@example.com"));
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingUser()
    {
        using var context = new DataContext(_options);
        var userToAdd = new UserEntity
        {
            Email = "Test@example.com",
            Password = "password",
            Profile = new ProfileEntity { FirstName = "Test", LastName = "Test" },
            Address = new AddressEntity { StreetName = "Street", PostalCode = "11111", City = "City" },
            Role = new RoleEntity { RoleName = "User" },
            PhoneNumber = new PhoneNumberEntity { PhoneNumber = "111222333" }
        };
        context.Users.Add(userToAdd);
        context.SaveChanges();
        // Act
        var repository = new UserRepository(context);
        var result = repository.Exists(u => u.Email == "Test@example.com").Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseForNonExistingUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        var repository = new UserRepository(context);
        // Act
        var result = repository.Exists(u => u.Email == "noexisting@example.com").Result;
        // Assert
        Assert.False(result);
    }
    public void Dispose()
    {
        using var context = new DataContext(_options);
        context.Database.EnsureDeleted();
    }
}
