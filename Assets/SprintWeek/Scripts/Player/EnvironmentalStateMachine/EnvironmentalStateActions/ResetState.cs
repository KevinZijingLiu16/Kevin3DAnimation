using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    private float _elapseTime = 0.0f;
    private float _resetDuration = 1f;
    private float _lerpDuration = 10f;
    private float _rotationSpeed = 500f;
    private float _minMoveSpeed = 0.1f;
    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;    
    }
    public override void EnterState()
    {
        Debug.Log("ResetState Entered");
        _elapseTime = 0f;
        Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        Context.CurrentIntersectingCollider = null;
    }
    public override void UpdateState()
    {
        Debug.Log("ResetState Update");
        _elapseTime += Time.deltaTime;
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ColliderCenterY, _elapseTime / _lerpDuration);
        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, 0f, _elapseTime / _lerpDuration);
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, 0f, _elapseTime / _lerpDuration);
        Context.CurrentIKTargetTransform.localPosition = Vector3.Lerp(Context.CurrentIKTargetTransform.localPosition, Context.CurrentOriginalTargetPosition, _elapseTime / _lerpDuration);
        Context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIKTargetTransform.rotation, Context.OriginalTargetRotaion, _rotationSpeed * Time.deltaTime);

    }
    public override void ExitState()
    {

    }
    public override void OnTriggerEnter(Collider other)
    {
        
    }
    public override void OnTriggerStay(Collider other)
    {

    }
    public override void OnTriggerExit(Collider other)
    {

    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        float speed = Context.CharacterController.velocity.magnitude;
        if (_elapseTime >= _resetDuration && speed > _minMoveSpeed)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
        }
        return Statekey;
        
    }

}
