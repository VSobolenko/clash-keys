using ClashKeys.Game.Fighting;
using ClashKeys.Game.Map;
using ClashKeys.Game.PlayerComponents;
using ClashKeys.UI;
using Game.AssetContent;
using Game.FSMCore;
using Game.FSMCore.States;
using Game.Pools;
using UnityEngine;

namespace ClashKeys.Game.Core.States
{
internal class ObstacleCourseState : IQuiteState
{
    private readonly FSMCore _fsm;
    private readonly Player _player;
    private readonly IObjectPoolManager _objectPool;
    private readonly IResourceManager _resourceManager;
    private readonly WindowDirector _windowDirector;

    private GridMap _grid;
    private PlayerGridMover _gridMover;
    private Highway[] _highway;

    public ObstacleCourseState(FSMCore fsm, 
                               Player player, 
                               IObjectPoolManager objectPool,
                               IResourceManager resourceManager, 
                               WindowDirector windowDirector)
    {
        _fsm = fsm;
        _player = player;
        _objectPool = objectPool;
        _resourceManager = resourceManager;
        _windowDirector = windowDirector;
    }

    void IQuiteState.ActivateState(IStateMachine machine)
    {
        ConfigureGridPlayer();
        GenerateMap();
        var window = _windowDirector.OpenGameWindow();
        window.OnClickToMoveUp += ProcessMovePlayerUp;
        window.OnClickToMoveLeft += ProcessMovePlayerLeft;
        window.OnClickToMoveRight += ProcessMovePlayerRight;
        window.OnClickToMoveDown += ProcessMovePlayerDown;
        _player.OnEntered += OnPlayerEnterToCollider;
    }

    public void Finish()
    {
        var window = _windowDirector.GetWindow<GameWindowMediatorUI>();
        window.OnClickToMoveUp -= ProcessMovePlayerUp;
        window.OnClickToMoveLeft -= ProcessMovePlayerLeft;
        window.OnClickToMoveRight -= ProcessMovePlayerRight;
        window.OnClickToMoveDown -= ProcessMovePlayerDown;
        _windowDirector.CloseWindow<GameWindowMediatorUI>();
        _player.OnEntered -= OnPlayerEnterToCollider;
    }

    private void OnPlayerEnterToCollider(GameObject collided)
    {
        if (collided.TryGetComponent<IDeadlyObstacle>(out var item) == false)
            return;

        _fsm.Defeat(new DefeatStateArgs
        {
            Reason = "You collided on the road"
        });
    }

    public void UpdateState()
    {
        foreach (var highway in _highway)
        {
            highway.UpdateView(Time.deltaTime);
        }
    }

    private void GenerateMap()
    {
        _highway = new Highway[4];

        IObstacleCourseFactory factory = new ObjectsStackSFactory(new[] {"Item1",}, _objectPool, _resourceManager);

        var settings1 = new Highway.Settings {speed = 1f, direction = Vector2.left};
        var settings2 = new Highway.Settings {speed = 2f, direction = Vector2.right};
        var settings3 = new Highway.Settings {speed = 3f, direction = Vector2.left};
        var settings4 = new Highway.Settings {speed = 4f, direction = Vector2.right};

        var settingsSpawn1 = new SpawnRegulator.Settings {mode = SpawnRegulator.SpawnMode.ConstantFrequency, frequencyPerSecond = 0.3f,};
        var settingsSpawn2 = new SpawnRegulator.Settings {mode = SpawnRegulator.SpawnMode.RandomFrequency, randomFrequencyPerSecond = new Vector2(0.3f, 0.6f),};
        var settingsSpawn3 = new SpawnRegulator.Settings {mode = SpawnRegulator.SpawnMode.ConstantFrequency, frequencyPerSecond = 0.5f,};
        var settingsSpawn4 = new SpawnRegulator.Settings {mode = SpawnRegulator.SpawnMode.ConstantFrequency, frequencyPerSecond = 0.7f,};

        _highway[0] = new Highway(settings1, _grid.GetRowWorldCenter(2), settingsSpawn1, factory);
        _highway[1] = new Highway(settings2, _grid.GetRowWorldCenter(5), settingsSpawn2, factory);
        _highway[2] = new Highway(settings3, _grid.GetRowWorldCenter(6), settingsSpawn3, factory);
        _highway[3] = new Highway(settings4, _grid.GetRowWorldCenter(9), settingsSpawn4, factory);
    }

    private void ConfigureGridPlayer()
    {
        _grid = new GridMap(new Vector2Int(10, 12), Vector2.one);
        _gridMover = new PlayerGridMover(_grid, _player.transform);

#if UNITY_EDITOR
        var debug = new GameObject("[Debug] Grid View").AddComponent<GridGizmosVisualizer>();
        debug.Grid = _grid;
#endif
    }

    private void ProcessMovePlayerUp()
    {
        if (_gridMover.CanMoveForward(out var target) == false)
            return;

        _player.transform.position = target.WorldPosition;

        if (target.Y != _grid.Height - 1)
            return;

        _fsm.EnemyFighting(new EnemyFightingStateArgs
        {
            PivotPoint = new Vector3(0, 0, _grid.Height),
        });
    }

    private void ProcessMovePlayerLeft()
    {
        if (_gridMover.CanMoveLeft(out var target))
            _player.transform.position = target.WorldPosition;
    }

    private void ProcessMovePlayerRight()
    {
        if (_gridMover.CanMoveRight(out var target))
            _player.transform.position = target.WorldPosition;
    }

    private void ProcessMovePlayerDown()
    {
        if (_gridMover.CanMoveBack(out var target))
            _player.transform.position = target.WorldPosition;
    }
}
}