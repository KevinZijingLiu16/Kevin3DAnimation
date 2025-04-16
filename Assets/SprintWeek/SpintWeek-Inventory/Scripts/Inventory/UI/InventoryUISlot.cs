using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Inventory.Data;
using Inventory.Model;



namespace Inventory.UI
{
    public class InventoryUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
       
        [SerializeField] private Image iconImage; 
        [SerializeField] private TextMeshProUGUI quantityText; 
        public ItemTooltip tooltip;
        private UIDragIcon _dragIcon;

        private InventoryItemData _itemData;
        private InventorySlot _logicSlot;
        private InventoryUIController _uiController;

        //By making DraggedSlot static, to ensure that there is a single, globally accessible reference to the currently dragged slot. Because static field belongs to the class itself rather than any particular instance. And make it to be the only one reference at the same time. This simplifies the logic for handling drag-and-drop operations and ensures that the system can always determine which slot is being dragged, regardless of which instance of InventoryUISlot is currently being interacted with.

        public static InventoryUISlot DraggedSlot { get; private set; } // Static reference to store the dragged slot

        // This is the index of the slot in the inventory
        private int _index;
        //this function will be called in the InventoryUIController to set the index of the slot when the actual slot prefab is created
        public void SetIndex(int index)
        {
            _index = index;
        }
        // This function will be called in the InventoryUIController when refreshing the UI
        public void SetItem(Sprite icon, int quantity, InventoryItemData itemData, InventorySlot logicSlot)
        {
            iconImage.sprite = icon;
            iconImage.color = Color.white;
            quantityText.text = quantity >= 1 ? quantity.ToString() : "";
            _itemData = itemData;
            _logicSlot = logicSlot;  
        }

        public void SetEmpty(Sprite emptyIcon)
        {
            iconImage.sprite = emptyIcon;
            iconImage.color = new Color(1, 1, 1, 0.3f);
            quantityText.text = "";
            _itemData = null;
        }
        public void SetSlot(InventorySlot logicSlot)
        {
            _logicSlot = logicSlot;
        }
        public void SetUIController(InventoryUIController controller)
        {
            _uiController = controller;
        }


        public void SetDragIcon(UIDragIcon icon)
        {
            _dragIcon = icon;
        }
        // GetIndex will be called in the InventoryUIController when the player wants to remove an item from the inventory or drag and drop
        public int GetIndex()
        {
            return _index;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_itemData != null && tooltip != null)
            {
                tooltip.Show(_itemData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltip != null)
            {
                tooltip.Hide();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DraggedSlot = this; // Store the dragged slot reference
            if (_logicSlot == null || _logicSlot.IsEmpty) return;

            var itemData = _logicSlot.ItemStack.ItemData;
            int quantity = _logicSlot.ItemStack.Quantity;
            //pass the item data like icon image and the quantity to the drag icon 
            _dragIcon.Show(itemData.icon, quantity);
        }


        public void OnDrag(PointerEventData eventData)
        {
           _dragIcon.UpdatePosition();
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            //clear the static reference to the dragged slot, indicating that the drag operation has ended
            DraggedSlot = null;
            _dragIcon.Hide();
            //get the gameobject that the pointer is currently over
            var pointerTarget = eventData.pointerEnter;
            //try to get the InventoryUISlot component from the pointer target
            //InventoryUISlot targetSlot = null;
            //if (pointerTarget != null)
            //{
            //    targetSlot = pointerTarget.GetComponentInParent<InventoryUISlot>();
            //}
            //else
            //{
            //    targetSlot = null;
            //}  To Make it shorthand: code below
            var targetSlot = pointerTarget != null ? pointerTarget.GetComponentInParent<InventoryUISlot>() : null;
            //check if the target slot is valid and not the same as the current slot
            if (targetSlot != null && targetSlot != this)
            {
                //swap the items between the two slots
                _uiController.SwapItems(GetIndex(), targetSlot.GetIndex());
            }
            else
            {
                //if not a valid target slot, drop the item into the world
                _uiController.DropItemIntoWorld(_index);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && _logicSlot != null && !_logicSlot.IsEmpty)
            {
                _uiController.TrySplitStack(_index);
            }
        }
       


    }
}
