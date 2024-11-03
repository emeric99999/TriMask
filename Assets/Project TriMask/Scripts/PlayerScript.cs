using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private string _playerName;

    [SerializeField] private float _playerSpeed = 6f;

    private Vector2 _direction;

    [SerializeField] private InputActionReference _moveactions;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = (Vector2)transform.position + _direction * _playerSpeed * Time.deltaTime; // * direction 
    }

    private void OnMoveAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _direction = ctx.ReadValue<Vector2>();
        }

        if (ctx.canceled)
        {
            _direction = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        _moveactions.action.performed += OnMoveAction;
        _moveactions.action.canceled += OnMoveAction;
    }

    private void OnDisable()
    {
        _moveactions.action.performed -= OnMoveAction;
        _moveactions.action.canceled -= OnMoveAction;
    }



}