namespace Northwind.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Northwind.Services.Context;

    /// <summary>
    /// Represents a stub for a product categories picture management service.
    /// </summary>
    public class ProductCategoryPictureService : IProductCategoryPicturesService
    {
        private NorthwindContext Context { get; set; } = new NorthwindContext(new Microsoft.EntityFrameworkCore.DbContextOptions<NorthwindContext>());

        /// <inheritdoc/>
        public bool DestroyPicture(int categoryId)
        {
            try
            {
                var category = new ProductCategory();
                if (new ProductCategoryManagementService().TryShowCategory(categoryId, out category))
                {
                    File.Delete("C:" + category.Description);
                    category.Description = "No Picture";
                    new ProductCategoryManagementService().UpdateCategories(categoryId, category);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryShowPicture(int categoryId, out byte[] bytes)
        {
            foreach (var category in this.Context.GetCategories())
            {
                var categoryObj = ProductCategoryManagementService.FromStrToProductCategory(category);
                if (categoryObj.Id == categoryId)
                {
                    try
                    {
                        bytes = File.ReadAllBytes(categoryObj.Description);
                        return true;
                    }
                    catch (FileNotFoundException)
                    {
                        bytes = null;
                        return false;
                    }
                }
            }

            bytes = null;
            return false;
        }

        /// <inheritdoc/>
        public bool UpdatePicture(int categoryId, Stream stream)
        {
            foreach (var categoryRow in this.Context.GetCategories())
            {
                var categoryObj = ProductCategoryManagementService.FromStrToProductCategory(categoryRow);
                if (categoryId == categoryObj.Id)
                {
                    categoryObj.Description = $"\\Users\\PrimoTibalt\\Desktop\\Studying\\C#\\ASPWEBAPI\\NorthwindApiApp\\NorthwindApiApp\\obj\\images\\{categoryObj.Id}";
                    new ProductCategoryManagementService().UpdateCategories(categoryId, categoryObj);
                    return true;
                }
            }

            return false;
        }
    }
}
