using UnityEngine;

public interface IPlacementStrategy
{
    bool TryGetPlacementPoint(out Vector3 point);
}