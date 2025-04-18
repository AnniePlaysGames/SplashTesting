using UnityEngine;
using Zenject;

public class BuildingFactory : IBuildingFactory
{
    private readonly DiContainer _container;

    public BuildingFactory(DiContainer container)
    {
        _container = container;
    }

    public Building CreatePreview(BuildingData data)
    {
        Building preview = _container.InstantiatePrefabForComponent<Building>(data.PreviewPrefab);
        preview.gameObject.SetActive(true);
        return preview;
    }

    public BuildingSource PlaceBuilding(BuildingData data, Vector3 position, Quaternion rotation)
    {
        BuildingSource placed = _container.InstantiatePrefabForComponent<BuildingSource>(data.BuildingPrefab, position, rotation, null);
        return placed;
    }
}