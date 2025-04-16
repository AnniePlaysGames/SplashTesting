using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<Bootstrapper>().AsSingle();
        Container.BindInterfacesTo<InputService>().AsSingle();
    }
}
