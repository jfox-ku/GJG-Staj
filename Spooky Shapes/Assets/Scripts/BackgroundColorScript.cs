using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorScript : MonoBehaviour
{

    public SetSO set;
    SpriteRenderer background;
    [SerializeField]
    private float completePercent;
    // Start is called before the first frame update
    void Awake()
    {
        
        background = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //background.color = Color.Lerp(set.BGstartColor, set.BGendColor, Mathf.Min(Player.position.y / set.colorSwapDistanceMax, 1));
        //completePercent = Player.position.y / set.colorSwapDistanceMax;

    }
}
