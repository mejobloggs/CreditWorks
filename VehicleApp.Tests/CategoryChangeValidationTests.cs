using VehicleApp.WebApi.Enums;
using VehicleApp.WebApi.Models;
using VehicleApp.WebApi.Validators;

namespace VehicleApp.Tests;

[TestClass]
public class CategoryChangeValidationTests
{
    private readonly IReadOnlyList<Category> defaultCategories = new List<Category> {
        new Category { Id = 1, Name = "Light", MinKg = 0, MaxKg = 499.99M, ImageName = string.Empty },
        new Category { Id = 2, Name = "Medium", MinKg = 500, MaxKg = 2499.99M, ImageName = string.Empty },
        new Category { Id = 3, Name = "Heavy", MinKg = 2500, ImageName = string.Empty }
    };

    private readonly CategoryChangeValidator validator = new();

    [TestMethod]
    public void DecreasingMediumMaxReturnsFalse()
    {
        var cats = defaultCategories.ToList();

        cats.Find(c => c.Id == 2).MaxKg = 2499.98M;

        var result = validator.IsValid(cats);

        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.ValidationType == ValidationType.ContainsContinuousRanges);
    }

    [TestMethod]
    public void HavingNoMaxNullStillReturnsTrue()
    {
        var cats = defaultCategories.ToList();

        cats.Last().MaxKg = 4444;

        var result = validator.IsValid(cats);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void ValidDeleteReturnsTrue()
    {
        var cats = defaultCategories.ToList();

        cats.Remove(cats.Last());

        var result = validator.IsValid(cats);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void InsertingAndAdjustingValidNewRangeReturnsTrue()
    {
        var cats = defaultCategories.ToList();

        cats.First().MaxKg = 249.99M;
        cats.Find(c => c.Id == 2).MinKg = 1000;

        cats.Add(new Category { Name = "LightMedium", MinKg = 250, MaxKg = 999.99M, ImageName = string.Empty });

        var result = validator.IsValid(cats);
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void DuplicateWeightsReturnsFalse()
    {
        var cats = defaultCategories.ToList();

        cats.Add(new Category { Name = "Medium2", MinKg = 501, MaxKg = 2499.99M, ImageName = string.Empty });

        var result = validator.IsValid(cats);
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.ValidationType == ValidationType.ContainsNoDuplicateValues);
    }
}