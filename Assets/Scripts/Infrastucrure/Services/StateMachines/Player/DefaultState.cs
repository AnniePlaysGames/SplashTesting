using UnityEngine;

public class DefaultState : IState
{
    public void Enter()
    {
        Debug.Log("����� ����� � ����������� ���������");
    }

    public void Exit()
    {
        Debug.Log("����� ����� �� ������������ ���������");
    }
}
