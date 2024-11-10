using System;
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

    public KeyCode _switchKey; //touche utilis�e pour changer de mode 
    public int _modeID = 0; // id du mode actuel (0 : double jump, 1 : invisibilit�, 2 : rayon X
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
    private float floorY;
    private bool _hasDoubleJump = true;
    public float limity;
    private float timerInvisibility = 0;
    private Color fade;
    private Color noFade;
    public static bool playerIsInvisibile = false;
    public bool xrayActive = false;
    [SerializeField] private SpriteRenderer backgroundSprite;
    

    public static void GameOver()
    {
        Debug.Log("perdu");
    }

    public void Start()
    {
        currentSpriteList = doubleJumpSpriteList;
        floorY = transform.position.y;
        fade = playerSprite.color;
        noFade = playerSprite.color;
        fade.a = 0.5f;
        _playerRigidbody.freezeRotation = true;
    }
    private void Update()
    {
        if (!xrayActive)
        {
            Move();
            Jump();
        }
        if (_modeID == 0) {DoubleJump(); } //doublejump 
        else if (_modeID == 1) { Invisible();  } //invisibility 
        else { X_ray(); } //detection

        setModeID();



    }

    private void setModeID()   // fonction qui change le _modeID lorsque la touche switch est press�e
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
                if (timer < animTime / 4) playerSprite.sprite = currentSpriteList[1];
                else if (timer < animTime / 2) playerSprite.sprite = currentSpriteList[2];
                else if (timer < 3 * animTime / 4) playerSprite.sprite = currentSpriteList[3];
                else playerSprite.sprite = currentSpriteList[4];
            }

            else timer = 0;

            if (_direction == Vector2.left)
            {
                playerSprite.flipX = true;
            }

            else if (_direction == Vector2.right)
            {
                playerSprite.flipX = false;
            }
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

    private void Jump()
    {
        if (transform.position.y <= floorY)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _playerRigidbody.AddForce(7 * Vector2.up, ForceMode2D.Impulse);
            }
        }
        if (transform.position.y > floorY + limity)
        {
            _playerRigidbody.velocity = Vector2.zero;
            Debug.Log("limite");
        }
    }

    private void DoubleJump()
    {

        if (transform.position.y > floorY && Input.GetKeyDown(KeyCode.W) && _hasDoubleJump)
        {
            _playerRigidbody.AddForce(9 * Vector2.up, ForceMode2D.Impulse);
            _hasDoubleJump = false;
        }
        if (transform.position.y <= floorY)
        {
            _hasDoubleJump = true;
        }
    }



    private void Invisible()
    {
        if ( Input.GetKeyDown(KeyCode.E))
        {
            playerIsInvisibile = true;
        }
            if (playerIsInvisibile)
            {
                if (timerInvisibility < 2)
                {
                    timerInvisibility += Time.deltaTime;
                    playerSprite.color = fade;
                }

            if (timerInvisibility >= 2 || Input.GetKeyDown(_switchKey))
                {
                    timerInvisibility = 0;
                    playerSprite.color = noFade;
                    playerIsInvisibile = false;
                }
            }
        
    }

    private void X_ray()
    {
        if (xrayActive)
        {
            backgroundSprite.color = fade;

        }
        if (!xrayActive && Input.GetKeyDown(KeyCode.E))
        {
            xrayActive = true;
            playerSprite.sprite = currentSpriteList[0];
        }
        else if (xrayActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(_switchKey)))
          {
            xrayActive = false;
            backgroundSprite.color = noFade;
          }
        
    }

}