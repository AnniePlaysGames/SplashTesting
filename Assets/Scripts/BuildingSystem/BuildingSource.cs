using UnityEngine;
using Zenject;

public class BuildingSource : BaseInteractable
{
    [SerializeField] private BuildingData _data;
    [SerializeField] private bool _isInfinite;

    private PlayerStateMachine _playerStates;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _isPickedUp;

    [Inject]
    public void Init(PlayerStateMachine playerStates)
    {
        _playerStates = playerStates;
    }

    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

    public override void Interact()
    {
        base.Interact();

        if (!_isInfinite)
        {
            HideSource();
        }

        var context = new BuildingContext(_data, OnCancel);
        _playerStates.Enter<BuildingState, IBuildingContext>(context);
    }

    private void HideSource()
    {
        _isPickedUp = true;
        gameObject.SetActive(false);
    }

    private void OnCancel()
    {
        if (_isInfinite || !_isPickedUp)
            return;

        RestoreSource();
    }

    private void RestoreSource()
    {
        transform.SetPositionAndRotation(_initialPosition, _initialRotation);
        gameObject.SetActive(true);
        _isPickedUp = false;
    }
}
