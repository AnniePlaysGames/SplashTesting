using System;
using UnityEngine;
using Zenject;

public class InputService : IInputService, IInitializable, IDisposable, ITickable
{
    private PlayerInputActions _inputActions;
    public Vector2 MoveAxis => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookDelta => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool IsJumpPressed => _inputActions.Player.Jump.WasPressedThisFrame();
    public bool InteractPressed => _inputActions.Player.Interact.WasPressedThisFrame();
    public bool CancelPressed => _inputActions.Player.Cancel.WasPressedThisFrame();
    public bool ExitPressed => _inputActions.Player.Exit.WasPressedThisFrame(); 
    public float ScrollDelta => _inputActions.Player.Scroll.ReadValue<float>();
    public bool CursorLocked { get; private set; }

    public void Initialize()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    public void Tick()
    {
        if (ExitPressed)
        {
            if (CursorLocked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }

        if (Application.isFocused && !CursorLocked && InteractPressed)
        {
            LockCursor();
        }
    }

    public void EnableInput() 
        => _inputActions?.Enable();

    public void DisableInput() 
        => _inputActions?.Disable();

    public void Dispose() 
        => _inputActions?.Dispose();

    public void LockCursor()
    {
        CursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        CursorLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
