using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    [SerializeField] float forgettingTime = 3f;
    //static is used to make the list of stimuli shared across all instances of the class
    static List<PerceptionStimuli> registeredStimuli = new List<PerceptionStimuli>();
    List<PerceptionStimuli> perceivableStimuli = new List<PerceptionStimuli>();
    Dictionary<PerceptionStimuli, Coroutine> ForgettingRoutines = new Dictionary<PerceptionStimuli, Coroutine>();

    public delegate void OnPerceptionUpdated(PerceptionStimuli stimuli, bool successfullySensed);
    public event OnPerceptionUpdated OnPerceptionUpdatedEvent;
    static public void RegisterStimuli(PerceptionStimuli stimuli)
    {
        if (registeredStimuli.Contains(stimuli)) return;
        registeredStimuli.Add(stimuli);
    }

    static public void UnRegistedStimuli(PerceptionStimuli stimuli)
    {

        registeredStimuli.Remove(stimuli);
    }

    protected abstract bool IsStimuliSensable(PerceptionStimuli stimuli);

    private void Update()
    {
        foreach (var stimuli in registeredStimuli)
        {
            if (IsStimuliSensable(stimuli))
            {
                if (!perceivableStimuli.Contains(stimuli))
                {
                    perceivableStimuli.Add(stimuli);
                    if (ForgettingRoutines.TryGetValue(stimuli, out Coroutine routine))
                    {
                        StopCoroutine(routine);
                        ForgettingRoutines.Remove(stimuli);
                    }
                    else
                    {
                        OnPerceptionUpdatedEvent?.Invoke(stimuli, true);
                        Debug.Log("I have found " + stimuli.gameObject);
                    }

                    
                }
            }
            else
            {
                if (perceivableStimuli.Contains(stimuli))
                {
                    perceivableStimuli.Remove(stimuli);
                    ForgettingRoutines.Add(stimuli, StartCoroutine(ForgetStimuli(stimuli)));
                }
            }
        }
    }
    protected virtual void DrawDebug()
    {

    }

    private void OnDrawGizmos()
    {
        DrawDebug();
    }
    IEnumerator ForgetStimuli(PerceptionStimuli stimuli)
    {
        yield return new WaitForSeconds(forgettingTime);
        ForgettingRoutines.Remove(stimuli);
        OnPerceptionUpdatedEvent?.Invoke(stimuli, false);
       Debug.Log("I have forgotten " + stimuli.gameObject);
    }
}

