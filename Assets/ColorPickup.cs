using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickup : MonoBehaviour
{
    [SerializeField] private MeshRenderer Renderer;

    public Color CurrentColor { get; private set; }
    private MaterialPropertyBlock _materialPropertyBlock;
    

    private void Awake()
    {
        _materialPropertyBlock = new();
        Renderer.GetPropertyBlock(_materialPropertyBlock);
    }

    public void SetColor(Color color)
    {
        CurrentColor = color;
        _materialPropertyBlock.SetColor("_Color",color);
        Renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
