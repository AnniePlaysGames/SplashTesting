using System;
using UnityEngine;
using Zenject;

public class InputService : IInputService, IInitializable, IDisposable
{
    private PlayerInputActions _inputActions;
    public Vector2 MoveAxis => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookDelta => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool IsJumpPressed => _inputActions.Player.Jump.WasPressedThisFrame();
    public bool InteractPressed => _inputActions.Player.Interact.WasPressedThisFrame();
    public bool CancelPressed => _inputActions.Player.Cancel.WasPressedThisFrame();

    public void Initialize()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    public void EnableInput() 
        => _inputActions?.Enable();

    public void DisableInput() 
        => _inputActions?.Disable();

    public void Dispose() 
        => _inputActions?.Dispose();
}
