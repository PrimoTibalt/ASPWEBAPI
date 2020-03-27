namespace Northwind.Services.Context
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using Microsoft.EntityFrameworkCore;
    using Northwind.Services.Products;

    /// <summary>
    /// Context for in-memory database Northwind.
    /// </summary>
    public class NorthwindContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NorthwindContext"/> class.
        /// Сreates DataSet for it.
        /// </summary>
        /// <param name="options">Options for context.</param>
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
            // Creating my own data set with Products, Categories and Employees.
            this.Set = new DataSet("Northwind");

            // Categories.
            DataTable categories = this.Set.Tables.Add("Categories");
            DataColumn categoryId = categories.Columns.Add("ID", typeof(int));
            categories.Columns.Add("Name", typeof(string));
            categories.Columns.Add("Description", typeof(string));
            categories.PrimaryKey = new DataColumn[]
            {
                categoryId,
            };

            // Products.
            DataTable products = this.Set.Tables.Add("Products");
            DataColumn productId = products.Columns.Add("ID", typeof(int));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("SupplierID", typeof(int));
            products.Columns.Add("CategoryID", typeof(int));
            products.Columns.Add("QuantityPerUnit", typeof(string));
            products.Columns.Add("UnitPrice", typeof(decimal));
            products.Columns.Add("UnitsInStock", typeof(short));
            products.Columns.Add("UnitsOnOrder", typeof(short));
            products.Columns.Add("ReorderLevel", typeof(short));
            products.Columns.Add("Discontinued", typeof(bool));
            products.PrimaryKey = new DataColumn[]
            {
                productId,
            };

            // Employees TO DO : make implementation of this unneccessary people.
            DataTable employees = this.Set.Tables.Add("Employees");

            this.Set.Relations.Add("ProductsCategories", this.Set.Tables["Categories"].Columns["ID"], this.Set.Tables["Products"].Columns["CategoryID"]);

            XmlReader reader = XmlReader.Create("InMemory.xml");
            _ = this.Set.ReadXml(reader, XmlReadMode.ReadSchema);
            reader.Dispose();
        }

        /// <summary>
        /// Gets or sets dataSet of nothwind.
        /// </summary>
        public DataSet Set { get; set; }

        /// <summary>
        /// Return row that is in Products table.
        /// </summary>
        /// <returns>Rows in Products.</returns>
        public IEnumerable<string> GetProducts()
        {
            List<string> rows = new List<string>();
            foreach (DataRow row in this.Set.Tables["Products"].Rows)
            {
                rows.Add($"ID:{row["ID"]},Name:{row["Name"]}, Description:{row["Description"]}");
            }

            return rows;
        }

        /// <summary>
        /// Return rows that is in Categories table.
        /// </summary>
        /// <returns>Rows from Categories.</returns>
        public IEnumerable<string> GetCategories()
        {
            List<string> rows = new List<string>();
            foreach (DataRow row in this.Set.Tables["Categories"].Rows)
            {
                rows.Add($"ID:{row["ID"]},Name:{row["Name"]}, Description:{row["Description"]}");
            }

            return rows;
        }

        /// <summary>
        /// Creates new row in Categories table by passed object of category.
        /// </summary>
        /// <param name="category">Object, to make new row.</param>
        /// <returns>Id of given ProductCategory.</returns>
        public int CreateNewCategory(ProductCategory category)
        {
            DataRow newCategory = this.Set.Tables["Categories"].NewRow();
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            newCategory["ID"] = category.Id;
            newCategory["Name"] = category.Name;
            newCategory["Description"] = category.Description;

            this.Set.Tables["Categories"].Rows.Add(newCategory);
            this.Set.Tables["Categories"].AcceptChanges();

            this.Set.AcceptChanges();
            this.Set.WriteXml("InMemory.xml");

            return category.Id;
        }
    }
}
