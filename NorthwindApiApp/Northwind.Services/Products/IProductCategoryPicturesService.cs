namespace Northwind.Services.Products
{
    using System.IO;

    /// <summary>
    /// Represents a management service for pictures.
    /// </summary>
    public interface IProductCategoryPicturesService
    {
        /// <summary>
        /// Try to show a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <param name="bytes">An array of picture bytes.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        bool TryShowPicture(int categoryId, out byte[] bytes);

        /// <summary>
        /// Update a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        bool UpdatePicture(int categoryId, Stream stream);

        /// <summary>
        /// Destroy a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        bool DestroyPicture(int categoryId);
    }
}
