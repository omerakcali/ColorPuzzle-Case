using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : Tile
{
    [SerializeField] private MeshRenderer Renderer;

    [SerializeField] private ColorPickup ColorPickup;
    public Color Color => _currentColor;

    private Color _currentColor;
    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        _materialPropertyBlock = new();
        Renderer.GetPropertyBlock(_materialPropertyBlock);
    }

    public void SetColor(Color newColor)
    {
        _currentColor = newColor;
        
        _materialPropertyBlock.SetColor("_Color",_currentColor);
        Renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    public override void SetTile(LevelTileData tileData)
    {
        if(tileData.TilePickupColor == Color.clear) SetEmptyMode();
        else SetColorPickupMode(tileData.TilePickupColor);
    }

    public void SetEmptyMode()
    {
        ColorPickup.gameObject.SetActive(false);
    }
    
    public void SetColorPickupMode(Color pickupColor)
    {
        ColorPickup.gameObject.SetActive(true);
        ColorPickup.SetColor(pickupColor);
    }

    public bool HasColorPickup() => ColorPickup.gameObject.activeInHierarchy;

    public Color GetPickupColor() => ColorPickup.CurrentColor;
}
