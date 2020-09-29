using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpzoneScript : MonoBehaviour
{

    public Transform parent;
    private float upAmount = 1.5f;
    public int upCount;

    //Maybe I should write a cooldown object instead of writing this structure for every cooldown. 
    //Used a similar one in wall hit cooldown.
    public float moveUpCooldownMax;
    private float cdKeeper;
    private RunManagerScript rm;

    private void Start() {
        upCount = 0;
        rm = FindObjectOfType<RunManagerScript>();
    }

    public void MoveUpConfiner() {

        if(cdKeeper == 0 && rm.allowLoad==true) {   
            upCount++;
            parent.position += new Vector3(0, upAmount, 0);
            StartCoroutine(moveUpCooldown());
        }
        
        
    }



    private IEnumerator moveUpCooldown() {
        cdKeeper = moveUpCooldownMax;
        while(cdKeeper > 0) {
            cdKeeper -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cdKeeper = 0;

        
    }

    private Vector3 playerLastPos;
    public void moveFollowDelta(Transform player,float velocityY) {
        if (playerLastPos == null) playerLastPos = player.position;
        var offset = player.position - playerLastPos;
        if (velocityY > 0 && offset!=Vector3.zero) {
            //Debug.Log("parentPos: "+parent.position);
            parent.position = new Vector3(this.parent.position.x,this.parent.position.y+velocityY/10f,this.parent.position.z);
        }
        


        playerLastPos = player.position;

    }
   


}
