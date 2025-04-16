using UnityEngine;

namespace Inventory.Model
{
    public class InventorySlot
    {
        // The index of the slot in the inventory, easy to track the item position
        public int Index { get; private set; }
        //The actual content of the slot
        // if not null, it means that the slot is occupied by an item stack
        // if null, it means that the slot is empty
        public InventoryItemStack ItemStack { get; private set; }
        // Property to check if the slot is empty
        public bool IsEmpty => ItemStack == null || ItemStack.Quantity <= 0;
        //Constructor, assign the index and starts empty
        public InventorySlot(int index)
        {
            Index = index;
            ItemStack = null;
        }

        // Function to sets the content to a stack
        public void AssignItem(InventoryItemStack stack)
        {
            ItemStack = stack;
        }
        // Function to remove the content of the slot like removing or dropping, when the slot has no item.
        public void Clear()
        {
            ItemStack = null;
        }
        public void Reduce(int amount)
        {
            if (!IsEmpty)
            {
                ItemStack.Reduce(amount);
            }
        }

    }
}
