// MenuEditorController - A controller that prepares all Menu pages.

using PickUpOrder.Models;
using System.Linq;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
	public class MenuEditorController : Controller
	{
		// Menu - Render the list without making any changes.
		// FIXME: Pass user value into this?
		public ActionResult MenuEditor()
		{
			// Open a database connection.
			var db = new PickUpOrderDBEntities2();

			// Display the view.
			return View(db.MenuItems);
		}

		[HttpPost]
		// Search - If a search query was made, generate a page
		//          with only the applicable items.
		public ActionResult Search()
		{
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

		// AddCategory - Render the AddCategory page.
		public ActionResult AddCategory()
        {
			return View();
        }

		// AddItem - Render the AddItem page.
		public ActionResult AddItem()
		{
			var newItem = new MenuItem();
			return View(newItem);
		}

		[HttpPost]
		// AddItem (POST) - Add the posted item to the menu.
		public ActionResult AddItem(MenuItem toAdd)
        {
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

		// DeleteCategory - Render the DeleteCategory page.
		public ActionResult DeleteCategory()
		{
			return View();
		}

		// DeleteItem - Render the DeleteItem page.
		public ActionResult DeleteItem()
		{
			return View();
		}

		// EditCategory - Render the EditCategory page.
		public ActionResult EditCategory()
		{
			return View();
		}

		// EditItem - Render the EditItem page.
		public ActionResult EditItem()
		{
			return View();
		}
	}
}