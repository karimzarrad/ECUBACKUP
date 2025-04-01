using System;
using CORE.Entities;

namespace CORE.Interfaces;

public interface IRepositoryProduct
{
Task<IReadOnlyList<Product>>GetProducts(string? brand,string? type,string? sort);
Task<Product?>GetProductByid(int id);
void CreateProduct(Product product);
void UpdateProduct(Product product);
void DeleteProduct(Product product);
Task<IReadOnlyList<String>>GetBrandsAsync();

Task<IReadOnlyList<String>>GetTypessAsync();


bool ProductExists(int id);
Task<bool>SaveChangeAsync();
}
