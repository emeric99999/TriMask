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

    public KeyCode _switchKey; //touche utilisée pour changer de mode 
    public int _modeID = 0; // id du mode actuel (0 : double jamp, 1 : invisibilité, 2 : rayon X
    public SpriteRenderer uiSprite;

    private void Update()
    {
        Move();
        if (_modeID == 0) { } //doublejump 
        else if (_modeID == 1) { } //invisibility 
        else { } //detection

        setModeID();
        
       

            }

    private void setModeID()   // fonction qui change le _modeID lorsque la touche switch est pressée
    {
        if (Input.GetKeyDown(_switchKey))
        { _modeID += 1;
            _modeID = _modeID % 3;
            if (_modeID == 0) { uiSprite.color = Color.white; }
            else if (_modeID == 1) {  uiSprite.color = Color.red; }
            else { uiSprite.color = Color.green; }
        }
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