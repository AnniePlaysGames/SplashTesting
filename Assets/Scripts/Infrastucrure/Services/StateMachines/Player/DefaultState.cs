using UnityEngine;
using Zenject;

public class DefaultState : IState
{
    private PlayerInteractor _interactor;

    [Inject]
    private void Init(PlayerInteractor interactor)
    {
        _interactor = interactor;
    }
    
    public void Enter()
    {
        Debug.Log("����� ����� � ����������� ���������");
        _interactor.enabled = true;
    }

    public void Exit()
    {
        Debug.Log("����� ����� �� ������������ ���������");
    }
}
