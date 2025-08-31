using Game.Pools;
using UnityEngine;

namespace ClashKeys.Game.Fighting
{
internal class BulletView : MonoPooledObject, IBulletSource
{
    public Transform Source { get; set; }
}
}