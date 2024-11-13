using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private List<Sprite> maskSprites;
    [SerializeField] private SpriteRenderer mask1Sprite;
    [SerializeField] private SpriteRenderer mask2Sprite;
    [SerializeField] private SpriteRenderer mask3Sprite;
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
    [SerializeField] private List<SpriteRenderer> backgroundSprite;
    private float dashTimer = 0;
    private RigidbodyConstraints2D dashConstraints;
    private RigidbodyConstraints2D normalConstraints;
    private bool isDashing;
    private float dashCooldown = 1;
    private float invisibleCoolDown = 4;
    public static float lastCheckpointX;
    public static float lastCheckpointY;
    [SerializeField] Player player;
    public float dashDuration;
    private float timerJump;
    private bool landing = false;
    private float landingLag = 0;
    private bool landed = true;
    [SerializeField] Camera playerCam;
    private float xrayTimer = 0;
    private bool termine = false;
    private bool pause = false;
    [SerializeField] private TextMeshPro text1;
    [SerializeField] private TextMeshPro text2;
    [SerializeField] private TextMeshPro text3;
    private Vector2 velolcityPlayer;
    private bool win = false;
    [SerializeField] private TextMeshPro winText;
    private float escapeTimer = 0;
    [SerializeField] private TextMeshPro menuText;
    [SerializeField] private TextMeshPro timerText;
    public static bool gameover = false;



    public void GameOver()
    {
        transform.position = new Vector3(lastCheckpointX, lastCheckpointY, transform.position.z);
        gameover = true;
       
    }



    public void Start()
    {
        mask1Sprite.sprite = maskSprites[0];
        mask2Sprite.sprite = maskSprites[1];
        mask3Sprite.sprite = maskSprites[2];
        currentSpriteList = doubleJumpSpriteList;
        floorY = transform.position.y;
        fade = playerSprite.color;
        noFade = playerSprite.color;
        fade.a = 0.5f;
        _playerRigidbody.freezeRotation = true;
        dashConstraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        normalConstraints = _playerRigidbody.constraints;
        winText.text = "Escape in 2m30 !";
    }
    private void Update()
    {

        if (!termine && !pause)
        {
            timerText.text = (Math.Round(escapeTimer*100)/100).ToString();
            escapeTimer += Time.deltaTime;
            if (escapeTimer < 3) { winText.enabled = true; }
            if (escapeTimer > 3 ) { winText.enabled = false; }
            if (escapeTimer > 150) termine = true;
            velolcityPlayer = _playerRigidbody.velocity;
            uiSprite.enabled = true; mask1Sprite.enabled = false; mask2Sprite.enabled = false; mask3Sprite.enabled = false;
            text1.enabled = false; text2.enabled = false; text3.enabled = false;

            if (!xrayActive)
            {
                if (!isDashing && !(landing && landed))
                {
                    Move();
                    Jump();
                }
                else if (landing && landed)
                {
                    if (landingLag < 0.1f) { landingLag += Time.deltaTime; playerSprite.sprite = currentSpriteList[12]; }
                    else { landing = false; landingLag = 0; }
                }
                Dash();


                if (_modeID == 0 && !isDashing) { DoubleJump(); } //doublejump 
                else if (_modeID == 1) { Invisible(); } //invisibility 

            }

            if (_modeID == 2) { X_ray(); } //detection
            setModeID();
            if (Input.GetKeyDown(KeyCode.Escape))
                pause = true;
            gameover = false;


        }

        else if (pause)
        {
            text1.enabled = true;
            text2.enabled = true;
            text3.enabled = true;
            uiSprite.enabled = false;
            mask1Sprite.enabled = true;
            mask2Sprite.enabled = true;
            mask3Sprite.enabled = true;
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.gravityScale = 0;
            menuText.enabled = true;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _playerRigidbody.gravityScale = 2.5f;
                _playerRigidbody.velocity = velolcityPlayer;
                menuText.enabled = false;
                pause = false;

            }
            else if (Input.GetKeyDown(KeyCode.Space)) { SceneManager.LoadScene("Menu"); }
            
        }
        else
        {
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.gravityScale = 0;
            menuText.enabled = true;
            menuText.text = "Press escape to return to the menu";
            if (win)
            { winText.enabled = true; winText.text = "Gentlemask escaped!"; }
            else { winText.enabled = true; winText.text = "Gentlemask was too slow..."; }
            if (Input.GetKeyDown (KeyCode.Escape))
            {
                SceneManager.LoadScene("Menu");
            }

        }

    }

    private void setModeID()   // fonction qui change le _modeID lorsque la touche switch est pressée
    {
        if (Input.GetKeyDown(_switchKey))
        {
            _modeID += 1;
            _modeID = _modeID % 3;
            if (_modeID == 0) { uiSprite.sprite = maskSprites[0]; currentSpriteList = doubleJumpSpriteList; }
            else if (_modeID == 1) { uiSprite.sprite = maskSprites[1]; currentSpriteList = stealthSpriteList; } //j'ai inversé les deux listes à leur création, my bad ^^l
            else { uiSprite.sprite = maskSprites[2]; currentSpriteList = xraySpriteList; }
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
        if (landed)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _playerRigidbody.velocity = Vector2.zero;
                _playerRigidbody.AddForce(14 * Vector2.up, ForceMode2D.Impulse);
                timerJump = 0;

                landed = false;
            }


        }
        else
        {
            landing = true;
            timerJump += Time.deltaTime;
            if (timerJump <= 0.5f) { playerSprite.sprite = currentSpriteList[9]; }
            else if (timerJump <= 0.7f) { playerSprite.sprite = currentSpriteList[10]; }
            else { playerSprite.sprite = currentSpriteList[11]; }

        }
        //if (transform.position.y > floorY + limity)
        //{
        //  _playerRigidbody.velocity = Vector2.zero;
        // Debug.Log("limite");
        // }
    }


    private void DoubleJump()
    {

        if (landing && !landed && Input.GetKeyDown(KeyCode.W) && _hasDoubleJump)
        {
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce(15 * Vector2.up, ForceMode2D.Impulse);
            _hasDoubleJump = false;
            timerJump = 0;
        }
        if (!_hasDoubleJump)
        {
            timerJump += Time.deltaTime;
            if (timerJump <= 0.25f) { playerSprite.sprite = currentSpriteList[13]; }
            else if (timerJump <= 0.50f) { playerSprite.sprite = currentSpriteList[14]; }
            else if (timerJump <= 0.75f) { playerSprite.sprite = currentSpriteList[15]; }
            else if (timerJump <= 1f) { playerSprite.sprite = currentSpriteList[16]; }
            else { playerSprite.sprite = currentSpriteList[11]; }




        }
        if (landed)
        {
            _hasDoubleJump = true;
        }
    }



    private void Invisible()
    {
        if (Input.GetKeyDown(KeyCode.E) && invisibleCoolDown >= 4)
        {
            playerIsInvisibile = true;
            invisibleCoolDown = 0;
        }
        else if (invisibleCoolDown < 4)
        { invisibleCoolDown += Time.deltaTime; }
        if (playerIsInvisibile)
        {
            if (timerInvisibility < 0.1)
            {
                timerInvisibility += Time.deltaTime;
                playerSprite.sprite = currentSpriteList[13];
            }
            else if (timerInvisibility < 0.3)
            { timerInvisibility += Time.deltaTime; playerSprite.sprite = currentSpriteList[14]; }
            else if (timerInvisibility < 0.4) { playerSprite.sprite = currentSpriteList[13]; timerInvisibility += Time.deltaTime; }
            else if (timerInvisibility < 2.4f)
            {
                timerInvisibility += Time.deltaTime;
                playerSprite.color = fade;
            }

            if (timerInvisibility >= 2.4f || Input.GetKeyDown(_switchKey))
            {
                timerInvisibility = 0;
                playerSprite.color = noFade;
                playerIsInvisibile = false;
            }
        }

    }

    private void X_ray()
    {
        if (!xrayActive && Input.GetKeyDown(KeyCode.E))
        {
            xrayActive = true;

        }
        else if (xrayActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(_switchKey)))
        {
            xrayActive = false;
            foreach (SpriteRenderer backSprite in backgroundSprite)
            { backSprite.color = noFade; }
            playerCam.orthographicSize /= 2;
            xrayTimer = 0;
        }
        else if (xrayActive)
        {

            if (xrayTimer < 0.2f) { xrayTimer += Time.deltaTime; playerSprite.sprite = currentSpriteList[13]; }
            else if (xrayTimer < 0.4f) { xrayTimer += Time.deltaTime; playerSprite.sprite = currentSpriteList[14]; }
            else
            {
                playerSprite.sprite = currentSpriteList[15];
                foreach (SpriteRenderer backSprite in backgroundSprite)
                { backSprite.color = fade; }
                if (playerCam.orthographicSize < 10) playerCam.orthographicSize *= 2;
            }


        }

    }

    private void Dash()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown >= 0.8f)

        {
            isDashing = true;
            dashCooldown = 0;
        }
        else if (dashCooldown < 0.8f)
        { dashCooldown += Time.deltaTime; }
        if (!playerSprite.flipX && isDashing)
            _playerRigidbody.velocity = 15 * Vector2.right;
        else if (playerSprite.flipX && isDashing)
            _playerRigidbody.velocity = 15 * Vector2.left;
        if (isDashing && dashTimer < dashDuration)
        {
            if (dashTimer < 0.05f)
            { playerSprite.sprite = currentSpriteList[5]; }
            else if (dashTimer < 0.25f)
            { playerSprite.sprite = currentSpriteList[6]; }
            else if (dashTimer < 0.35f)
            { playerSprite.sprite = currentSpriteList[7]; }
            else if (dashTimer < 0.5f)
            { playerSprite.sprite = currentSpriteList[8]; }
            dashTimer += Time.deltaTime;
            _playerRigidbody.constraints = dashConstraints;
        }
        else if (isDashing && dashTimer >= dashDuration)
        {
            _playerRigidbody.constraints = normalConstraints; isDashing = false;
            dashTimer = 0;
            _playerRigidbody.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Spikes"))
        {
            GameOver();
        }
        if (collision.collider.CompareTag("Floor"))
        { landed = true; }

        if (collision.collider.CompareTag("laser"))
        { GameOver(); }


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("laser"))
        { GameOver(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn") && transform.position.x > lastCheckpointX)
        { lastCheckpointX = transform.position.x; lastCheckpointY = transform.position.y; Debug.Log("new checpoint"); }
        if (collision.CompareTag("Finish"))
        {
            termine = true; win = true;
        }


    }
}

