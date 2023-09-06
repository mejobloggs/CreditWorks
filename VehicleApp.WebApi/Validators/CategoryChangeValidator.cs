using VehicleApp.WebApi.Entities;

namespace VehicleApp.WebApi.Validators;

public class CategoryChangeValidator
{
    //Improvement idea: return all failure messages to user instead of stopping at the first failure
    public ValidationResult IsValid(IReadOnlyList<Models.Category> categories)
    {
        var nonDeletedCategories = categories.Where(c => !c.Delete).ToList().AsReadOnly();

        var validators = new List<Func<IReadOnlyList<Models.Category>, ValidationResult>>()
        {
            ContainsNoDuplicateValues,
            ContainsContinuousRanges
            //could add more rules here if new validation cases are discovered
        };
        
        foreach(var validator in validators)
        {
            var result = validator(nonDeletedCategories);
            if(!result.IsValid)
            {
                return result;
            }
        }

        return new ValidationResult(true);
    }

    private ValidationResult ContainsNoDuplicateValues(IReadOnlyList<Models.Category> categories)
    {
        var validationType = Enums.ValidationType.ContainsNoDuplicateValues;

        if (categories.Select(c => c.MinKg).Distinct().Count() != categories.Count)
        {
            return new ValidationResult(false, "This change causes a duplicate MinKg", validationType);
        }

        if (categories.Select(c => c.MaxKg).Distinct().Count() != categories.Count)
        {
            return new ValidationResult(false, "This change causes a duplicate MaxKg", validationType);
        }

        return new ValidationResult(true);
    }

    private const string continuousRangesErrorMessage = "Broken weight range detected between {0} and {1}";
    private ValidationResult ContainsContinuousRanges(IReadOnlyList<Models.Category> categories)
    {
        var validationType = Enums.ValidationType.ContainsContinuousRanges;

        if (categories.Count == 1)
            return new ValidationResult(true);

        var orderedCats = categories.OrderBy(c => c.MinKg).ToList();

        for (int i = 1; i < orderedCats.Count; i++)
        {
            var previousCat = orderedCats[i - 1];
            var currentCat = orderedCats[i];

            decimal? previousMaxKg = previousCat.MaxKg;
            decimal currentMinKg = currentCat.MinKg;

            //Only the last category should have null MaxKg, so if the previous Max is null, thats a gap
            if (previousMaxKg == null)
            {
                return new ValidationResult(false, string.Format(continuousRangesErrorMessage, currentCat.Name, previousCat.Name), validationType);
            }

            if (previousMaxKg + 0.01M != currentMinKg)
            {
                return new ValidationResult(false, string.Format(continuousRangesErrorMessage, currentCat.Name, previousCat.Name), validationType);
            }
        }

        return new ValidationResult(true);
    }

}
