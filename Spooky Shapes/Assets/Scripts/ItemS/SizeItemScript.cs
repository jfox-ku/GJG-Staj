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

        Debug.Log("Setting player size to "+ new Vector3(1 + (count+1) / 10f, 1 + (count + 1) / 10f, 1));
        player.transform.localScale = new Vector3(1f + (count + 1) / 10f,1f+ (count + 1) / 10f,1);

    }
}
