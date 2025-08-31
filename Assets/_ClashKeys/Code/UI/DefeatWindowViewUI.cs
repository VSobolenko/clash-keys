using System;
using Game.GUI.Windows;
using TMPro;
using UnityEngine.UI;

namespace ClashKeys.UI
{
internal sealed class DefeatWindowViewUI : WindowUI
{
    public TextMeshProUGUI reasonText;
    public Button restartButton;
}

internal sealed class DefeatWindowMediatorUI : BaseMediator<DefeatWindowViewUI>
{
    public event Action OnClickRestart;

    public DefeatWindowMediatorUI(DefeatWindowViewUI window) : base(window)
    {
    }

    public override void OnInitialize() => window.restartButton.onClick.AddListener(ProcessClickRestart);

    public override void OnDestroy() => window.restartButton.onClick.RemoveListener(ProcessClickRestart);

    private void ProcessClickRestart() => OnClickRestart?.Invoke();

    public void SetDefeatReasonText(string text) => window.reasonText.text = text;
}
}