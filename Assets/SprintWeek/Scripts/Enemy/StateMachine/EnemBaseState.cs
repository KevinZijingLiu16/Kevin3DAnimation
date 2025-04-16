using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    protected EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    protected bool IsInChaseRange()
    {
       Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;

        float playerDistanceSquared = toPlayer.sqrMagnitude;

        return playerDistanceSquared <= stateMachine.DetectPlayerRange * stateMachine.DetectPlayerRange;
    }
    //Method overloding Move()
    //First one is for when we do not want to pass the direction (motion), then it is Vector3.zero. Which means we are not subject to any motion except force from other sources.For example, when we are in idle state, we do not want to move the character controller, but we want to apply the force from other sources like gravity or pushing from player's attack. So we pass Vector3.zero as the motion vector.
    protected void Move(float deltaTime)
    { 
       Move(Vector3.zero, deltaTime);
    }
    //Second one is the core. To achieve the movement logic, we need to pass the motion vector. For example this methed will be called when chasing the player, move to player, with a specific direction.
    protected void Move(Vector3 motion, float deltaTime)
    {
        //CharacterController.Move is just a built in method to move the character controller. It takes a vector3 as a parameter, which is the direction we want to move in.So dont mess with it.
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
    protected void FacePlayer()
    { 
      if(stateMachine.Player == null)
        {
            return;
        }
       Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
       
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);

    }

}
