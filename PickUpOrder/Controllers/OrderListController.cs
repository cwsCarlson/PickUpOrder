// OrderListController - A controller that prepares all OrderList pages.

using PickUpOrder.Models;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
    public class OrderListController : Controller
    {
        // OrderList - Render the list without making any changes.
        public ActionResult OrderList()
        {
            return View();
        }

        // OrderList (POST) - Apply updatedOrder's status to the order
        //                    in the database with the same ID.
        [HttpPost]
        public ActionResult OrderList(Order updatedOrder)
        {
            var db = new PickUpOrderDBEntities2();
            db.Orders.Find(updatedOrder.OrderID).OrderStatus
                = updatedOrder.OrderStatus;
            db.SaveChanges();
            return View();
        }

        // ChangeOrderStatus (GET) - Render the ChangeOrderStatus page
        //                           with order number toChange loaded.
        [HttpGet]
        public ActionResult ChangeOrderStatus(int toChange)
        {
            var db = new PickUpOrderDBEntities2();
            return View(db.Orders.Find(toChange));
        }
    }
}