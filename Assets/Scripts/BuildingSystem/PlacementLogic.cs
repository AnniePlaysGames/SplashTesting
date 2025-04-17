using System;
using UnityEngine;
using Zenject;

public class PlacementLogic : MonoBehaviour
{
    [SerializeField] private Building _prefab;
    [SerializeField] private float _movementSmoothness = 1f;
    [SerializeField] private float _fallbackOffset = 2f;
    [SerializeField] private float _maxBuildDistance = 5f;

    private Building _previewInstance;
    private IPlacementValidator _validator;
    private IPlacementStrategy _strategy;

    public event Action<GameObject> OnPlacementChanged;
    public event Action<bool> OnValidate;

    [Inject]
    private void Init(IPlacementValidator validator, IInputService input)
    {
        _validator = validator;
    }

    private void Update()
    {
        UpdatePreview();
    }

    public void StartPreview()
    {
        _previewInstance = Instantiate(_prefab);

        if (_previewInstance.Anchor == null)
        {
            Debug.LogError("Anchor не назначен в Building-префабе!", _previewInstance);
        }

        _strategy = CreateStrategyFor(_previewInstance.PlacementType);

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

    private void UpdatePreview()
    {
        if (_previewInstance == null || _strategy == null) return;

        Vector3 targetPosition;
        bool valid;

        if (_strategy.TryGetPlacementPoint(out Vector3 point))
        {
            float distance = Vector3.Distance(Camera.main.transform.position, point);
            if (distance <= _maxBuildDistance)
            {
                Vector3 anchorOffset = _previewInstance.transform.position - _previewInstance.Anchor.transform.position;
                targetPosition = point + anchorOffset;

                valid = _validator.Validate(targetPosition);
            }
            else
            {
                targetPosition = GetFallbackPosition();
                valid = false;
            }
        }
        else
        {
            targetPosition = GetFallbackPosition();
            valid = false;
        }

        float t = 1f - Mathf.Exp(-Time.deltaTime / _movementSmoothness);
        _previewInstance.transform.position = Vector3.Lerp(_previewInstance.transform.position, targetPosition, t);

        OnValidate?.Invoke(valid);
    }

    private Vector3 GetFallbackPosition()
    {
        Bounds bounds = _previewInstance.GetComponentInChildren<Renderer>().bounds;
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

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (Camera.main == null) return;

        Vector3 camPos = Camera.main.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(camPos, _maxBuildDistance);
#endif
    }
}