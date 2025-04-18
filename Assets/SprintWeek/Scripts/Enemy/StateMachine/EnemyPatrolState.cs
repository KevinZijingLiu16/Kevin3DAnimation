using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private int currentPointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private Vector3 currentVelocity = Vector3.zero;
    private float acceleration = 8f;

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;
    private const float dampTime = 0.1f;
    private const float waitDuration = 5f;

    public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        MoveToCurrentPoint();
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.CanSensePlayer())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        if (stateMachine.Agent.pathPending) return;

        if (isWaiting)
        {
            waitTimer += deltaTime;
            if (waitTimer >= waitDuration)
            {
                isWaiting = false;
                waitTimer = 0f;
                GoToNextPoint();
            }
            return;
        }

        if (stateMachine.Agent.remainingDistance <= stateMachine.Agent.stoppingDistance)
        {
            isWaiting = true;
            waitTimer = 0f;
            stateMachine.Animator.SetFloat(SpeedHash, 0f);
            return;
        }

        Vector3 desiredVelocity = stateMachine.Agent.desiredVelocity;
        currentVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, acceleration * deltaTime);

        if (Vector3.Dot(stateMachine.transform.forward, currentVelocity.normalized) < 0)
        {
            currentVelocity = Vector3.zero;
        }

        Move(currentVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        FaceDirection(currentVelocity);
        stateMachine.Animator.SetFloat(SpeedHash, currentVelocity.magnitude > 0.1f ? 0.5f : 0f, dampTime, deltaTime);

        stateMachine.Agent.nextPosition = stateMachine.transform.position;
    }

    public override void Exit()
    {
        stateMachine.Animator.SetFloat(SpeedHash, 0f);
    }

    private void MoveToCurrentPoint()
    {
        if (stateMachine.PatrolPoints.Length == 0) return;
        stateMachine.Agent.destination = stateMachine.PatrolPoints[currentPointIndex].position;
    }

    private void GoToNextPoint()
    {
        if (stateMachine.PatrolPoints.Length == 0) return;
        currentPointIndex = (currentPointIndex + 1) % stateMachine.PatrolPoints.Length;
        MoveToCurrentPoint();
    }

    private void FaceDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            stateMachine.transform.rotation = targetRotation;
        }
    }
}
