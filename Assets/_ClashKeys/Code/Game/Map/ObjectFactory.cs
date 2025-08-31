using Game.AssetContent;
using Game.Extensions;
using Game.Pools;
using UnityEngine;

namespace ClashKeys.Game.Map
{
internal interface IObstacleCourseFactory
{
    ObstacleCourseItem Spawn();
    void DeSpawn(ObstacleCourseItem item);
}

internal abstract class ObstacleCourseItem : MonoPooledObject
{
}

internal interface IObstacleCourseDaa
{
}

internal abstract class ConfigurableObstacleCourseItem<T> : ObstacleCourseItem where T : IObstacleCourseDaa
{
    public abstract void UpdateData(T data);
}

internal class ObjectsStackSFactory : IObstacleCourseFactory
{
    private readonly string[] _itemKeys;
    private readonly IObjectPoolManager _objectPool;
    private readonly IResourceManager _prefabFactory;

    public ObjectsStackSFactory(string[] itemKeys, IObjectPoolManager objectPool, IResourceManager prefabFactory)
    {
        _itemKeys = itemKeys;
        _objectPool = objectPool;
        _prefabFactory = prefabFactory;

        PrepareItems();
    }

    private void PrepareItems()
    {
        foreach (var itemKey in _itemKeys)
        {
            var prefab = _prefabFactory.LoadAsset<GameObject>(itemKey).GetComponent<ObstacleCourseItem>();
            _objectPool.Prepare(prefab, 5);
        }
    }

    public ObstacleCourseItem Spawn()
    {
        var key = _itemKeys.Random();
        var prefab = _prefabFactory.LoadAsset<GameObject>(key).GetComponent<ObstacleCourseItem>();
        var instance = _objectPool.Get(prefab);

        return instance;
    }

    public void DeSpawn(ObstacleCourseItem instance) => _objectPool.Release(instance);
}

internal class ChameleonFactory<T> : IObstacleCourseFactory where T : IObstacleCourseDaa
{
    private readonly string _itemKey;
    private readonly T[] _data;
    private readonly IObjectPoolManager _objectPool;
    private readonly IResourceManager _prefabFactory;

    public ChameleonFactory(IObjectPoolManager objectPool,
                            IResourceManager prefabFactory,
                            string itemKey,
                            T[] data)
    {
        _itemKey = itemKey;
        _data = data;
        _objectPool = objectPool;
        _prefabFactory = prefabFactory;
        PrepareItems();
    }

    private void PrepareItems()
    {
        var prefab = _prefabFactory.LoadAsset<GameObject>(_itemKey).GetComponent<ObstacleCourseItem>();
        _objectPool.Prepare(prefab, 5);
    }

    public ObstacleCourseItem Spawn()
    {
        var prefab = _prefabFactory.LoadAsset<GameObject>(_itemKey).GetComponent<ConfigurableObstacleCourseItem<T>>();
        var instance = _objectPool.Get(prefab);
        var randomData = _data.Random();

        instance.UpdateData(randomData);

        return instance;
    }

    public void DeSpawn(ObstacleCourseItem instance) => _objectPool.Release(instance);
}
}