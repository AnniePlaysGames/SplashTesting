using UnityEngine;

public class WallPlacementStrategy : IPlacementStrategy
{
    private readonly LayerMask _wallMask = LayerMask.GetMask("Wall");

    public bool TryGetPlacementPoint(out Vector3 point)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _wallMask))
        {
            point = hit.point;
            return true;
        }

        point = default;
        return false;
    }
}