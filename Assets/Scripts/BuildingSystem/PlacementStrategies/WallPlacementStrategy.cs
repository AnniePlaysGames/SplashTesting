using UnityEngine;

public class WallPlacementStrategy : IPlacementStrategy
{
    private readonly LayerMask _wallMask = LayerMask.GetMask("Wall");
    private readonly LayerMask _obstacleMask = ~LayerMask.GetMask("Ignore Raycast");

    public PlacementResult CalculatePlacement(PlacementContext context)
    {
        Ray ray = context.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _wallMask))
        {
            if (Vector3.Distance(context.Camera.transform.position, hit.point) > context.MaxBuildDistance)
                return Fallback(context);

            if (Physics.Raycast(context.Camera.transform.position, hit.point - context.Camera.transform.position,
                    out RaycastHit obstruction, Vector3.Distance(context.Camera.transform.position, hit.point), _obstacleMask))
            {
                if (obstruction.collider.gameObject != hit.collider.gameObject)
                    return Fallback(context);
            }

            Vector3 anchorOffset = context.Preview.transform.position - context.Preview.Anchor.transform.position;
            Vector3 position = hit.point + anchorOffset;

            Quaternion baseRotation = Quaternion.LookRotation(-hit.normal);
            Quaternion finalRotation = baseRotation * Quaternion.Euler(0, 0, context.RotationY);

            return new PlacementResult
            {
                Position = position,
                Rotation = finalRotation,
                IsValid = true
            };
        }

        return Fallback(context);
    }

    private PlacementResult Fallback(PlacementContext context)
    {
        Vector3 fallback = context.Camera.transform.position + context.Camera.transform.forward * context.FallbackOffset;

        return new PlacementResult
        {
            Position = fallback,
            Rotation = Quaternion.Euler(0, 0, context.RotationY),
            IsValid = false
        };
    }
}