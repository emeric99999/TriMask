using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class camera : MonoBehaviour
{

    [SerializeField] private BoxCollider2D detectionCamera;
    [SerializeField] private SpriteRenderer detectionZone;
    private Color cameraColorDetected;
    private Color cameraColorNotDetected;
    private float timerCamera;
    // Start is called before the first frame update
    void Start()
    {
        cameraColorDetected = Color.red;
        cameraColorNotDetected = detectionZone.color;
        cameraColorDetected.a = cameraColorNotDetected.a;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Player.playerIsInvisibile)
        { 
            detectionCamera.isTrigger = false;
            detectionZone.color = cameraColorDetected;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Player.playerIsInvisibile)
        {
            detectionCamera.isTrigger = false;
            detectionZone.color = cameraColorDetected;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!detectionCamera.isTrigger && timerCamera < 2)
        {
            timerCamera += Time.deltaTime;
        }
        else if (!detectionCamera.isTrigger && timerCamera >= 2)
        {
            detectionCamera.isTrigger = true;
            detectionZone.color = cameraColorNotDetected;
        }

    }
}
