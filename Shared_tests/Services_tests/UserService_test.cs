using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Dtos;
using Shared.Entities;
using Shared.Repositories;
using Shared.Services;

namespace Shared_tests.Services_tests;

public class UserService_test : IDisposable
{
    private readonly DbContextOptions<DataContext> _options;

    public UserService_test()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }


    [Fact]
    public void CreateUser_ShouldAddUserToDatabase()
    {
        // Arrange
        using var context = new DataContext(_options);
        var addressRepository = new AddressRepository(context);
        var phoneNumberRepository = new PhoneNumberRepository(context);
        var profileRepository = new ProfileRepository(context);
        var roleRepository = new RoleRepository(context);
        var userRepository = new UserRepository(context);
        var userService = new UserService(addressRepository, phoneNumberRepository, profileRepository, roleRepository, userRepository);

        var userDto = new UserDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "Test@example.com",
            Password = "password",
            RoleName = "User",
            PhoneNumber = "123456789",
            StreetName = "Teststreet",
            PostalCode = "12345",
            City = "City"
        };
        // Act
        var result = userService.CreateUser(userDto).Result;
        // Assert
        Assert.True(result);
        
    }

    [Fact]
    public void GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Users.AddRange(
            new UserEntity
            {
                Profile = new ProfileEntity { FirstName = "User1", LastName = "One" },
                Address = new AddressEntity { StreetName = "Street1", PostalCode = "11111", City = "City1" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "111111111" },
                Role = new RoleEntity { RoleName = "Role1" },
                Email = "user1@example.com",
                Password = "password1"
            },
            new UserEntity
            {
                Profile = new ProfileEntity { FirstName = "User2", LastName = "Two" },
                Address = new AddressEntity { StreetName = "Street2", PostalCode = "22222", City = "City2" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "222222222" },
                Role = new RoleEntity { RoleName = "Role2" },
                Email = "user2@example.com",
                Password = "password2"
            }
        );
        context.SaveChanges();

        var addressRepository = new AddressRepository(context);
        var phoneNumberRepository = new PhoneNumberRepository(context);
        var profileRepository = new ProfileRepository(context);
        var roleRepository = new RoleRepository(context);
        var userRepository = new UserRepository(context);
        var userService = new UserService(addressRepository, phoneNumberRepository, profileRepository, roleRepository, userRepository);
        // Act
        var result = userService.GetAllUsers().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOneUser_ShouldReturnSingleUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Users.Add(
            new UserEntity
            {
                Profile = new ProfileEntity { FirstName = "Test", LastName = "Test" },
                Address = new AddressEntity { StreetName = "Teststreet", PostalCode = "12345", City = "City" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "123456789" },
                Role = new RoleEntity { RoleName = "User" },
                Email = "Test@example.com",
                Password = "password"
            }
        );
        context.SaveChanges();

        var addressRepository = new AddressRepository(context);
        var phoneNumberRepository = new PhoneNumberRepository(context);
        var profileRepository = new ProfileRepository(context);
        var roleRepository = new RoleRepository(context);
        var userRepository = new UserRepository(context);
        var userService = new UserService(addressRepository, phoneNumberRepository, profileRepository, roleRepository, userRepository);
        // Act
        var userDto = new UserEntity { Email = "Test@example.com" };
        var result = userService.GetOneUser(userDto).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.FirstName);
        Assert.Equal("Test", result.LastName);
        Assert.Equal("123456789", result.PhoneNumber);
        Assert.Equal("User", result.RoleName);
        Assert.Equal("Teststreet", result.StreetName);
        Assert.Equal("12345", result.PostalCode);
        Assert.Equal("City", result.City);
    }

    [Fact]
    public void UpdateUser_ShouldUpdateUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Users.Add(
            new UserEntity
            {
                Profile = new ProfileEntity { FirstName = "Old", LastName = "User" },
                Address = new AddressEntity { StreetName = "Old Street", PostalCode = "11111", City = "Old City" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "111111111" },
                Role = new RoleEntity { RoleName = "OldRole" },
                Email = "olduser@example.com",
                Password = "oldpassword"
            }
        );
        context.SaveChanges();

        var addressRepository = new AddressRepository(context);
        var phoneNumberRepository = new PhoneNumberRepository(context);
        var profileRepository = new ProfileRepository(context);
        var roleRepository = new RoleRepository(context);
        var userRepository = new UserRepository(context);
        var userService = new UserService(addressRepository, phoneNumberRepository, profileRepository, roleRepository, userRepository);

        var updatedUserDto = new UserDto
        {
            FirstName = "New",
            LastName = "User",
            Email = "olduser@example.com",
            Password = "newpassword",
            RoleName = "NewRole",
            PhoneNumber = "999999999",
            StreetName = "New Street",
            PostalCode = "22222",
            City = "New City"
        };
        // Act
        var result = userService.UpdateUser(updatedUserDto).Result;
        // Assert
        Assert.True(result);
        var updatedUser = context.Users.Include(u => u.Profile).Include(u => u.Address).Include(u => u.Role).Include(u => u.PhoneNumber).FirstOrDefault(u => u.Email == "olduser@example.com");
        Assert.NotNull(updatedUser);
        Assert.Equal("New", updatedUser.Profile.FirstName);
        Assert.Equal("User", updatedUser.Profile.LastName);
        Assert.Equal("999999999", updatedUser.PhoneNumber.PhoneNumber);
        Assert.Equal("NewRole", updatedUser.Role.RoleName);
        Assert.Equal("New Street", updatedUser.Address.StreetName);
        Assert.Equal("22222", updatedUser.Address.PostalCode);
        Assert.Equal("New City", updatedUser.Address.City);
    }

    [Fact]
    public void DeleteUser_ShouldDeleteUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Users.Add(
            new UserEntity
            {
                Profile = new ProfileEntity { FirstName = "Test", LastName = "Test" },
                Address = new AddressEntity { StreetName = "TestStreet", PostalCode = "12345", City = "City" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "987654321" },
                Role = new RoleEntity { RoleName = "User" },
                Email = "Test@example.com",
                Password = "Password"
            }
        );
        context.SaveChanges();

        var addressRepository = new AddressRepository(context);
        var phoneNumberRepository = new PhoneNumberRepository(context);
        var profileRepository = new ProfileRepository(context);
        var roleRepository = new RoleRepository(context);
        var userRepository = new UserRepository(context);
        var userService = new UserService(addressRepository, phoneNumberRepository, profileRepository, roleRepository, userRepository);
        // Act
        var userDtoToDelete = new UserDto { Email = "Test@example.com" };
        var result = userService.DeleteUser(userDtoToDelete).Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Users.FirstOrDefault(u => u.Email == "Test@example.com"));
    }

    [Fact]
    public void CheckIfUserExistsAsync_ShouldReturnTrueForExistingUser()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Users.Add(
            new UserEntity
            {
                Profile = new ProfileEntity { FirstName = "Existing", LastName = "User" },
                Address = new AddressEntity { StreetName = "Existing Street", PostalCode = "11111", City = "Existing City" },
                PhoneNumber = new PhoneNumberEntity { PhoneNumber = "111111111" },
                Role = new RoleEntity { RoleName = "ExistingRole" },
                Email = "existing@example.com",
                Password = "existingpassword"
            }
        );
        context.SaveChanges();

        var addressRepository = new AddressRepository(context);
        var phoneNumberRepository = new PhoneNumberRepository(context);
        var profileRepository = new ProfileRepository(context);
        var roleRepository = new RoleRepository(context);
        var userRepository = new UserRepository(context);
        var userService = new UserService(addressRepository, phoneNumberRepository, profileRepository, roleRepository, userRepository);
        // Act
        var result = userService.CheckIfUserExistsAsync("existing@example.com").Result;
        // Assert
        Assert.True(result);
    }

    public void Dispose()
    {
        using var context = new DataContext(_options);
        context.Database.EnsureDeleted();
    }
}
