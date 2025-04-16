using UnityEngine;

public class EnemyFrozenState : EnemyBaseState
{
    private readonly int FlairHash = Animator.StringToHash("Flair");
    
    private const float CrossFadeDuration = 0.1f;

    public EnemyFrozenState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // Play idle animation (can use Locomotion with Speed 0)
        stateMachine.Animator.CrossFadeInFixedTime(FlairHash, CrossFadeDuration);
       
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;

        Debug.Log(">> [EnemyFrozenState] Entered Frozen State");
    }

    public override void Tick(float deltaTime)
    {
        // Do nothing, stay frozen
    }

    public override void Exit()
    {
       
    }
}
