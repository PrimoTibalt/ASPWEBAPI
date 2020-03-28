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
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool DestroyPicture(int categoryId)
        {
            try
            {
                var category = new ProductCategory();
                if (this.TryShowCategory(categoryId, out category))
                {
                    File.Delete("C:" + category.Description);
                    category.Description = "No Picture";
                    this.UpdateCategories(categoryId, category);
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
        public bool DestroyProduct(int productId)
        {
            try
            {
                this.Context.DeleteProduct(productId);
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
                ProductCategory currentCategory = this.FromStrToProductCategory(category);
                if (names.Contains(currentCategory.Name))
                {
                    categoriesWithSameName.Add(currentCategory);
                }
            }

            return categoriesWithSameName;
        }

        /// <inheritdoc/>
        public IList<Product> LookupProductsByName(IList<string> names)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            List<Product> productsWithSameName = new List<Product>();
            foreach (var product in this.Context.GetProducts())
            {
                Product currentProduct = this.FromStrToProduct(product);
                if (names.Contains(currentProduct.Name))
                {
                    productsWithSameName.Add(currentProduct);
                }
            }

            return productsWithSameName;
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
            foreach (var category in this.Context.GetCategories())
            {
                var categoryObj = this.FromStrToProductCategory(category);
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

        /// <inheritdoc/>
        public bool UpdatePicture(int categoryId, Stream stream)
        {
            foreach (var categoryRow in this.Context.GetCategories())
            {
                var categoryObj = this.FromStrToProductCategory(categoryRow);
                if (categoryId == categoryObj.Id)
                {
                    categoryObj.Description = $"\\Users\\PrimoTibalt\\Desktop\\Studying\\C#\\ASPWEBAPI\\NorthwindApiApp\\NorthwindApiApp\\obj\\images\\{categoryObj.Id}";
                    this.UpdateCategories(categoryId, categoryObj);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool UpdateProduct(int productId, Product product)
        {
            try
            {
                if (this.Context.UpdateProduct(product))
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