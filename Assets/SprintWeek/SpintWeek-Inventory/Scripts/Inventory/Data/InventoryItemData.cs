using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class InventoryItemData : ScriptableObject
    {
        [Header("Item Info")]
        public string itemName;
        public Sprite icon;
        public int maxStackSize = 1;
        public GameObject worldPrefab; 

        [Header("Optional")]
        [TextArea]
        public string description;
    }
}
