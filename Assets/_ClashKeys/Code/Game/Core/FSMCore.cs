using System;
using System.Collections.Generic;
using ClashKeys.DI.VContainerDependency;
using ClashKeys.Game.Core.States;
using Game.FSMCore.Machines;
using Game.FSMCore.States;
using VContainer;
using VContainer.Unity;

namespace ClashKeys.Game.Core
{
internal class FSMCore : ITickable, IDisposable
{
    private readonly IObjectResolver _resolver;
    private readonly LiteStateMachine _fsm;

    public FSMCore(IObjectResolver resolver)
    {
        _resolver = resolver;
        _fsm = new LiteStateMachine().EnableLogger();
    }

    public FSMCore Initialize()
    {
        var states = new HashSet<IState>
        {
            _resolver.Instantiate<EntryPointState>(Lifetime.Scoped),
            _resolver.Instantiate<ObstacleCourseState>(Lifetime.Scoped),
            _resolver.Instantiate<EnemyFightingState>(Lifetime.Scoped),
            _resolver.Instantiate<ChestState>(Lifetime.Scoped),
            _resolver.Instantiate<VictoryState>(Lifetime.Scoped),
            _resolver.Instantiate<DefeatState>(Lifetime.Scoped),
        };

        foreach (var state in states)
            _fsm.AddState(state);

        return this;
    }

    public void Entry() => _fsm.TransitTo<EntryPointState>();
    public void ObstacleCourse() => _fsm.TransitTo<ObstacleCourseState>();
    public void EnemyFighting(EnemyFightingStateArgs stateArgs) => _fsm.TransitTo<EnemyFightingState, EnemyFightingStateArgs>(stateArgs);
    public void Chest(ChestStateArgs args) => _fsm.TransitTo<ChestState, ChestStateArgs>(args);
    public void Victory() => _fsm.TransitTo<VictoryState>();
    public void Defeat(DefeatStateArgs args) => _fsm.TransitTo<DefeatState, DefeatStateArgs>(args);

    public void Tick() => _fsm?.Update();

    public void Dispose() => _fsm.StopMachine();
}
}