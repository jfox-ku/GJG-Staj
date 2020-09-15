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

    // Start is called before the first frame update

    public void startAsset() {
        if (asset != null) {
            toSpawn = asset.prefab;
            numToGenerate = asset.numToGen;
            generateCooldown = asset.genCooldown;
            speed = asset.travelTime;
            nextGenCooldown = asset.nextGenCooldown;
            doesGenerate = true;
            
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
        for (int i = 0; i< num; i++) {
            //Debug.Log("Object generated at " + Time.time);
            var obj = Instantiate(sp, this.transform);
            StartCoroutine(MovePiece(obj, speed));
            yield return new WaitForSeconds(cooldown);
        }

    }

    private IEnumerator MovePiece(GameObject obj, float duration) {
        var totalDuration = duration;
        
        while(duration > 0) {
            duration -= Time.deltaTime;
            obj.transform.position = Vector2.Lerp(this.transform.position, target.transform.position, (totalDuration-duration)/totalDuration);
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("Reached Target.");
        Destroy(obj);
    }



}
