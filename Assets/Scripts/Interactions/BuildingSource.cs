using UnityEngine;
using Zenject;

public class BuildingSource : BaseInteractable
{
    [SerializeField] private BuildingData _data;

    private PlayerStateMachine _playerStates;

    [Inject]
    public void Init(PlayerStateMachine playerStates)
    {
        _playerStates = playerStates;
    }

    public override void Interact()
    {
        base.Interact();
        _playerStates.Enter<BuildingState, BuildingData>(_data);
    }
}