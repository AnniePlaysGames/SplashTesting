using System;
using UnityEngine;
using Zenject;

public class PlacementLogic : MonoBehaviour
{
    [SerializeField] private Building _prefab;
    [SerializeField] private float _movementSmoothness = 1f;
    [SerializeField] private float _fallbackOffset = 2f;

    private Building _previewInstance;
    private IPlacementValidator _validator;

    public event Action<GameObject> OnPlacementChanged;
    public event Action<bool> OnValidate;

    [Inject]
    private void Init(IPlacementValidator validator)
    {
        _validator = validator;
    }

    private void Update()
    {
        UpdatePreview();

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartPreview();
        }
    }

    public void StartPreview()
    {
        _previewInstance = Instantiate(_prefab);

        if (_previewInstance.Anchor == null)
        {
            Debug.LogError("Anchor не назначен в Building-префабе!", _previewInstance);
        }

        OnPlacementChanged?.Invoke(_previewInstance.gameObject);
    }

    public void StopPreview()
    {
        if (_previewInstance != null)
        {
            Destroy(_previewInstance);
            _previewInstance = null;
            OnPlacementChanged?.Invoke(_previewInstance.gameObject);
        }
    }

    private void UpdatePreview()
    {
        if (_previewInstance == null || _previewInstance.Anchor == null) return;

        Vector3 targetPosition = CalculatePreviewPosition();

        Vector3 anchorOffset = _previewInstance.transform.position - _previewInstance.Anchor.transform.position;
        Vector3 finalPosition = targetPosition + anchorOffset;

        float t = 1f - Mathf.Exp(-Time.deltaTime / _movementSmoothness);
        _previewInstance.transform.position = Vector3.Lerp(
            _previewInstance.transform.position,
            finalPosition,
            t
        );

        bool canPlace = _validator.Validate(finalPosition);
        OnValidate?.Invoke(canPlace);
    }

    private Vector3 CalculatePreviewPosition()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.point;
        }

        return ray.origin + ray.direction * _fallbackOffset; 
    }
}
