using System;
using Zenject;

public class PlayerStateMachine : StateMachineBase, IInitializable, IDisposable
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

        _buildingState.OnStopBuilding += OnStopBuilding;
    }

    private void OnStopBuilding()
    {
        Enter<DefaultState>();
    }

    public void Dispose()
    {
        _buildingState.OnStopBuilding -= OnStopBuilding;
    }
}
