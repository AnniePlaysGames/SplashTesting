using System;
using UnityEngine;

[ExecuteInEditMode]
public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float _groundCheckDistance = 0.15f;
    [SerializeField] private LayerMask _groundMask = Physics.DefaultRaycastLayers;

    private const float OriginOffset = 0.001f;

    public bool IsGrounded { get; private set; }
    public event Action OnGrounded;

    private Vector3 RaycastOrigin => transform.position + Vector3.up * OriginOffset;
    private float RaycastDistance => _groundCheckDistance + OriginOffset;

    private void LateUpdate() 
        => PerformGroundCheck();

    private void PerformGroundCheck()
    {
        bool wasGrounded = IsGrounded;

        IsGrounded = Physics.Raycast(
            RaycastOrigin,
            Vector3.down,
            out _,
            RaycastDistance,
            _groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (IsGrounded && !wasGrounded)
        {
            OnGrounded?.Invoke();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color gizmoColor = IsGrounded ? Color.white : Color.red;
        Debug.DrawLine(RaycastOrigin, RaycastOrigin + Vector3.down * RaycastDistance, gizmoColor);
    }
#endif
}