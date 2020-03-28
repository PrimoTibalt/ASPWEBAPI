namespace Northwind.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Northwind.Services.Context;

    /// <summary>
    /// Represents a stub for a product categories management service.
    /// </summary>
    public class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private NorthwindContext Context { get; set; } = new NorthwindContext(new Microsoft.EntityFrameworkCore.DbContextOptions<NorthwindContext>());

        /// <summary>
        /// Transmits string to ProductCategory instance.
        /// </summary>
        /// <param name="from">String in format "ID:value,Name:value,Description:value".</param>
        /// <returns>Instance, gotten from string.</returns>
        public static ProductCategory FromStrToProductCategory(string from)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                throw new ArgumentNullException(nameof(from));
            }

            ProductCategory to = new ProductCategory();
            var splited = from.Split(',');
            to.Id = int.Parse(splited[0].Split(':')[1], CultureInfo.InvariantCulture);
            to.Name = splited[1].Split(':')[1];
            to.Description = splited[2].Split(':')[1];
            return to;
        }

        /// <inheritdoc/>
        public int CreateCategory(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            this.Context.CreateNewCategory(productCategory);
            return productCategory.Id;
        }

        /// <inheritdoc/>
        public bool DestroyCategory(int categoryId)
        {
            try
            {
                this.Context.DeleteCategory(categoryId);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public IList<ProductCategory> LookupCategoriesByName(IList<string> names)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            List<ProductCategory> categoriesWithSameName = new List<ProductCategory>();
            foreach (var category in this.Context.GetCategories())
            {
                ProductCategory currentCategory = ProductCategoryManagementService.FromStrToProductCategory(category);
                if (names.Contains(currentCategory.Name))
                {
                    categoriesWithSameName.Add(currentCategory);
                }
            }

            return categoriesWithSameName;
        }

        /// <inheritdoc/>
        public IList<ProductCategory> ShowCategories(int offset, int limit)
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (var category in this.Context.GetCategories())
            {
                categories.Add(ProductCategoryManagementService.FromStrToProductCategory(category));
            }

            return categories;
        }

        /// <inheritdoc/>
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            foreach (var category in this.Context.GetCategories())
            {
                productCategory = ProductCategoryManagementService.FromStrToProductCategory(category);
                if (productCategory.Id == categoryId)
                {
                    return true;
                }
            }

            productCategory = new ProductCategory();
            return false;
        }

        /// <inheritdoc/>
        public bool UpdateCategories(int categoryId, ProductCategory productCategory)
        {
            try
            {
                if (this.Context.UpdateCategory(productCategory))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
