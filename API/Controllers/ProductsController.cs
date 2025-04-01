using CORE.Entities;
using CORE.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IRepositoryProduct Repo) : ControllerBase
    {
       

       
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,
        string?type,string?sort)
        {
            return Ok(await Repo.GetProducts(brand,type,sort));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>>GetProduct(int id)
        {
            var product= await Repo.GetProductByid(id);
            if(product==null) return NotFound();
            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>>CreateProduct(Product product)
        {
          Repo.CreateProduct(product);
         if(await Repo.SaveChangeAsync())
         return CreatedAtAction("GetProduct",new{id=product.Id},product);
          return BadRequest("Problem created product");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult>UpdateProduct(int id, Product product)
        {
          if(id!=product.Id||!ProductExists(id)) return BadRequest("cannot find product");
          Repo.UpdateProduct(product);
         if (await Repo.SaveChangeAsync())
         return NoContent();
          return BadRequest("problem updating product");
        }
        private bool ProductExists(int id)
        {
            return Repo.ProductExists(id);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult>DeleteProduct(int id)
        {
            
            var product= await Repo.GetProductByid(id);
            if(product==null) return NotFound();
           Repo.DeleteProduct(product);
           if(await Repo.SaveChangeAsync())
           return NoContent();
           return BadRequest("problem deleting product");
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>>GetBrand()
        {
            return  Ok(await Repo.GetBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<String>>>GetTypes()
        {

            return Ok(await Repo.GetTypessAsync());
        }
    }
}
