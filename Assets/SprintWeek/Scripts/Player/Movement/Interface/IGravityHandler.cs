using UnityEngine;

public interface IGravityHandler
{
    Vector3 ApplyGravity(Vector3 movement, bool isGrounded);
}
