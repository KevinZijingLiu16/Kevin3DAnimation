using Inventory.Data;
using UnityEngine;

namespace Inventory.Model
{

    public class InventoryItemStack
    {
        public InventoryItemData ItemData { get; private set; } 
        public int Quantity { get; private set; }

        // Constructor, initializes the item data and quantity, every time a new item is added to the inventory it will be created as a new stack
        public InventoryItemStack(InventoryItemData data, int quantity)
        {
            ItemData = data;
            Quantity = quantity;
        }

        // Properties to check if the stack reached its maximum capacity
        public bool IsFull => Quantity >= ItemData.maxStackSize;
        
        

        public void Add(int amount)
        {
            //return the minimum value between the sum of the current quantity and the amount to be added and the maximum stack size. 
            //This is to prevent the quantity from exceeding the maximum stack size
            Quantity = Mathf.Min(Quantity + amount, ItemData.maxStackSize);
            
        }

        public void Remove(int amount)
        {
            //return the maximum value between the subtraction of the current quantity and the amount to be removed and 0.
            //This is to prevent the quantity from becoming negative
            Quantity = Mathf.Max(Quantity - amount, 0);
        }
        public void Reduce(int amount)
        {
            Quantity -= amount;
        }

    }
}
