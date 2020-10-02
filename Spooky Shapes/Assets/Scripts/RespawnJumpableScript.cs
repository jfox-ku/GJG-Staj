using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RespawnJumpableScript : MonoBehaviour
{

    public List<GameObject> Jumpables;
    public GameObject currentPiece;
    public bool RandomSpawn = true;
    public Type jumpableChoice;
    public float spawnCooldown = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Create(jumpableChoice);


        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn(Type imp) {
        //Debug.Log("Respawn called");       
        StartCoroutine(waitAndRespawn(imp));



    }

    public void Create(Type tip) {
        GameObject toSpawn;
        GameObject piece;
        JumpableScript js;

        if (RandomSpawn) {
            toSpawn = Jumpables[UnityEngine.Random.Range(0, Jumpables.Count)];
            piece = JumpablePoolManager.instance.Retrieve(toSpawn.GetComponent<JumpableScript>().tip);
            js = piece.GetComponent<JumpableScript>();
            
        } else {

            piece = JumpablePoolManager.instance.Retrieve(tip);
            js = piece.GetComponent<JumpableScript>();

        }
        currentPiece = piece;
        piece.transform.position = this.transform.position;
        
        js.SetAsRespawning(this);

    }
    


    private void OnDestroy() {
        
    }


    public IEnumerator waitAndRespawn(Type tip) {
       // Debug.Log("Waiting...");
        yield return new WaitForSeconds(spawnCooldown);
        //Debug.Log("Spawning back.");
        Create(tip);
       


    }

    public void Destroy() {
        StopAllCoroutines();
        if (currentPiece != null) {
            var js = currentPiece.GetComponent<JumpableScript>();
            //js.DestEvent -= Respawn;
            
            js.Disable();
        }
        
    }


}
