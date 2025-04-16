using UnityEngine;

public class RotationHandler : IRotationHandler
{
    private readonly Transform transform;
    private readonly float rotationSpeed;

    public RotationHandler(Transform transform, float rotationSpeed)
    {
        this.transform = transform;
        this.rotationSpeed = rotationSpeed;
    }

    public void Rotate(Vector3 movementDirection)
    {
        if (movementDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
