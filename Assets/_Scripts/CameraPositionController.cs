using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    private void Awake()
    {
        GameEvents.NewLevelLoad.AddListener(OnLevelLoad);
    }

    private void OnLevelLoad(LevelData levelData)
    {
        transform.localPosition = new Vector3(levelData.ColumnCount / 2f, 0, levelData.RowCount / 2f);
    }
}
