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

namespace AlventaMVC.Controllers
{
    public class Order_DetailController : Controller
    {
        private readonly Order_DetailRepo repo = new Order_DetailRepo();

        public async Task<ActionResult> Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var order_Detail = await repo.GetAllAsync();

            return View(order_Detail.ToPagedList(pageNumber,pageSize));
        }

        public async Task<ActionResult> Details(string startDate, string finishDate)
        {
            if (startDate == null || finishDate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order_Detail = from od in await repo.GetAllAsync()
                               where od.Order.OrderDate >= Convert.ToDateTime(startDate) && 
                                     od.Order.OrderDate <= Convert.ToDateTime(finishDate)
                               select od;
            if (order_Detail == null)
            {
                return HttpNotFound();
            }

            return View("Index", order_Detail);
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
