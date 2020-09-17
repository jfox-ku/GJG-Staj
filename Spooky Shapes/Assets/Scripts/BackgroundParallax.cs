using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform playerTransform;

    [Tooltip("Mirror player movement times parallaxRatio")]
    public float parallaxRatio;
    private Vector2 playerStartPos;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPos = playerTransform.position;
    }

    // Update is called once per frame
    
    void Update()
    {
        var dir = (Vector2)playerTransform.position - playerStartPos;

        transform.position = new Vector2(transform.position.x,transform.position.y + (dir.y * parallaxRatio));
        
        playerStartPos = playerTransform.position;
    }



}
