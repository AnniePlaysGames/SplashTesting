using Zenject;

public class BuildingSource : BaseInteractable
{
    private PlayerStateMachine _playerStates;

    [Inject]
    public void Init(PlayerStateMachine playerStates)
    {
        _playerStates = playerStates;
    }

    public override void Interact()
    {
        base.Interact();
        _playerStates.Enter<BuildingState>();
    }
}
