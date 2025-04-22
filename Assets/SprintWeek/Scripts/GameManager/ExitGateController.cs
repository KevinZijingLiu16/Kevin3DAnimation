using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitGateController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer gateRenderer;
    [SerializeField] private Color unlockedColor = Color.green;

    private Collider gateCollider;
    private bool isUnlocked = false;

    private void Awake()
    {
        gateCollider = GetComponent<Collider>();
        gateCollider.isTrigger = false;
    }

  
    public void UnlockGate()
    {
        isUnlocked = true;
        gateCollider.isTrigger = true;

        if (gateRenderer != null)
        {
            gateRenderer.material.color = unlockedColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUnlocked) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the exit!");
            GameManager.Instance.TriggerVictory();
        }
    }
}
