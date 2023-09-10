using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VehicleApp.WebApi.Database;

namespace VehicleApp.WebApi.DataHelpers;

public class CategoryDataHelper
{
    private readonly VehicleAppDb db;

    public CategoryDataHelper(VehicleAppDb db)
    {
        this.db = db;
    }
    public async Task Update(IReadOnlyList<Models.Category> categories)
    {
        var dbCategories = await db.Categories.ToListAsync();

        //default .net concurrency check only checks modified,
        //so adding a custom check to also cover hard deletes
        ConcurrencyCheck(categories, dbCategories);

        //deletes
        foreach (var category in categories.Where(c => c.Delete))
        {
            if (dbCategories.Find(c => c.Id == category.Id) is Entities.Category existing)
            {
                db.Categories.Remove(existing);
            }
        }

        //updates
        foreach (var category in categories.Where(c => c.Id > 0 && !c.Delete))
        {
            if (dbCategories.Find(c => c.Id == category.Id) is Entities.Category existing)
            {
                existing.Update(category);
            }
        }

        //adds
        foreach (var category in categories.Where(c => c.Id == 0))
        {
            db.Categories.Add(new Entities.Category(category));
        }

        await db.SaveChangesAsync();
    }

    private void ConcurrencyCheck(IReadOnlyList<Models.Category> categories, IReadOnlyList<Entities.Category> dbCategories)
    {
        //ensures that all existing database categories exist in the new categories, and also have same rowversion

        bool allCurrent = dbCategories.All(dbCat => categories.Any(cat =>  cat.Id == dbCat.Id && cat.RowVersion.SequenceEqual(dbCat.RowVersion)));
        if(!allCurrent)
        {
            throw new DbUpdateConcurrencyException();
        }
    }
}
