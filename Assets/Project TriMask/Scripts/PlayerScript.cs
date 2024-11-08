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
    public int _modeID = 0; // id du mode actuel (0 : double jump, 1 : invisibilité, 2 : rayon X
    [SerializeField] private SpriteRenderer uiSprite;
    [SerializeField] private SpriteRenderer playerSprite;
    private List<Sprite> currentSpriteList;
    [SerializeField] private List<Sprite> doubleJumpSpriteList;
    [SerializeField] private List<Sprite> xraySpriteList;
    [SerializeField] private List<Sprite> stealthSpriteList;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private BoxCollider2D _playerCollider;
    private float timer = 0;
    [SerializeField] private float animTime; 


    public void Start()
    {
        currentSpriteList = doubleJumpSpriteList;
    }
    private void Update()
    {
        Move();
        Jump();
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
            if (_modeID == 0) { uiSprite.color = Color.white; currentSpriteList = doubleJumpSpriteList; }
            else if (_modeID == 1) { uiSprite.color = Color.red; currentSpriteList = xraySpriteList; }
            else { uiSprite.color = Color.green; currentSpriteList = stealthSpriteList; }
        }
    }


    private void Move()
    {
        transform.position = (Vector2)transform.position + _direction * _playerSpeed * Time.deltaTime; // * direction 

        if (_direction != Vector2.zero)
        {
            if (timer < animTime)
            {
                timer += Time.deltaTime;
                if (timer < animTime/4) playerSprite.sprite = currentSpriteList[1];
                else if (timer < animTime/2) playerSprite.sprite = currentSpriteList[2];
                else if (timer < 3*animTime/4) playerSprite.sprite = currentSpriteList[3];
                else playerSprite.sprite = currentSpriteList[4];
            }

            else timer = 0;
        }

        else playerSprite.sprite = currentSpriteList[0];

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

        private void Jump ()
        {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _playerRigidbody.AddForce(7*Vector2.up,ForceMode2D.Impulse);

     

        }
        }



        }