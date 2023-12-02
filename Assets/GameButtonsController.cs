using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonsController : MonoBehaviour
{
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button NextLevelButton;
    [SerializeField] private Button TryAgainButton;
    private void Awake()
    {
        SetStartGameMode();
        GameEvents.LevelCompleted.AddListener(SetNextLevelMode);
        GameEvents.NewLevelLoad.AddListener(OnNewLevelLoad);
        GameEvents.LevelFailed.AddListener(SetFailMode);
    }

    private void HideUI()
    {
        StartGameButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(false);
        TryAgainButton.gameObject.SetActive(false);
    }
    
    private void SetStartGameMode()
    {
        HideUI();
        StartGameButton.gameObject.SetActive(true);
    }

    private void SetNextLevelMode()
    {
        HideUI();
        NextLevelButton.gameObject.SetActive(true);
    }

    private void SetFailMode()
    {
        HideUI();
        TryAgainButton.gameObject.SetActive(true);
    }
    
    private void OnNewLevelLoad(LevelData arg0)
    {
        HideUI();
    }
}
