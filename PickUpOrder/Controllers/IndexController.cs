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
			// Create a test order. This will be removed later.
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

			// Establish a connection with the SQL server and save the number of rows.
			Console.WriteLine("Establishing connection.");
			SqlConnection conn = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=PickUpOrderDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
			Console.WriteLine("Opening connection.");
			conn.Open();
			Console.WriteLine("Building command.");
			SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM orders;", conn);
			Console.WriteLine("Placing command.");
			ViewBag.NumOrders = Convert.ToInt32(cmd.ExecuteScalar());

			// Send the model to view.
			ViewBag.Menu = testMenu;
			ViewBag.OrderItems = new List<MenuItem>();
			return View();
		}

		[HttpGet]
		public ActionResult AddToOrder(int id)
		{
			// Since the menu DB isn't implemented yet,
			// recreate the displayed menu.
			Order TestOrder = new Order();
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
			testMenu.Add(testItem1);
			testMenu.Add(testItem2);
			testMenu.Add(testItem3);
			System.Diagnostics.Debug.WriteLine("First cost: " + TestOrder.CalcFinalCost());

			// Add the item's ID to the content string.
			if (TestOrder.NumContents == null || TestOrder.NumContents == 0)
			{
				TestOrder.OrderContents += id;
				TestOrder.NumContents = 0;
			}
			else
				TestOrder.OrderContents += "," + id;

			// Modify the other fields.
			TestOrder.NumContents++;
			TestOrder.RawCost += testMenu[id].Price;
			System.Diagnostics.Debug.WriteLine("New cost: " + TestOrder.CalcFinalCost());

			// Set the ViewBag so they'll be visible.
			List<MenuItem> items = new List<MenuItem>();
            String[] itemIDs = TestOrder.OrderContents.Split(',');
			System.Diagnostics.Debug.WriteLine("When adding :" + TestOrder.OrderContents);
			foreach (String itmId in itemIDs)
				items.Add(testMenu[Int32.Parse(itmId)]);

			// Return.
			ViewBag.Menu = testMenu;
			ViewBag.OrderItems = items;
			return View();
        }
	}
}