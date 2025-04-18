using UnityEngine;
using Zenject;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _sensitivity = 2f;
    [SerializeField] private float _smoothing = 1.5f;

    private Vector2 _velocity;
    private Vector2 _frameVelocity;

    private IInputService _input;

    [Inject]
    private void Init(IInputService input)
        => _input = input;

    private void Start()
    {
        _input.LockCursor();
    }

    private void Update()
    {
        Vector2 lookDelta = _input.LookDelta;
        Vector2 rawFrameVelocity = Vector2.Scale(lookDelta, Vector2.one * _sensitivity * Time.deltaTime);

        _frameVelocity = Vector2.Lerp(_frameVelocity, rawFrameVelocity, 1f / _smoothing);
        _velocity += _frameVelocity;
        _velocity.y = Mathf.Clamp(_velocity.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-_velocity.y, Vector3.right);
        _player.localRotation = Quaternion.AngleAxis(_velocity.x, Vector3.up);
    }
}