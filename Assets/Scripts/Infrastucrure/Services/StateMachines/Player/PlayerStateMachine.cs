using Zenject;

public class PlayerStateMachine : StateMachineBase, IInitializable
{
    private readonly DefaultState _defaultState;
    private readonly BuildingState _buildingState;

    [Inject]
    public PlayerStateMachine(DefaultState defaultState, BuildingState buildingState)
    {
        _defaultState = defaultState;
        _buildingState = buildingState;
    }

    public void Initialize()
    {
        AddState(_defaultState);
        AddState(_buildingState);

        Enter<DefaultState>();
    }
}
