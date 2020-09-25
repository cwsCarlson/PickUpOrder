using MyMvcApplication.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyMvcApplication.Controllers
{
	public class IndexController : Controller
	{
		public ActionResult Index()
		{
			List<IndexModel> newList = new List<IndexModel>();

			IndexModel newIndexModel = new IndexModel
			{
				ID = 1,
				Description = "First Item",
				Comments = "This is the first instance of the Model"
			};

			IndexModel newIndexModel2 = new IndexModel
			{
				ID = 2,
				Description = "Second Item",
				Comments = "This is the second instance of the Model"
			};

			// Add the items to the list
			newList.Add(newIndexModel);
			newList.Add(newIndexModel2);

			// Send the model to view.
			return View(newList);
		}
	}
}