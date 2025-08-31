using System;
using Game.GUI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace ClashKeys.UI
{
internal sealed class GameWindowViewUI : WindowUI
{
    [Space, SerializeField] public Button forwardButton;
    [SerializeField] public Button leftButton;
    [SerializeField] public Button rightButton;
    [SerializeField] public Button downButton;
}

internal sealed class GameWindowMediatorUI : BaseMediator<GameWindowViewUI>
{
    public event Action OnClickToMoveUp;
    public event Action OnClickToMoveLeft;
    public event Action OnClickToMoveRight;
    public event Action OnClickToMoveDown;

    public GameWindowMediatorUI(GameWindowViewUI window) : base(window)
    {
    }

    public override void OnInitialize()
    {
        window.forwardButton.onClick.AddListener(ProcessClickToMoveUp);
        window.leftButton.onClick.AddListener(ProcessClickToMoveLeft);
        window.rightButton.onClick.AddListener(ProcessClickToMoveRight);
        window.downButton.onClick.AddListener(ProcessClickToMoveDown);
    }

    public override void OnDestroy()
    {
        window.forwardButton.onClick.RemoveListener(ProcessClickToMoveUp);
        window.leftButton.onClick.RemoveListener(ProcessClickToMoveLeft);
        window.rightButton.onClick.RemoveListener(ProcessClickToMoveRight);
        window.downButton.onClick.RemoveListener(ProcessClickToMoveDown);
    }

    private void ProcessClickToMoveUp() => OnClickToMoveUp?.Invoke();
    private void ProcessClickToMoveLeft() => OnClickToMoveLeft?.Invoke();
    private void ProcessClickToMoveRight() => OnClickToMoveRight?.Invoke();
    private void ProcessClickToMoveDown() => OnClickToMoveDown?.Invoke();
}
}