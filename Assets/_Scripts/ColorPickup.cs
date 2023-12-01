using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickup : MonoBehaviour
{
    [SerializeField] private MeshRenderer Renderer;

    public int CurrentColor { get; private set; }
    private MaterialPropertyBlock _materialPropertyBlock;
    

    private void Awake()
    {
        _materialPropertyBlock = new();
        Renderer.GetPropertyBlock(_materialPropertyBlock);
    }

    public void SetColor(int colorId)
    {
        var color = ColorManager.Instance.GetColorById(colorId);
        CurrentColor = colorId;
        _materialPropertyBlock.SetColor("_Color",color);
        Renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
