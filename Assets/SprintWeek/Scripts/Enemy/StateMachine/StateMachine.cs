using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{

    private State currentState;

    public void SwitchState(State newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState?.Enter();
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != null)
        {
        currentState.Tick(Time.deltaTime);
         
        }
            
    }

    
}
