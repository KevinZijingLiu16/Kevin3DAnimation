using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory.UI
{
    public class UIDragIcon : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private Canvas canvasReference;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            //just a double check to see to get the canvas reference
            if (canvasReference == null)
            {
                canvasReference = GetComponentInParent<Canvas>();
            }

            Debug.Log("Canvas RenderMode: " + canvasReference.renderMode);
           // Debug.Log("Canvas worldCamera: " + canvasReference.worldCamera);

            Hide();
        }
        /// This function will be called in the InventoryUISlot when the player starts dragging an item
        public void Show(Sprite icon, int amount)
        {
            iconImage.sprite = icon;
            //if the amount is 0 or less, set the text to empty
            quantityText.text = amount >= 1 ? amount.ToString() : "";
            gameObject.SetActive(true);
            UpdatePosition();
        }
        // this method will be called every time the player drags the item and change the dragged icon position.
        public void UpdatePosition()
        {

            if (canvasReference == null)
            {
                Debug.LogWarning("Canvas reference is null. Can't update drag icon position.");
                return;
            }
            //if the canvas is in screen space overlay, set the camera to null
            //if the canvas is in screen space camera, set the camera to the worldCamera of the canvas
            Camera camera = canvasReference.renderMode == RenderMode.ScreenSpaceOverlay
     ? null
     : canvasReference.worldCamera;


            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasReference.transform as RectTransform,
                Input.mousePosition,
                camera,
                out Vector2 localPoint
            );

            _rectTransform.anchoredPosition = localPoint;
        }


        
        // this function will be called at the beginning of the drag and drop process.
        // and will will be called in the InventoryUISlot when the player stops dragging an item
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
