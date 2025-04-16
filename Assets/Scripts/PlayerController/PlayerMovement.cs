using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -10f;

    public event Action OnJump; 

    private CharacterController _controller;
    private Vector3 _velocity;

    private IInputService _input;

    [Inject]
    private void Init(IInputService input) 
        => _input = input;

    private void Awake() 
        => _controller = GetComponent<CharacterController>();

    private void Update() 
        => HandleMovement();

    private void HandleMovement()
    {
        Vector2 moveInput = _input.MoveAxis;
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);

        ApplyGravity();
        HandleJump();

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;

        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }

    private void HandleJump()
    {
        if (_input.IsJumpPressed && _groundCheck.IsGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            OnJump?.Invoke();
        }
    }
}