using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    [SerializeField] private Image ProgressBarImage;
    [SerializeField] private LevelProgressManager LevelProgressManager;

    private Tween _fillTween;

    private void Start()
    {
        GameEvents.NewLevelLoad.AddListener(OnNewLevelLoaded);
        GameEvents.TileColored.AddListener(OnTileColored);
    }

    private void OnTileColored(FloorTile arg0)
    {
        _fillTween?.Kill();
        _fillTween = ProgressBarImage.DOFillAmount(LevelProgressManager.GetCurrentProgress(), .25f);
    }

    private void OnNewLevelLoaded(LevelData arg0)
    {
        _fillTween?.Kill();
        ProgressBarImage.fillAmount = 0f;
    }
}
