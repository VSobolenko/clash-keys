using UnityEngine;

namespace ClashKeys.Game.Map
{
public struct CellData
{
    public int X { get; }
    public int Y { get; }
    public CellType Type { get; set; }
    public Vector3 WorldPosition { get; set; }

    public CellData(int x, int y)
    {
        this = new CellData();
        X = x;
        Y = y;
    }
}

public enum CellType
{
    Empty,
    Obstacle
}

internal class GridMap
{
    private readonly CellData[,] _grid;
    private readonly Vector2 _cellSize;

    public int Wight => _grid.GetLength(0);
    public int Height => _grid.GetLength(1);

    public GridMap(Vector2Int gridSize, Vector2 cellSize)
    {
        _grid = new CellData[gridSize.x, gridSize.y];
        _cellSize = cellSize;
        FillDefaultGrid();
    }

    public CellData this[int x, int y]
    {
        get => _grid[x, y];
        set => _grid[x, y] = value;
    }

    public bool InBounds(int x, int y) => x >= 0 && x < Wight && y >= 0 && y < Height;
    public bool InBounds(CellData data) => InBounds(data.X, data.Y);

    public Vector3 GetCellWorldPosition(int x, int y)
    {
        var cellSizeX = x * _cellSize.x + _cellSize.x / 2f;
        var cellSizeZ = y * _cellSize.y + _cellSize.y / 2f;

        return new Vector3(cellSizeX, 0, cellSizeZ);
    }

    public bool TryGetCellFromWorld(Vector3 position, out CellData data)
    {
        var x = Mathf.FloorToInt(position.x / _cellSize.x);
        var y = Mathf.FloorToInt(position.z / _cellSize.y);
        var inBounds = InBounds(x, y);

        data = inBounds ? _grid[x, y] : default;

        return inBounds;
    }

    public Vector3 GetRowWorldCenter(int y)
    {
        if (y < 0 || y >= Height)
            throw new System.ArgumentOutOfRangeException(nameof(y));

        var left = _grid[0, y].WorldPosition;
        var right = _grid[Wight - 1, y].WorldPosition;

        return new Vector3(
            (left.x + right.x) / 2f,
            0,
            left.z
        );
    }

    private void FillDefaultGrid()
    {
        for (var x = 0; x < Wight; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _grid[x, y] = new CellData(x, y)
                {
                    Type = CellType.Empty,
                    WorldPosition = GetCellWorldPosition(x, y)
                };
            }
        }
    }
}
}