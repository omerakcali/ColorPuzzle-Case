using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private FloorTile FloorTilePrefab;
    [SerializeField] private Tile WallTilePrefab;
    [SerializeField] private PlayerController Player;
    [SerializeField] private LevelData TestLevelData;

    private Tile[,] _currentLevel;

    private void Awake()
    {
        Player.Init(this);
        
        LoadLevel(TestLevelData);
    }

    private void LoadLevel(LevelData level)
    {
        int row = 0;
        int column = 0;

        int rowCount = level.Tiles.Count / level.ColumnCount;

        _currentLevel = new Tile[rowCount, level.ColumnCount];
        for (int i = 0; i < level.Tiles.Count ; i++)
        {
            if (row == rowCount)
            {
                row = 0;
                column++;
            }
            SpawnTile(row, column, level.Tiles[i]);
            row++;
        }
        Player.Setup(level.PlayerPosition, level.PlayerStartColor);
        var playerTile = _currentLevel[level.PlayerPosition.x, level.PlayerPosition.y] as FloorTile;
        playerTile.SetColor(level.PlayerStartColor);
    }

    private void SpawnTile(int row, int column, LevelTileData levelTileData)
    {
        Tile pickedPrefab = null;
        switch (levelTileData.TileType)
        {
            case TileType.Floor:
                pickedPrefab = FloorTilePrefab;
                break;
            case TileType.Empty:
                pickedPrefab = null;
                break;
            case TileType.Wall:
                pickedPrefab = WallTilePrefab;
                break;
        }
        
        if(pickedPrefab == null) return;
        var tile = Instantiate(pickedPrefab, transform);
        tile.transform.position = new Vector3(row, 0, column);
        _currentLevel[row, column] = tile;

        tile.SetTile(levelTileData);
    }

    public bool CanMove(Vector2Int position, Vector2Int direction)
    {
        try
        {
            var nextTile = _currentLevel[position.x + direction.x, position.y + direction.y];
            return nextTile != null && nextTile is FloorTile;
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
    }

    public List<FloorTile> GetFloorTilesInWay(Vector2Int startPosition, Vector2Int direction)
    {
        var tiles = new List<FloorTile>();

        var position = startPosition;
        Tile nextTile = _currentLevel[position.x + direction.x, position.y + direction.y];
        while (nextTile is FloorTile nextFloorTile)
        {
            tiles.Add(nextFloorTile);
            position += direction;
            nextTile =_currentLevel[position.x + direction.x, position.y + direction.y];
        }

        return tiles;
    }
}
