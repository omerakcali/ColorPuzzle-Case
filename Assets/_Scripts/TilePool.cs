using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class TilePool : MonoBehaviour
{
    [SerializeField] private FloorTile FloorTilePrefab;
    [SerializeField] private Tile WallTilePrefab;
    [SerializeField] private int InitPoolSize = 5;
    
    private Stack<FloorTile> _floorTilePool = new();
    private Stack<Tile> _wallTilePool = new();


    private void Awake()
    {
        for (int i = 0; i < InitPoolSize; i++)
        {
            var floorTile = Instantiate(FloorTilePrefab,transform);
            floorTile.gameObject.SetActive(false);
            _floorTilePool.Push(floorTile);

            var wallTile = Instantiate(WallTilePrefab, transform);
            wallTile.gameObject.SetActive(false);
            _wallTilePool.Push(wallTile);
        }
    }


    public FloorTile RequestFloorTile()
    {
        if (_floorTilePool.Count == 0)
        {
            var tile = Instantiate(FloorTilePrefab);
            tile.SetColor("Floor");
            return tile;
        }
        else
        {
            var tile = _floorTilePool.Pop();
            tile.gameObject.SetActive(true);
            tile.transform.SetParent(null);
            tile.SetColor("Floor");
            return tile;
        }
    }
    
    public Tile RequestWallTile()
    {
        if (_wallTilePool.Count == 0)
        {
            return Instantiate(WallTilePrefab);
        }
        else
        {
            var tile = _wallTilePool.Pop();
            tile.gameObject.SetActive(true);
            tile.transform.SetParent(null);
            return tile;
        }
    }
    
    public void DisposeFloorTile(FloorTile tile)
    {
        tile.SetColor("Floor");
        tile.gameObject.SetActive(false);
        tile.transform.SetParent(transform);
        _floorTilePool.Push(tile);
    }

    public void DisposeWallTile(Tile tile)
    {
        tile.gameObject.SetActive(false);
        tile.transform.SetParent(transform);
        _wallTilePool.Push(tile);
    }
    
}
