// OrderModel - The partial class of Order, which contains all methods.
//              The object itself is defined in DatabaseModel.

using System.Collections.Generic;
using System.Linq;

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
        // Constructor - Make a blank Order.
        public Order()
        {
            OrderContents = null;
            RawCost = 0;
            OrderStatus = (int) PickUpOrder.Models.OrderStatus.NotSubmitted;
        }

        // CalcFinalCost - Calculates the final cost of an order
        //                 based on the raw cost already saved.
        public double CalcFinalCost()
        {
            // Divide by 100 since the raw cost is in cents.
            return RawCost *
                   (1 + Properties.Settings.Default.taxPercentage) / 100;
        }

        // AddSingleItem - Add one quantity of toAdd to this order.
        public void AddSingleItem(MenuItem toAdd)
        {
            // If the price is null, then this item is invalid, so ignore it.
            if (toAdd.Price == null)
                return;

            // If this is the first item, make a new string.
            // Otherwise, add this item's ID to the end in a CSV format.
            if (OrderContents == null)
                OrderContents = toAdd.ItemID.ToString();
            else
                OrderContents += $",{toAdd.ItemID}";

            // Update the raw cost.
            RawCost += (int) toAdd.Price;
        }

        // AddMultipleItems - Add toAdd to this order with the given quantity.
        //                    This is just an encapsulation of AddSingleItem.
        public void AddMultipleItems(MenuItem toAdd, int qty)
        {
            for (var i = 0; i < qty; i++)
                AddSingleItem(toAdd);
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
            string removeID = toRemove.ItemID.ToString();
            if(itemIDs.Contains(removeID))
            {
                // If this is the only item, set the list to null.
                if (itemIDs.Length == 1)
                    OrderContents = null;
                else
                {
                    // If this is the first ID, remove it
                    // and its trailing comma.
                    if (itemIDs[0] == removeID)
                        OrderContents =
                            OrderContents.Substring(removeID.Length + 1);
                    else
                    {
                        // Otherwise, remove it and the leading comma.
                        int startIdx = OrderContents.IndexOf(removeID);
                        OrderContents =
                            OrderContents.Substring(0, startIdx - 1)
                            + OrderContents.Substring(startIdx +
                                                      removeID.Length);
                    }
                }

                // Update the price if possible.
                if(toRemove.Price != null)
                    RawCost -= (int) toRemove.Price;
            }
        }

        // RemoveMultipleItems - Remove the given quantity of toRemove
        //                       from this order.
        //                       This is just an encapsulation of
        //                       RemoveSingleItem.
        public void RemoveMultipleItems(MenuItem toRemove, int qty)
        {
            for (var i = 0; i < qty; i++)
                RemoveSingleItem(toRemove);

            // If toRemove was deleted, recalculate the raw cost.
            if (toRemove.Price == null)
            {
                var newCost = 0;
                if (OrderContents != null)
                {
                    foreach (MenuItem item in ContentsToItemList())
                    {
                        if (item.Price != null)
                            newCost += (int)item.Price;
                    }
                }
                RawCost = newCost;
            }
        }

        // ContentsToItemList - Return a list of the items referenced by
        //                      the orderContents string.
        public List<MenuItem> ContentsToItemList()
        {
            if (OrderContents == null)
                return null;

            // Establish a database connection.
            var db = new PickUpOrderDBEntities2();

            // Create the list to be returned.
            var itemObjects = new List<MenuItem>();

            // Split the ID string. For each ID value,
            // get the corresponding item and add it to itemObjects.
            string[] itemIDs = OrderContents.Split(',');
            foreach(string item in itemIDs)
            {
                int id = int.Parse(item);
                itemObjects.Add(db.MenuItems.Find(id));
            }
            return itemObjects;
        }
    }
}