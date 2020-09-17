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
        return TOP.position.y - BOT.position.y;
    }

    public float GetSpawnHeight() {
        return transform.position.y - BOT.position.y;
    }

    public Vector2 GetTopPos() {
       // Debug.Log("Top pos:"+ TOP.position);
        return TOP.position;
    }

}
