using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using Inventory.Player;
using Inventory.Model;
using System.Collections.Generic;
using System.Linq;

public class GameUIManager : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI cubeCountText;
    public TextMeshProUGUI sphereCountText;
    public TextMeshProUGUI capsuleCountText;

    [Header("References")]
    public GameTimer gameTimer;
    public PlayerInventory playerInventory;

    [Header("End Panels")]
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryTimeText;

    public GameObject failurePanel;

    [Header("Warnings")]
    public TextMeshProUGUI warningText;


    private void Update()
    {
        UpdateTimer();
        UpdateItemCounts();
    }

    private void UpdateTimer()
    {
        timerText.text = $"Time Left: {gameTimer.RemainingTime:F1}s";
    }

    private void UpdateItemCounts()
    {
        var items = playerInventory.GetAllItems();

        int cubeCount = GetCount(items, "Cube");
        int sphereCount = GetCount(items, "Sphere");
        int capsuleCount = GetCount(items, "Capsule");

        
        cubeCountText.color = Color.white;
        sphereCountText.color = Color.white;
        capsuleCountText.color = Color.white;
        warningText.text = ""; 

        bool hasExtra = false;
        if (cubeCount > 5)
        {
            cubeCountText.color = Color.red;
            warningText.text += "Too many Cubes. ";
            hasExtra = true;
        }

        if (sphereCount > 5)
        {
            sphereCountText.color = Color.red;
            warningText.text += "Too many Spheres. ";
            hasExtra = true;
        }

        if (capsuleCount > 5)
        {
            capsuleCountText.color = Color.red;
            warningText.text += "Too many Capsules. ";
            hasExtra = true;
        }

        if (hasExtra)
        {
            warningText.text += "\nOpen inventory and drop extra items to continue.";
        }

      
        cubeCountText.text = $"Cube: {cubeCount}/5";
        sphereCountText.text = $"Sphere: {sphereCount}/5";
        capsuleCountText.text = $"Capsule: {capsuleCount}/5";
    }


    private int GetCount(List<InventoryItemStack> list, string itemName)
    {
        return list
            .Where(i => i.ItemData.itemName == itemName)
            .Sum(i => i.Quantity);
    }

   
    public void ShowVictory(float finishTime)
    {
        victoryPanel.SetActive(true);
        victoryTimeText.text = $"You still have {finishTime:F1} seconds!";
    }

    
    public void ShowFailure()
    {
        failurePanel.SetActive(true);
    }

 
    public void OnClickRestart()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnClickMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
