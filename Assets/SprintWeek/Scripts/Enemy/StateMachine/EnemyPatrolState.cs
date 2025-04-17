using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private int currentPointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

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

        Vector3 toTarget = stateMachine.PatrolPoints[currentPointIndex].position - stateMachine.transform.position;
        toTarget.y = 0f;

        if (toTarget.magnitude < 0.5f)
        {
            isWaiting = true;
            waitTimer = 0f;
            stateMachine.Animator.SetFloat(SpeedHash, 0f);
        }
        else
        {
            Move(toTarget.normalized * stateMachine.MovementSpeed, deltaTime);
            FaceDirection(toTarget);
            stateMachine.Animator.SetFloat(SpeedHash, 0.5f, dampTime, deltaTime);
        }
        //if (IsInChaseRange())
        //{
        //    stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        //    return;
        //}


    }

    public override void Exit()
    {
        stateMachine.Animator.SetFloat(SpeedHash, 0f);
    }

    private void MoveToCurrentPoint()
    {
        stateMachine.Agent.destination = stateMachine.PatrolPoints[currentPointIndex].position;
    }

    private void GoToNextPoint()
    {
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
