using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using AlventaDB.Models;

namespace AlventaMVC.Infrastructure
{
    public class WorkbookExcel
    {
        string path;
        Application oXL;
        _Workbook oWB;
        _Worksheet oSheet;
        IEnumerable<Order_Detail> data;
        object misvalue = Missing.Value;

        public WorkbookExcel(IEnumerable<Order_Detail> data, string path)
        {
            this.path = path;
            this.data = data;
            oXL = new Application();
            oXL.Visible = false;

            oWB = oXL.Workbooks.Add("");
            oSheet = (_Worksheet)oWB.ActiveSheet;

            oSheet.Cells[1, 1] = "Номер заказа";
            oSheet.Cells[1, 2] = "Дата заказа";
            oSheet.Cells[1, 3] = "Артикул товара";
            oSheet.Cells[1, 4] = "Название товара";
            oSheet.Cells[1, 5] = "Количество реализованных единиц товара";
            oSheet.Cells[1, 6] = "Цена реализации за единицу продукции";
            oSheet.Cells[1, 7] = "Скидка,%";
            oSheet.Cells[1, 8] = "Итоговая цена";

            oSheet.get_Range("A1", "H1").Font.Bold = true;
            oSheet.get_Range("A1", "H1").VerticalAlignment = XlVAlign.xlVAlignCenter;
            oSheet.get_Range("A1", "H1").HorizontalAlignment = XlVAlign.xlVAlignCenter;

            int row = 2;

            foreach (var o in order_Detail)
            {
                oSheet.Cells[row, 1] = o.OrderID;
                oSheet.Cells[row, 2] = o.Order.OrderDate;
                oSheet.Cells[row, 3] = o.ProductID;
                oSheet.Cells[row, 4] = o.Product.ProductName;
                oSheet.Cells[row, 5] = o.Quantity;
                oSheet.Cells[row, 6] = o.UnitPrice;
                oSheet.Cells[row, 7] = o.Discount;

                oSheet.Cells[row, 8].Formula = o.Discount == 0 ? $"=E{row}*F{row}" : $"=(E{row}*F{row})-(E{row}*F{row}*G{row})";

                row++;
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            oXL.UserControl = false;
            oWB.SaveAs(path, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            oWB.Close();
        }
    }
}