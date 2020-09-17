using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreScript : MonoBehaviour
{
    public Transform player;
    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        txt.text = "Score: "+(int)player.position.y;
    }
}
