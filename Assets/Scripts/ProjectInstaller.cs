using Core.AudioSourcePool;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        GameInstaller.Install(Container);
        
        Container.BindInterfacesAndSelfTo<AudioManager>()
            .FromNewComponentOnNewGameObject()
            .WithGameObjectName("AudioManager")
            .AsSingle()
            .NonLazy();
    }
}