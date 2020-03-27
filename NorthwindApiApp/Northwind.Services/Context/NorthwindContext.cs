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
                rows.Add($"ID:{row["ID"]}," +
                    $"Name:{row["Name"]}," +
                    $"SupplierId:{row["SupplierId"]}," +
                    $"CategoryId:{row["CategoryId"]}," +
                    $"QuantityPerUnit:{row["QuantityPerUnit"]}," +
                    $"UnitPrice:{row["UnitPrice"].ToString().Replace(',', '.')}," +
                    $"UnitsInStock:{row["UnitsInStock"]}," +
                    $"UnitsOnOrder:{row["UnitsOnOrder"]}," +
                    $"ReorderLevel:{row["ReorderLevel"]}," +
                    $"Discontinued:{row["Discontinued"]}");
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
        public void CreateNewCategory(ProductCategory category)
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

            this.UpdateXml();
        }

        /// <summary>
        /// Updates one of the rows in Categories table.
        /// </summary>
        /// <param name="category">Parameters to update.</param>
        /// <returns>Is successful.</returns>
        public bool UpdateCategory(ProductCategory category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var rows = this.Set.Tables["Categories"].Rows;
            DataRow currentRow = null;
            foreach (DataRow row in rows)
            {
                if (int.Parse(row["ID"].ToString()) == category.Id)
                {
                    currentRow = row;
                }
            }

            if (currentRow is null)
            {
                return false;
            }

            currentRow["ID"] = category.Id;
            currentRow["Name"] = category.Name;
            currentRow["Description"] = category.Description;
            currentRow.AcceptChanges();
            this.UpdateXml();

            return true;
        }

        /// <summary>
        /// Deletes a row from the table of Categories with given id.
        /// </summary>
        /// <param name="id">id of a row to delete.</param>
        public void DeleteCategory(int id)
        {
            var rows = this.Set.Tables["Categories"].Rows;
            DataRow currentRow = null;
            foreach (DataRow row in rows)
            {
                if (int.Parse(row["ID"].ToString()) == id)
                {
                    currentRow = row;
                }
            }

            if (currentRow is null)
            {
                throw new KeyNotFoundException("No Category with such Id!");
            }

            this.Set.Tables["Categories"].Rows.Remove(currentRow);
            this.UpdateXml();
        }

        /// <summary>
        /// Changes properties of product with same id.
        /// </summary>
        /// <param name="product">Id of product to change and changes.</param>
        /// <returns>Is Successfull.</returns>
        public bool UpdateProduct(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var rows = this.Set.Tables["Products"].Rows;
            DataRow currentRow = null;
            foreach (DataRow row in rows)
            {
                if (int.Parse(row["ID"].ToString()) == product.Id)
                {
                    currentRow = row;
                }
            }

            if (currentRow is null)
            {
                return false;
            }

            currentRow["ID"] = product.Id;
            currentRow["Name"] = product.Name;
            currentRow["SupplierId"] = product.SupplierId;
            currentRow["CategoryId"] = product.CategoryId;
            currentRow["QuantityPerUnit"] = product.QuantityPerUnit;
            currentRow["UnitPrice"] = product.UnitPrice;
            currentRow["UnitsInStock"] = product.UnitsInStock;
            currentRow["UnitsOnOrder"] = product.UnitsOnOrder;
            currentRow["ReorderLevel"] = product.ReorderLevel;
            currentRow["Discontinued"] = product.Discontinued;
            currentRow.AcceptChanges();
            this.UpdateXml();

            return true;
        }

        /// <summary>
        /// Creates new row in Products table by passed object of product.
        /// </summary>
        /// <param name="product">Object, to make new row.</param>
        public void CreateNewProduct(Product product)
        {
            DataRow newProduct = this.Set.Tables["Products"].NewRow();
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            newProduct["ID"] = product.Id;
            newProduct["Name"] = product.Name;
            newProduct["SupplierId"] = product.SupplierId;
            newProduct["CategoryId"] = product.CategoryId;
            newProduct["QuantityPerUnit"] = product.QuantityPerUnit;
            newProduct["UnitPrice"] = product.UnitPrice;
            newProduct["UnitsInStock"] = product.UnitsInStock;
            newProduct["UnitsOnOrder"] = product.UnitsOnOrder;
            newProduct["ReorderLevel"] = product.ReorderLevel;
            newProduct["Discontinued"] = product.Discontinued;

            this.Set.Tables["Products"].Rows.Add(newProduct);
            this.Set.Tables["Products"].AcceptChanges();

            this.UpdateXml();
        }

        /// <summary>
        /// Deletes row from table Products with same id as given.
        /// </summary>
        /// <param name="id">Id of product to delete.</param>
        public void DeleteProduct(int id)
        {
            var rows = this.Set.Tables["Products"].Rows;
            DataRow currentRow = null;
            foreach (DataRow row in rows)
            {
                if (int.Parse(row["ID"].ToString()) == id)
                {
                    currentRow = row;
                }
            }

            if (currentRow is null)
            {
                throw new KeyNotFoundException("No Product with such Id!");
            }

            this.Set.Tables["Products"].Rows.Remove(currentRow);
            this.UpdateXml();
        }

        private void UpdateXml()
        {
            this.Set.Tables["Categories"].AcceptChanges();
            this.Set.Tables["Products"].AcceptChanges();

            this.Set.AcceptChanges();
            this.Set.WriteXml("InMemory.xml");
        }
    }
}
