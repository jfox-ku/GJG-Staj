using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaverItemScript : ItemScript
{
    public GameObject saverObject; //prefab to instantiate
    public GameObject saverInstance; //

   

    // Start is called before the first frame update
    void Start()
    {
        itemName = "saverItem";
    }

    public override void effector() {

        if (saverInstance == null) {
            saverInstance = (GameObject)Instantiate(saverObject, Vector3.zero, Quaternion.identity);
        } else {
            saverInstance.gameObject.SetActive(true);
        }
        
        //Debug.Log("New Saver Added!");
        
        var JS = saverInstance.GetComponent<JumpableScript>();
        JS.jumpForce = JS.asset.basePushForce;
        for(int i = 0; i < count; i++) {
            JS.jumpForce += JS.asset.basePushForce * 0.3f;
        }

        JS.respawning = true;
        //Debug.Log("Saver jumpforce set to: " + JS.jumpForce+". Base was: "+JS.asset.basePushForce);

    }

    public override void OnPickUp() {
        effector();
    }

    //private void Respawn(GameObject destroyedObj) {
    //    Debug.Log("Saver respawn called!");
    //    var JumpableScript = destroyedObj.GetComponent<JumpableScript>();
    //    JumpableScript.DestEvent -= Respawn;

    //    consumedCount++;
    //    if(count-consumedCount > 0) {
    //        effector();
    //    }

    //}

    private void OnDisable() {
        
    }


}
