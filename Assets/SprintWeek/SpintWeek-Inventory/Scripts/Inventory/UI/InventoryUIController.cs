using System.Collections.Generic;
using Inventory.Model;
using Inventory.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryUIController : MonoBehaviour
    {
        [Header("Prefabs & References")]
        public GameObject slotPrefab; //actual prefab for the slot
        public Transform slotParent;  //the panel that contains all the slots
        public ItemTooltip tooltipReference; //reference to the tooltip in the scene
        [Header("Icons")]
        public Sprite emptySlotIcon;  
        private InventoryModel _inventoryModel;     
        private List<InventoryUISlot> _uiSlots;
        [Header("Drop item Setting")]
        [SerializeField] private Transform playerTransform;
        [SerializeField]
        private List<ItemWorldMapping> itemWorldMappings;

        public UIDragIcon dragIcon; 
        //Set everything up
        //Store the model reference
        //loop through all InventorySlot s in the model
        //Initiate the prefab and set the index
        public void Initialize(InventoryModel model)
        {
            // Store the reference to the InventoryModel instance
            _inventoryModel = model;
            // Initialize the list that will hold the UI slots
            _uiSlots = new List<InventoryUISlot>();
            // Loop through each InventorySlot in the InventoryModel
            foreach (InventorySlot logicSlot in _inventoryModel.Slots)
            {
                // Instantiate a new slot UI element from the prefab and set the transform as the parent
                GameObject slotGO = Instantiate(slotPrefab, slotParent);
                // Get the InventoryUISlot component from the instantiated GameObject
                InventoryUISlot uiSlot = slotGO.GetComponent<InventoryUISlot>();
                uiSlot.SetIndex(logicSlot.Index);
                uiSlot.tooltip = tooltipReference;
                uiSlot.SetUIController(this);
                uiSlot.SetSlot(logicSlot);
                uiSlot.SetDragIcon(dragIcon);
                _uiSlots.Add(uiSlot);
            }
            RefreshUI();
        }

        public void RefreshUI()
        {
            for (int i = 0; i < _inventoryModel.Slots.Count; i++)
            {
                InventorySlot logicSlot = _inventoryModel.Slots[i];
                InventoryUISlot uiSlot = _uiSlots[i];

                if (logicSlot.IsEmpty)
                {
                    uiSlot.SetEmpty(emptySlotIcon);
                }
                else
                {
                    // If the logic slot is not empty, get the item data and quantity from the logic slot
                    var itemData = logicSlot.ItemStack.ItemData;
                    var quantity = logicSlot.ItemStack.Quantity;
                    uiSlot.SetItem(itemData.icon, quantity, itemData, logicSlot);

                }
            }
        }

        public void SwapItems(int fromIndex, int toIndex)
        {
            var fromSlot = _inventoryModel.Slots[fromIndex];
            var toSlot = _inventoryModel.Slots[toIndex];

            if (fromSlot.IsEmpty) return;

            //if the slot is not empty and same item type,and the to slot is not full
            if (!toSlot.IsEmpty &&
                fromSlot.ItemStack.ItemData == toSlot.ItemStack.ItemData &&
                !toSlot.ItemStack.IsFull)
            {
                //check how much space is available in the to slot
                int space = toSlot.ItemStack.ItemData.maxStackSize - toSlot.ItemStack.Quantity;
                //check how much we can transfer based on how much space is available in toSlot and how much we have in fromSlot, return a minimum value.
                int transferAmount = Mathf.Min(space, fromSlot.ItemStack.Quantity);
                //stack the item in the toSlot
                toSlot.ItemStack.Add(transferAmount);
                //remove the item from the fromSlot
                fromSlot.Reduce(transferAmount);
                //check if the fromSlot is empty after the transfer, if so clear it
                if (fromSlot.ItemStack.Quantity <= 0)
                    fromSlot.Clear();
            }
            //if other situation, just swap the items with the stack data
            else
            {
                var temp = fromSlot.ItemStack;
                fromSlot.AssignItem(toSlot.ItemStack);
                toSlot.AssignItem(temp);
            }

            RefreshUI();
        }

        public void OnClickSortInventory()
        {
            _inventoryModel.SortAndStack();
            RefreshUI();
        }

        public void TrySplitStack(int fromIndex)
        {
            bool success = _inventoryModel.SplitOneFromStack(fromIndex);
            if (success)
            {
                RefreshUI();
            }
            else
            {
                Debug.Log("Cannot split: no empty slot available.");
            }
        }
        [System.Serializable]
        public class ItemWorldMapping
        {
            public InventoryItemData itemData;
            public GameObject worldPrefab;
        }

        public void DropItemIntoWorld(int fromIndex)
        {
            var slot = _inventoryModel.Slots[fromIndex];
            if (slot.IsEmpty) return;

            var itemData = slot.ItemStack.ItemData;
            int quantity = slot.ItemStack.Quantity;

            GameObject prefab = itemWorldMappings.Find(m => m.itemData == itemData)?.worldPrefab;

            if (prefab == null)
            {
                Debug.LogWarning("No prefab mapping for: " + itemData.name);
                return;
            }

            for (int i = 0; i < quantity; i++)
            {
                Vector3 dropPosition = playerTransform.position + playerTransform.forward * 2f;

                dropPosition.y = 1f + i * 0.2f; 
                dropPosition.x += Random.Range(-0.5f, 0.5f);
                dropPosition.z += Random.Range(-0.5f, 0.5f);

                GameObject clone = GameObject.Instantiate(prefab, dropPosition, Quaternion.identity);
            }


            _inventoryModel.RemoveItemAt(fromIndex);
            RefreshUI();
        }


    }
}
