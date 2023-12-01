using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="LevelData")]
public class LevelData : ScriptableObject
{
    public int ColumnCount;
    public List<LevelTileData> Tiles;
}

[Serializable]
public class LevelTileData
{
    public TileType TileType;
}
    
public enum TileType
{
    Empty,
    Wall,
    Floor
}