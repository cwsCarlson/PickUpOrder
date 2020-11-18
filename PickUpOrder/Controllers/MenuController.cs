using PickUpOrder.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
	public class MenuController : Controller
	{
		public List<MenuItem> testMenu = new List<MenuItem>();

		// Create a test menu. This will be removed later.
		MenuItem testItem1 = new MenuItem
		{
			Name = "Main Course 1",
			Description = "A nice course.",
			Price = 500,
			ID = 1
		};
		MenuItem testItem2 = new MenuItem
		{
			Name = "Main Course 2",
			Description = "The deluxe package.",
			Price = 1000,
			ID = 2
		};
		MenuItem testItem3 = new MenuItem
		{
			Name = "Fountain Drink",
			Description = "Just soda.",
			Price = 100,
			ID = 3
		};

		// Menu - Render the list without making any changes.
		// FIXME: Pass user value into this?
		public ActionResult Menu()
		{
			// Add the items to the list.
			testMenu.Add(testItem1);
			testMenu.Add(testItem2);
			testMenu.Add(testItem3);

			// Send the model to view.
			ViewBag.Menu = testMenu;
			return View();
		}

		[HttpPost]
		// Menu (POST) - Modify the user's current order.
		//               If adding is true, add qty instances of toModify.
		//               If adding is false, remove qty instances of toModify.
		public ActionResult Menu(bool adding, int IDtoModify)
        {
			// Add the items to the list and set the ViewBag.
			testMenu.Add(testItem1);
			testMenu.Add(testItem2);
			testMenu.Add(testItem3);
			ViewBag.Menu = testMenu;

			// FIXME: When the DB is implemented, change this to use it.
			MenuItem toModify = testMenu[IDtoModify - 1];
			int qty = Int32.Parse(Request.Form["qty"]);
			System.Diagnostics.Debug.WriteLine(toModify.Name + " x " + qty);

			// If this is coming from the addition page, add the item.
			if (adding)
            {
				var db = new PickUpOrderDBEntities2();
				
				// Pick the correct method based on the quantity.
				if(qty == 1)
					db.Orders.Find(1).AddSingleItem(toModify);
				else
					db.Orders.Find(1).AddMultipleItems(toModify, qty);
				db.SaveChanges();
				return View();
            }
			else
            {
				return View();
            }
        }

		// AddToOrder (GET) - Render the AddToOrder page
		//                    with menu item toAdd loaded.
		[HttpGet]
		public ActionResult AddToOrder(MenuItem toAdd)
		{
			ViewBag.toAdd = toAdd;
			return View(0);
        }

		// RemoveFromOrder (GET) - Render the RemoveFromOrder page
		//                         with menu item toRemove .
		[HttpGet]
		public ActionResult RemoveFromOrder(MenuItem toRemove)
		{
			//var db = new PickUpOrderDBEntities2();
			//db.Orders.Find(1).RemoveSingleItem(toRemove);
			//db.SaveChanges();
			return View(toRemove);
		}
	}
}