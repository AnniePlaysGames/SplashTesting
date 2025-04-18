using UnityEngine;

public class PlacementContext
{
    public Camera Camera;
    public Building Preview;
    public BoxCollider Collider;
    public float RotationY;
    public float MaxBuildDistance;
    public float FallbackOffset;
    public SlopePlacementModule SlopeModule;
}