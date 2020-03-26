namespace Northwind.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Northwind.Services.Context;

    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private NorthwindContext Context { get; set; } = new NorthwindContext(new Microsoft.EntityFrameworkCore.DbContextOptions<NorthwindContext>());

        /// <inheritdoc/>
        public int CreateCategory(ProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool DestroyCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool DestroyPicture(int categoryId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool DestroyProduct(int productId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<ProductCategory> LookupCategoriesByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<Product> LookupProductsByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<ProductCategory> ShowCategories(int offset, int limit)
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (var category in this.Context.GetCategories())
            {
                categories.Add(this.FromStrToProductCategory(category));
            }

            return categories;
        }

        private ProductCategory FromStrToProductCategory(string from)
        {
            ProductCategory to = new ProductCategory();
            var splited = from.Split(',');
            to.Id = int.Parse(splited[0].Split(':')[1], CultureInfo.InvariantCulture);
            to.Name = splited[1].Split(':')[1];
            to.Description = splited[2].Split(':')[1];
            return to;
        }

        /// <inheritdoc/>
        public IList<Product> ShowProducts(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<Product> ShowProductsForCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            productCategory = new ProductCategory() { Id = 1, };
            return true;
        }

        /// <inheritdoc/>
        public bool TryShowPicture(int categoryId, out byte[] bytes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryShowProduct(int productId, out Product product)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateCategories(int categoryId, ProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdatePicture(int categoryId, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateProduct(int productId, Product product)
        {
            throw new NotImplementedException();
        }
    }
}