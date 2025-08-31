using Game.FSMCore;
using Game.FSMCore.States;

namespace ClashKeys.Game.Core.States
{
internal class EntryPointState : IQuiteState
{
    private readonly FSMCore _fsm;

    public EntryPointState(FSMCore fsm)
    {
        _fsm = fsm;
    }

    void IQuiteState.ActivateState(IStateMachine machine)
    {
        _fsm.ObstacleCourse();
    }

    public void Finish()
    {
    }

    public void UpdateState()
    {
    }
}
}