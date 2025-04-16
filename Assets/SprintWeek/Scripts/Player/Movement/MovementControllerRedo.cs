using UnityEngine;

public class MovementControllerRedo : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;
    public float runMultiplier = 3f;
    public float rotationSpeed = 10f;

    private IInputHandler inputHandler;
    private IGravityHandler gravityHandler;
    private IMovementHandler movementHandler;
    private IRotationHandler rotationHandler;
    private IAnimationHandler animationHandler;
    private IAttackHandler attackHandler;

    private Vector3 movementVector;
    private CharacterController controller;
    private Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        attackHandler = GetComponent<IAttackHandler>();

        inputHandler = new InputHandler();
        gravityHandler = new GravityHandler();
        movementHandler = new MovementHandler(controller, moveSpeed, runMultiplier);
        rotationHandler = new RotationHandler(transform, rotationSpeed);
        animationHandler = new AnimationHandler(animator);
       
    }

    private void OnEnable() => inputHandler.Enable();
    private void OnDisable() => inputHandler.Disable();

    private void Update()
    {
       if(controller == null || animator == null)
        {
           
            return;
        }
        movementVector = movementHandler.CalculateMovement(inputHandler.MovementInput, inputHandler.IsRunPressed);
        movementVector = gravityHandler.ApplyGravity(movementVector, controller.isGrounded);

        movementHandler.Move(movementVector);
        rotationHandler.Rotate(new Vector3(movementVector.x, 0f, movementVector.z));
        animationHandler.UpdateAnimations(inputHandler.IsMovementPressed, inputHandler.IsRunPressed);

        if (inputHandler.IsHadougenPressed)
        {
            animationHandler.PlayHadougen();
            
        }
    }
}
