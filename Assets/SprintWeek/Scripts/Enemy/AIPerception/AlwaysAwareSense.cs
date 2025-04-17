using UnityEngine;

public class AlwaysAwareSense : SenseComponent
{
    [SerializeField] float awareDistance = 10f;
    protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        return Vector3.Distance(transform.position, stimuli.transform.position) <= awareDistance;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, awareDistance);
        
    }

}
