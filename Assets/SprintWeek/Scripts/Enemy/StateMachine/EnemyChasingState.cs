using System;
using UnityEditorInternal;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;
    private const float dampTime = 0.1f;
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
   

    }



    public override void Tick(float deltaTime)
    {

        if (!IsInChaseRange())
        {
            stateMachine.ClearPerception();
            stateMachine.SwitchState(new EnemyIdleState(stateMachine,true));
            Debug.Log("Enemy in Idle State");
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
            Debug.Log("Enemy in Attack State");
            return;
        }
        MoveToPlayer(deltaTime);
        FacePlayer();


        stateMachine.Animator.SetFloat(SpeedHash, 1f, dampTime, deltaTime);
        stateMachine.sightVisulizer.gameObject.SetActive(false);
    }

   

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }
    private void MoveToPlayer(float deltaTime)
    {
        stateMachine.Agent.destination = stateMachine.Player.transform.position;

        Move(stateMachine.Agent.desiredVelocity * stateMachine.MovementSpeed, deltaTime);

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    private bool IsInAttackRange()
    {
        Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;
        float playerDistanceSquared = toPlayer.sqrMagnitude;
        return playerDistanceSquared <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
