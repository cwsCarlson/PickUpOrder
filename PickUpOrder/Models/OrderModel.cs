using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// OrderModel - The partial class of Order, which contains all methods.
//              The object itself is defined in DatabaseModel.

namespace PickUpOrder.Models
{
    // OrderStatus - An enum used to translate the OrderStatus integer
    //               to a readable string.
    public enum OrderStatus
    {
        NotSubmitted = 0,
        Received = 1,
        Preparing = 2,
        Done = 3,
        PickedUp = 4
    }

    public partial class Order
    {
        const double TAX_PERCENTAGE = 0.07;
        // Constructor - Make a blank Order.
        public Order()
        {

        }

        // CalcFinalCost - Calculates the final cost of an order
        //                 based on the raw cost already saved.
        public double CalcFinalCost()
        {
            // Divide by 100 since the raw cost is in cents.
            return ((double) RawCost) * (1 + TAX_PERCENTAGE) / 100;
        }

        // AddSingleItem - Add one quantity of toAdd to this order.
        public void AddSingleItem(MenuItem toAdd)
        {
            System.Diagnostics.Debug.WriteLine("Adding an item");
            System.Diagnostics.Debug.WriteLine("It's called " + toAdd.Name);
            // If this is the first item, make a new string.
            // Otherwise, add this item's ID to the end in a CSV format.
            if (OrderContents == null)
                OrderContents = toAdd.ID.ToString();
            else
                OrderContents += "," + toAdd.ID.ToString();
            System.Diagnostics.Debug.WriteLine(OrderContents);

            // Update the raw cost.
            RawCost += toAdd.Price;
        }

        // AddMultipleItems - Add toAdd to this order with the given quantity.
        public void AddMultipleItems(MenuItem toAdd, int qty)
        {

        }

        // RemoveSingleItem - Remove one quantity of toRemove from this order.
        public void RemoveSingleItem(MenuItem toRemove)
        {
            // If there are no items, then do nothing.
            if (OrderContents == null)
                return;

            // Create a list of all current item IDs.
            string[] itemIDs = OrderContents.Split(',');

            // Convert this item's ID into a string
            // and see whether it is in the ID list.
            string removeID = toRemove.ID.ToString();
            if(itemIDs.Contains(removeID))
            {
                // If this is the only item, set the list to null.
                if (itemIDs.Length == 1)
                    OrderContents = null;
                else
                {
                    // If this is the first ID, remove it and the trailing comma.
                    if (itemIDs[0] == removeID)
                        OrderContents = OrderContents.Substring(removeID.Length + 1);
                    else
                    {
                        // Otherwise, remove it and the leading comma.
                        int startIdx = OrderContents.IndexOf(removeID);
                        System.Diagnostics.Debug.WriteLine(OrderContents + ":" + OrderContents.Substring(0, startIdx - 1)
                            + ":" + OrderContents.Substring(startIdx + removeID.Length));
                        OrderContents = OrderContents.Substring(0, startIdx - 1)
                            + OrderContents.Substring(startIdx + removeID.Length);
                    }
                }

                // Update the price.
                RawCost -= toRemove.Price;
            }
        }

        // RemoveMultipleItems - Remove the given quantity of toRemove
        //                       from this order.
        public void RemoveMultipleItems(MenuItem toRemove, int qty)
        {

        }

        // ContentsToItemList - Return a list of the items referenced by
        //                      the orderContents string.
        public List<MenuItem> ContentsToItemList()
        {
            if (OrderContents == null)
                return null;

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
            var testMenu = new List<MenuItem>();
            testMenu.Add(testItem1);
            testMenu.Add(testItem2);
            testMenu.Add(testItem3);

            // Create the list to be returned.
            var itemObjects = new List<MenuItem>();

            // Split the ID string. For each ID value,
            // get the corresponding item and add it to itemObjects.
            var itemIDs = OrderContents.Split(',');
            foreach(String item in itemIDs)
            {
                int id = Int32.Parse(item);
                itemObjects.Add(testMenu[id - 1]);
            }
            return itemObjects;
        }
    }
}