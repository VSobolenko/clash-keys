using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ClashKeys.DI.VContainerDependency
{
public static class VContainerExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Instantiate<T>(this IObjectResolver resolver, Lifetime lifetime = Lifetime.Singleton)
    {
        var registrationBuilder = new RegistrationBuilder(typeof(T), lifetime);
        var registration = registrationBuilder.Build();

        return (T) resolver.Resolve(registration);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Instantiate<T>(this IObjectResolver resolver, Lifetime lifetime = Lifetime.Singleton,
                                   params object[] args)
    {
        var registrationBuilder = new RegistrationBuilder(typeof(T), lifetime);

        if (args is {Length: > 0})
        {
            for (int i = 0; i < args.Length; i++)
            {
                registrationBuilder.WithParameter(args[i].GetType(), args[i]);
            }
        }

        Registration registration = registrationBuilder.Build();

        return (T) resolver.Resolve(registration);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object Instantiate(this IObjectResolver resolver, Type type, Lifetime lifetime = Lifetime.Singleton,
                                     params object[] args)
    {
        var registrationBuilder = new RegistrationBuilder(type, lifetime);

        if (args is {Length: > 0})
        {
            for (int i = 0; i < args.Length; i++)
            {
                registrationBuilder.WithParameter(args[i].GetType(), args[i]);
            }
        }

        Registration registration = registrationBuilder.Build();

        return resolver.Resolve(registration);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterNonLazy<T>(this IContainerBuilder builder,
                                                         Lifetime lifetime = Lifetime.Singleton)
    {
        return RegisterNonLazy<T>(builder, null, lifetime);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterNonLazy<T>(this IContainerBuilder builder,
                                                         Action<T> executeAfterResolving,
                                                         Lifetime lifetime = Lifetime.Singleton)
    {
        RegistrationBuilder registrationBuilder = builder.Register<T>(lifetime);

        builder.RegisterBuildCallback(container =>
        {
            var result = container.Resolve<T>();
            executeAfterResolving?.Invoke(result);
        });

        return registrationBuilder;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GameObject InstantiateAndInject([NotNull] this IObjectResolver resolver, [NotNull] GameObject prefab,
                                                  Transform parent = null)
    {
        if (prefab == null)
            throw new NullReferenceException(nameof(prefab));

        bool prefabWasActive = prefab.activeSelf;
        prefab.SetActive(false);
        GameObject instance = resolver.Instantiate(prefab, parent);
        prefab.SetActive(prefabWasActive);
        instance.SetActive(prefabWasActive);

        return instance;
    }
}
}