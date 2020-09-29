using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreScript : MonoBehaviour
{
    public Transform player;
    public Text txt;
    public bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        txt.text = "HighScore: " + (int)PlayerPrefs.GetInt("Score");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameStarted)
        txt.text = "Score: "+(int)player.position.y;
    }




}
