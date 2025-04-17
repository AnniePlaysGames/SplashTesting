using UnityEngine;

public interface IPlacementValidator
{
    bool Validate(Vector3 position, Quaternion rotation, Vector3 size);
}