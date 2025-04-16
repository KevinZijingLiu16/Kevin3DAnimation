using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    [Header("Input")]
    [SerializeField] private InputActionReference toggleInventoryActionRef;

    private void Start()
    {
       
        inventoryPanel.SetActive(false);

   
        toggleInventoryActionRef.action.Enable();
        toggleInventoryActionRef.action.performed += ToggleInventory;
    }

    private void OnDestroy()
    {
    
        toggleInventoryActionRef.action.performed -= ToggleInventory;
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
