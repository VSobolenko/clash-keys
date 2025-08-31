using System;
using UnityEngine;

namespace ClashKeys.Game.Fighting
{
internal class EnemyView : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    public event Action<EnemyView, GameObject> OnEntered;

    public SimpleWeapon<BulletView> Weapon { get; private set; }

    private void Update() => Weapon?.Update(Time.deltaTime);

    private void OnTriggerEnter(Collider other) => OnEntered?.Invoke(this, other.gameObject);

    public void SetActiveInteraction(bool value) => _collider.enabled = value;

    public void AttackWeapon(SimpleWeapon<BulletView> weapon) => Weapon = weapon;

    public void SkipWeapon() => Weapon = null;

#if UNITY_EDITOR

    private void Reset()
    {
        _collider = GetComponent<Collider>();
    }
#endif
}
}