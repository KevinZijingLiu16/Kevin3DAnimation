using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    float _elapsedTime = 0f;
    float _lerpDuration = 5f;
    float _approachWeight = 0.5f;
    float _rotationSpeed = 500f;
    float _approachRotationWeight = 0.75f;
    float _riseDistanceThreshold = 1.5f;
    float _approachDuration = 20f;
    public ApproachState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }
    public override void EnterState()
    {
        Debug.Log("ApproachState Entered");
       
    }
    public override void UpdateState()
    {
        Quaternion expectedGroundRotation = Quaternion.LookRotation(-Vector3.up, Context.RootTransform.forward);
        _elapsedTime += Time.deltaTime;
        Context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIKTargetTransform.rotation, expectedGroundRotation,_rotationSpeed*Time.deltaTime);
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight,_approachRotationWeight, _elapsedTime / _lerpDuration);
        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, _approachWeight, _elapsedTime / _lerpDuration);
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
        if (CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }
        bool isWithinArmsReach = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.CurrentShoulderTransform.position) < _riseDistanceThreshold;
        bool isClosePointOnColliderReal = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

        if (isClosePointOnColliderReal && isWithinArmsReach)
        {
            Debug.Log("Transitioning to RiseState");
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Rise;
        }

        bool isOverStateLifeDuration = _elapsedTime >= _approachDuration;
        if (isOverStateLifeDuration)
        {
            Debug.Log("Approach timed out, resetting.");
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }
        float distance = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.CurrentShoulderTransform.position);
       // Debug.Log($"[ApproachState] Distance to target: {distance}, Threshold: {_riseDistanceThreshold}");


        return Statekey;
    }

}
