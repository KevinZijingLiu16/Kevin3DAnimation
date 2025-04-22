using UnityEngine;

public class HitSense : SenseComponent
{
    private void OnEnable()
    {
        WeaponDamage.OnAnyHitEnemy += OnHitEnemy;
    }

    private void OnDisable()
    {
        WeaponDamage.OnAnyHitEnemy -= OnHitEnemy;
    }

    private void OnHitEnemy(Collider hitTarget, GameObject attacker)
    {
        if (hitTarget.gameObject != this.gameObject) return;

        var attackerStimuli = attacker.GetComponent<PerceptionStimuli>();
        if (attackerStimuli != null)
        {
            if (!perceivableStimuli.Contains(attackerStimuli))
            {
                perceivableStimuli.Add(attackerStimuli);
                NotifyStimuliSensed(attackerStimuli, true);
            }

          
            if (ForgettingRoutines.TryGetValue(attackerStimuli, out Coroutine oldRoutine))
            {
                StopCoroutine(oldRoutine);
            }
            ForgettingRoutines[attackerStimuli] = StartCoroutine(ForgetStimuli(attackerStimuli));

            Debug.Log("HitSense perceived attacker: " + attacker.name);
        }
    }


    protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        return false; 
    }

    protected override void DrawDebug()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 1f);
    }
}
