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

namespace AlventaMVC.Controllers
{
    public class Order_DetailController : Controller
    {
        private readonly Order_DetailRepo repo = new Order_DetailRepo();

        public async Task<ActionResult> Index()
        {
            var order_Detail = from od in await repo.GetAllAsync()
                               where od.Quantity > 100
                               select od;
            return View(order_Detail);
            //return View(await repo.GetAllAsync());
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

            return View(order_Detail);
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
