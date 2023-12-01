using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="LevelData")]
public class LevelData : ScriptableObject
{
    public int ColumnCount;
    public List<LevelTileData> Tiles;
    public Vector2Int PlayerPosition;
    public int PlayerStartColor;
}

[Serializable]
public class LevelTileData
{
    public TileType TileType;
    public int TilePickupColor = -1;

    public LevelTileData()
    {
        TileType = TileType.Empty;
        TilePickupColor = -1;
    }
}
    
public enum TileType
{
    Empty,
    Wall,
    Floor
}