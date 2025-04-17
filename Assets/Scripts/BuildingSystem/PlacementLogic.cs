using System;
using UnityEngine;
using Zenject;

public partial class PlacementLogic : MonoBehaviour
{
    [SerializeField] private float _movementSmoothness = 1f;
    [SerializeField] private float _fallbackOffset = 2f;
    [SerializeField] private float _maxBuildDistance = 5f;
    [SerializeField] private float _rotationStep = 45f;

    private float _rotationY;
    private Building _previewInstance;
    private BoxCollider _mainCollider;
    private BuildingData _currentData;
    private bool _isBlocked;

    private IPlacementValidator _validator;
    private IPlacementStrategy _strategy;
    private IInputService _input;
    private IBuildingFactory _buildingFactory;

    public bool Valid { get; private set; }

    public event Action<GameObject> OnPlacementChanged;
    public event Action<bool> OnValidate;

    [Inject]
    private void Init(IInputService input, IBuildingFactory factory)
    {
        _input = input;
        _buildingFactory = factory;
    }

    public void StartPreview(BuildingData data)
    {
        _currentData = data;
        _previewInstance = _buildingFactory.CreatePreview(data);
        _mainCollider = _previewInstance.GetComponent<BoxCollider>();
        _strategy = CreateStrategyFor(data.PlacementType);
        _validator = CreateValidatorFor(data.PlacementType);

        OnPlacementChanged?.Invoke(_previewInstance.gameObject);
    }

    public void StopPreview()
    {
        if (_previewInstance != null)
        {
            Destroy(_previewInstance.gameObject);
            _previewInstance = null;
            OnPlacementChanged?.Invoke(null);
        }
    }

    public void Place()
    {
        if (_previewInstance == null || _mainCollider == null) return;

        Vector3 position = _previewInstance.transform.position;
        Quaternion rotation = _previewInstance.transform.rotation;

        _buildingFactory.PlaceBuilding(_currentData, position, rotation);
    }

    private void Update()
    {
        if (_previewInstance == null || _strategy == null || _mainCollider == null) return;

        Vector3 targetPosition;
        _isBlocked = false;
        HandleRotation();

        if (_strategy.TryGetPlacementPoint(out Vector3 point))
        {
            float distance = Vector3.Distance(Camera.main.transform.position, point);
            if (distance <= _maxBuildDistance)
            {
                Vector3 anchorOffset = _previewInstance.transform.position - _previewInstance.Anchor.transform.position;
                targetPosition = point + anchorOffset;

                Vector3 dirToPoint = targetPosition - Camera.main.transform.position;
                float dirDistance = dirToPoint.magnitude;
                Ray ray = new Ray(Camera.main.transform.position, dirToPoint.normalized);

                if (Physics.Raycast(ray, out RaycastHit hit, dirDistance, ~LayerMask.GetMask("Ignore Raycast")))
                {
                    if (Vector3.Distance(hit.point, targetPosition) > 0.1f)
                    {
                        targetPosition = GetFallbackPosition();
                    }
                }
            }
            else
            {
                targetPosition = GetFallbackPosition();
            }
        }
        else
        {
            targetPosition = GetFallbackPosition();
        }

        float t = 1f - Mathf.Exp(-Time.deltaTime / _movementSmoothness);
        _previewInstance.transform.position = Vector3.Lerp(_previewInstance.transform.position, targetPosition, t);
        _previewInstance.transform.rotation = Quaternion.Euler(0, _rotationY, 0);

        Vector3 worldCenter = _previewInstance.transform.TransformPoint(_mainCollider.center);
        Vector3 worldSize = Vector3.Scale(_mainCollider.size, _previewInstance.transform.lossyScale);

        Valid = !_isBlocked && _validator.Validate(worldCenter, _previewInstance.transform.rotation, worldSize);

        OnValidate?.Invoke(Valid);
    }

    private void HandleRotation()
    {
        if (_previewInstance == null)
            return;

        float rotationInput = _input.ScrollDelta;
        if (Mathf.Abs(rotationInput) > 0.1f)
        {
            float step = _rotationStep * Mathf.Sign(rotationInput);
            _rotationY += step;
        }
    }

    private Vector3 GetFallbackPosition()
    {
        _isBlocked = true;

        Bounds bounds = _mainCollider.bounds;
        Vector3 centerOffset = bounds.center - _previewInstance.transform.position;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Vector3 fallbackTarget = ray.origin + ray.direction * _fallbackOffset;

        return fallbackTarget - centerOffset;
    }

    private IPlacementStrategy CreateStrategyFor(PlacementType type)
    {
        return type switch
        {
            PlacementType.Ground => new GroundPlacementStrategy(),
            PlacementType.Wall => new WallPlacementStrategy(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private IPlacementValidator CreateValidatorFor(PlacementType type)
    {
        return type switch
        {
            PlacementType.Ground => new GroundValidator(),
            PlacementType.Wall => new WallValidator(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}