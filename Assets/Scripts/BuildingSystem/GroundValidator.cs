using UnityEngine;

public class GroundValidator : IPlacementValidator
{
    private readonly LayerMask _checkMask;
    private const string GroundLayerName = "Ground";
    private const string IgnoreRaycastLayerName = "Ignore Raycast";

    public GroundValidator()
    {
        _checkMask = ~LayerMask.GetMask(GroundLayerName, IgnoreRaycastLayerName);
    }

    public bool Validate(Vector3 position, Quaternion rotation, Vector3 size)
    {
        Vector3 halfExtents = size * 0.5f;
        Collider[] colliders = Physics.OverlapBox(position, halfExtents, rotation, _checkMask, QueryTriggerInteraction.Ignore);
        return colliders.Length == 0;
    }
}
