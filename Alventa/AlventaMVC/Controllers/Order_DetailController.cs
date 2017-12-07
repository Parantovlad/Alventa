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
                order_Detail = await repo.GetAllAsync();
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

            return View(order_Detail.ToPagedList(pageNumber,pageSize));
        }

        [HttpPost]
        public ActionResult Send(string email)
        {
            MailAddress from = new MailAddress("parantovlad@mail.ru");
            MailAddress to = new MailAddress(email);

            MailMessage m = new MailMessage(from, to);
            m.Subject = "Test";
            m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
            m.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("parantovlad@mail.ru", "par110587");
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
