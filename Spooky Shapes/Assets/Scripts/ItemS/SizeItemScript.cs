using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeItemScript : ItemScript
{

    private void Awake() {
        itemName = "sizeItem";
    }

    public override void OnPickUp() {
        effector();
    }


    public override void effector() {
        if (count > 14) return;

        var player = GameObject.FindGameObjectWithTag("Player");

        
        var newScale = new Vector3(1f + (count + 1) / 10f, 1f + (count + 1) / 10f, 1);
        Debug.Log("Setting player size to " + newScale);
        player.transform.localScale = newScale;

    }
}
