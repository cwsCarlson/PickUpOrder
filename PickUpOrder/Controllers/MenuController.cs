﻿using PickUpOrder.Models;
using System;
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
			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Display the view.
			return View(db.MenuItems);
		}

		[HttpPost]
		// Menu (POST) - If a search query was made, generate a page
		//               with only the applicable items.
		// FIXME: Header will change after the category DropDownFor
		//        is added to the View form.
		public ActionResult Search()
        {
			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Get the query information.
			var query = Request.Form["query"];
			var category = Request.Form["category"];
			System.Diagnostics.Debug.WriteLine("Q: " + query);
			System.Diagnostics.Debug.WriteLine("C:" + Request.Form["category"]);

			// An empty category string means there is no category filter.
			if(category == "")
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

			// Pass the untruncated menu.
			return View(db.MenuItems);
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