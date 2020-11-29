// MenuEditorController - A controller that prepares all Menu pages.

using PickUpOrder.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
	public class MenuEditorController : Controller
	{
		// MenuEditor (GET) - Render the list without making any changes.
		[HttpGet]
		public ActionResult MenuEditor()
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Display the view.
			return View(db.MenuItems);
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
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Display the view.
			return View();
		}

		// Search (POST) - If a search query was made, generate a page
		//                 with only the applicable items.
		[HttpPost]
		public ActionResult Search()
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Get the query information.
			string query = Request.Form["query"];
			string category = Request.Form["category"];

			// An empty category string means there is no category filter.
			if (category.Equals(""))
			{
				IQueryable<MenuItem> filtered =
					db.MenuItems.Where(p => p.Name.Contains(query) ||
											p.Description.Contains(query));

				// Display the Menu view with the filter applied.
				return View("MenuEditor", filtered);
			}
			else
			{
				int catVal = int.Parse(category);
				IQueryable<MenuItem> filtered =
					db.MenuItems.Where(p => p.Category == catVal &&
										   (p.Name.Contains(query) ||
											p.Description.Contains(query)));

				// Display the Menu view with the filter applied.
				return View("MenuEditor", filtered);
			}
		}

		// AddCategory (GET) - Render the AddCategory page.
		[HttpGet]
		public ActionResult AddCategory()
        {
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			var newCategory = new Category();
			return View(newCategory);
        }

		// AddCategory (POST) - Add the posted category to the menu.
		[HttpPost]
		public ActionResult AddCategory(Category toAdd)
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Add and save changes.
			// (The new category's ID is automatically generated here.)
			db.Categories.Add(toAdd);
			db.SaveChanges();

			// Redirect to the editor page.
			return View("MenuEditor", db.MenuItems);
		}

		// AddItem (GET) - Render the AddItem page.
		[HttpGet]
		public ActionResult AddItem()
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			var newItem = new MenuItem();
            return View(newItem);
        }

		// AddItem (POST) - Add the posted item to the menu.
		[HttpPost]
		public ActionResult AddItem(MenuItem toAdd)
        {
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Convert the price fields to the raw price.
			toAdd.Price = int.Parse(Request.Form["dollars"]) * 100 +
				          int.Parse(Request.Form["cents"]);

			// Add and save changes.
			// (The new item's ID is automatically generated here.)
			db.MenuItems.Add(toAdd);
			db.SaveChanges();

			// Redirect to the editor page.
			return View("MenuEditor", db.MenuItems);
        }

		// DeleteCategory (GET) - Render the DeleteCategory page.
		[HttpGet]
		public ActionResult DeleteCategory(int IDtoDelete)
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			var db = new PickUpOrderDBEntities2();
			ViewBag.ToDelete = db.Categories.Find(IDtoDelete);
			return View();
		}

		// DeleteCategory (POST) - Remove oldCat from the menu
		//                         and move its members to newCat.
		[HttpPost]
		public ActionResult DeleteCategory(Category oldCat)
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Get the value of newCat.
			Category newCat =
				db.Categories.Find(int.Parse(Request.Form["newCat"]));

			// Change all members of oldCat to newCat.
			List<MenuItem> toModify =
				db.MenuItems.Where(e => (int) e.Category ==
				                        oldCat.CategoryID).ToList();
			foreach (MenuItem i in toModify)
				i.Category = newCat.CategoryID;

			// Remove oldCat and save all changes.
			db.Categories.Remove(db.Categories.Single(e => e.CategoryID ==
			                                               oldCat.CategoryID));
			db.SaveChanges();

			// Redirect to the editor page.
			return View("MenuEditor", db.MenuItems);
		}

		// DeleteItem (GET) - Render the DeleteItem page.
		[HttpGet]
		public ActionResult DeleteItem(int IDtoDelete)
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			var db = new PickUpOrderDBEntities2();
			return View(db.MenuItems.Find(IDtoDelete));
		}

		// NullifyItem (GET) - Nullify toDelete.
		[HttpGet]
		public ActionResult NullifyItem(MenuItem toNullify)
		{
			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Apply and save changes.
			db.MenuItems.Find(toNullify.ItemID).NullifyItem();
			db.SaveChanges();

			// Redirect to the editor page.
			return View("MenuEditor", db.MenuItems);
		}

		// EditCategory (GET) - Render the EditCategory page.
		[HttpGet]
		public ActionResult EditCategory(int IDtoModify)
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			var db = new PickUpOrderDBEntities2();
			return View(db.Categories.Find(IDtoModify));
		}

		// AddCategory (POST) - Add the posted category to the menu.
		[HttpPost]
		public ActionResult EditCategory(Category modified)
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Apply and save changes.
			db.Categories.Find(modified.CategoryID).CategoryName
				= modified.CategoryName;
			db.SaveChanges();

			// Redirect to the editor page.
			return View("MenuEditor", db.MenuItems);
		}

		// EditItem (GET) - Render the EditItem page.
		[HttpGet]
		public ActionResult EditItem(int IDtoModify)
        {
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			var db = new PickUpOrderDBEntities2();
			return View(db.MenuItems.Find(IDtoModify));
        }

		// EditItem (POST) - Apply the value of modified's fields
		//                   to the item with the same ID number.
		[HttpPost]
		public ActionResult EditItem(MenuItem modified)
		{
			// If the user is not logged in or has inappropriate permissions,
			// redirect to the appropriate page.
			if (!Request.Cookies.AllKeys.Contains("UserID"))
				return RedirectToAction("Login", "Login");
			if (Account.GetCookieType(Request.Cookies["UserID"].Value)
			    < AccountType.Manager)
				return RedirectToAction("OrderList", "OrderList");

			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Convert the price fields to the raw price.
			modified.Price = int.Parse(Request.Form["dollars"]) * 100 +
						     int.Parse(Request.Form["cents"]);

			// If the price has changed, modify all orders that have the item
			// to reflect the new price.
			if (modified.Price != db.MenuItems.Find(modified.ItemID).Price)
            {
				IQueryable<Order> ordersToMod =
					db.Orders.Where(m => m.OrderStatus == 
					    (int) OrderStatus.NotSubmitted &&
						(m.OrderContents.Contains($"{modified.ItemID},") ||
						m.OrderContents.EndsWith(modified.ItemID.ToString())));
				
				// In each order, see how often the item occurs and modify.
				foreach(Order o in ordersToMod)
                {
					int toChange =
						o.OrderContents.Count(s =>
						                      s.Equals($"{modified.ItemID},"));
					if (o.OrderContents.EndsWith(modified.ItemID.ToString()))
						toChange++;
					o.RawCost += (int) (toChange * (modified.Price -
						          db.MenuItems.Find(modified.ItemID).Price));
				}
            }

			// Apply and save changes.
			db.MenuItems.Find(modified.ItemID).Name = modified.Name;
			db.MenuItems.Find(modified.ItemID).Description =
				modified.Description;
			db.MenuItems.Find(modified.ItemID).Price = modified.Price;
			db.MenuItems.Find(modified.ItemID).Category = modified.Category;
			db.SaveChanges();

			// Redirect to the editor page.
			return View("MenuEditor", db.MenuItems);
		}
	}
}