using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float totalTime = 300f; // 5 mins
    private float remainingTime;

    public UnityEvent OnTimeout;

    public float RemainingTime => remainingTime;

    private void Start()
    {
        remainingTime = totalTime;
    }

    private void Update()
    {
        if (remainingTime <= 0f) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            OnTimeout?.Invoke();
        }
    }
}
