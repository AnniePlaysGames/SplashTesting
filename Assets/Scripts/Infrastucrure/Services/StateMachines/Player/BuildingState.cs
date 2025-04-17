using System;
using UnityEngine;
using Zenject;

public class BuildingState : IPayloadedState<BuildingData>, ITickable
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

    public void Enter(BuildingData data)
    {
        Debug.Log("����� ����� � ��������� �������������");
        _placementLogic.StartPreview(data);
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
        if (_inputService.InteractPressed && _placementLogic.Valid)
        {
            _placementLogic.Place();
            OnStopBuilding?.Invoke();
        }
    }
}