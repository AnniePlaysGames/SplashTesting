using UnityEngine;

public class DefaultState : IState
{
    public void Enter()
    {
        Debug.Log("Игрок зашёл в стандартное состояние");
    }

    public void Exit()
    {
        Debug.Log("Игрок вышел из стандартного состояния");
    }
}
