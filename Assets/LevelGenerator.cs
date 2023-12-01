using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private FloorTile FloorTilePrefab;
    [SerializeField] private Tile WallTilePrefab;

    [SerializeField] private LevelData TestLevelData;

    private Tile[,] _currentLevel;

    private void Awake()
    {
        int row = 0;
        int column = 0;

        int rowCount = TestLevelData.Tiles.Count / TestLevelData.ColumnCount;

        _currentLevel = new Tile[rowCount, TestLevelData.ColumnCount];
        for (int i = 0; i < TestLevelData.Tiles.Count ; i++)
        {
            if (row == rowCount)
            {
                row = 0;
                column++;
            }
            SpawnTile(row, column, TestLevelData.Tiles[i].TileType);
            row++;
        }
    }

    private void SpawnTile(int row, int column, TileType tileType)
    {
        Tile pickedPrefab = null;
        switch (tileType)
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
    }
}
