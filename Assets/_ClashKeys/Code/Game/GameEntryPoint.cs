using ClashKeys.Game.Core;
using VContainer.Unity;

namespace ClashKeys.Game
{
internal class GameEntryPoint : IStartable
{
    private readonly FSMCore _fsm;

    public GameEntryPoint(FSMCore fsm)
    {
        _fsm = fsm;
    }

    public void Start()
    {
        _fsm.Initialize();
        _fsm.Entry();
    }
}
}