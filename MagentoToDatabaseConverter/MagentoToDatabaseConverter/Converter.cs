using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagentoToDatabaseConverter
{
    public partial class Converter : Form
    {
        public Converter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            char delimiter = textBox.Text[0];
            try
            {
                string[] lines = System.IO.File.ReadAllLines(labelSourcePath.Text);
                
                int numLines = lines.Length;
                string firstLine = lines[0];
                string[] columns = firstLine.Split(delimiter);
                int skuId, locationId, codeFournisseurId, nameId, sizesId, categoriesId, statusId, silverDefaultId, priceCategoryId;
                skuId = locationId = codeFournisseurId = nameId = sizesId = categoriesId =statusId = silverDefaultId = priceCategoryId=0;

                labelStatus.Text = "Getting column indexes...";
                for (int i = 0; i < columns.Length; i++)
                {
                    switch(columns[i])
                    {
                        case "sku":
                            skuId = i;
                            break;
                        case "casier":
                            locationId = i;
                            break;
                        case "code_fournisseur":
                            codeFournisseurId = i;
                            break;
                        case "name":
                            nameId = i;
                            break;
                        case "sizes":
                            sizesId = i;
                            break;
                        case "_category":
                            categoriesId = i;
                            break;
                        case "status":
                            statusId = i;
                            break;
                        case "is_silver_default":
                            silverDefaultId = i;
                            break;
                        case "price_category":
                            priceCategoryId = i;
                            break;
                        default:
                            break;

                    }
                }
                List<POD> pods = new List<POD>();
                List<Poster> posters = new List<Poster>();
                for (int i = 1; i < numLines;)
                {
                    string[] values = lines[i].Split(delimiter);
                    string sku = Utilities.getOriginalSku(values[skuId]);
                    string name = values[nameId];
                    string categories = "";
                    string firstSize = values[sizesId];
                    bool enabled = values[statusId].Contains("1");
                    bool isSilverDefault;
                    if (values[silverDefaultId].Contains("Yes"))
                    {
                        isSilverDefault = true;
                    }
                    else if (values[silverDefaultId].Contains("No"))
                    {
                        isSilverDefault = false;
                    }
                    else
                    {
                        throw new Exception("Silver Default does not have a value");
                    }
                    List<string> categoryList = new List<string>();

                    labelStatus.Text = "Reading input file...";
                    while (true)
                    {
                        i++;
                        if (i == numLines) break;
                        string[] nextValues = lines[i].Split(delimiter);
                        if (nextValues[0].Length > 1) break;
                        if (nextValues[categoriesId].Length > 1)
                        {
                            categoryList.Add(nextValues[categoriesId]);
                        }
                    }
                    foreach(string cat in categoryList)
                    {
                        categories += cat + ",";
                    }
                    if (values[skuId].Contains("C_POD") )
                    {
                        string ratio = Utilities.getRatio(firstSize);
                        pods.Add(new POD(sku, ratio, name, categories, enabled, isSilverDefault));

                    }
                    else if (values[skuId].Contains("POSTER"))
                    {
                        string location = values[locationId];
                        string priceCategory = values[priceCategoryId];
                        string codeFournisseur = values[codeFournisseurId];
                        posters.Add(new Poster(sku, firstSize, location, codeFournisseur, name, categories, priceCategory, enabled, isSilverDefault));
                    }
                }

                labelStatus.Text = "Filling Posters sheet...";
                XLS.fillPosters(posters, labelDestinationPath.Text);
                labelStatus.Text = "Filling POD sheet...";
                XLS.fillPOD(pods, labelDestinationPath.Text);
                MessageBox.Show("Success");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            labelStatus.Text = "";
            Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialogSource.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            labelSourcePath.Text = openFileDialogSource.FileName;
        }

            
        private void openFileDialogDestination_FileOk(object sender, CancelEventArgs e)
        {
            labelDestinationPath.Text = openFileDialogDestination.FileName;
        }

        private void buttonSelectDestination_Click(object sender, EventArgs e)
        {
            openFileDialogDestination.ShowDialog();
        }
    }
}
