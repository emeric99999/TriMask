
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer plateformSprite;
    [SerializeField] Camera playerCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCam.orthographicSize > 6)
        { plateformSprite.enabled = true; }
        else { plateformSprite.enabled = false;}
    }
}
