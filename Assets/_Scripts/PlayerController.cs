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
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private float MoveSpeed = 10f;

    private LevelController _levelController;
    
    private InputActions _inputActions;
    private Vector2Int _currentPosition;
    private int _currentColor;
    private bool Moving => _moveTween.IsActive();
    private bool _failed;

    private MaterialPropertyBlock _materialPropertyBlock;
    private Sequence _moveTween;

    public void Init(LevelController level)
    {
        _levelController = level;
        
        _inputActions = new();
        _inputActions.Player.Enable();
        _inputActions.Player.MoveVertical.performed += OnMoveVerticalInputPerformed;
        _inputActions.Player.MoveHorizontal.performed += OnMoveHorizontalInputPerformed;

        _materialPropertyBlock = new();
        Renderer.GetPropertyBlock(_materialPropertyBlock);
    }
    
    private void Move(Vector2 direction)
    {
        if (_levelController.CanMove(_currentPosition,Vector2Int.CeilToInt(direction)))
        {
            var tiles = _levelController.GetFloorTilesInWay(_currentPosition, Vector2Int.CeilToInt(direction));
            _moveTween = DOTween.Sequence();
            
            for (int i = 0; i < tiles.Count; i++)
            {
                var floorTile = tiles[i];

                var tween = transform.DOMove(floorTile.transform.position, 1 / MoveSpeed);
                _moveTween.Append(tween);
                _moveTween.AppendCallback(() =>
                {

                    if (floorTile.HasColorPickup())
                    {
                        floorTile.SetEmptyMode();
                        SetColor(floorTile.GetPickupColor());
                    }
                    bool fail = floorTile.Color != 0 && floorTile.Color != _currentColor;
                    
                    if (floorTile.Color == 0)
                        floorTile.SetColor(_currentColor);
                    else if(fail)
                    {
                        Fail();
                    }
                });
            }

            _moveTween.SetEase(Ease.InOutSine);
            _moveTween.AppendCallback(() =>
            {
                _currentPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            });
        }
    }

    private void Fail()
    {
        _failed = true;
        _moveTween.Kill();
        gameObject.SetActive(false);
    }

    private void OnMoveVerticalInputPerformed(InputAction.CallbackContext context)
    {
        if(Moving || _failed) return;
        var value = context.ReadValue<float>();
        Move(value > 0 ? Vector2.up : Vector2.down);
    }

    private void OnMoveHorizontalInputPerformed(InputAction.CallbackContext context)
    {
        if(Moving || _failed) return;
        var value = context.ReadValue<float>();
        Move(value > 0 ? Vector2.right : Vector2.left);
    }

    public void Setup(Vector2Int position, int colorId)
    {
        _currentPosition = position;
        transform.position = new Vector3(position.x,0,position.y);
        _failed = false;
        gameObject.SetActive(true);
        SetColor(colorId);
    }

    private void SetColor(int colorId)
    {
        var color = ColorManager.Instance.GetColorById(colorId);
        _currentColor = colorId;  
        _materialPropertyBlock.SetColor("_Color", color);
        Renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
