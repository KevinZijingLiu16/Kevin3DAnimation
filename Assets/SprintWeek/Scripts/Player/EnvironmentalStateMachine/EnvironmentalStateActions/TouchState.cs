using UnityEngine;

public class TouchState : EnvironmentInteractionState
{
    public float _elapsedTime = 0f;
    public float _resetThreshold = 1.5f;

    public TouchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }
    public override void EnterState()
    {
        _elapsedTime = 0f;
    }
    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
    }
    public override void ExitState()
    {

    }
    public override void OnTriggerEnter(Collider other)
    {
        StartIKTargetPositionTracking(other);
    }
    public override void OnTriggerStay(Collider other)
    {
        UpdateIKTargetPosition(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        ResetIKTargetPositionTracking(other);
    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (_elapsedTime >= _resetThreshold  || CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }

        return Statekey;
    }
}
