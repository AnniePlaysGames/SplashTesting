using UnityEngine;

public class BuildingState : IState
{
    public void Enter()
    {
        Debug.Log("����� ����� � ��������� �������������");
    }

    public void Exit()
    {
        Debug.Log("����� ����� �� ��������� �������������");
    }
}
