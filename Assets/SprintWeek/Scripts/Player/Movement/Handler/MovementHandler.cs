using UnityEngine;

public class MovementHandler : IMovementHandler
{
    //readonly fields for the controller, move speed, and run multiplier, to ensure they are set once and not modified later, no need to worry about them being modified by other classes
    private readonly CharacterController controller;
    private readonly float moveSpeed;
    private readonly float runMultiplier;

    public MovementHandler(CharacterController controller, float moveSpeed, float runMultiplier)
    {
        this.controller = controller;
        this.moveSpeed = moveSpeed;
        this.runMultiplier = runMultiplier;
    }

    public Vector3 CalculateMovement(Vector2 input, bool isRunning)
    {
        float multiplier = isRunning ? runMultiplier : 1f;
        return new Vector3(input.x, 0f, input.y) * multiplier;
    }

    public void Move(Vector3 movement)
    {
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
}
