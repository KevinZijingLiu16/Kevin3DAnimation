using UnityEngine;

public class SightSense : SenseComponent
{
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float sightHalfAngle = 45f;
    [SerializeField] private float eyeHeight = 1.5f;
    protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        float distance = Vector3.Distance(stimuli.transform.position, transform.position);
        // Check if the stimuli is within sight range
        if (distance > sightDistance) { return false; }

        Vector3 forwardDir = transform.forward;
        Vector3 stimuliDir = (stimuli.transform.position - transform.position).normalized;
        // Check if the stimuli is within the angle of sight
        if ( Vector3.Angle(forwardDir,stimuliDir) > sightHalfAngle)
        {
            return false;
        }
        // check if there is any obstacles between the enemy and the stimuli
        if (Physics.Raycast(transform.position + Vector3.up * eyeHeight, stimuliDir, out RaycastHit hitInfo, sightDistance))
        {
            if(hitInfo.collider.gameObject != stimuli.gameObject)
            {
                return false;
            }
            
        }
        return true;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Vector3 drawCenter = transform.position + Vector3.up * eyeHeight;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(drawCenter, sightDistance);

        Vector3 leftLimtDir = Quaternion.AngleAxis(sightHalfAngle, Vector3.up) * transform.forward;
        Vector3 rightLimitDir = Quaternion.AngleAxis(-sightHalfAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(drawCenter, drawCenter + leftLimtDir * sightDistance);
        Gizmos.DrawLine(drawCenter, drawCenter + rightLimitDir * sightDistance);

    }


}
