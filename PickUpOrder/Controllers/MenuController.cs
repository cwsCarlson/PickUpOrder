using PickUpOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
	public class MenuController : Controller
	{
		// Menu - Render the list without making any changes.
		// FIXME: Pass user value into this?
		public ActionResult Menu()
		{
			// Display the view.
			return View();
		}

		[HttpPost]
		// Menu (POST) - Modify the user's current order.
		//               If adding is true, add qty instances of toModify.
		//               If adding is false, remove qty instances of toModify.
		public ActionResult Menu(bool adding, int IDtoModify)
        {
			// Retrieve the item being added.
			var db = new PickUpOrderDBEntities2();
			MenuItem toModify = db.MenuItems.Find(IDtoModify);

			// Retrieve the quantity.
			int qty = int.Parse(Request.Form["qty"]);
			System.Diagnostics.Debug.WriteLine(toModify.Name + " x " + qty);

			// Process the appropriate changes.
			if (adding)		
				db.Orders.Find(1).AddMultipleItems(toModify, qty);
			else
				db.Orders.Find(1).RemoveMultipleItems(toModify, qty);
			db.SaveChanges();

			return View();
		}

		// AddToOrder (GET) - Render the AddToOrder page
		//                    with menu item toAdd loaded.
		[HttpGet]
		public ActionResult AddToOrder(int IDtoAdd)
		{
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
			//System.Diagnostics.Debug.WriteLine((int) ViewBag.instances);

			// This should never be called; it's just for safety.
			// If the item isn't in the order, redirect to the main page.
			if (ViewBag.instances == 0)
				return Menu();

			ViewBag.toRemove = toRemove;
			return View();
		}
	}
}