using UnityEngine;

public class GroundValidator : IPlacementValidator
{
    private readonly LayerMask _groundMask;
    private const string GroundLayerName = "Ground";

    public GroundValidator()
    {
        _groundMask = LayerMask.GetMask(GroundLayerName);
    }

    public bool Validate(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f, ~_groundMask);
        return colliders.Length == 0;
    }
}