using UnityEngine;
using UnityEngine.Events;
using Inventory.Model;
using Inventory.Player;
using System.Collections.Generic;
using System.Linq;

public class ItemObjectiveChecker : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    public UnityEvent OnObjectiveComplete;

    private const int RequiredCount = 5;

    private void Update()
    {
        var stacks = playerInventory.GetAllItems();

        int cubeCount = GetItemCount(stacks, "Cube");
        int sphereCount = GetItemCount(stacks, "Sphere");
        int capsuleCount = GetItemCount(stacks, "Capsule");

        if (cubeCount == RequiredCount &&
            sphereCount == RequiredCount &&
            capsuleCount == RequiredCount)
        {
            OnObjectiveComplete?.Invoke();
            enabled = false;
        }
    }

    private int GetItemCount(List<InventoryItemStack> list, string itemName)
    {
        return list
            .Where(i => i.ItemData.itemName == itemName)
            .Sum(i => i.Quantity);
    }
}
