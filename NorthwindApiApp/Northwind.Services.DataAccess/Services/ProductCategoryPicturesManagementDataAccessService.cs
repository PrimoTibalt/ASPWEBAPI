using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;


namespace Northwind.Services.DataAccess.Services
{
    public class ProductCategoryPicturesManagementDataAccessService : IProductCategoryPicturesService
    {
        private IProductCategoryDataAccessObject accessObject { get; set; }

        public ProductCategoryPicturesManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            this.accessObject = factory.GetProductCategoryDataAccessObject();
        }

        public bool DestroyPicture(int categoryId)
        {
            var transfer = this.accessObject.FindProductCategory(categoryId);
            transfer.Picture = null;
            return this.accessObject.UpdateProductCategory(transfer);
        }

        public bool TryShowPicture(int categoryId, out byte[] bytes)
        {
            try
            {
                bytes = this.accessObject.FindProductCategory(categoryId).Picture;
                return true;
            }
            catch (ArgumentNullException)
            {
                bytes = null;
                return false;
            }
        }

        public bool UpdatePicture(int categoryId, Stream stream)
        {
            using (stream)
            {
                var transfer = this.accessObject.FindProductCategory(categoryId);
                stream.Read(transfer.Picture, 0, int.MaxValue);
                return this.accessObject.UpdateProductCategory(transfer);
            }
        }
    }
}
