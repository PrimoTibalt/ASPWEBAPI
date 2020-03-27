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
    [Route("api/categories")]
    public class ProductCategoriesController : Controller
    {
        private IProductManagementService productManagementService { get; set; }

        public ProductCategoriesController(IProductManagementService service)
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
                return Ok(this.productManagementService.ShowCategories(Int32.MaxValue, Int32.MaxValue));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductCategory> GetOne(int id)
        {
            ProductCategory category = new ProductCategory();
            try
            {
                if (this.productManagementService.TryShowCategory(id, out category))
                {
                    return Ok(category);
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

        [HttpPost("{name}/{description}")]
        public IActionResult Create(string name, string description)
        {
            try
            {
                this.productManagementService.CreateCategory(new ProductCategory()
                {
                    Id = this.productManagementService.ShowCategories(10, 10).Count + 1,
                    Name = name,
                    Description = description + $" - item {this.productManagementService.ShowCategories(10, 10).Count + 1}.",
                });
                return Ok(this.productManagementService.ShowCategories(10, 10));
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
                if (this.productManagementService.DestroyCategory(id))
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
        public IActionResult Update(int id, [FromBody]ProductCategory category)
        { 
            if (this.productManagementService.UpdateCategories(id, category))
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