using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        // Например, связываем интерфейс с реализацией
        //Container.Bind<IMyService>().To<MyService>().AsSingle();

        // Или можно привязать компонент
        //Container.Bind<MyPlayer>().FromComponentInHierarchy().AsSingle();
    }
}
