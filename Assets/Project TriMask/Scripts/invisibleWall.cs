using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisibleWall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer wallSprite;
    [SerializeField] Camera playerCam;
    private Color normalColor;
    private Color revealColor;
    void Start()
    {
        normalColor = wallSprite.color;
        revealColor = wallSprite.color;
        revealColor.a = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCam.orthographicSize > 6)
        { wallSprite.color = revealColor; }
        else { wallSprite.color = normalColor; }
    }
}
