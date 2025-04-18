using UnityEngine;

public class GroundPlacementStrategy : IPlacementStrategy
{
    private readonly LayerMask _groundMask = LayerMask.GetMask("Ground");
    private readonly LayerMask _obstacleMask = ~LayerMask.GetMask("Ignore Raycast");

    public PlacementResult CalculatePlacement(PlacementContext context)
    {
        Ray ray = context.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _groundMask))
        {
            float distanceToHit = Vector3.Distance(context.Camera.transform.position, hit.point);
            if (distanceToHit > context.MaxBuildDistance)
                return Fallback(context);

            if (Physics.Raycast(context.Camera.transform.position, ray.direction, out RaycastHit obstructionHit, distanceToHit, _obstacleMask))
            {
                if ((1 << obstructionHit.collider.gameObject.layer & _groundMask) == 0)
                    return Fallback(context);
            }

            Vector3 anchorOffset = context.Preview.transform.position - context.Preview.Anchor.transform.position;
            Quaternion targetRotation = Quaternion.Euler(0, context.RotationY, 0);

            if (context.SlopeModule != null && context.SlopeModule.IsSlopeAllowed(hit.normal))
            {
                targetRotation = context.SlopeModule.GetRotation(hit.normal, context.RotationY);
            }

            Vector3 targetPosition = hit.point + anchorOffset;

            return new PlacementResult
            {
                Position = targetPosition,
                Rotation = targetRotation,
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
            Rotation = Quaternion.Euler(0, context.RotationY, 0),
            IsValid = false
        };
    }
}