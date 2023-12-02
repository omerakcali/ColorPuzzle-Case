using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelProgressManager : MonoBehaviour
{
    private int _floorTileCount;
    private int _currentColoredFloorTileCount;

    private void Awake()
    {
        GameEvents.TileColored.AddListener(OnTileColored);
        GameEvents.NewLevelLoad.AddListener(OnNewLevelLoaded);
    }

    public void OnNewLevelLoaded(LevelData levelData)
    {
        int floorTileCount = 0;
        foreach (var tile in levelData.Tiles)
        {
            if (tile.TileType == TileType.Floor) floorTileCount++;
        }

        _floorTileCount = floorTileCount;
        _currentColoredFloorTileCount = 0;
    }

    public void OnTileColored(FloorTile tile)
    {
        _currentColoredFloorTileCount++;
        
        Debug.Log(GetCurrentProgress());
        if (_currentColoredFloorTileCount == _floorTileCount) CompleteLevel();
    }

    private void CompleteLevel()
    {
        
    }

    public float GetCurrentProgress()
    {
        return (float)_currentColoredFloorTileCount / _floorTileCount;
    }
}

public class GameEvents
{
    public static UnityEvent<LevelData> NewLevelLoad = new();
    public static UnityEvent<FloorTile> TileColored = new();
}