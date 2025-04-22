using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;
    private const float dampTime = 0.1f;

    private float idleTimer = 0f;
    private const float maxIdleTime = 8f;
    private bool returnToPatrol = false;

    public EnemyIdleState(EnemyStateMachine stateMachine, bool returnToPatrol = false) : base(stateMachine)
    {
        this.returnToPatrol = returnToPatrol;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        idleTimer = 0f; 
    }

  

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FacePlayer();
        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            Debug.Log("In Range");
            return;
        }
      
        stateMachine.Animator.SetFloat(SpeedHash, 0f,dampTime,deltaTime);
        idleTimer += deltaTime;
        if (returnToPatrol && idleTimer >= maxIdleTime)
        {
            stateMachine.SwitchState(new EnemyPatrolState(stateMachine));
            Debug.Log("Idle Time Exceeded, getting back to patrol");
        }
        stateMachine.sightVisulizer.gameObject.SetActive(true);
    }

    public override void Exit()
    {
       
    }
}
