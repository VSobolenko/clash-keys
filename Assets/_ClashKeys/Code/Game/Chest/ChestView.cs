using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClashKeys.Game.Chest
{
internal class ChestView : MonoBehaviour
{
    [SerializeField] private Button worldButtonInteract;

    public event Action<ChestView> OnClickOpenChest;

    private void Start() => worldButtonInteract.onClick.AddListener(ProcessClickOpenChest);

    private void OnDestroy() => worldButtonInteract.onClick.RemoveListener(ProcessClickOpenChest);

    private void ProcessClickOpenChest() => OnClickOpenChest?.Invoke(this);
}
}