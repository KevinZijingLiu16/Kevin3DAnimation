using UnityEngine;

public class NoiseSense : SenseComponent
{
    [SerializeField] private float hearingRadius = 100f;

    private void OnEnable()
    {
        NoiseSystem.OnNoiseEmitted += OnNoiseHeard;
    }

    private void OnDisable()
    {
        NoiseSystem.OnNoiseEmitted -= OnNoiseHeard;
    }

    private void OnNoiseHeard(Vector3 position, GameObject source)
    {
        float distance = Vector3.Distance(transform.position, position);
        if (distance > hearingRadius) return;

        var stimuli = source.GetComponent<PerceptionStimuli>();
        if (stimuli == null) return;

        if (!perceivableStimuli.Contains(stimuli))
        {
            perceivableStimuli.Add(stimuli);
            NotifyStimuliSensed(stimuli, true);
        }

      
        if (ForgettingRoutines.TryGetValue(stimuli, out Coroutine oldRoutine))
        {
            StopCoroutine(oldRoutine);
        }

        ForgettingRoutines[stimuli] = StartCoroutine(ForgetStimuli(stimuli));
        Debug.Log("NoiseSense heard noise from " + source.name);
    }

    protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        return false; 
    }

    protected override void DrawDebug()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, hearingRadius);
    }
}
