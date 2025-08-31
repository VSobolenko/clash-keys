using ClashKeys.DI.VContainerDependency;
using Game.AssetContent;
using Game.Factories;
using VContainer;
using VContainer.Unity;

namespace ClashKeys.DI
{
internal class BootstrapLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        BindManagers(builder);
    }

    private void BindManagers(IContainerBuilder builder)
    {
        builder.Register<VContainerFactoryGameObjects>(Lifetime.Singleton).As<IFactoryGameObjects>();
        builder.RegisterInstance<IResourceManager>(ResourceManagerInstaller.Addressable());
        builder.Register<IResourceFactoryManager>(resolver =>
        {
            var gameObjectFactory = resolver.Resolve<IFactoryGameObjects>();
            var resourceManager = resolver.Resolve<IResourceManager>();
            var factory = ResourceManagerInstaller.Factory(resourceManager, gameObjectFactory);

            return factory;
        }, Lifetime.Singleton);
    }
}
}