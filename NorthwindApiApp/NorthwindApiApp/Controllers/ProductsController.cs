using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : Controller
    {
        private IProductManagementService productManagementService { get; set; }

        public ProductsController(IProductManagementService service)
        {
            this.productManagementService = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Product>> GetSeveral()
        {
            try
            {
                return Ok(this.productManagementService.ShowProducts(Int32.MaxValue, Int32.MaxValue));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetOne(int id)
        {
            Product product = new Product();
            try
            {
                if (this.productManagementService.TryShowProduct(id, out product))
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("{name}")]
        public IActionResult Create(string name)
        {
            try
            {
                this.productManagementService.CreateProduct(new Product()
                {
                    Id = this.productManagementService.ShowProducts(10, 10).Count,
                    Name = name,
                    SupplierId = this.productManagementService.ShowProducts(10, 10).Count + 5,
                    CategoryId = (new Random().Next(1, 3)),
                    QuantityPerUnit = $"{this.productManagementService.ShowProducts(10, 10).Count} items in 1 unit",
                    UnitPrice = 100 * name.Length,
                    UnitsInStock = 10,
                    UnitsOnOrder = 2,
                    ReorderLevel = 1,
                    Discontinued = false,
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (this.productManagementService.DestroyProduct(id))
                {
                    return Ok();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]Product product)
        {
            if (this.productManagementService.UpdateProduct(id, product))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}