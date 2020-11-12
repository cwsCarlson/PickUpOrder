using PickUpOrder.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace MyMvcApplication.Controllers
{
	public class IndexController : Controller
	{
		public List<MenuItem> testMenu = new List<MenuItem>();
		public ActionResult Index()
		{
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

			// Add the items to the list.
			testMenu.Add(testItem1);
			testMenu.Add(testItem2);
			testMenu.Add(testItem3);
			System.Diagnostics.Debug.WriteLine("In the Index method " + testMenu.Count);

			// Send the model to view.
			ViewBag.Menu = testMenu;
			ViewBag.OrderItems = new List<MenuItem>();
			return View();
		}

		[HttpGet]
		public ActionResult AddToOrder(MenuItem toAdd)
		{
			System.Diagnostics.Debug.WriteLine("Trying to add something.");
			System.Diagnostics.Debug.WriteLine("It's called " + toAdd.Name);
			var db = new PickUpOrderDBEntities2();
			db.Orders.Find(1).AddSingleItem(toAdd);
			db.SaveChanges();
			return View();
        }
	}
}