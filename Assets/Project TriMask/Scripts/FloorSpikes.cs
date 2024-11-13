
using UnityEngine;

public class FloorSpikes : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] BoxCollider2D detectionZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
