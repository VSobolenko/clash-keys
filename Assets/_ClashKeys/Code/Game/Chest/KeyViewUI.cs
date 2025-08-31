using System;
using ClashKeys.Common;
using ClashKeys.UI;
using Game.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClashKeys.Game.Chest
{
[RequireComponent(typeof(RectTransform))]
internal class KeyViewUI : InputEventHandler
{
    [SerializeField] private Image viewImage;

    public RectTransform rectTransform;
    public Vector3 InitialPos { get; private set; }
    public ColorId Id { get; private set; }
    public event Action<KeyViewUI> OnFinishMove;

    private Vector2 _dragOffset;

    private void Start()
    {
        InitialPos = transform.position;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        CachePressedPositionOffset(eventData);
        base.OnPointerDown(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position + _dragOffset;
        base.OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        OnFinishMove?.Invoke(this);
    }

    private void CachePressedPositionOffset(PointerEventData eventData)
    {
        var offset = (Vector2) rectTransform.position - eventData.pressPosition;
        _dragOffset = offset;
    }

    public void SetId(ColorId id)
    {
        Id = id;
        viewImage.color = id.ToColor();
    }

    public void SetInitialPosition() => transform.position = InitialPos;

#if UNITY_EDITOR
    private void Reset()
    {
        rectTransform = GetComponent<RectTransform>();
        viewImage = GetComponentInChildren<Image>(true);
    }
#endif
}
}