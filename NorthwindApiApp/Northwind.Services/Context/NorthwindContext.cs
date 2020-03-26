namespace Northwind.Services.Context
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.EntityFrameworkCore;

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

            for (int item = 0; item < 10; item++)
            {
                DataRow newRowProducts = products.NewRow();
                int categoryIdValue = (item < 3) ? 1 : (item < 7) ? 2 : 3;
                newRowProducts["ID"] = item;
                newRowProducts["Name"] = $"item number {item}";
                newRowProducts["SupplierID"] = item + 1;
                newRowProducts["CategoryID"] = categoryIdValue;
                newRowProducts["QuantityPerUnit"] = $"{item+100} in each unit";
                newRowProducts["UnitPrice"] = (item * 10) + 12 - 0.5;
                newRowProducts["UnitsInStock"] = (short?) Math.Sqrt(item);
                newRowProducts["UnitsOnOrder"] = (short?)Math.Sqrt(((item - 1) < 0) ? 1 : item - 1);
                newRowProducts["ReorderLevel"] = 1;
                newRowProducts["Discontinued"] = (item > 5) ? true : false;
            }

            DataRow first = categories.NewRow();
            first["ID"] = 1;
            first["Name"] = "Shit for parents";
            first["Description"] = "Any thing, that will make angry my parents";
            categories.Rows.Add(first);

            DataRow second = categories.NewRow();
            second["ID"] = 2;
            second["Name"] = "My dreams";
            second["Description"] = "Useless things";
            categories.Rows.Add(second);

            DataRow third = categories.NewRow();
            third["ID"] = 3;
            third["Name"] = "Starbuckes coffee";
            third["Description"] = "It isn't a coffee, just bait";
            categories.Rows.Add(third);

            categories.AcceptChanges();
            products.AcceptChanges();
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
    }
}
