using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreatorScript : MonoBehaviour
{
    public PieceCreatorSO asset;

    private GameObject toSpawn;
    public Transform target;
    private int numToGenerate;
    private float generateCooldown;
    private float nextGenCooldown;
    private float speed;
    private bool doesGenerate = false;
    private Queue<JumpableScript> myPieces;


    // Start is called before the first frame update
    private void Start() {
        
    }

    public void startAsset() {
        if (asset != null) {
            toSpawn = asset.prefab;
            numToGenerate = asset.numToGen;
            generateCooldown = asset.genCooldown;
            speed = asset.travelTime;
            nextGenCooldown = asset.nextGenCooldown;
            doesGenerate = true;
            myPieces = new Queue<JumpableScript>();
            
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetToSpawn(GameObject JumpablePrefab) {
        toSpawn = JumpablePrefab;
        
    }

    public void startProducing() {
        StartCoroutine(Produce());
    }

    private IEnumerator Produce() {
        doesGenerate = true;
        while (true) {
            //Debug.Log("Produce called generate at "+Time.time);
            StartCoroutine(Generator(numToGenerate, generateCooldown, speed));
            yield return new WaitForSeconds(nextGenCooldown);
            if (doesGenerate == false) break;
        }
    }



   
    private IEnumerator Generator(int num, float cooldown, float speed) {
        GameObject sp = toSpawn;
        
        Type toSpawnType = toSpawn.GetComponent<JumpableScript>().tip;
        if (JumpablePoolManager.instance == null) Debug.LogError("PoolManager is empty!!!");
        for (int i = 0; i< num; i++) {
            //Debug.Log("Object generated at " + Time.time);
            var obj = JumpablePoolManager.instance.Retrieve(toSpawnType);
            obj.GetComponent<JumpableScript>().respawning = false;
            myPieces.Enqueue(obj.GetComponent<JumpableScript>());
            StartCoroutine(MovePiece(obj, speed));
            yield return new WaitForSeconds(cooldown);
        }

    }

    private IEnumerator MovePiece(GameObject obj, float duration) {
        var totalDuration = duration;
        var js = obj.GetComponent<JumpableScript>();

        
        while(duration > 0) {
            duration -= Time.deltaTime;

            if (!gameObject.activeInHierarchy) yield break;

            obj.transform.position = Vector2.Lerp(this.transform.position, target.transform.position, (totalDuration-duration)/totalDuration);
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log("Reached Target.");
        if(obj.activeInHierarchy && !js.respawning)
        js.Disable();

    }


    private void OnDestroy() {
        StopAllCoroutines();
    }

    public void Destroy() {
        StopAllCoroutines();
        int counter = 0;
        int nullCounter = 0;
        foreach (JumpableScript jj in myPieces) {
            if (jj != null && jj.gameObject.activeInHierarchy) {
                jj.Disable();
                counter++;
            } else {
                nullCounter++;
            }
           


        }

        //Debug.Log("Destroyed "+counter+" active pieces. I was holding "+nullCounter+" null pieces");
        myPieces.Clear();
    }

}
