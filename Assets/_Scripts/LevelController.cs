using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private TilePool TilePool;
    [SerializeField] private PlayerController Player;
    [SerializeField] private List<LevelData> Levels;

    private int _currentLevelIndex;
    private Tile[,] _currentLevel;

    private void Awake()
    {
        _currentLevelIndex = PlayerPrefs.GetInt("level", 0);
        Player.Init(this);
        GameEvents.LevelCompleted.AddListener(OnLevelWon);
    }

    public void StartNextLevel()
    {
        if(_currentLevel!=null) UnloadCurrentLevel();
        LoadLevel(Levels[_currentLevelIndex]);
    }

    public void OnLevelWon()
    {
        _currentLevelIndex++;
        if (_currentLevelIndex == Levels.Count) _currentLevelIndex = 0;
        PlayerPrefs.SetInt("level",_currentLevelIndex);
    }
    
    private void LoadLevel(LevelData level)
    {
        int rowIndex = 0;
        int columnIndex = 0;


        _currentLevel = new Tile[level.ColumnCount, level.RowCount];
        for (int i = 0; i < level.Tiles.Count ; i++)
        {
            if (columnIndex == level.ColumnCount)
            {
                columnIndex = 0;
                rowIndex++;
            }
            SpawnTile(rowIndex, columnIndex, level.Tiles[i]);
            columnIndex++;
        }
        
        GameEvents.NewLevelLoad.Invoke(level);

        Player.Setup(level.PlayerPosition, level.PlayerStartColor);
        var playerTile = _currentLevel[level.PlayerPosition.x, level.PlayerPosition.y] as FloorTile;
        playerTile.SetColor(level.PlayerStartColor);
    }

    private void UnloadCurrentLevel()
    {
        foreach (var tile in _currentLevel)
        {
            if(tile == null) continue; //empty grid
            if(tile is FloorTile floorTile) TilePool.DisposeFloorTile(floorTile);
            else TilePool.DisposeWallTile(tile);
        }

        _currentLevel = null;
    }

    private void SpawnTile(int row, int column, LevelTileData levelTileData)
    {
        
        if(levelTileData.TileType == TileType.Empty) return;
        var tile = levelTileData.TileType == TileType.Floor ? TilePool.RequestFloorTile() : TilePool.RequestWallTile();
        tile.transform.SetParent(transform);
        tile.transform.position = new Vector3(column, 0, row);
        _currentLevel[column, row] = tile;

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
