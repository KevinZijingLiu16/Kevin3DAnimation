using UnityEngine;

public class PerceptionStimuli : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SenseComponent.RegisterStimuli(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        SenseComponent.UnRegistedStimuli(this);
    }
}
