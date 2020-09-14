using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreatorScript : MonoBehaviour
{
    public PieceCreatorSO asset;

    private GameObject prefab1;
    public Transform target;
    private int numToGenerate;
    private float generateCooldown;
    private float nextGenCooldown;
    private float speed;
    public bool doesGenerate = false;

    // Start is called before the first frame update

    private void Start() {
        if (asset != null) {
            prefab1 = asset.prefab;
            numToGenerate = asset.numToGen;
            generateCooldown = asset.genCooldown;
            speed = asset.travelTime;
            nextGenCooldown = asset.nextGenCooldown;
            doesGenerate = true;
            StartCoroutine(Produce());
        }
    }

    // Update is called once per frame
    void Update() {

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
        GameObject toSpawn = prefab1;
        for (int i = 0; i< num; i++) {
            //Debug.Log("Object generated at " + Time.time);
            var obj = Instantiate(toSpawn, this.transform);
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
