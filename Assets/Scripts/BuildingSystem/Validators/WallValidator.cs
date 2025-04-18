using UnityEngine;

public class WallValidator : IPlacementValidator
{
    private readonly LayerMask _collisionMask;

    public WallValidator()
    {
        _collisionMask = ~LayerMask.GetMask("Wall", "Ignore Raycast");
    }

    public bool Validate(Vector3 position, Quaternion rotation, Vector3 size)
    {
        Vector3 halfExtents = size * 0.5f;
        return Physics.OverlapBox(position, halfExtents, rotation, _collisionMask, QueryTriggerInteraction.Ignore).Length == 0;
    }
}