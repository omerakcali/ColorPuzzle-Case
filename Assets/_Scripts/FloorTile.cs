using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : Tile
{
    [SerializeField] private MeshRenderer Renderer;
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
}
