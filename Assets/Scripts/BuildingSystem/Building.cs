using UnityEngine;

public class Building : MonoBehaviour
{
    [field: SerializeField] public BuildingAnchor Anchor { get; private set; }
    [field: SerializeField] public PlacementType PlacementType { get; private set; }
}
