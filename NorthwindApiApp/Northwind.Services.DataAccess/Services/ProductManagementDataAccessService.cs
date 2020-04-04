using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.Services.DataAccess.Services
{
    public class ProductManagementDataAccessService : IProductManagementService
    {
        private IProductDataAccessObject accesssObject { get; set; }

        public ProductManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            this.accesssObject = factory.GetProductDataAccessObject();
        }

        /// <inheritdoc/>
        public int CreateProduct(Product product)
        {
            return this.accesssObject.InsertProduct(new ProductTransferObject()
            {
                Id = product.Id,
                Name = product.Name,
                Discontinued = product.Discontinued,
                SupplierId = product.SupplierId,
                CategoryId = product.CategoryId,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                UnitsOnOrder = product.UnitsOnOrder,
            });
        }

        /// <inheritdoc/>
        public bool DestroyProduct(int productId)
        {
            return accesssObject.DeleteProduct(productId);
        }

        /// <inheritdoc/>
        public IList<Product> LookupProductsByName(IList<string> names)
        {
            var listOfProductsTransfer = this.accesssObject.SelectProductsByName(names).ToList();
            List<Product> listOfProducts = new List<Product>();
            listOfProductsTransfer.ForEach((elem) => listOfProducts.Add(new Product 
            {
                Id = elem.Id,
                Name = elem.Name,
                Discontinued = elem.Discontinued,
                SupplierId = elem.SupplierId,
                CategoryId = elem.CategoryId,
                UnitPrice = elem.UnitPrice,
                UnitsInStock = elem.UnitsInStock,
                QuantityPerUnit = elem.QuantityPerUnit,
                ReorderLevel = elem.ReorderLevel,
                UnitsOnOrder = elem.UnitsOnOrder,
            }));
            return listOfProducts;
        }

        /// <inheritdoc/>
        public IList<Product> ShowProducts(int offset, int limit)
        {
            var listOfProductsTransfer = this.accesssObject.SelectProducts(0, int.MaxValue).ToList();
            List<Product> listOfProducts = new List<Product>();
            listOfProductsTransfer.ForEach((elem) => listOfProducts.Add(new Product
            {
                Id = elem.Id,
                Name = elem.Name,
                Discontinued = elem.Discontinued,
                SupplierId = elem.SupplierId,
                CategoryId = elem.CategoryId,
                UnitPrice = elem.UnitPrice,
                UnitsInStock = elem.UnitsInStock,
                QuantityPerUnit = elem.QuantityPerUnit,
                ReorderLevel = elem.ReorderLevel,
                UnitsOnOrder = elem.UnitsOnOrder,
            }));
            return listOfProducts;
        }

        /// <inheritdoc/>
        public IList<Product> ShowProductsForCategory(int categoryId)
        {
            var listOfProductsTransfer = this.accesssObject.SelectProductByCategory(new List<int>() { categoryId }).ToList();
            List<Product> listOfProducts = new List<Product>();
            listOfProductsTransfer.ForEach((elem) => listOfProducts.Add(new Product
            {
                Id = elem.Id,
                Name = elem.Name,
                Discontinued = elem.Discontinued,
                SupplierId = elem.SupplierId,
                CategoryId = elem.CategoryId,
                UnitPrice = elem.UnitPrice,
                UnitsInStock = elem.UnitsInStock,
                QuantityPerUnit = elem.QuantityPerUnit,
                ReorderLevel = elem.ReorderLevel,
                UnitsOnOrder = elem.UnitsOnOrder,
            }));
            return listOfProducts;
        }

        /// <inheritdoc/>
        public bool TryShowProduct(int productId, out Product product)
        {
            try
            {
                var transfer = this.accesssObject.FindProduct(productId);
                product = new Product
                {
                    Id = transfer.Id,
                    Name = transfer.Name,
                    Discontinued = transfer.Discontinued,
                    SupplierId = transfer.SupplierId,
                    CategoryId = transfer.CategoryId,
                    UnitPrice = transfer.UnitPrice,
                    UnitsInStock = transfer.UnitsInStock,
                    QuantityPerUnit = transfer.QuantityPerUnit,
                    ReorderLevel = transfer.ReorderLevel,
                    UnitsOnOrder = transfer.UnitsOnOrder,
                };
                return true;
            }
            catch (ArgumentNullException)
            {
                product = null;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool UpdateProduct(int productId, Product product)
        {
            var transfer = new ProductTransferObject
            {
                Id = productId,
                Name = product.Name,
                Discontinued = product.Discontinued,
                SupplierId = product.SupplierId,
                CategoryId = product.CategoryId,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                UnitsOnOrder = product.UnitsOnOrder,
            };
            return this.accesssObject.UpdateProduct(transfer);
        }
    }
}
