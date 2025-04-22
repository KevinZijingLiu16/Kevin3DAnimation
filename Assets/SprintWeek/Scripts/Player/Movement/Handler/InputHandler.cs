using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : IInputHandler
{
    private PlayerInput playerInput;
    public Vector2 MovementInput { get; private set; }
    public bool IsMovementPressed => MovementInput != Vector2.zero;
    public bool IsRunPressed { get; private set; }
    public bool IsHadougenPressed { get; private set; }
    public bool IsInkAttackPressed { get; private set; }

    public InputHandler()
    {
        playerInput = new PlayerInput();

        playerInput.CharacterControl.Move.performed += OnMove;
        playerInput.CharacterControl.Move.canceled += OnMove;

        playerInput.CharacterControl.Run.performed += ctx => IsRunPressed = ctx.ReadValueAsButton();
        playerInput.CharacterControl.Run.canceled += ctx => IsRunPressed = ctx.ReadValueAsButton();

        playerInput.CharacterControl.Hadougen.performed += ctx => IsHadougenPressed = ctx.ReadValueAsButton();
        playerInput.CharacterControl.Hadougen.canceled += ctx => IsHadougenPressed = false;

        playerInput.CharacterControl.InkAttack.performed += ctx => IsInkAttackPressed = ctx.ReadValueAsButton();
        playerInput.CharacterControl.InkAttack.canceled += ctx => IsInkAttackPressed = false;
    }

    public void Enable() => playerInput.Enable();
    public void Disable() => playerInput.Disable();

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
    }
}
