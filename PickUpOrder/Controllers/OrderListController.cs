// OrderListController - A controller that prepares all OrderList pages.

using PickUpOrder.Models;
using System.Linq;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
    public class OrderListController : Controller
    {
        // OrderList (GET) - Render the list without making any changes.
        [HttpGet]
        public ActionResult OrderList()
        {
            // If the user is not logged in or has inappropriate permissions,
            // redirect to the appropriate page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");
            if(Account.GetCookieType(Request.Cookies["UserID"].Value)
               < AccountType.Employee)
                return RedirectToAction("Menu", "Menu");

            return View();
        }

        // OrderList (POST) - Apply updatedOrder's status to the order
        //                    in the database with the same ID.
        [HttpPost]
        public ActionResult OrderList(Order updatedOrder)
        {
            // If the user is not logged in or has inappropriate permissions,
            // redirect to the appropriate page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");
            if (Account.GetCookieType(Request.Cookies["UserID"].Value)
               < AccountType.Employee)
                return RedirectToAction("Menu", "Menu");

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
            // If the user is not logged in or has inappropriate permissions,
            // redirect to the appropriate page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");
            if (Account.GetCookieType(Request.Cookies["UserID"].Value)
               < AccountType.Employee)
                return RedirectToAction("Menu", "Menu");

            var db = new PickUpOrderDBEntities2();
            return View(db.Orders.Find(toChange));
        }

        // Help (GET) - Render the help page.
        [HttpGet]
        public ActionResult Help()
        {
            // If the user is not logged in or has inappropriate permissions,
            // redirect to the appropriate page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");
            if (Account.GetCookieType(Request.Cookies["UserID"].Value)
                < AccountType.Employee)
                return RedirectToAction("Menu", "Menu");

            return View();
        }
    }
}