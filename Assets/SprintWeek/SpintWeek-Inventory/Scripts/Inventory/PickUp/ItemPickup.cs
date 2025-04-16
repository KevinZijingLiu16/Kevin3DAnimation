using Inventory.Data;
using Inventory.Model;
using UnityEngine;
using Inventory.Player;

namespace Inventory.Pickup
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Pickup Settings")]
        public InventoryItemData itemData;
        public int amount = 1;

        private void OnTriggerEnter(Collider other)
        {
            // Check if the player has an inventory
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

            if (playerInventory != null)
            {
                bool added = playerInventory.TryAddItem(itemData, amount);
                if (added)
                {
                    Destroy(gameObject); // Or setActive(false) for object pooling
                }
                else
                {
                    Debug.Log("Inventory full. Item not picked up.");
                }
            }
        }
    }
}
