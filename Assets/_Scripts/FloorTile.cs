using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : Tile
{
    public Color Color => _currentColor;

    private Color _currentColor;

    public void SetColor(Color newColor)
    {
        _currentColor = newColor;
    }
}
