using System;
using System.Text.Json;
using CORE.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed()
{
public static async Task SeedAsync(StoreContext context)
{
    if(!context.Products.Any())
    {
        var productData=await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
        var product=JsonSerializer.Deserialize<List<Product>>(productData);
        if (product==null) return;
         context.AddRange(product);
         await context.SaveChangesAsync();
    }
}
}
