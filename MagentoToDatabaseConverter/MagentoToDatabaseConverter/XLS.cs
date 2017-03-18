using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoToDatabaseConverter
{
    class XLS
    {
        public static void fillPosters(List<Poster> posters, string destination)
        {
            int numColumns = 9;
            object[,] arr = new object[posters.Count + 1, numColumns];
            arr[0, 0] = "Sku";
            arr[0, 1] = "Size";
            arr[0, 2] = "Location";
            arr[0, 3] = "Price Category";
            arr[0, 4] = "Categories";
            arr[0, 5] = "Name";
            arr[0, 6] = "Enabled";
            arr[0, 7] = "Is Silver Default";
            arr[0, 8] = "Supplier Code";
            int j;
            for (int i = 0; i < posters.Count; i++)
            {
                j = i + 1;
                Poster p = posters[i];
                arr[j, 0] = p.sku;
                arr[j, 1] = p.size;
                arr[j, 2] = p.location;
                arr[j, 3] = p.priceCategory;
                arr[j, 4] = p.categories;
                arr[j, 5] = p.name;
                arr[j, 6] = p.enabled;
                arr[j, 7] = p.silverDefault;
                arr[j, 8] = p.supplierCode;
            }
            
            addRangeToSheet(1, arr, destination);
        }

        public static void fillPOD(List<POD> pods, string destination)
        {

            int numColumns = 6;
            object[,] arr = new object[pods.Count + 1, numColumns];
            arr[0, 0] = "Sku";
            arr[0, 1] = "Ratio";
            arr[0, 2] = "Categories";
            arr[0, 3] = "Name";
            arr[0, 4] = "Enabled";
            arr[0, 5] = "Silver Default";
            int j;
            for (int i = 0; i < pods.Count; i++)
            {
                j = i + 1;
                POD p = pods[i];
                arr[j, 0] = p.sku;
                arr[j, 1] = p.ratio;
                arr[j, 2] = p.categories;
                arr[j, 3] = p.name;
                arr[j, 4] = p.enabled;
                arr[j, 5] = p.silverDefault;
            }
            
            addRangeToSheet(2, arr, destination);
        }

        private static void addRangeToSheet(int sheet, object[,] arr, string destination)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(destination, 0, false);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(sheet);

            xlWorkSheet.Cells.ClearContents();
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 3];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[arr.GetLength(0), 3];
            Microsoft.Office.Interop.Excel.Range range = xlWorkSheet.get_Range(c1, c2);
            range.NumberFormat = "@";

            c1 = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 2];
            c2 = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[arr.GetLength(0), arr.GetLength(1)+1];
            range = xlWorkSheet.get_Range(c1, c2);
            range.Value = arr;

            xlWorkBook.Save();
            xlWorkBook.Close();
        }
    }
}
