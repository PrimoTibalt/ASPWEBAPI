using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Services.Products;

namespace Northwind.Services.DataAccess.Services
{
    public class ProductManagementDataAccessService : IProductManagementService
    {
        public int CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public bool DestroyProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public IList<Product> LookupProductsByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        public IList<Product> ShowProducts(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public IList<Product> ShowProductsForCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public bool TryShowProduct(int productId, out Product product)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProduct(int productId, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
