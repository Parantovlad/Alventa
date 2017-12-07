using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlventaDB.EF;
using AlventaDB.Models;
using AlventaDB.Repos;
using PagedList.Mvc;
using PagedList;
using System.Net.Mail;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace AlventaMVC.Controllers
{
    public class Order_DetailController : Controller
    {
        private readonly Order_DetailRepo repo = new Order_DetailRepo();

        public async Task<ActionResult> Index(int? page, string from, string to)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            IEnumerable<Order_Detail> order_Detail;

            if (from == null || to == null)
            {
                order_Detail = (await repo.GetAllAsync()).Take(50);
            }
            else
            {
                order_Detail = from od in await repo.GetAllAsync()
                               where od.Order.OrderDate >= Convert.ToDateTime(@from) &&
                                     od.Order.OrderDate <= Convert.ToDateTime(to)
                               select od;
            }

            if (order_Detail == null)
            {
                return HttpNotFound();
            }

            Application oXL;
            _Workbook oWB;
            _Worksheet oSheet;
            string path = Server.MapPath("~/Files/test.xls");

            try
            {
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
            catch (Exception)
            {
                throw;
            }

            return View(order_Detail.ToPagedList(pageNumber,pageSize));
        }

        [HttpPost]
        public ActionResult Send(string email)
        {
            string path = Server.MapPath("~/Files/test.xls");
            MailAddress from = new MailAddress("parantovlad@mail.ru");
            MailAddress to = new MailAddress(email);

            MailMessage m = new MailMessage(from, to);
            m.Subject = "Test";
            m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
            m.IsBodyHtml = true;
            m.Attachments.Add(new Attachment(path));

            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("parantovlad@mail.ru", "******");
            smtp.EnableSsl = true;
            smtp.Send(m);
            
            return PartialView();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
