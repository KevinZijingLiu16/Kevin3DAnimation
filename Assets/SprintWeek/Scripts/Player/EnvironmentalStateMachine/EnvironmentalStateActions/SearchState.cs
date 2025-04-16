using UnityEngine;

public class SearchState : EnvironmentInteractionState
{
    public float _approachDistanceThreshold = 5f;
    public SearchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }
    public override void EnterState()
    {
        Debug.Log("SearchState Entered");
    }
    public override void UpdateState()
    {

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
        //Debug.Log("SearchState TriggerExit");
        ResetIKTargetPositionTracking(other);
    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        //if (CheckShouldReset())
        //{
        //    return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        //}
        bool isCloseToTarget = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.RootTransform.position) < _approachDistanceThreshold;
        bool isClosePointOnColliderValid = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;
        if (isClosePointOnColliderValid && isCloseToTarget)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Approach;
        }
        return Statekey;
    }

}


