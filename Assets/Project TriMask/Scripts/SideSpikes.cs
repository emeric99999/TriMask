using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SideSpikes : MonoBehaviour
{

    [SerializeField] BoxCollider2D detectionZone;
    [SerializeField] Rigidbody2D spikesBody;
    public float spikeSpeed;
    [SerializeField] BoxCollider2D hitzone;
    [SerializeField] SpriteRenderer spikesSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player") && spikesSprite != null && hitzone!= null)
        {
            spikesBody.velocity = new Vector2(9f,0);
            spikesSprite.enabled = true; hitzone.enabled = true;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            spikesBody.velocity = Vector2.zero;
            GameObject.Destroy(hitzone); GameObject.Destroy(spikesSprite); }
    }




}
