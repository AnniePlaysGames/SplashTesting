using UnityEngine;

public interface IBuildingFactory
{
    Building CreatePreview(BuildingData data, Vector3 position);
    BuildingSource PlaceBuilding(BuildingData data, Vector3 position, Quaternion rotation);
}