using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.Services.DataAccess.Services
{
    public class ProductCategoriesManagementDataAccessService : IProductCategoryManagementService
    {
        private IProductCategoryDataAccessObject accessObject { get; set; }

        public ProductCategoriesManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            this.accessObject = factory.GetProductCategoryDataAccessObject();
        }

        public int CreateCategory(ProductCategory productCategory)
        {
            return this.accessObject.InsertProductCategory(new ProductCategoryTransferObject()
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Description = productCategory.Description,
                Picture = new byte[3],
            });
        }

        public bool DestroyCategory(int categoryId)
        {
            return this.accessObject.DeleteProductCategory(categoryId);
        }

        public IList<ProductCategory> LookupCategoriesByName(IList<string> names)
        {
            var listOfTransfer = this.accessObject.SelectProductCategoriesByName(names);
            List<ProductCategory> listOfCategories = new List<ProductCategory>();
            listOfTransfer.ToList().ForEach((elem) => listOfCategories.Add(new ProductCategory()
            {
                Id = elem.Id,
                Name = elem.Name,
                Description = elem.Description,
            }));
            return listOfCategories;
        }

        public IList<ProductCategory> ShowCategories(int offset, int limit)
        {
            var listOfTransfer = this.accessObject.SelectProductCategories(0, int.MaxValue);
            List<ProductCategory> listOfCategories = new List<ProductCategory>();
            listOfTransfer.ToList().ForEach((elem) => listOfCategories.Add(new ProductCategory()
            {
                Id = elem.Id,
                Name = elem.Name,
                Description = elem.Description,
            }));
            return listOfCategories;
        }

        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            try
            {
                var transfer = this.accessObject.FindProductCategory(categoryId);
                productCategory = new ProductCategory
                {
                    Id = transfer.Id,
                    Name = transfer.Name,
                    Description = transfer.Description,
                };
                return true;
            }
            catch (ArgumentNullException)
            {
                productCategory = null;
                return false;
            }
        }

        public bool UpdateCategories(int categoryId, ProductCategory productCategory)
        {
            var transfer = new ProductCategoryTransferObject()
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Description = productCategory.Description,
            };
            return this.accessObject.UpdateProductCategory(transfer);
        }
    }
}
