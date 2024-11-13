using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timingSpikes : MonoBehaviour
{
    [SerializeField] SpriteRenderer spikeSprite;
    [SerializeField] BoxCollider2D hitzone;
    private float spikesTimer = 0;
    private bool spikes = true;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        spikesTimer += Time.deltaTime;
        if (spikes)
        { if (spikesTimer >0.5f) { spikes = false; hitzone.enabled = false; spikeSprite.enabled = false; } }
        else if (!spikes)
        { if (spikesTimer > 2.5f) { spikes = true; hitzone.enabled = true; spikeSprite.enabled = true; spikesTimer = 0; } }

        

    }
}
