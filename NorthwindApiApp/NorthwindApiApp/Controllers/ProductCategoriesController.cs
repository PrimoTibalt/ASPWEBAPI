using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Northwind.Services.Products;
using NorthwindApiApp.Utilities;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class ProductCategoriesController : Controller
    {
        private IProductCategoryManagementService productCategoryManagementService { get; set; }

        private IProductCategoryPicturesService productCategoryPicturesService { get; set; }

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public ProductCategoriesController(IProductCategoryManagementService categoryService, IProductCategoryPicturesService pictureService)
        {
            this.productCategoryManagementService = categoryService;
            this.productCategoryPicturesService = pictureService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ProductCategory>> GetSeveral()
        {
            try
            {
                return Ok(this.productCategoryManagementService.ShowCategories(Int32.MaxValue, Int32.MaxValue));
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
                if (this.productCategoryManagementService.TryShowCategory(id, out category))
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
                var categories = this.productCategoryManagementService.ShowCategories(10, 10);
                int newId = ProductCategoriesController.CalculateNewId(categories);

                this.productCategoryManagementService.CreateCategory(new ProductCategory()
                {
                    Id = newId,
                    Name = name,
                    Description = description + $" - item {this.productCategoryManagementService.ShowCategories(10, 10).Count + 1}.",
                });
                return Ok(this.productCategoryManagementService.ShowCategories(10, 10));
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
                if (this.productCategoryManagementService.DestroyCategory(id))
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
            if (this.productCategoryManagementService.UpdateCategories(id, category))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}/{property}")]
        public IActionResult GetPicture(int id, string property)
        {
            byte[] bytes = null;
            try
            {
                if (this.productCategoryPicturesService.TryShowPicture(id, out bytes))
                {
                    return Ok(File(bytes, "multipart/form-data;"));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPut("{id}/{property}")]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadPicture(int id, string property)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");

                return BadRequest(ModelState);
            }

            if (!this.productCategoryPicturesService.UpdatePicture(id, null))
            {
                return BadRequest("No such id");
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File",
                            $"The request couldn't be processed (Error 2).");
                        // Log error

                        return BadRequest(ModelState);
                    }
                    else
                    {
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = id.ToString();

                        var streamedFileContent = await Utilities.FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, ModelState,
                            new string[] { ".png", ".jpg", ".jped", ".gif" }, Int32.MaxValue);

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }
                        
                        using (var targetStream = System.IO.File.Create(
                            Path.Combine(@"C:\Users\PrimoTibalt\Desktop\Studying\C#\ASPWEBAPI\NorthwindApiApp\NorthwindApiApp\obj\images", trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return Created(nameof(ProductCategoriesController), null);
        }

        [HttpDelete("{id}/{property}")]
        public IActionResult DeletePicture(int id, string property)
        {
            if (this.productCategoryPicturesService.DestroyPicture(id))
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader =
                MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            // UTF-7 is insecure and shouldn't be honored. UTF-8 succeeds in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }

        private static int CalculateNewId(IList<ProductCategory> categories)
        {
            int goodNewId = -1;
            for (int newId = 1; newId < Int32.MaxValue; newId++)
            {
                foreach (var category in categories)
                {
                    if (category.Id == newId)
                    {
                        break;
                    }

                    goodNewId = newId;
                }

                if (goodNewId != -1)
                {
                    break;
                }
            }

            return goodNewId;
        }
    }

    public class FormData
    {
        public string Note { get; set; }
    }
}