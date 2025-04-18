using System;
using Zenject;

public class BuildingState : IPayloadedState<IBuildingContext>, ITickable
{
    private PlacementLogic _placementLogic;
    private IInputService _inputService;
    private PlayerInteractor _interactor;

    private IBuildingContext _context;

    public event Action OnStopBuilding;

    [Inject]
    public void Init(PlacementLogic logic, IInputService inputService, PlayerInteractor interactor)
    {
        _placementLogic = logic;
        _inputService = inputService;
        _interactor = interactor;
    }

    public void Enter(IBuildingContext context)
    {
        _context = context;

        _placementLogic.StartPreview(_context.Data);
        _interactor.enabled = false;
    }

    public void Exit()
    {
        _placementLogic.StopPreview();
        _interactor.enabled = true;
    }

    public void Tick()
    {
        if (_inputService.CancelPressed)
        {
            _context.OnCancel?.Invoke();
            OnStopBuilding?.Invoke();
        }

        if (_inputService.InteractPressed && _placementLogic.Valid)
        {
            _placementLogic.Place();
            OnStopBuilding?.Invoke();
        }
    }
}