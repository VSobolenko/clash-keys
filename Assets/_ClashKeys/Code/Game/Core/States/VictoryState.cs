using ClashKeys.UI;
using Game.FSMCore;
using Game.FSMCore.States;
using UnityEngine.SceneManagement;

namespace ClashKeys.Game.Core.States
{
internal class VictoryState : IQuiteState
{
    private readonly WindowDirector _windowDirector;

    public VictoryState(WindowDirector windowDirector)
    {
        _windowDirector = windowDirector;
    }

    void IQuiteState.ActivateState(IStateMachine machine)
    {
        var window = _windowDirector.OpenVictoryWindow();
        window.OnClickRestart += RestartLevel;
    }

    public void Finish()
    {
        var window = _windowDirector.GetWindow<VictoryWindowMediatorUI>();
        window.OnClickRestart -= RestartLevel;

        _windowDirector.CloseWindow<VictoryWindowMediatorUI>();
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