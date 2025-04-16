using UnityEngine;
using Inventory.Data;
using NUnit.Framework;
using System.Collections.Generic;
namespace Inventory.Model
{
    public class InventoryModel 
    {

        //Property store a list of inventory slots.
       public List<InventorySlot> Slots { get; private set; }
        // Constructor, initialize the list (empty) then create slotCount number of slots with unique indexes, if slotCount is 10, indexes 0 to 9
        public InventoryModel(int slotCount)
        {
            Slots = new List<InventorySlot>();

            for (int i = 0; i < slotCount; i++)
            {
                Slots.Add(new InventorySlot(i));
            }

        }
        // Function to add an item or stack of items to the inventory

        //public bool TryAddItem(InventoryItemData itemData, int amount)
        //{
        //    // Try to stack into existing slot first
        //    //check is not empty
        //    //check has the same item type
        //    //check is not full
        //    foreach (var slot in Slots)
        //    {
        //        if (!slot.IsEmpty &&
        //            slot.ItemStack.ItemData == itemData &&
        //            !slot.ItemStack.IsFull)
        //        {
        //            int spaceLeft = itemData.maxStackSize - slot.ItemStack.Quantity;
        //            int addAmount = System.Math.Min(spaceLeft, amount);
        //            slot.ItemStack.Add(addAmount);
        //            amount -= addAmount;

        //            if (amount <= 0) return true;
        //        }
        //    }

        //    // Add to empty slots
        //    foreach (var slot in Slots)
        //    {
        //        if (slot.IsEmpty)
        //        {
        //            int addAmount = System.Math.Min(itemData.maxStackSize, amount);
        //            slot.AssignItem(new InventoryItemStack(itemData, addAmount));
        //            amount -= addAmount;

        //            if (amount <= 0) return true;
        //        }
        //    }

        //    return false; // not enough space
        //}


        // Modified version: always add to a new slot instead of stacking automatically
        public bool TryAddItem(InventoryItemData itemData, int amount)
        {
            while (amount > 0)
            {
                bool addedToSlot = false;

                foreach (var slot in Slots)
                {
                    if (slot.IsEmpty)
                    {
                        int addAmount = Mathf.Min(itemData.maxStackSize, amount);
                        slot.AssignItem(new InventoryItemStack(itemData, addAmount));
                        amount -= addAmount;
                        addedToSlot = true;
                        break;
                    }
                }

                if (!addedToSlot)
                    return false; // No more space
            }

            return true; // All items added
        }


        // this function will be called when the player wants to remove an item from the inventory by the index of the slot
        public void RemoveItemAt(int slotIndex)
        {
            //check if the index is valid
            if (slotIndex >= 0 && slotIndex < Slots.Count)
            {
                Slots[slotIndex].Clear();
            }
        }

        public void SortAndStack()
        {
            // create a new list to store the sorted and stacked slots
            var newSlots = new List<InventorySlot>();

            // create a dictionary to store the total quatity of each item
            Dictionary<InventoryItemData, int> itemTotals = new  Dictionary<InventoryItemData, int>();
            //1.collect all items from the current slots
            //This loop iterates through all the slots in the inventory. If a slot is not empty, it retrieves the item data and quantity from the slot and adds the quantity to the corresponding entry in the itemTotals dictionary
            foreach (var slot in Slots)
            {
                if (!slot.IsEmpty)
                {
                    var data = slot.ItemStack.ItemData;
                    var qty = slot.ItemStack.Quantity;
                    if (!itemTotals.ContainsKey(data))
                        itemTotals[data] = 0;
                    itemTotals[data] += qty;
                }
            }

            // 2.recreate the slot list
            //This loop iterates through the itemTotals dictionary. For each item type, it creates new slots and assigns the stacked items to them. The quantity of items is decreased by the amount stacked in each new slot until all items are stacked.
            foreach (var kvp in itemTotals)
            {
                var item = kvp.Key;
                int remaining = kvp.Value;

                while (remaining > 0)
                {
                    int stackAmount = Mathf.Min(remaining, item.maxStackSize);
                    var newSlot = new InventorySlot(newSlots.Count);
                    newSlot.AssignItem(new InventoryItemStack(item, stackAmount));
                    newSlots.Add(newSlot);
                    remaining -= stackAmount;
                }
            }

            // 3. fill the empty slots
            //This loop adds empty slots to the newSlots list until it has the same number of slots as the original inventory.
            while (newSlots.Count < Slots.Count)
            {
                newSlots.Add(new InventorySlot(newSlots.Count));
            }

            // 4. replace the old slots with the new ones
            //Replaces the old Slots list with the new sorted and stacked newSlots list.
            Slots = newSlots;
        }

        public bool SplitOneFromStack(int fromIndex)
        {
            //get the slot from which we want to split the item, by the specified index
            var fromSlot = Slots[fromIndex];
            //if the slot is empty or 1, we cannot split it
            if (fromSlot.IsEmpty || fromSlot.ItemStack.Quantity <= 1)
                return false;
            //loop through the slots to find an empty slot
            foreach (var slot in Slots)
            {
                if (slot.IsEmpty)
                {
                    //get the item data from the fromSlot
                    var item = fromSlot.ItemStack.ItemData;
                    fromSlot.Reduce(1);
                    //assign a new stack with 1 item to the empty slot
                    slot.AssignItem(new InventoryItemStack(item, 1));
                    return true;
                }
            }
            //if no empty slot is found, return false
            return false; 
        }


    }


}
