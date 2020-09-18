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

        playerScript.itemDragWait = count * 0.1f;
    }

    }
