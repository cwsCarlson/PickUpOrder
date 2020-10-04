using PickUpOrder.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyMvcApplication.Controllers
{
	public class IndexController : Controller
	{
		public ActionResult Index()
		{
			List<MenuItem> testMenu = new List<MenuItem>();

			MenuItem testItem1 = new MenuItem
			{
				Name = "Main Course 1",
				Description = "A nice course.",
				Price = 500
			};

			MenuItem testItem2 = new MenuItem
			{
				Name = "Main Course 2",
				Description = "The deluxe package.",
				Price = 1000
			};

			MenuItem testItem3 = new MenuItem
			{
				Name = "Fountain Drink",
				Description = "Just soda.",
				Price = 100
			};

			// Add the items to the list
			testMenu.Add(testItem1);
			testMenu.Add(testItem2);
			testMenu.Add(testItem3);

			// Send the model to view.
			return View(testMenu);
		}
	}
}