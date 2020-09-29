using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaverFollowScript : MonoBehaviour
{

    private Transform Player;
    private Transform startPos;

    public int count = 0;

    // Start is called before the first frame update
    public void OnEnable()
    {
        startPos = GameObject.FindGameObjectWithTag("SaverItemStartPos").transform;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(Player.position.x,startPos.position.y);
    }




}
