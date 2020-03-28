namespace Northwind.Services.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represent picture for each category.
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// Gets or sets id of picture.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets picture content(the image).
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Gets or sets picture name in filesystem.
        /// </summary>
        [Display(Name = "File Name")]
        public string UntrustedName { get; set; }

        /// <summary>
        /// Gets or sets a note for a picture.
        /// </summary>
        [Display(Name = "Note")]
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets size of a picture.
        /// </summary>
        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets time, when picture was add.
        /// </summary>
        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime UploadDT { get; set; }
    }
}
