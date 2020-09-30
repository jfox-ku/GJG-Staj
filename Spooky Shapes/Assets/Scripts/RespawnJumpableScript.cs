using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RespawnJumpableScript : MonoBehaviour
{

    public List<GameObject> Jumpables;
    public GameObject currentPiece;
    public bool RandomSpawn = true;
    public int jumpableChoice;
    public float spawnCooldown = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

        if (RandomSpawn) {
            Spawn(null);
        } else if(jumpableChoice < Jumpables.Count){
            Spawn(Jumpables[jumpableChoice]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn(GameObject imp) {
        Debug.Log("Respawn called");       
        StartCoroutine(waitAndRespawn());



    }
    

    public void Spawn(GameObject imp) {
        if (imp == null) {
            var toSpawn = Jumpables[UnityEngine.Random.Range(0, Jumpables.Count)];
            var obj = JumpablePoolManager.instance.Retrieve(toSpawn.GetComponent<JumpableScript>().tip);
            obj.transform.position = transform.position;
            var jump = obj.GetComponent<JumpableScript>();
            jump.respawning = true;
            jump.DestEvent += Respawn;
            currentPiece = obj;

        } else {
            var toSpawn = imp;
            var obj = JumpablePoolManager.instance.Retrieve(toSpawn.GetComponent<JumpableScript>().tip);
            obj.transform.position = transform.position;
            var jump = obj.GetComponent<JumpableScript>();
            jump.respawning = true;
            jump.DestEvent += Respawn;
            currentPiece = obj;
        }
        
        
    }


    private void OnDestroy() {
        
    }


    public IEnumerator waitAndRespawn() {
       // Debug.Log("Waiting...");
        yield return new WaitForSeconds(spawnCooldown);
        //Debug.Log("Spawning back.");
        Spawn(null);
       


    }

    public void Destroy() {
        var js = currentPiece.GetComponent<JumpableScript>();
        js.unloadDisable();
        Destroy(this.gameObject);
    }


}
