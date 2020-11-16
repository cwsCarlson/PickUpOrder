using PickUpOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
    public class OrderListController : Controller
    {
        public ActionResult OrderList()
        {
            return View();
        }
    }
}