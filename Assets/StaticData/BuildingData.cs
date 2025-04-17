using UnityEngine;

[CreateAssetMenu(menuName = "Building/Data")]
public class BuildingData : ScriptableObject
{
    public Building PreviewPrefab;
    public Building BuildingPrefab;
    public PlacementType PlacementType;
}