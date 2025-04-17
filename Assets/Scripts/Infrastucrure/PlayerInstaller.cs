using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlacementLogic _placementLogic;
    [SerializeField] private PlayerInteractor _playerInteractor;

    public override void InstallBindings()
    {
        Container.Bind<PlacementLogic>().FromInstance(_placementLogic).AsSingle();
        Container.Bind<PlayerInteractor>().FromInstance(_playerInteractor).AsSingle();

        Container.Bind<IBuildingFactory>().To<BuildingFactory>().AsSingle();

        Container.Bind<DefaultState>().AsSingle();
        Container.Bind<BuildingState>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle();
    }
}