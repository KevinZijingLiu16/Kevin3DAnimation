using UnityEngine;

public class ScaleAndGrowOnEnable : MonoBehaviour
{
    public float duration = 0.5f;
    public float targetScale = 1.5f;

    private float timer;
    private Vector3 initialScale = Vector3.zero;
    private Vector3 finalScale;
    private bool isScaling;

    private void OnEnable()
    {
        timer = 0f;
        isScaling = true;
        transform.localScale = initialScale;
        finalScale = Vector3.one * targetScale;
    }

    private void Update()
    {
        if (!isScaling) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

        if (t >= 1f)
        {
            isScaling = false;
        }
    }
}
