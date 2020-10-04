using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PickUpOrder.Models
{
    public class MenuItem
    {
        // Name - the item name.
        public string Name { get; set; }

        // Price - the item's price in cents.
        public int Price { get; set; }
        
        // Description - A quick note about the item.
        public string Description { get; set; }
    }
}