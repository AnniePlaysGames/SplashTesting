using System;
using System.Collections.Generic;

public abstract class StateMachineBase : IStateMachine
{
    private readonly Dictionary<Type, IState> _states = new();
    private IState _activeState;

    public void Enter<TState>() where TState : class, IState
        => ChangeState<TState>().Enter();

    protected void AddState(IState state) 
        => _states[state.GetType()] = state;

    protected TState ChangeState<TState>() where TState : class, IState
    {
        _activeState?.Exit();

        if (!_states.TryGetValue(typeof(TState), out var state))
            throw new Exception($"Состояние {typeof(TState).Name} не найдено!");

        _activeState = state;
        return (TState)state;
    }

    protected TState GetState<TState>() where TState : class, IState
        => _states[typeof(TState)] as TState;
}
