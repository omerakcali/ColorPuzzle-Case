using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class ColorManager : MonoBehaviour
{
    [SerializeField] private GameColorsData ColorsData;

    public static ColorManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Color GetColorById(int id) => ColorsData.GetById(id).Color;

    public Color GetColorByName(string name) => ColorsData.GetByName(name).Color;
}
