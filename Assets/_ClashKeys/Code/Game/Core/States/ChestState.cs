using ClashKeys.Game.Chest;
using ClashKeys.UI;
using Game.AssetContent;
using Game.FSMCore;
using Game.FSMCore.States;
using UnityEngine;

namespace ClashKeys.Game.Core.States
{
internal class ChestStateArgs
{
    public Vector3 chestPosition;
}

internal class ChestState : IActivatedState<ChestStateArgs>
{
    private readonly FSMCore _fsm;
    private readonly WindowDirector _windowDirector;
    private readonly IResourceFactoryManager _resourceFactoryManager;

    public ChestState(FSMCore fsm, WindowDirector windowDirector, IResourceFactoryManager resourceFactoryManager)
    {
        _fsm = fsm;
        _windowDirector = windowDirector;
        _resourceFactoryManager = resourceFactoryManager;
    }

    void IActivatedState<ChestStateArgs>.ActivateState(IStateMachine machine, ChestStateArgs data)
    {
        var chest = _resourceFactoryManager.CreateGameObjectWithComponent<ChestView>(
            "Chest", data.chestPosition, Quaternion.identity, null);
        chest.OnClickOpenChest += ProcessClickChest;
    }

    private void ProcessClickChest(ChestView chest)
    {
        chest.OnClickOpenChest -= ProcessClickChest;
        Object.Destroy(chest.gameObject);

        var window = _windowDirector.OpenChestWindow();
        window.OnLockOpened += ProcessCompleteLockOpen;
    }

    public void Finish()
    {
        SafeCloseWindow();
    }

    private void SafeCloseWindow()
    {
        var window = _windowDirector.GetWindow<ChestWindowMediatorUI>();

        if (window == null)
            return;

        window.OnLockOpened -= ProcessCompleteLockOpen;
        _windowDirector.CloseWindow<ChestWindowMediatorUI>();
    }

    private void ProcessCompleteLockOpen()
    {
        _fsm.Victory();
    }

    public void UpdateState() { }
}
}