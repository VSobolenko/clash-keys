using ClashKeys.DI.VContainerDependency;
using ClashKeys.Game;
using ClashKeys.Game.Core;
using ClashKeys.Game.Fighting;
using ClashKeys.Game.PlayerComponents;
using ClashKeys.UI;
using Game.AssetContent;
using Game.Factories;
using Game.GUI;
using Game.GUI.Windows;
using Game.GUI.Windows.Factories;
using Game.Pools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ClashKeys.DI
{
internal class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Player _player;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterManagers(builder);
        RegisterManagersUI(builder);
        RegisterSceneComponents(builder);
        RegisterGameElements(builder);
        RegisterGameDirectors(builder);

        builder.RegisterEntryPoint<GameEntryPoint>();
    }

    private void RegisterManagers(IContainerBuilder builder)
    {
        builder.Register<IObjectPoolManager>(resolver =>
        {
            var gameObjectFactory = resolver.Resolve<IFactoryGameObjects>();
            var parent = new GameObject();
            var objectPool = ObjectPoolInstaller.Key(gameObjectFactory, parent.transform, 8);

            return objectPool;
        }, Lifetime.Singleton);
    }

    private void RegisterManagersUI(IContainerBuilder builder)
    {
        builder.Register<VContainerWindowMediatorInstantiator>(Lifetime.Singleton).As<IMediatorInstantiator>().AsSelf();
        builder.Register<IWindowsManager>(resolver =>
        {
            var gameObjectFactory = resolver.Resolve<IFactoryGameObjects>();
            var resourceManager = resolver.Resolve<IResourceManager>();
            var mediatorInstantiator = resolver.Resolve<IMediatorInstantiator>();

            var windowFactory = GuiInstaller.WindowFactory(mediatorInstantiator, resourceManager, gameObjectFactory);

            return GuiInstaller.ManagerAsync(windowFactory, GuiInstaller.Empty(), null);
        }, Lifetime.Transient);
    }

    private void RegisterSceneComponents(IContainerBuilder builder)
    {
        builder.RegisterInstance<Player>(_player);
    }

    private void RegisterGameElements(IContainerBuilder builder)
    {
        builder.Register<FSMCore>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<BulletController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
    }

    private void RegisterGameDirectors(IContainerBuilder builder)
    {
        builder.Register<WindowDirector>(Lifetime.Singleton).AsSelf();
    }
}
}