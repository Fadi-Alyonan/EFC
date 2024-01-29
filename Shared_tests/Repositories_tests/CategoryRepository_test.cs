using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using Shared.Repositories;

namespace Shared_tests.Repositories_tests;

public class CategoryRepository_test : IDisposable
{
    private readonly DbContextOptions<ProductDataContext> _options;

    public CategoryRepository_test()
    {
        _options = new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }


    [Fact]
    public void Create_Category_ShouldAddToDatabase()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var repository = new CategoryRepository(context);
        var category = new Category
        {
            CategoryName = "Electronics"
        };
        // Act
        var result = repository.Create(category).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Electronics", result.CategoryName);
    }

    [Fact]
    public void GetAll_ShouldReturnAllCategories()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        context.Categories.AddRange(
            new Category { CategoryName = "Electronics" },
            new Category { CategoryName = "Clothing" }
        );
        context.SaveChanges();

        var repository = new CategoryRepository(context);
        // Act
        var result = repository.GetAll().Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetOne_ShouldReturnSingleCategory()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var categoryToAdd = new Category { CategoryName = "Furniture" };
        context.Categories.Add(categoryToAdd);
        context.SaveChanges();

        var repository = new CategoryRepository(context);
        // Act
        var result = repository.GetOne(c => c.CategoryName == "Furniture").Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Furniture", result.CategoryName);
    }

    [Fact]
    public void Update_ShouldUpdateCategory()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var categoryToAdd = new Category { CategoryName = "OldCategory" };
        context.Categories.Add(categoryToAdd);
        context.SaveChanges();

        var repository = new CategoryRepository(context);
        var updatedCategory = new Category { CategoryName = "NewCategory" };
        categoryToAdd.CategoryName = updatedCategory.CategoryName;
        // Act
        var result = repository.Update(c => c.CategoryId == categoryToAdd.CategoryId, categoryToAdd).Result;
        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewCategory", result.CategoryName);
    }

    [Fact]
    public void Delete_ShouldDeleteCategory()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var categoryToAdd = new Category { CategoryName = "Category" };
        context.Categories.Add(categoryToAdd);
        context.SaveChanges();

        var repository = new CategoryRepository(context);
        // Act
        var result = repository.Delete(c => c.CategoryName == "Category").Result;
        // Assert
        Assert.True(result);
        Assert.Null(context.Categories.FirstOrDefault(c => c.CategoryName == "Category"));
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingCategory()
    {
        // Arrange
        using var context = new ProductDataContext(_options);
        var categoryToAdd = new Category { CategoryName = "Category" };
        context.Categories.Add(categoryToAdd);
        context.SaveChanges();

        var repository = new CategoryRepository(context);
        // Act
        var result = repository.Exists(c => c.CategoryName == "Category").Result;
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseForNonExistingCategory()
    {
        using var context = new ProductDataContext(_options);
        var repository = new CategoryRepository(context);
        var result = repository.Exists(c => c.CategoryName == "NonExistingCategory").Result;

        Assert.False(result);
    }

    public void Dispose()
    {
        using var context = new ProductDataContext(_options);
        context.Database.EnsureDeleted();
    }
}
