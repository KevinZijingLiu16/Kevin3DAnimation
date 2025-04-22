using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private readonly int attackHash = Animator.StringToHash("Attack");
    private const float CrossFadeDuration = 0.1f;
   
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
      stateMachine.Animator.CrossFadeInFixedTime(attackHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator) >= 1f)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
        FacePlayer();

        stateMachine.sightVisulizer.gameObject.SetActive(false);
    }

    public override void Exit()
    {
       
    }

   
}
