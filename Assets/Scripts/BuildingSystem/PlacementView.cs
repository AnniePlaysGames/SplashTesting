using UnityEngine;
using Zenject;

public class PlacementView : MonoBehaviour
{
    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _invalidMaterial;
    private Renderer _previewInstance;
    private PlacementLogic _placementLogic;

    [Inject]
    private void Init(PlacementLogic placementLogic)
    {
        _placementLogic = placementLogic;
    }

    private void OnEnable()
    {
        _placementLogic.OnPlacementChanged += Attach;
        _placementLogic.OnValidate += SetValidState;
    }

    private void OnDisable()
    {
        _placementLogic.OnPlacementChanged -= Attach;
        _placementLogic.OnValidate -= SetValidState;
    }

    public void Attach(GameObject instance)
    {
        if (instance == null)
        {
            _previewInstance = null;
        }
        else
        {
            _previewInstance = instance.GetComponentInChildren<Renderer>();
        }
    }

    public void SetValidState(bool isValid)
    {
        if (_previewInstance != null)
        {
            _previewInstance.material = isValid ? _validMaterial : _invalidMaterial;
        }
    }
}
