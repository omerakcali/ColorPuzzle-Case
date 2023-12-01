using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    private LevelController _levelController;
    
    private InputActions _inputActions;
    private Vector2Int _currentPosition;
    private bool _moving;


    private void Move(Vector2 direction)
    {
        if (_levelController.CanMove(_currentPosition,Vector2Int.CeilToInt(direction)))
        {
            var tiles = _levelController.GetFloorTilesInWay(_currentPosition, Vector2Int.CeilToInt(direction));

            var lastTile = tiles[^1];
            _moving = true;
            transform.DOMove(lastTile.transform.position, 5f).SetSpeedBased().OnComplete(() =>
            {
                _moving = false;
                _currentPosition = new Vector2Int((int)transform.position.x,(int)transform.position.z);
            });
        }
    }

    public void Init(LevelController level)
    {
        _levelController = level;
        
        _inputActions = new();
        _inputActions.Player.Enable();
        _inputActions.Player.MoveVertical.performed += OnMoveVerticalInputPerformed;
        _inputActions.Player.MoveHorizontal.performed += OnMoveHorizontalInputPerformed;
    }
    
    
    private void OnMoveVerticalInputPerformed(InputAction.CallbackContext context)
    {
        if(_moving) return;
        var value = context.ReadValue<float>();
        Move(value > 0 ? Vector2.up : Vector2.down);
    }

    private void OnMoveHorizontalInputPerformed(InputAction.CallbackContext context)
    {
        if(_moving) return;
        var value = context.ReadValue<float>();
        Move(value > 0 ? Vector2.right : Vector2.left);
    }

    public void Setup(Vector2Int position, Color color)
    {
        _currentPosition = position;
        transform.position = new Vector3(position.x,0,position.y);
    }
}
