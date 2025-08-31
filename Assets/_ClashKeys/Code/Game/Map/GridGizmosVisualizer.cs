using UnityEngine;

namespace ClashKeys.Game.Map
{
internal class GridGizmosVisualizer : MonoBehaviour
{
    public bool enableGizmos = true;
    public GridMap Grid { get; set; }

    private void OnDrawGizmos()
    {
        if (Grid == null || enableGizmos == false)
            return;

        Gizmos.color = Color.white;

        for (var x = 0; x < Grid.Wight; x++)
        {
            for (var y = 0; y < Grid.Height; y++)
            {
                var cell = Grid[x, y];
                var pos = cell.WorldPosition;

                var size = new Vector3(Vector2.one.x, 0.1f, Vector2.one.y);

                Gizmos.color = cell.Type == CellType.Obstacle ? Color.red : Color.green;
                Gizmos.DrawWireCube(pos, size);
            }
        }
    }
}
}