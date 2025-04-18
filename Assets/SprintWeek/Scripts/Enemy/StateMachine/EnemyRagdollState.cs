using System.Collections;
using UnityEngine;

public class EnemyRagdollState : EnemyBaseState
{
    private float ragdollTime = 5f;
    private Ragdoll ragdoll;
    private readonly int StandHash = Animator.StringToHash("Stand");
    private const float CrossFadeDuration = 0.1f;
    private const float standUpTime = 5f;

    public EnemyRagdollState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        ragdoll = stateMachine.GetComponent<Ragdoll>();
    }

    public override void Enter()
    {
        ragdoll.ToggleRagdoll(true);
        stateMachine.StartCoroutine(RecoverFromRagdoll());
    }

    private IEnumerator RecoverFromRagdoll()
    {
        yield return new WaitForSeconds(ragdollTime);
        ragdoll.ToggleRagdoll(false);
        stateMachine.Animator.CrossFadeInFixedTime(StandHash, CrossFadeDuration);
        yield return new WaitForSeconds(standUpTime); 
        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
