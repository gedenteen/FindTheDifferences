using System.Collections;
using System.Collections.Generic;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SaveManager>().FromNew().AsSingle().NonLazy();
    }
}
