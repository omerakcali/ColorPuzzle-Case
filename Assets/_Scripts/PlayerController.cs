using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    private InputActions InputActions;

    private void Awake()
    {
        InputActions = new();
        InputActions.Player.Enable();
        InputActions.Player.Move.performed += OnMoveInputPerformed;
    }

    private void OnMoveInputPerformed(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        Move(direction);
    }

    private void Move(Vector2 direction)
    {
        
    }
    
    public void Setup(Vector2 position, Color color)
    {
        transform.position = new Vector3(position.x,0,position.y);
    }
}
