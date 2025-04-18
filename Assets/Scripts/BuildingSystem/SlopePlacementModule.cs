using UnityEngine;

public class SlopePlacementModule : MonoBehaviour
{
    [SerializeField, Range(0f, 90f)] private float _maxSlopeAngle = 45f;

    public bool IsSlopeAllowed(Vector3 normal)
    {
        float slopeAngle = Vector3.Angle(normal, Vector3.up);
        return slopeAngle <= _maxSlopeAngle;
    }

    public Quaternion GetRotation(Vector3 normal, float yRotation)
    {
        return Quaternion.FromToRotation(Vector3.up, normal) * Quaternion.Euler(0, yRotation, 0);
    }
}