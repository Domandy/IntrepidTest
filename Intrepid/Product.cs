using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrepid
{
    class Product
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Product()
        {

        }
        //Import File to List of Products
        public List<Product> ReadTDFile(string filePath)
        {
            return
                File.ReadAllLines(filePath)
                    .Skip(1)
                    .Where(line => line.Length > 1)
                    .Select(Product.ProductFromFile)
                    .ToList();
        }
        //Product objects from file
        internal static Product ProductFromFile(string line)
        {
            var columns = line.Split('\t');

            return new Product
            {
                ID = int.Parse(columns[0]),
                Status = columns[1],
                Category = columns[2],
                Description = columns[3],
                Price = decimal.Parse(columns[4])
            };
        }
        //Display on DataTable 
        public static DataTable ListToDataTable(List<Product> products)
        {
            var dataTable = new DataTable();
            // add columns
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Status");
            dataTable.Columns.Add("Category");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Price");
            //add rows
            foreach (var p in products)
            {
                dataTable.Rows.Add(p.ID, p.Status, p.Category, p.Description, p.Price);
            }
            return dataTable;
        }

        //Remove discontinued so not displayed on dataTable
        public List<Product> RemoveDiscontinued(List<Product> products)
        {
            products.RemoveAll(x => x.Status == "Discontinued");

            return products;
        }

        //Discounts
        public List<Product> Discounts(List<Product> products)
        {
            //10% discount for clothing that is not status of Wholesale or Discounted. 
            products.Where(x => (x.Category == "Clothing" && x.Status != "Wholesale") || x.Category == "Clothing" && x.Status != "Discounted")
                .ToList()
                .ForEach(p => p.Price = p.Price - (p.Price * 10 / 100));

            //20% discount for clothing where discounted
            products.Where(x => x.Category == "Clothing" && x.Status == "Discounted")
                    .ToList()
                    .ForEach(p => p.Price = p.Price - (p.Price * 20 / 100));

            //15% discount for all products labeled discounted but not clothing.
            products.Where(x => x.Status == "Discounted" && x.Category != "Clothing")
                    .ToList()
                    .ForEach(p => p.Price = p.Price - (p.Price * 15 / 100));

            products.ToList()
                    .ForEach(p => p.Price = Decimal.Round(p.Price, 2));

            return products;
        }
        
        //Export ID, DESC and PRICE to Results.txt file. currently writes to debug folder.
        public static void ExportFile(List<Product> products)
        {
            var sb = new StringBuilder();
            // var productString = products.ConvertAll(x => Convert.ToString(x));
            // string file = string.Join(", ", productString);
            foreach (var p in products)
            {
                sb.AppendLine(p.ID + "," + p.Description + "," + p.Price + ",");
            }
            var file = sb.ToString();
           File.WriteAllText("Results.txt", file);
           MessageBox.Show("Export Complete.", "Program Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
