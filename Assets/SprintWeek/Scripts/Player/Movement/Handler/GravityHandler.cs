using UnityEngine;

public class GravityHandler : IGravityHandler
{
    // -0.05 is to make sure when the player is grounded, the player still falls a little bit, to touch the ground without character controller shaking.
    private const float GroundedGravity = -0.05f;
    private const float Gravity = -9.81f;
    private const float fallingAdapter = 50f;

    public Vector3 ApplyGravity(Vector3 movement, bool isGrounded)
    {
        movement.y += isGrounded ? GroundedGravity : Gravity * Time.deltaTime * fallingAdapter ;
        return movement;
    }
}
