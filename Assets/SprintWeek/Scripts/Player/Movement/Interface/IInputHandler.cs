using UnityEngine;

public interface IInputHandler
{
    Vector2 MovementInput { get; }
    bool IsMovementPressed { get; }
    bool IsRunPressed { get; }
    bool IsHadougenPressed { get; }

    void Enable();
    void Disable();
}
