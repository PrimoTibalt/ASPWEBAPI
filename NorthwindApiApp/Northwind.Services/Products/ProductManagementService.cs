namespace Northwind.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Northwind.Services.Context;

    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private NorthwindContext Context { get; set; } = new NorthwindContext(new Microsoft.EntityFrameworkCore.DbContextOptions<NorthwindContext>());

        /// <inheritdoc/>
        public int CreateProduct(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Id = ProductManagementService.CalculateNewId(this.ShowProducts(10, 10));

            this.Context.CreateNewProduct(product);
            return product.Id;
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
        public bool UpdateProduct(int productId, Product product)
        {
            try
            {
                if (product is null)
                {
                    return false;
                }

                product.Id = productId;
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

        private static int CalculateNewId(IList<Product> products)
        {
            int goodNewId = -1;
            List<int> idCollection = new List<int>();
            foreach (var product in products)
            {
                idCollection.Add(product.Id);
            }

            for (int newId = 1; newId < int.MaxValue; newId++)
            {
                if (!idCollection.Contains(newId))
                {
                    goodNewId = newId;
                    break;
                }
            }

            return goodNewId;
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