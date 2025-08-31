using System;
using Game.GUI.Windows;
using Game.GUI.Windows.Factories;
using VContainer;

namespace ClashKeys.DI.VContainerDependency
{
internal class VContainerWindowMediatorInstantiator : IMediatorInstantiator
{
    private readonly IObjectResolver _resolver;

    public VContainerWindowMediatorInstantiator(IObjectResolver resolver)
    {
        _resolver = resolver;
    }

    public TMediator Instantiate<TMediator>(WindowUI windowUI, params object[] extraArgs)
        where TMediator : class, IMediator
    {
        var window = _resolver.Instantiate<TMediator>(Lifetime.Transient, windowUI);

        return window;
    }

    public IMediator Instantiate(Type mediatorType, WindowUI windowUI, params object[] extraArgs)
    {
        var window = (IMediator) _resolver.Instantiate(mediatorType, Lifetime.Transient, windowUI);

        return window;
    }
}
}