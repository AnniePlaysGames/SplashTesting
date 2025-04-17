using System;
using UnityEngine;
using Zenject;

public class BuildingState : IState, ITickable
{
    private PlacementLogic _placementLogic;
    private IInputService _inputService;

    public event Action OnStopBuilding; 

    [Inject]
    public void Init(PlacementLogic logic, IInputService inputService)
    {
        _placementLogic = logic;
        _inputService = inputService;
    }

    public void Enter()
    {
        Debug.Log("����� ����� � ��������� �������������");
        _placementLogic.StartPreview();
    }

    public void Exit()
    {
        Debug.Log("����� ����� �� ��������� �������������");
        _placementLogic.StopPreview();
    }

    public void Tick()
    {
        if (_inputService.CancelPressed)
        {
            OnStopBuilding?.Invoke();
        }
    }
}
