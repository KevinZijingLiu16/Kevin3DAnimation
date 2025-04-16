using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext Context;
    private float _movingAwayOffset = 0.2f;
    private bool _shouldReset;
    private float _movingAwayTime = 0f;
    private float _movingAwayThreshold = 1f;

    public EnvironmentInteractionState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        Context = context;
    }

    private Vector3 GetClosestPointOnCollider(Collider intersectingCollider, Vector3 positionToCheck)
    {
        return intersectingCollider.ClosestPoint(positionToCheck);
    }
   
    protected void StartIKTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") && Context.CurrentIntersectingCollider == null)
        {
            Context.CurrentIntersectingCollider = intersectingCollider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(intersectingCollider, Context.RootTransform.position);
        Context.SetCurrentSide(closestPointFromRoot);

            SetIKTargetPosition();
         }

    }

    protected void UpdateIKTargetPosition(Collider intersectingCollider)
    {
        if (intersectingCollider == null || Context.ClosestPointOnColliderFromShoulder == Vector3.positiveInfinity)
            return;

        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            SetIKTargetPosition();
        }
    }

    protected void ResetIKTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            Context.CurrentIntersectingCollider = null;
            Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
            _shouldReset = true;
        }
    }
    private void SetIKTargetPosition()
    {
        if (Context.ClosestPointOnColliderFromShoulder == Vector3.positiveInfinity)
        {
            Debug.LogWarning("ClosestPointOnColliderFromShoulder is Infinity. Skipping IK target update.");
            return;
        }

        Vector3 shoulderFlatPos = new Vector3(
            Context.CurrentShoulderTransform.position.x,
            Context.CharacterShoulderHeight,
            Context.CurrentShoulderTransform.position.z
        );

        Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(Context.CurrentIntersectingCollider, shoulderFlatPos);

        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        if (rayDirection == Vector3.zero || float.IsNaN(rayDirection.x))
        {
            Debug.LogWarning("Invalid rayDirection (zero or NaN). Skipping IK target update.");
            return;
        }

        Vector3 offset = rayDirection.normalized * 0.05f;
        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;

        Context.CurrentIKTargetTransform.position = new Vector3(offsetPosition.x, Context.InteractionPointYOffset, offsetPosition.z);
    }


    protected bool CheckIsBadAngle()
    { 
     if(Context.CurrentIntersectingCollider == null)
        {
            return false;
        }

       Vector3 targetDirection = Context.ClosestPointOnColliderFromShoulder - Context.CurrentShoulderTransform.position;
        Vector3 shoulderDirection = Context.CurrentBodySide == EnvironmentInteractionContext.EBodySide.Right ? Context.RootTransform.right : -Context.RootTransform.right;

        float dotProduct = Vector3.Dot(targetDirection.normalized, shoulderDirection.normalized);
        bool isBadAngle = dotProduct < -0.5f;    

        return isBadAngle;


    }

    protected bool CheckShouldReset()
    {
        if (_shouldReset)
        {
            Debug.Log("[CheckShouldReset] _shouldReset = true (probably OnTriggerExit)");
            Context.LowestDistance = Mathf.Infinity;
            _shouldReset = false;
            return true;
        }

        bool isPlayerStopped = Context.CharacterController.velocity.magnitude < 0.1f;
        bool isMovingAway = CheckIsMovingAway();
        bool isBadAngle = CheckIsBadAngle();
        bool isPlayerJumping = Context.CharacterController.velocity.y > 1f;

        Debug.Log($"[CheckShouldReset] stopped: {isPlayerStopped}, away: {isMovingAway}, angleBad: {isBadAngle}, jumping: {isPlayerJumping}");

      
        if ((isMovingAway || isBadAngle) && isPlayerStopped)
        {
            Context.LowestDistance = Mathf.Infinity;
            return true;
        }

        return false;
    }

    protected bool CheckIsMovingAway()
    {
        float currentDistanceToTarget = Vector3.Distance(Context.RootTransform.position, Context.ClosestPointOnColliderFromShoulder);
        bool isSearchingForNewInteraction = Context.CurrentIntersectingCollider == null;

        if (isSearchingForNewInteraction)
        {
            _movingAwayTime = 0f;
            return false;
        }

        bool isGettingCloserToTarget = currentDistanceToTarget < Context.LowestDistance;

        if (isGettingCloserToTarget)
        {
            Context.LowestDistance = currentDistanceToTarget;
            _movingAwayTime = 0f; // reset
            return false;
        }

        bool isMovingAway = currentDistanceToTarget > Context.LowestDistance + _movingAwayOffset;

        if (isMovingAway)
        {
            _movingAwayTime += Time.deltaTime;
            if (_movingAwayTime >= _movingAwayThreshold)
            {
                Debug.Log("[CheckIsMovingAway] ¡ú Away CONFIRMED");
                Context.LowestDistance = Mathf.Infinity;
                _movingAwayTime = 0f;
                return true;
            }
        }
        else
        {
            _movingAwayTime = 0f;
        }

        return false;
    }


}
