using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlacementLogic _placementLogic;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GroundValidator>().AsSingle();
        Container.Bind<PlacementLogic>().FromInstance(_placementLogic).AsSingle();

        Container.Bind<DefaultState>().AsSingle();
        Container.Bind<BuildingState>().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle();
    }
}
