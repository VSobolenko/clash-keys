using UnityEngine;

namespace ClashKeys.Game.Fighting
{
internal interface IDeadlyObstacle
{
}

internal interface IBulletSource
{
    Transform Source { get; set; }
    Transform transform { get; }
}
}