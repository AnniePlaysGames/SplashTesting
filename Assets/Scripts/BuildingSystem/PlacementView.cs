using UnityEngine;

public class PlacementView : MonoBehaviour
{
    [SerializeField] private PlacementLogic placementLogic;
    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _invalidMaterial;
    private Renderer _previewInstance;

    private void OnEnable()
    {
        placementLogic.OnPlacementChanged += Attach;
        placementLogic.OnValidate += SetValidState;
    }

    private void OnDisable()
    {
        placementLogic.OnPlacementChanged -= Attach;
        placementLogic.OnValidate -= SetValidState;
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
