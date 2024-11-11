using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSpikes : MonoBehaviour
{

    [SerializeField] BoxCollider2D detectionZone;
    [SerializeField] Rigidbody2D spikesBody;
    public float spikeSpeed;
    [SerializeField] BoxCollider2D hitzone;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -1.2)
        { GameObject.Destroy(hitzone);
        }
    }
    
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Player.playerIsInvisibile)
        {
            spikesBody.gravityScale = spikeSpeed;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !Player.playerIsInvisibile)
        {
            spikesBody.gravityScale = spikeSpeed;
        }
    }




}
