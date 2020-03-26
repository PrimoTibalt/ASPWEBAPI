using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace NorthwindApiApp.Context
{
    public class NorthwindContext : DbContext
    {
        private DataSet set { get; set; }

        /// <summary>
        /// Initialize instance of NorthwindContext and creates DataSet for it.
        /// </summary>
        public NorthwindContext()
        {
            // Creating my own data set with Products, Categories and Employees.
            set = new DataSet("Northwind");

            // Categories.
            DataTable categories = set.Tables.Add("Categories");
            DataColumn categoryId = categories.Columns.Add("ID", typeof(Int32));
            categories.Columns.Add("Name", typeof(string));
            categories.Columns.Add("Description", typeof(string));
            categories.PrimaryKey = new DataColumn[]
            {
                categoryId
            };

            // Products.
            DataTable products = set.Tables.Add("Products");
            DataColumn productId = products.Columns.Add("ID", typeof(Int32));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("SupplierID", typeof(Int32));
            products.Columns.Add("CategoryID", typeof(Int32));
            products.Columns.Add("QuantityPerUnit", typeof(string));
            products.Columns.Add("UnitPrice", typeof(decimal?));
            products.Columns.Add("UnitsInStock", typeof(short?));
            products.Columns.Add("UnitsOnOrder", typeof(short?));
            products.Columns.Add("ReorderLevel", typeof(short?));
            products.Columns.Add("Discontinued", typeof(bool));
            products.PrimaryKey = new DataColumn[]
            {
                productId
            };

            // Employees TO DO : make implementation of this unneccessary people.
            DataTable employees = set.Tables.Add("Employees");

            set.Relations.Add("ProductsCategories", set.Tables["Products"].Columns["CategoryID"], set.Tables["Categorie"].Columns["ID"]);

            for (int item = 0; item < 10; item++)
            {
                DataRow newRowCategories = categories.NewRow();
                DataRow newRowProducts = products.NewRow();
                int categoryIdValue = (item < 3) ? 1 : (item < 7) ? 2 : 3;
                newRowProducts["ID"] = item;
                newRowProducts["Name"] = $"item number {item}";
                newRowProducts["SupplierID"] = item + 1;
                newRowProducts["CategoryID"] = categoryIdValue;
                newRowProducts["QuantityPerUnit"] = $"{item+100} in each unit";
                newRowProducts["UnitPrice"] = item * 10 + 12 - 0.5;
                newRowProducts["UnitsInStock"] = (short?) Math.Sqrt(item);
                newRowProducts["UnitsOnOrder"] = (short?)Math.Sqrt(((item - 1) < 0) ? 1 : item - 1);
                newRowProducts["ReorderLevel"] = 1;
                newRowProducts["Disscontinued"] = (item > 5) ? true : false;
            }

            DataRow first = categories.NewRow();
            first["ID"] = 1;
            first["Name"] = "Shit for parents";
            first["Discription"] = "Any thing, that will make angry my parents";

            DataRow second = categories.NewRow();
            second["ID"] = 2;
            second["Name"] = "My dreams";
            second["Description"] = "Useless things";

            DataRow third = categories.NewRow();
            third["ID"] = 3;
            third["Name"] = "Starbuckes coffee";
            third["Discription"] = "It isn't a coffee, just bait";
        }



    }
}
