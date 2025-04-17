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

    public bool Validate(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f, _checkMask);
        return colliders.Length == 0;
    }
}