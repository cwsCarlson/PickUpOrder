// CategoryModel - The partial class of Category, which contains all methods.
//                 The object itself is defined in DatabaseModel.

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PickUpOrder.Models
{
    public partial class Category
    {
        // GetCategoryDropdown - Build a dropdown list of all categories.
        public static List<SelectListItem> GetCategoryDropdown()
        {
            var db = new PickUpOrderDBEntities2();
            var cats = new List<SelectListItem>();

            // If there are no categories, add the four defaults.
            if(db.Categories.Count() == 0)
            {
                db.Categories.Add(new Category
                    { CategoryName = "Main Dishes" });
                db.Categories.Add(new Category
                    { CategoryName = "Appetizers" });
                db.Categories.Add(new Category
                    { CategoryName = "Desserts" });
                db.Categories.Add(new Category
                    { CategoryName = "Drinks" });
                db.SaveChanges();
            }

            foreach (Category c in db.Categories)
            {
                cats.Add(new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryID.ToString()
                });
            }
            return cats;
        }
    }
}