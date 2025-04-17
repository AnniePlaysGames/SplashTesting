using UnityEngine;

public interface IBuildingFactory
{
    Building CreatePreview(BuildingData data);
    Building PlaceBuilding(BuildingData data, Vector3 position, Quaternion rotation);
}