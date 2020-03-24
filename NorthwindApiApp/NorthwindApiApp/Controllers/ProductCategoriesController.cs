using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductCategoriesController : Controller
    {
        private IProductManagementService productManagementService { get; set; }

        public ProductCategoriesController(IProductManagementService service)
        {
            this.productManagementService = service;
        }

        [HttpGet("many{offset, limit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Product>> GetSeveral(int offset, int limit)
        {
            try
            {
                return Ok(this.productManagementService.ShowProducts(offset, limit));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("one{id}")]
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

        [HttpPost]
        public IActionResult Create()
        {
            try
            {
                this.productManagementService.CreateProduct(new Product()
                {
                    // TO DO: make normal creating of Product.
                });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
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

        [HttpPut]
        public IActionResult Update(int id, Product product)
        {
            try
            {
                if(this.productManagementService.UpdateProduct(id, product))
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
    }
}