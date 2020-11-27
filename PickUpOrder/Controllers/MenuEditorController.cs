// MenuEditorController - A controller that prepares all Menu pages.

using PickUpOrder.Models;
using System.Linq;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
	public class MenuEditorController : Controller
	{
		// Menu - Render the list without making any changes.
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

		// Help - Render the help page.
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

		[HttpPost]
		// Search - If a search query was made, generate a page
		//          with only the applicable items.
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
			var query = Request.Form["query"];
			var category = Request.Form["category"];

			// An empty category string means there is no category filter.
			if (category == "")
			{
				var filtered =
					db.MenuItems.Where(p => p.Name.Contains(query) ||
											p.Description.Contains(query));

				// Display the Menu view with the filter applied.
				return View("MenuEditor", filtered);
			}
			else
			{
				var catVal = int.Parse(category);
				var filtered =
					db.MenuItems.Where(p => p.Category == catVal &&
										   (p.Name.Contains(query) ||
											p.Description.Contains(query)));

				// Display the Menu view with the filter applied.
				return View("MenuEditor", filtered);
			}
		}

		[HttpGet]
		// AddCategory - Render the AddCategory page.
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

		[HttpPost]
		// AddCategory (POST) - Add the posted category to the menu.
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

		[HttpGet]
		// AddItem - Render the AddItem page.
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

		[HttpPost]
		// AddItem (POST) - Add the posted item to the menu.
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

		[HttpGet]
		// DeleteCategory (GET) - Render the DeleteCategory page.
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

		[HttpPost]
		// DeleteCategory (POST) - Remove oldCat from the menu
		//                         and move its members to newCat.
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
			var newCat = db.Categories.Find(int.Parse(Request.Form["newCat"]));

			// Change all members of oldCat to newCat.
			var toModify =
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

		[HttpGet]
		// DeleteItem (GET) - Render the DeleteItem page.
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

		[HttpGet]
		// NullifyItem (GET) - Nullify toDelete.
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

		[HttpGet]
		// EditCategory - Render the EditCategory page.
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

		[HttpPost]
		// AddCategory (POST) - Add the posted category to the menu.
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

		[HttpGet]
		// EditItem - Render the EditItem page.
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

		[HttpPost]
		// EditItem - Apply the value of modified's fields
		//            to the item with the same ID number.
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
			if(modified.Price != db.MenuItems.Find(modified.ItemID).Price)
            {
				var ordersToMod =
					db.Orders.Where(m => m.OrderStatus == 
					    (int) OrderStatus.NotSubmitted &&
						(m.OrderContents.Contains(modified.ItemID + ",") ||
						m.OrderContents.EndsWith(modified.ItemID.ToString())));
				
				// In each order, see how often the item occurs and modify.
				foreach(Order o in ordersToMod)
                {
					var toChange =
						o.OrderContents.Count(s => s.Equals(modified.ItemID + ","));
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