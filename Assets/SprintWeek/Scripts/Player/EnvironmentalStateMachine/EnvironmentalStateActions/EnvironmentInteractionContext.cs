using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext 
{
    public enum EBodySide
    {
        Left,
        Right
    }
     private TwoBoneIKConstraint _leftIkConstraint;
     private TwoBoneIKConstraint _rightIkConstraint;
    private MultiRotationConstraint _leftHandRotationConstraint;
     private MultiRotationConstraint _rightHandRotationConstraint;

     private CharacterController _characterController;
    private Transform _rootTransform;
    private Vector3 _leftOriginalTargetPosition;
    private Vector3 _rightOriginalTargetPosition;



    public EnvironmentInteractionContext(
        TwoBoneIKConstraint leftIkConstraint,
        TwoBoneIKConstraint rightIkConstraint,
        MultiRotationConstraint leftHandRotationConstraint,
        MultiRotationConstraint rightHandRotationConstraint,
        CharacterController characterController,
        Transform rootTransform)
    {
        _leftIkConstraint = leftIkConstraint;
        _rightIkConstraint = rightIkConstraint;
        _leftHandRotationConstraint = leftHandRotationConstraint;
        _rightHandRotationConstraint = rightHandRotationConstraint;
        _characterController = characterController;
        _rootTransform = rootTransform;
        _leftOriginalTargetPosition = _leftIkConstraint.data.target.transform.localPosition;
        _rightOriginalTargetPosition = _rightIkConstraint.data.target.transform.localPosition;
        OriginalTargetRotaion = _leftIkConstraint.data.target.rotation;

        CharacterShoulderHeight = leftIkConstraint.data.root.transform.position.y;
        SetCurrentSide(Vector3.positiveInfinity);

       
    }

    public TwoBoneIKConstraint LeftIkConstraint => _leftIkConstraint;
    public TwoBoneIKConstraint RightIkConstraint => _rightIkConstraint;
    public MultiRotationConstraint LeftHandRotationConstraint => _leftHandRotationConstraint;
    public MultiRotationConstraint RightHandRotationConstraint => _rightHandRotationConstraint;
    public CharacterController CharacterController => _characterController;
    public Transform RootTransform => _rootTransform;
    public float CharacterShoulderHeight { get; private set; }


    public Collider CurrentIntersectingCollider { get;  set; }
    public TwoBoneIKConstraint CurrentIKConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentIKTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float InteractionPointYOffset { get; set; } = 0.0f;
    public float ColliderCenterY {  get; set; } 
    public Vector3 CurrentOriginalTargetPosition { get; private set; } 
    public Quaternion OriginalTargetRotaion { get; private set; }
    public float LowestDistance { get; set; } = Mathf.Infinity;

    public void SetCurrentSide(Vector3 positionToCheck)
    { 
        Vector3 leftShoulder = _leftIkConstraint.data.target.position;
        Vector3 rightShoulder = _rightIkConstraint.data.target.position;

        bool isLeftCloser = Vector3.Distance(positionToCheck, leftShoulder) < Vector3.Distance(positionToCheck, rightShoulder);
        if (isLeftCloser)
        {
            Debug.Log("Left Side is closer");
            CurrentBodySide = EBodySide.Left;
            CurrentIKConstraint = _leftIkConstraint;
            CurrentMultiRotationConstraint = _leftHandRotationConstraint;
            CurrentOriginalTargetPosition = _leftOriginalTargetPosition;
        }
        else
        {
            Debug.Log("Right Side is closer");
            CurrentBodySide = EBodySide.Right;
            CurrentIKConstraint = _rightIkConstraint;
            CurrentMultiRotationConstraint = _rightHandRotationConstraint;
            CurrentOriginalTargetPosition = _rightOriginalTargetPosition;

        }

        CurrentShoulderTransform = CurrentIKConstraint.data.root.transform;
        CurrentIKTargetTransform = CurrentIKConstraint.data.target.transform;

    }

}
