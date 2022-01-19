using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrepid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Product> listToExport;
        private void btn_Import_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            var fileContent = string.Empty;


            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {   

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            Product product = new Product();
            List<Product> productlist = product.ReadTDFile(filePath);
            List<Product> processedList = product.RemoveDiscontinued(productlist);
            List<Product> discountedList = product.Discounts(processedList);
            DataTable dataTable = Product.ListToDataTable(discountedList);
            dataGridView1.DataSource = dataTable;

            listToExport = discountedList;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            Product.ExportFile(listToExport);
        }
    }
}
