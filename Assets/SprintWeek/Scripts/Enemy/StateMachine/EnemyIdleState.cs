using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;
    private const float dampTime = 0.1f;

   

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
       
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

        
    }

    public override void Exit()
    {
        
    }
}
