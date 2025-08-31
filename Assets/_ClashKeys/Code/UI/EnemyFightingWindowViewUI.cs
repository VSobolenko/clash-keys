using System;
using Game.GUI.Windows;
using Game.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClashKeys.UI
{
internal sealed class EnemyFightingWindowViewUI : WindowUI
{
    public InputEventHandler leftArrow;
    public InputEventHandler rightArrow;
    public Button attackButton;
}

internal sealed class EnemyFightingWindowMediatorUI : BaseMediator<EnemyFightingWindowViewUI>
{
    public event Action<Vector2Int> OnClickMoveStart;
    public event Action<Vector2Int> OnClickMoveEnd;
    public event Action OnClickAttack;

    public EnemyFightingWindowMediatorUI(EnemyFightingWindowViewUI window) : base(window)
    {
    }

    public override void OnInitialize()
    {
        window.leftArrow.PointerDown += ProcessClickLeftArrowStart;
        window.leftArrow.PointerUp += ProcessClickLeftArrowEnd;

        window.rightArrow.PointerDown += ProcessClickRightArrowStart;
        window.rightArrow.PointerUp += ProcessClickRightArrowEnd;

        window.attackButton.onClick.AddListener(ProcessClickAttack);
    }

    public override void OnDestroy()
    {
        window.leftArrow.PointerDown -= ProcessClickLeftArrowStart;
        window.leftArrow.PointerUp -= ProcessClickLeftArrowEnd;

        window.rightArrow.PointerDown -= ProcessClickRightArrowStart;
        window.rightArrow.PointerUp -= ProcessClickRightArrowEnd;

        window.attackButton.onClick.RemoveListener(ProcessClickAttack);
    }

    private void ProcessClickLeftArrowStart(PointerEventData pointerEventData) =>
        OnClickMoveStart?.Invoke(Vector2Int.left);

    private void ProcessClickLeftArrowEnd(PointerEventData pointerEventData) => OnClickMoveEnd?.Invoke(Vector2Int.left);

    private void ProcessClickRightArrowStart(PointerEventData pointerEventData) =>
        OnClickMoveStart?.Invoke(Vector2Int.right);

    private void ProcessClickRightArrowEnd(PointerEventData pointerEventData) =>
        OnClickMoveEnd?.Invoke(Vector2Int.right);

    private void ProcessClickAttack() => OnClickAttack?.Invoke();
}
}