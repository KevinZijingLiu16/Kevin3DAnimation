using Inventory.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Inventory.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image iconImage;

        private RectTransform _panelRect;

        private void Awake()
        {
            _panelRect = panel.GetComponent<RectTransform>();
            Hide();
        }

      
        public void Show(InventoryItemData data)
        {
            panel.SetActive(true);

            itemNameText.text = data.itemName;
            descriptionText.text = data.description;
            iconImage.sprite = data.icon;

         // UpdateTooltipPosition(); 

        }

        public void Hide()
        {
            panel.SetActive(false);
        }

        private void UpdateTooltipPosition()
        {
            Vector2 screenPosition = Input.mousePosition;
            Vector2 offset = new Vector2(20, -20);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panel.transform.parent as RectTransform,
                screenPosition + offset,
                null,
                out Vector2 localPoint
            );

            _panelRect.anchoredPosition = localPoint;
        }
    }
}
