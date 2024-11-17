
using UnityEngine;

public class SideSpikes : MonoBehaviour
{

    [SerializeField] BoxCollider2D detectionZone;
    [SerializeField] Rigidbody2D spikesBody;
    [SerializeField] BoxCollider2D hitzone;
    [SerializeField] SpriteRenderer spikesSprite;
    private Vector2 positionInitiale;

    
    // Start is called before the first frame update
    void Start()
    {
       positionInitiale = transform.position;
        spikesBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.gameover) 
        {
            hitzone.enabled = false; spikesSprite.enabled = false;
            transform.position = positionInitiale;
            spikesBody.velocity = Vector2.zero;
            detectionZone.enabled = true;
        }
    }
    
    



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            spikesBody.velocity = Vector2.zero;
            hitzone.enabled = false; spikesSprite.enabled = false;
            transform.position = positionInitiale;
            detectionZone.enabled = true;
        }

        if (collision.collider.CompareTag("Player"))
            spikesBody.velocity = Vector2.zero;
        hitzone.enabled = false; spikesSprite.enabled = false;
        transform.position = positionInitiale;
        detectionZone.enabled = true;
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spikesSprite.enabled && !hitzone.enabled)
        {
            spikesBody.velocity = new Vector2(9f, 0);
            spikesSprite.enabled = true; hitzone.enabled = true;
            detectionZone.enabled = false;
        }
    }


}
