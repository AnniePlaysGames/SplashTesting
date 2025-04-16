using UnityEngine;

public interface IInputService
{
    Vector2 MoveAxis { get; }
    Vector2 LookDelta { get; }
    bool IsJumpPressed { get; }

    void EnableInput();
    void DisableInput();
}
