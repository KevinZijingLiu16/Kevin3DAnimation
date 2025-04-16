using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateManager<Estate>: MonoBehaviour where Estate : Enum
{
  
    protected Dictionary<Estate, BaseState<Estate>> States = new Dictionary<Estate, BaseState<Estate>>();

    protected BaseState<Estate> CurrentState;
    protected bool isTransitioningState = false;

    void Start()
    { 
        CurrentState.EnterState();
    }
    void Update()
    {
        Estate nextStateKey = CurrentState.GetNextState();
        if(!isTransitioningState && nextStateKey.Equals(CurrentState.Statekey))
        {
            CurrentState.UpdateState();
        }
        else if(!isTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
        
    }

    public void TransitionToState(Estate stateKey)
    {
        isTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        isTransitioningState = false;
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
    void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }
    void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }




}
