using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    PlayerInput playerInput;
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;

    int isWalkingHash;
    int isRunningHash;

    public float moveSpeed = 5f;
    public float runMultiplier = 3f;
    private float rotationFactorPerFrame = 10f;

    CharacterController characterController;
    Animator animator;

    private void Awake()
    {
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInput.CharacterControl.Move.performed += OnMovementInput;
        playerInput.CharacterControl.Move.canceled += OnMovementInput;
        playerInput.CharacterControl.Run.performed += OnRun;
        playerInput.CharacterControl.Run.canceled += OnRun;

    }

    private void OnRun(InputAction.CallbackContext context)
    {
         isRunPressed = context.ReadValueAsButton();
       
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }


    // Update is called once per frame
    void Update()
    {
        HandleGravity();
        HandleRotation();
        HandleAnimation();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime * moveSpeed);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime*moveSpeed);
        }
        
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
    void HandleGravity()
    {
        if (characterController.isGrounded)
        {
        float groundedGravity = -0.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
            
        }
        else
        {
            float gravity = -9.81f;
            currentMovement.y += gravity * Time.deltaTime;
            currentRunMovement.y += gravity * Time.deltaTime;
        }


    }
    void HandleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (isRunPressed && isMovementPressed && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if (( !isMovementPressed || !isRunPressed ) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }


    }
    void HandleRotation()
    {
        Vector3 positionToLookAt;
      

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;
        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame*Time.deltaTime);
        }
       
        


    }
}
