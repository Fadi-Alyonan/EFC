using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class RoleRepository_test : IDisposable
{
    private readonly DbContextOptions<DataContext> _options;

    public RoleRepository_test()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Create_RoleEntity_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new DataContext(_options);
        var repository = new RoleRepository(context);
        var role = new RoleEntity
        {
            RoleName = "Admin"
        };
        // Act
        var result = repository.Create(role).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
    }

    [Fact]
    public void GetAll_ShouldReturnAllRoles()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Roles.AddRange(
            new RoleEntity { RoleName = "Admin" },
            new RoleEntity { RoleName = "User" }
        );
        context.SaveChanges();
        // Act
        var repository = new RoleRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSingleRole()
    {
        // Arrange
        using var context = new DataContext(_options);
        var roleToAdd = new RoleEntity { RoleName = "Admin" };
        context.Roles.Add(roleToAdd);
        context.SaveChanges();
        // Act
        var repository = new RoleRepository(context);
        var result = repository.GetOne(r => r.RoleName == "Admin").Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
    }

    [Fact]
    public void Update_ShouldUpdateRole()
    {
        // Arrange
        using var context = new DataContext(_options);
        var roleToAdd = new RoleEntity { RoleName = "OldRole" };
        context.Roles.Add(roleToAdd);
        context.SaveChanges();

        var repository = new RoleRepository(context);
        var updatedRole = new RoleEntity { RoleName = "NewRole" };
        roleToAdd.RoleName = updatedRole.RoleName;
        // Act
        var result = repository.Update(r => r.RoleId == roleToAdd.RoleId, roleToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewRole", result.RoleName);
    }

    [Fact]
    public void Delete_ShouldDeleteRole()
    {
        // Arrange
        using var context = new DataContext(_options);
        var roleToAdd = new RoleEntity { RoleName = "RoleToDelete" };
        context.Roles.Add(roleToAdd);
        context.SaveChanges();
        // Act
        var repository = new RoleRepository(context);
        var result = repository.Delete(r => r.RoleName == "RoleToDelete").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Roles.FirstOrDefault(r => r.RoleName == "RoleToDelete"));
    }
    public void Dispose()
    {
        using var context = new DataContext(_options);
        context.Database.EnsureDeleted();
    }

}
