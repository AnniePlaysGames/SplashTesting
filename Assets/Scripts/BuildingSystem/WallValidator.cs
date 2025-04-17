using UnityEngine;

public partial class PlacementLogic
{
    public class WallValidator : IPlacementValidator
    {
        private readonly LayerMask _wallMask;

        public WallValidator()
        {
            _wallMask = LayerMask.GetMask("Wall");
        }

        public bool Validate(Vector3 position, Quaternion rotation, Vector3 size)
        {
            Vector3 halfExtents = size * 0.5f;
            Collider[] colliders = Physics.OverlapBox(position, halfExtents, rotation, _wallMask);

            // Здесь можно проверять, чтобы касался стены
            return colliders.Length > 0;
        }
    }
}