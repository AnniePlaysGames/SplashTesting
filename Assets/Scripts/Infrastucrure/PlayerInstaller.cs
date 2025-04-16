using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<DefaultState>().AsSingle();
        Container.Bind<BuildingState>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle();

        Container.BindInterfacesAndSelfTo<GroundValidator>().AsSingle();
    }
}
