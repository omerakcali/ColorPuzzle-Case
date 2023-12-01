using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : Tile
{
    [SerializeField] private MeshRenderer Renderer;

    [SerializeField] private ColorPickup ColorPickup;
    public int Color => _currentColor;

    private int _currentColor;
    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        _materialPropertyBlock = new();
        Renderer.GetPropertyBlock(_materialPropertyBlock);
    }

    public void SetColor(int newColorId)
    {
        var color = ColorManager.Instance.GetColorById(newColorId);
        _currentColor = newColorId;
        
        _materialPropertyBlock.SetColor("_Color",color);
        Renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    public override void SetTile(LevelTileData tileData)
    {
        if(tileData.TilePickupColor == -1) SetEmptyMode();
        else SetColorPickupMode(tileData.TilePickupColor);
    }

    public void SetEmptyMode()
    {
        ColorPickup.gameObject.SetActive(false);
    }
    
    public void SetColorPickupMode(int pickupColorId)
    {
        ColorPickup.gameObject.SetActive(true);
        ColorPickup.SetColor(pickupColorId);
    }

    public bool HasColorPickup() => ColorPickup.gameObject.activeInHierarchy;

    public int GetPickupColor() => ColorPickup.CurrentColor;
}
