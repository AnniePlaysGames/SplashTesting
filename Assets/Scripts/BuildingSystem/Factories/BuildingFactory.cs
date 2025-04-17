using UnityEngine;

public class BuildingFactory : IBuildingFactory
{
    public Building CreatePreview(BuildingData data)
    {
        Building preview = Object.Instantiate(data.PreviewPrefab);
        preview.gameObject.SetActive(true);
        return preview;
    }

    public Building PlaceBuilding(BuildingData data, Vector3 position, Quaternion rotation)
    {
        Building placed = Object.Instantiate(data.BuildingPrefab, position, rotation);
        return placed;
    }
}