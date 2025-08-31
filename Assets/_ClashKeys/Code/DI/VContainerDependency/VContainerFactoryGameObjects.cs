using System;
using System.Collections.Generic;
using Game.Factories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ClashKeys.DI.VContainerDependency
{
public class VContainerFactoryGameObjects : IFactoryGameObjects
{
    private readonly IObjectResolver _container;

    private const string NewGameObjectName = "Empty GameObject";

    public VContainerFactoryGameObjects(IObjectResolver container)
    {
        _container = container;
    }

    //Create empty GameObject
    public GameObject InstantiateEmpty() =>
        InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, null);

    public GameObject InstantiateEmpty(Transform parent) =>
        InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, parent);

    public GameObject InstantiateEmpty(Vector3 position, Quaternion rotation, Transform parent) =>
        InternalInstantiateBase(NewGameObjectName, position, rotation, parent);

    public GameObject InstantiateEmpty(params Type[] components) =>
        InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, null, components);

    public GameObject InstantiateEmpty(Transform parent, params Type[] components) =>
        InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, parent, components);

    public GameObject InstantiateEmpty(Vector3 position, Quaternion rotation, Transform parent,
                                       params Type[] components) =>
        InternalInstantiateBase(NewGameObjectName, position, rotation, parent, components);

    public GameObject InstantiateEmpty(string name) =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, null);

    public GameObject InstantiateEmpty(string name, Transform parent) =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, parent);

    public GameObject InstantiateEmpty(string name, Vector3 position, Quaternion rotation, Transform parent) =>
        InternalInstantiateBase(name, position, rotation, parent);

    public GameObject InstantiateEmpty(string name, params Type[] components) =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, null, components);

    public GameObject InstantiateEmpty(string name, Transform parent, params Type[] components) =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, parent, components);

    public GameObject InstantiateEmpty(string name, Vector3 position, Quaternion rotation, Transform parent,
                                       params Type[] components) =>
        InternalInstantiateBase(name, position, rotation, parent, components);

    // Creates a new object from a prefab
    public GameObject InstantiatePrefab(GameObject prefab) =>
        InternalInstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, null);

    public GameObject InstantiatePrefab(GameObject prefab, Transform parent) =>
        InternalInstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, parent);

    public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) =>
        InternalInstantiatePrefab(prefab, position, rotation, parent);

    // Creates a new object from a prefab that already has a component T
    public T Instantiate<T>(T prefab) where T : Component =>
        _container.Instantiate(prefab);

    public T Instantiate<T>(T prefab, IEnumerable<object> extraArgs) where T : Component =>
        InternalInstantiate(prefab, Vector3.zero, Quaternion.identity, null);

    public T Instantiate<T>(T prefab, Transform parent) where T : Component =>
        InternalInstantiate(prefab, Vector3.zero, Quaternion.identity, parent);

    public T Instantiate<T>(T prefab, Transform parent, IEnumerable<object> extraArgs) where T : Component =>
        InternalInstantiate(prefab, Vector3.zero, Quaternion.identity, parent);

    public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component =>
        InternalInstantiate(prefab, position, rotation, parent);

    public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent,
                            IEnumerable<object> extraArgs) where T : Component =>
        InternalInstantiate(prefab, position, rotation, parent);

    // Creates a new object and adds a new component to it
    public T InstantiateAndAddNewComponent<T>() where T : Component =>
        InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, null).AddComponent<T>();

    public T InstantiateAndAddNewComponent<T>(Transform parent) where T : Component
    {
        var component = InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, null)
            .AddComponent<T>();
        component.transform.SetParent(parent);

        return component;
    }

    public T InstantiateAndAddNewComponent<T>(Transform parent, IEnumerable<object> extraArgs) where T : Component =>
        InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, parent)
            .AddComponent<T>();

    public T InstantiateAndAddNewComponent<T>(IEnumerable<object> extraArgs) where T : Component =>
        InternalInstantiateBase(NewGameObjectName, Vector3.zero, Quaternion.identity, null).AddComponent<T>();

    public T InstantiateAndAddNewComponent<T>(string name) where T : Component =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, null).AddComponent<T>();

    public T InstantiateAndAddNewComponent<T>(string name, Transform parent) where T : Component =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, parent)
            .AddComponent<T>();

    public T InstantiateAndAddNewComponent<T>(string name, Transform parent, IEnumerable<object> extraArgs)
        where T : Component =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, parent)
            .AddComponent<T>();

    public T InstantiateAndAddNewComponent<T>(string name, IEnumerable<object> extraArgs)
        where T : Component =>
        InternalInstantiateBase(name, Vector3.zero, Quaternion.identity, null)
            .AddComponent<T>();

    // Add a new component to an object prefab
    public T AddComponent<T>(GameObject gameObject) where T : Component =>
        gameObject.AddComponent<T>();

    public T AddComponent<T>(GameObject gameObject, IEnumerable<object> extraArgs) where T : Component =>
        gameObject.AddComponent<T>();

    private static GameObject InternalInstantiateBase(string name, Vector3 position, Quaternion rotation,
                                                      Transform parent, params Type[] components)
    {
        GameObject gameObject;

        if (components != null && components.Length > 0)
            gameObject = new GameObject(name, components);
        else
            gameObject = new GameObject(name);

        gameObject.transform.SetParent(parent);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;

        return gameObject;
    }

    private T InternalInstantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent)
        where T : Component => _container.Instantiate(prefab, position, rotation, parent);

    private GameObject InternalInstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation,
                                                 Transform parent) =>
        _container.Instantiate(prefab, position, rotation, parent);
}
}