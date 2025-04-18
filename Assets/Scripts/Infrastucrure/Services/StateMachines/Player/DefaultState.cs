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
        _interactor.enabled = true;
    }

    public void Exit()
    {

    }
}
