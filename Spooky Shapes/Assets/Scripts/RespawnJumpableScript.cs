using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RespawnJumpableScript : MonoBehaviour
{

    public List<GameObject> Jumpables;
    private JumpableScript jump;
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
        //Debug.Log("Respawn called");       
        if (jump != null) {
            StartCoroutine(waitAndRespawn());

        }


    }
    

    public void Spawn(GameObject imp) {
        if (imp == null) {
            var toSpawn = Jumpables[UnityEngine.Random.Range(0, Jumpables.Count)];
            var obj = Instantiate(toSpawn, transform.position, Quaternion.identity, this.transform);
            jump = obj.GetComponent<JumpableScript>();
            jump.respawning = true;
            jump.DestEvent += Respawn;

        } else {
            var toSpawn = imp;
            var obj = Instantiate(toSpawn, transform.position, Quaternion.identity, this.transform);
            jump = obj.GetComponent<JumpableScript>();
            jump.respawning = true;
            jump.DestEvent += Respawn;
        }
        
        
    }


    private void OnDisable() {
        if (jump != null) jump.DestEvent -= Respawn;
    }


    public IEnumerator waitAndRespawn() {
       // Debug.Log("Waiting...");
        yield return new WaitForSeconds(spawnCooldown);
        //Debug.Log("Spawning back.");
        jump.Enable();
        jump.transform.position = this.transform.position;


    }

}
