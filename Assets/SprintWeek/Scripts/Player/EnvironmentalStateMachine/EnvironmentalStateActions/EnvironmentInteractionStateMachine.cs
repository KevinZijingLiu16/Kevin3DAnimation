using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
  public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset,
    }

    private EnvironmentInteractionContext _context;

    [SerializeField]private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftHandRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightHandRotationConstraint;

    [SerializeField] private CharacterController _characterController;

    private void Awake()
    {
        ValidateConstraints();
        
       _context = new EnvironmentInteractionContext(
            _leftIkConstraint,
            _rightIkConstraint,
            _leftHandRotationConstraint,
            _rightHandRotationConstraint,
            _characterController,
            transform.root
        );

        InitializedState();
        ConstructEnvironmentDetectionCollider();

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if(_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, 0.1f);
        }
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK Constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right IK Constraint is not assigned.");
        Assert.IsNotNull(_leftHandRotationConstraint, "Left Hand Rotation Constraint is not assigned.");
        Assert.IsNotNull(_rightHandRotationConstraint, "Right Hand Rotation Constraint is not assigned.");
        Assert.IsNotNull(_characterController, "Character Controller is not assigned.");
    }
    private void InitializedState()
    {
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));

        CurrentState = States[EEnvironmentInteractionState.Reset];

    }

    private void ConstructEnvironmentDetectionCollider()
    {
        float wingSpan = _characterController.height;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingSpan, wingSpan, wingSpan);
        boxCollider.center = new Vector3(_characterController.center.x, _characterController.center.y + (0.25f * wingSpan), _characterController.center.z + (0.5f*wingSpan));
        boxCollider.isTrigger = true;
        _context.ColliderCenterY = _characterController.center.y;
    }



}
