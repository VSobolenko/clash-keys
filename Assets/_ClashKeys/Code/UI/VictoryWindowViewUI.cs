using System;
using Game.GUI.Windows;
using UnityEngine.UI;

namespace ClashKeys.UI
{
internal sealed class VictoryWindowViewUI : WindowUI
{
    public Button restartButton;
}

internal sealed class VictoryWindowMediatorUI : BaseMediator<VictoryWindowViewUI>
{
    public event Action OnClickRestart;

    public VictoryWindowMediatorUI(VictoryWindowViewUI window) : base(window)
    {
    }

    public override void OnInitialize() => window.restartButton.onClick.AddListener(ProcessClickRestart);

    public override void OnDestroy() => window.restartButton.onClick.RemoveListener(ProcessClickRestart);

    private void ProcessClickRestart() => OnClickRestart?.Invoke();
}
}