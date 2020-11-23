// MenuController - A controller that prepares all Menu pages.

using PickUpOrder.Models;
using System.Linq;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
	public class MenuController : Controller
	{
		// Menu - Render the list without making any changes.
		public ActionResult Menu()
		{
			// If the user is not logged in, redirect to the login page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Display the view.
			return View(db.MenuItems);
		}

		// Menu (POST) - Modify the user's current order.
		//               If adding is true, add qty instances of toModify.
		//               If adding is false, remove qty instances of toModify.
		[HttpPost]
		public ActionResult Menu(bool adding, int IDtoModify)
        {
			// If the user is not logged in, redirect to the login page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");

			// Retrieve the item being added.
			var db = new PickUpOrderDBEntities2();
			MenuItem toModify = db.MenuItems.Find(IDtoModify);

			// Retrieve the quantity.
			int qty = int.Parse(Request.Form["qty"]);

			// Retrieve the order this is being added to.
			var user =
				db.Accounts.Find(Account.GetCookieID(Request.Cookies["UserID"].Value));
			var targetOrder = db.Orders.Find(user.MostRecentOrder());

			// Process the appropriate changes.
			if (adding)		
				targetOrder.AddMultipleItems(toModify, qty);
			else
				targetOrder.RemoveMultipleItems(toModify, qty);
			db.Orders.Find(targetOrder.OrderID).OrderContents = targetOrder.OrderContents;
			db.SaveChanges();

			// Pass the untruncated menu.
			return View(db.MenuItems);
		}

		// Search - If a search query was made, generate a page
		//          with only the applicable items.
		[HttpPost]
		public ActionResult Search()
		{
			// If the user is not logged in, redirect to the login page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Get the query information.
			var query = Request.Form["query"];
			var category = Request.Form["category"];

			// An empty category string means there is no category filter.
			if (category == "")
			{
				var filtered =
					db.MenuItems.Where(p => p.Name.Contains(query) ||
											p.Description.Contains(query));

				// Display the Menu view with the filter applied.
				return View("Menu", filtered);
			}
			else
			{
				var catVal = int.Parse(category);
				var filtered =
					db.MenuItems.Where(p => p.Category == catVal &&
										   (p.Name.Contains(query) ||
											p.Description.Contains(query)));

				// Display the Menu view with the filter applied.
				return View("Menu", filtered);
			}
		}

		// AddToOrder (GET) - Render the AddToOrder page
		//                    with menu item toAdd loaded.
		[HttpGet]
		public ActionResult AddToOrder(int IDtoAdd)
		{
			// If the user is not logged in, redirect to the login page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");

			// Get the item being added.
			var db = new PickUpOrderDBEntities2();
			var toAdd = db.MenuItems.Find(IDtoAdd);

			// This should never be called; it's just for safety.
			// If the item has been deleted, redirect to the menu page.
			if (toAdd.Price == null)
				return Menu();

			ViewBag.toAdd = toAdd;
			return View();
        }

		// RemoveFromOrder (GET) - Render the RemoveFromOrder page
		//                         with menu item toRemove loaded.
		[HttpGet]
		public ActionResult RemoveFromOrder(int IDtoRemove)
		{
			// If the user is not logged in, redirect to the login page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");

			// Get the item being removed.
			var db = new PickUpOrderDBEntities2();
			var toRemove = db.MenuItems.Find(IDtoRemove);

			// This should never be called; it's just for safety.
			// If the item has been deleted, redirect to the menu page.
			if (toRemove.Price == null)
				return Menu();

			// Get the number of times toRemove appears in the order.
			// This is the maximum value for the page's textbox.
			var itemStr = db.Orders.Find(1).OrderContents.Split(',');
			ViewBag.instances = itemStr.Count(f =>
			                                  f == toRemove.ItemID.ToString());

			// This should never be called; it's just for safety.
			// If the item isn't in the order, redirect to the main page.
			if (ViewBag.instances == 0)
				return Menu();

			ViewBag.toRemove = toRemove;
			return View();
		}

		// Submit (GET) - Submit the user's current order.
		[HttpGet]
		public ActionResult Submit()
        {
			// If the user is not logged in, redirect to the login page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");

			// Open the database connection.
			var db = new PickUpOrderDBEntities2();

			// Get the user.
			var curUser =
				db.Accounts.Find(Account.GetCookieID(
					             Request.Cookies["UserID"].Value));

			// Set their most recent order to "Received".
			var toSubmit = curUser.MostRecentOrder();
			db.Orders.Find(toSubmit).OrderStatus = (int) OrderStatus.Received;
			db.SaveChanges();

			// Return the view.
			return View("Menu", db.MenuItems);
        }
	}
}