using ClashKeys.UI;
using Game.FSMCore;
using Game.FSMCore.States;
using UnityEngine.SceneManagement;

namespace ClashKeys.Game.Core.States
{
internal class DefeatStateArgs
{
    public string Reason;
}

internal class DefeatState : IActivatedState<DefeatStateArgs>
{
    private readonly FSMCore _fsm;
    private readonly WindowDirector _windowDirector;

    public DefeatState(FSMCore fsm, WindowDirector windowDirector)
    {
        _fsm = fsm;
        _windowDirector = windowDirector;
    }

    void IActivatedState<DefeatStateArgs>.ActivateState(IStateMachine machine, DefeatStateArgs data)
    {
        var window = _windowDirector.OpenDefeatWindow();
        window.SetDefeatReasonText(data.Reason);
        window.OnClickRestart += RestartLevel;
    }

    public void Finish()
    {
        var window = _windowDirector.GetWindow<DefeatWindowMediatorUI>();
        window.OnClickRestart -= RestartLevel;

        _windowDirector.CloseWindow<DefeatWindowMediatorUI>();
    }

    private void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void UpdateState()
    {
    }
}
}