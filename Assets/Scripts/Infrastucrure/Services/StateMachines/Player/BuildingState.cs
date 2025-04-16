using UnityEngine;

public class BuildingState : IState
{
    public void Enter()
    {
        Debug.Log("Игрок зашёл в состояние строительства");
    }

    public void Exit()
    {
        Debug.Log("Игрок вышел из состояния строительства");
    }
}
