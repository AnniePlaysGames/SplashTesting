using UnityEngine;

public class GroundPlacementStrategy : IPlacementStrategy
{
    private readonly LayerMask _groundMask = LayerMask.GetMask("Ground");
    private readonly LayerMask _obstacleMask = ~LayerMask.GetMask("Ignore Raycast");

    public PlacementResult CalculatePlacement(PlacementContext context)
    {
        Ray ray = context.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, _groundMask))
            return Fallback(context);

        float distanceToHit = Vector3.Distance(context.Camera.transform.position, hit.point);
        if (distanceToHit > context.MaxBuildDistance)
            return Fallback(context);

        if (Physics.Raycast(context.Camera.transform.position, ray.direction, out RaycastHit obstructionHit, distanceToHit, _obstacleMask))
        {
            bool isGround = (_groundMask.value & (1 << obstructionHit.collider.gameObject.layer)) != 0;
            if (!isGround)
                return Fallback(context);
        }

        if (context.SlopeModule != null && !context.SlopeModule.IsSlopeAllowed(hit.normal))
        {
            return Fallback(context); 
        }

        Vector3 anchorOffset = context.Preview.transform.position - context.Preview.Anchor.transform.position;

        Quaternion rotation = Quaternion.Euler(0f, context.RotationY, 0f);
        if (context.SlopeModule != null)
        {
            rotation = context.SlopeModule.GetRotation(hit.normal, context.RotationY);
        }

        Vector3 position = hit.point + anchorOffset;

        return new PlacementResult
        {
            Position = position,
            Rotation = rotation,
            IsValid = true
        };
    }

    private PlacementResult Fallback(PlacementContext context)
    {
        Vector3 fallbackPosition = context.Camera.transform.position + context.Camera.transform.forward * context.FallbackOffset;

        return new PlacementResult
        {
            Position = fallbackPosition,
            Rotation = Quaternion.Euler(0f, context.RotationY, 0f),
            IsValid = false
        };
    }
}