using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="LevelData")]
public class LevelData : ScriptableObject
{
    public int ColumnCount;
    public List<LevelTileData> Tiles;
    public Vector2Int PlayerPosition;
    public Color PlayerStartColor;
}

[Serializable]
public class LevelTileData
{
    public TileType TileType;
    public Color TilePickupColor = Color.clear;
}
    
public enum TileType
{
    Empty,
    Wall,
    Floor
}