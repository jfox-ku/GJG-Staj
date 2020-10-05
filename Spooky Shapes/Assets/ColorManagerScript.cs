using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManagerScript : MonoBehaviour
{
    public SetSO set;
    public WallsColorController walls;
    public BackgroundRepeater backgrounds;


    public Transform PlayerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    




}
