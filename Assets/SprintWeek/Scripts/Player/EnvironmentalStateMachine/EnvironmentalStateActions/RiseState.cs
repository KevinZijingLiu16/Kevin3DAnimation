using UnityEngine;

public class RiseState : EnvironmentInteractionState
{
    private float _elapsedTime = 0f;
    private float _lerpDuration = 5f;
    private float _riseWeight = 1f;
    Quaternion _expectedHandRotation;
    float _maxDistance = 0.5f;
    protected LayerMask _interactableLayerMask = LayerMask.GetMask("Interactable");
    private float _rotationSpeed = 1000f;
    private float _touchDistanceThreshold = 0.1f;
    private float _touchTimeThreshold = 5f;
    public RiseState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }
    public override void EnterState()
    {
        Debug.Log("RiseState Entered");
        _elapsedTime = 0f;
    }
    public override void UpdateState()
    {
        CalculateExpectedHandRotation();
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ClosestPointOnColliderFromShoulder.y, _elapsedTime / _lerpDuration);

        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, _riseWeight, _elapsedTime / _lerpDuration);

        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, _riseWeight, _elapsedTime / _lerpDuration);
        _elapsedTime += Time.deltaTime;

        Context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIKTargetTransform.rotation, _expectedHandRotation, _rotationSpeed * Time.deltaTime);
    }
    public override void ExitState()
    {

    }
    private void CalculateExpectedHandRotation()
    {
       Vector3 startPos = Context.CurrentShoulderTransform.position;
        Vector3 endPos = Context.ClosestPointOnColliderFromShoulder;

        Vector3 direction = (endPos - startPos).normalized;

        if(Physics.Raycast(startPos, direction, out RaycastHit hit, _maxDistance, _interactableLayerMask))
        {
            Vector3 surfaceNormal = hit.normal;
            Vector3 targetForward = -surfaceNormal;
            _expectedHandRotation = Quaternion.LookRotation(targetForward, Vector3.up);

        }
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

        if (Vector3.Distance(Context.CurrentIKTargetTransform.position, Context.ClosestPointOnColliderFromShoulder) < _touchDistanceThreshold && _elapsedTime >= _touchTimeThreshold)
        { 
          return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Touch;
        }

        return Statekey;
    }
}
