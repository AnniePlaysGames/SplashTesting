using UnityEngine;

public interface IInputService
{
    Vector2 MoveAxis { get; }
    Vector2 LookDelta { get; }
    bool IsJumpPressed { get; }
    bool InteractPressed { get; }
    bool CancelPressed { get; }
    bool ExitPressed { get; }
    float ScrollDelta { get; }
    bool CursorLocked { get; }

    void EnableInput();
    void DisableInput();
    void LockCursor();
    void UnlockCursor();
}
