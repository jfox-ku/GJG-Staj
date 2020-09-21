using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : ItemScript
{

    public override void OnPickUp() {
        effector();


    }

    public override void effector() {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerScript = player.GetComponent<PlayerScript>();
        var dragWait = 0.1f + count * 0.2f;

        Debug.Log("Player dragWait time set to: "+dragWait);

        playerScript.itemDragWait = dragWait;
    }

    }
