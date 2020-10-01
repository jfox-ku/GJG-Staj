using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushForceItem : ItemScript
{
    public override void OnPickUp() {
        effector();


    }

    public override void effector() {
        if (count > 15) return;
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerScript = player.GetComponent<PlayerScript>();
        var throwMult = count;

        //Debug.Log("Player dragWait time set to: "+dragWait);

        playerScript.throwMultiplier = throwMult;
        playerScript.maxSpeed += 1f;
    }
}
