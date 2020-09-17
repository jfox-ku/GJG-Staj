using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPartScript : MonoBehaviour
{
    public static int count;
    public int id;

    public List<PieceCreatorScript> pclst;
    public SetSO set;

    public Transform TOP;
    public Transform BOT;

    public SpriteRenderer background;

    // Start is called before the first frame update
    void Start()
    {
        id = count;
        count++;
        background.color = Color.Lerp(set.BGstartColor, set.BGendColor, Mathf.Min(transform.position.y/set.colorSwapDistanceMax,1));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpCreators() {
        foreach (PieceCreatorScript pcs in pclst) {
            var spawn = set.getRandomJumpable();
            //Debug.Log("Assigning jumpables to creators." + pcs.name + "=>" + spawn.name);
            pcs.startAsset();
            pcs.SetToSpawn(spawn);
            pcs.startProducing();

        }
    }


    public int getCount() {
        return count;
    }

    public float getHeight() {
        return Vector2.Distance(TOP.position,BOT.position);
    }
}
