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
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            this.Context.CreateNewCategory(productCategory);
            return productCategory.Id;
        }

        /// <inheritdoc/>
        public int CreateProduct(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            this.Context.CreateNewProduct(product);
            return product.Id;
        }

        /// <inheritdoc/>
        public bool DestroyCategory(int categoryId)
        {
            try
            {
                this.Context.DeleteCategory(categoryId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool DestroyPicture(int categoryId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool DestroyProduct(int productId)
        {
            try
            {
                this.Context.DeleteProduct(productId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        /// <inheritdoc/>
        public IList<Product> ShowProducts(int offset, int limit)
        {
            List<Product> products = new List<Product>();
            foreach (var product in this.Context.GetProducts())
            {
                products.Add(this.FromStrToProduct(product));
            }

            return products;
        }

        /// <inheritdoc/>
        public IList<Product> ShowProductsForCategory(int categoryId)
        {
            List<Product> productsWithSameCategory = new List<Product>();
            foreach (var product in this.Context.GetProducts())
            {
                Product currentProduct = this.FromStrToProduct(product);
                if (currentProduct.CategoryId == categoryId)
                {
                    productsWithSameCategory.Add(currentProduct);
                }
            }

            return productsWithSameCategory;
        }

        /// <inheritdoc/>
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            foreach (var category in this.Context.GetCategories())
            {
                productCategory = this.FromStrToProductCategory(category);
                if (productCategory.Id == categoryId)
                {
                    return true;
                }
            }

            productCategory = new ProductCategory();
            return false;
        }

        /// <inheritdoc/>
        public bool TryShowPicture(int categoryId, out byte[] bytes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryShowProduct(int productId, out Product product)
        {
            foreach (var productOfContext in this.Context.GetProducts())
            {
                product = this.FromStrToProduct(productOfContext);
                if (product.Id == productId)
                {
                    return true;
                }
            }

            product = new Product();
            return false;
        }

        /// <inheritdoc/>
        public bool UpdateCategories(int categoryId, ProductCategory productCategory)
        {
            try
            {
                this.Context.UpdateCategory(productCategory);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool UpdatePicture(int categoryId, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateProduct(int productId, Product product)
        {
            try
            {
                this.Context.UpdateProduct(product);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        private Product FromStrToProduct(string from)
        {
            Product to = new Product();
            var splited = from.Split(',');
            to.Id = int.Parse(splited[0].Split(':')[1], CultureInfo.InvariantCulture);
            to.Name = splited[1].Split(':')[1];
            to.SupplierId = int.Parse(splited[2].Split(':')[1], CultureInfo.InvariantCulture);
            to.CategoryId = int.Parse(splited[3].Split(':')[1], CultureInfo.InvariantCulture);
            to.QuantityPerUnit = splited[4].Split(':')[1];
            to.UnitPrice = decimal.Parse(splited[5].Split(':')[1], CultureInfo.InvariantCulture);
            to.UnitsInStock = short.Parse(splited[6].Split(':')[1], CultureInfo.InvariantCulture);
            to.UnitsOnOrder = short.Parse(splited[7].Split(':')[1], CultureInfo.InvariantCulture);
            to.ReorderLevel = short.Parse(splited[8].Split(':')[1], CultureInfo.InvariantCulture);
            to.Discontinued = bool.Parse(splited[9].Split(':')[1]);
            return to;
        }
    }
}