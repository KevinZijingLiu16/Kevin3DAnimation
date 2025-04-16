using UnityEngine;

public class AnimationHandler : IAnimationHandler
{
    private readonly Animator animator;
    private readonly int isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int isRunningHash = Animator.StringToHash("IsRunning");
    private readonly int hadougenHash = Animator.StringToHash("Hadougen");

    public AnimationHandler(Animator animator)
    {
        this.animator = animator;
    }

    public void UpdateAnimations(bool isMoving, bool isRunning)
    {
        SetBool(isWalkingHash, isMoving);
        SetBool(isRunningHash, isMoving && isRunning);
    }

    private void SetBool(int hash, bool value)
    {
        if (animator.GetBool(hash) != value)
            animator.SetBool(hash, value);
    }
    public void PlayHadougen()
    {
        animator.SetTrigger(hadougenHash);
    }

}
