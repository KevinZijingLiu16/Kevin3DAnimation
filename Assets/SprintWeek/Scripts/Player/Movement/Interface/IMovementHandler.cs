using UnityEngine;

public interface IMovementHandler
{
    Vector3 CalculateMovement(Vector2 input, bool isRunning);
    void Move(Vector3 movement);

}
