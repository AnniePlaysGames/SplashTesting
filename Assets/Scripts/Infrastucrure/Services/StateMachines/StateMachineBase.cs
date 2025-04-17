using System;
using System.Collections.Generic;
using Zenject;

public abstract class StateMachineBase : IStateMachine, ITickable
{
    private readonly Dictionary<Type, IExitableState> _states = new();
    private IExitableState _activeState;

    public void Enter<TState>() where TState : class, IState
        => ChangeState<TState>().Enter();

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        => ChangeState<TState>().Enter(payload);

    protected void AddState(IExitableState state)
        => _states[state.GetType()] = state;

    public void Tick()
    {
        if (_activeState is ITickable tickable)
            tickable.Tick();
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
        _activeState?.Exit();

        var state = GetState<TState>();
        _activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState
        => _states[typeof(TState)] as TState;
}