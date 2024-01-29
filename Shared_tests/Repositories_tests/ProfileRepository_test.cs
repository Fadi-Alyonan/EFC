
using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class ProfileRepository_test : IDisposable
{
    private readonly DbContextOptions<DataContext> _options;

    public ProfileRepository_test()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Create_ProfileEntity_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new DataContext(_options);
        var repository = new ProfileRepository(context);
        var profile = new ProfileEntity
        {
            FirstName = "First",
            LastName = "Last"
        };
        // Act
        var result = repository.Create(profile).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("First", result.FirstName);
        Assert.Equal("Last", result.LastName);
    }

    [Fact]
    public void GetAll_ShouldReturnAllProfiles()
    {
        // Arrange
        using var context = new DataContext(_options);
        context.Profiles.AddRange(
            new ProfileEntity { FirstName = "First 1", LastName = "Last 1" },
            new ProfileEntity { FirstName = "First 2", LastName = "Last 2" }
        );
        context.SaveChanges();
        // Act
        var repository = new ProfileRepository(context);
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSingleProfile()
    {
        // Arrange
        using var context = new DataContext(_options);
        var profileToAdd = new ProfileEntity { FirstName = "First", LastName = "Last" };
        context.Profiles.Add(profileToAdd);
        context.SaveChanges();
        // Act
        var repository = new ProfileRepository(context);
        var result = repository.GetOne(p => p.FirstName == "First").Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("First", result.FirstName);
        Assert.Equal("Last", result.LastName);
    }

    [Fact]
    public void Update_ShouldUpdateProfile()
    {
        // Arrange
        using var context = new DataContext(_options);
        var profileToAdd = new ProfileEntity { FirstName = "OldFirstName", LastName = "OldLastName" };
        context.Profiles.Add(profileToAdd);
        context.SaveChanges();

        var repository = new ProfileRepository(context);
        var updatedProfile = new ProfileEntity { FirstName = "NewFirstName", LastName = "NewLastName" };
        profileToAdd.FirstName = updatedProfile.FirstName;
        profileToAdd.LastName = updatedProfile.LastName;

        // Act
        var result = repository.Update(p => p.ProfileId == profileToAdd.ProfileId, profileToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewFirstName", result.FirstName);
        Assert.Equal("NewLastName", result.LastName);
    }

    [Fact]
    public void Delete_ShouldDeleteProfile()
    {
        // Arrange
        using var context = new DataContext(_options);
        var profileToAdd = new ProfileEntity { FirstName = "First", LastName = "Last" };
        context.Profiles.Add(profileToAdd);
        context.SaveChanges();
        // Act
        var repository = new ProfileRepository(context);
        var result = repository.Delete(p => p.FirstName == "First").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Profiles.FirstOrDefault(p => p.FirstName == "First"));
    }
    public void Dispose()
    {
        using var context = new DataContext(_options);
        context.Database.EnsureDeleted();
    }

}
