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
                    return Ok();
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
                this.productManagementService.CreateCategory(new ProductCategory()
                {
                    // TO DO: make normal creating of ProductCategory.
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

        [HttpPut]
        public IActionResult Update(int id, ProductCategory category)
        {
            try
            {
                if(this.productManagementService.UpdateCategories(id, category))
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