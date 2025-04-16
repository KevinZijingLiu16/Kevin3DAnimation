using Inventory.Data;
using Inventory.Model;
using Inventory.UI;
using UnityEngine;

namespace Inventory.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Inventory Settings")]
        public int slotCount = 20;
        public InventoryUIController uiController;

        private InventoryModel _model;

        private void Start()
        {
            _model = new InventoryModel(slotCount);
            uiController.Initialize(_model);
        }
        //PlayerInventoy.TryAddItem is used when the item is picked up
        public bool TryAddItem(InventoryItemData itemData, int amount)
        {
            //InventoryModel.TryAddItem is to stack, or put the additional item in the first empty slot
            //because PlayerInventory and InventoryModel are from different namespace and different class, so the function name can be the same
            //pass the itemData and amount to the InventoryModel, where the actual try add item logic is.
            bool added = _model.TryAddItem(itemData, amount);
            //if pass successfully, then refresh the UI
            if (added)
            {
                uiController.RefreshUI();
            }
            return added;
        }
    }
}
