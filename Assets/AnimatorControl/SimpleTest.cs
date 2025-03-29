using UnityEngine;

public class SimpleTest : MonoBehaviour
{
    public Animator animator;

    public void SetVelocity(float value)
    {
        animator.SetFloat("Velocity", value);
        Debug.Log("Animator: " + animator.name + ", Velocity: " + animator.GetFloat("Velocity"));
    }
}
