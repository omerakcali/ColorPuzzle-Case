using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameColorsData : ScriptableObject
{
    public List<GameColorData> Colors;

    public GameColorData GetById(int id)
    {
        foreach (var colorData in Colors)
        {
            if (colorData.Id == id) return colorData;
        }

        return null;
    }
    
    public GameColorData GetByName(string name)
    {
        foreach (var colorData in Colors)
        {
            if (colorData.Name == name) return colorData;
        }

        return null;
    }
}

[Serializable]
public class GameColorData
{
    public string Name;
    public int Id;
    public Color Color;
}
