using System;
using UnityEngine;
using Zenject;

public class PlacementLogic : MonoBehaviour
{
    [SerializeField] private float _movementSmoothness = 1f;
    [SerializeField] private float _fallbackOffset = 2f;
    [SerializeField] private float _maxBuildDistance = 5f;

    private float _rotationY;
    private Building _previewInstance;
    private BoxCollider _mainCollider;
    private BuildingData _currentData;
    private SlopePlacementModule _slopeModule;

    private IPlacementStrategy _strategy;
    private IPlacementValidator _validator;
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
        _slopeModule = _previewInstance.GetComponent<SlopePlacementModule>();

        _strategy = CreateStrategy(_previewInstance.PlacementType);
        _validator = CreateValidator(_previewInstance.PlacementType);

        Vector3 toCamera = Camera.main.transform.position - _previewInstance.transform.position;
        _rotationY = Quaternion.LookRotation(toCamera, Vector3.up).eulerAngles.z;

        OnPlacementChanged?.Invoke(_previewInstance.gameObject);
    }

    public void StopPreview()
    {
        if (_previewInstance == null) return;

        Destroy(_previewInstance.gameObject);
        _previewInstance = null;
        _mainCollider = null;
        _slopeModule = null;

        OnPlacementChanged?.Invoke(null);
    }

    public void Place()
    {
        if (!Valid) return;

        _buildingFactory.PlaceBuilding(
            _currentData,
            _previewInstance.transform.position,
            _previewInstance.transform.rotation
        );
    }

    private void Update()
    {
        if (_previewInstance == null || _mainCollider == null || _strategy == null || _validator == null)
            return;

        HandleRotation();

        PlacementContext context = new PlacementContext
        {
            Camera = Camera.main,
            RotationY = _rotationY,
            Preview = _previewInstance,
            Collider = _mainCollider,
            MaxBuildDistance = _maxBuildDistance,
            FallbackOffset = _fallbackOffset,
            SlopeModule = _slopeModule
        };

        PlacementResult result = _strategy.CalculatePlacement(context);

        _previewInstance.transform.position = SmoothMove(_previewInstance.transform.position, result.Position);
        _previewInstance.transform.rotation = result.Rotation;

        Vector3 worldCenter = result.Position + result.Rotation * _mainCollider.center;
        Vector3 worldSize = Vector3.Scale(_mainCollider.size, _previewInstance.transform.lossyScale);

        Valid = result.IsValid && _validator.Validate(worldCenter, result.Rotation, worldSize);
        OnValidate?.Invoke(Valid);
    }

    private void HandleRotation()
    {
        if (Mathf.Abs(_input.ScrollDelta) > 0.1f)
        {
            float step = 45f * Mathf.Sign(_input.ScrollDelta);
            _rotationY += step;
        }
    }

    private Vector3 SmoothMove(Vector3 from, Vector3 to)
    {
        float t = 1f - Mathf.Exp(-Time.deltaTime / _movementSmoothness);
        return Vector3.Lerp(from, to, t);
    }

    private IPlacementStrategy CreateStrategy(PlacementType type) => type switch
    {
        PlacementType.Ground => new GroundPlacementStrategy(),
        PlacementType.Wall => new WallPlacementStrategy(),
        _ => throw new ArgumentOutOfRangeException()
    };

    private IPlacementValidator CreateValidator(PlacementType type) => type switch
    {
        PlacementType.Ground => new GroundValidator(),
        PlacementType.Wall => new WallValidator(),
        _ => throw new ArgumentOutOfRangeException()
    };
}