// MenuItemModel - The partial class of MenuItem, which contains all methods.
//                 The object itself is defined in DatabaseModel.

namespace PickUpOrder.Models
{
    public partial class MenuItem
    {
        // NullifyItem - Set all nullable properties to null
        //               to indicate that this item has been removed.
        public void NullifyItem()
        {
            Description = null;
            Price = null;
            Category = null;
        }
    }
}