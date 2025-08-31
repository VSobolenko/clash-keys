using ClashKeys.Game.Map;
using UnityEngine;

namespace ClashKeys.Game.PlayerComponents
{
internal class PlayerGridMover
{
    private GridMap _grid;
    private Transform _player;

    public PlayerGridMover(GridMap grid, Transform player)
    {
        _grid = grid;
        _player = player;
    }

    public void UpdateGrid(GridMap gridMap) => _grid = gridMap;

    public bool CanMoveForward(out CellData target) => CanMoveTo(Vector2Int.up, out target);
    public bool CanMoveLeft(out CellData target) => CanMoveTo(Vector2Int.left, out target);
    public bool CanMoveRight(out CellData target) => CanMoveTo(Vector2Int.right, out target);
    public bool CanMoveBack(out CellData target) => CanMoveTo(Vector2Int.down, out target);

    private bool CanMoveTo(Vector2Int direction, out CellData target)
    {
        target = default;

        if (_grid.TryGetCellFromWorld(_player.position, out var activeCell) == false)
            return false;

        if (activeCell.Type != CellType.Empty)
            return false;

        var cellIndexes = new Vector2Int(activeCell.X + direction.x, activeCell.Y + direction.y);

        if (_grid.InBounds(cellIndexes.x, cellIndexes.y) == false)
            return false;

        target = _grid[cellIndexes.x, cellIndexes.y];

        return true;
    }
}
}