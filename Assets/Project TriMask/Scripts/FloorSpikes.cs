using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpikes : MonoBehaviour
{

    [SerializeField] BoxCollider2D detectionZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player.GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
