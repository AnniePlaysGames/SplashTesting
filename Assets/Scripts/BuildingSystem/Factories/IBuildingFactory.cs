using UnityEngine;

public interface IBuildingFactory
{
    Building CreatePreview(BuildingData data);
    BuildingSource PlaceBuilding(BuildingData data, Vector3 position, Quaternion rotation);
}