using System;
using System.Security.Cryptography.X509Certificates;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IRepositoryProduct
{
    public void CreateProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products.Select(u=>u.Brand).
        Distinct()
        .ToListAsync();
    }

    public async Task<Product?> GetProductByid(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProducts(string? brand,string?type,string? sort)
    {
        var query=context.Products.AsQueryable();
        if(!String.IsNullOrEmpty(brand))
        {
            query=query.Where(x=>x.Brand==brand);
        }
        if(!String.IsNullOrEmpty(type))
        {
            query=query.Where(x=>x.Type==type);
        }
        query=sort switch
        {
            "PriceAsc"=>query.OrderBy(x=>x.Price),
            "PriceDsc"=>query.OrderByDescending(x=>x.Price),
            _ =>query.OrderBy(x=>x.Name)
        };

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypessAsync()
    {
        return await context.Products.Select(u=>u.Type)
        .Distinct()
        .ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(x=>x.Id==id);
    }

    public async Task<bool> SaveChangeAsync()
    {
       return await context.SaveChangesAsync()>0;
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State=EntityState.Modified;
    }
}
