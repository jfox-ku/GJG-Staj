using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform playerTransform;

    public Transform bgFront;
    public Transform bgBack;

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

        bgFront.position = new Vector2(bgFront.position.x,bgFront.position.y + (dir.y * -0.2f));
        bgBack.position = new Vector2(bgBack.position.x, bgBack.position.y + (dir.y * -0.01f));
        playerStartPos = playerTransform.position;
    }
}
