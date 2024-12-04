
using UnityEngine;

public class FloorSpikes : MonoBehaviour
{
    public static bool touche;
    [SerializeField] Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.GameOver();
            touche = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
