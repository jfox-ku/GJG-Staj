using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsColorController : MonoBehaviour
{

    public SpriteRenderer leftWallSprite;
    public SpriteRenderer rightWallSprite;

    // Start is called before the first frame update
    void Start()
    {if(leftWallSprite==null || rightWallSprite == null) {
            Debug.LogWarning("Set wall sprites.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeColor(Color clr) {
        leftWallSprite.color = clr;
        rightWallSprite.color = clr;
    }


}
