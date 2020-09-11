using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreatorScript : MonoBehaviour
{
    public GameObject prefab1;
    public Transform target;
    public int numToGenerate;
    public float generateCooldown;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(generator(numToGenerate,generateCooldown,speed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator generator(int num, float cooldown, float speed) {
        GameObject toSpawn = prefab1;
        for (int i = 0; i< num; i++) {
            var obj = Instantiate(toSpawn, this.transform);
            StartCoroutine(movePiece(obj, speed));
            yield return new WaitForSeconds(cooldown);
        }

    }

    private IEnumerator movePiece(GameObject obj, float duration) {
        var totalDuration = duration;
        
        while(duration > 0) {
            duration -= Time.deltaTime;
            obj.transform.position = Vector2.Lerp(this.transform.position, target.transform.position, (totalDuration-duration)/totalDuration);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Reached Target.");
        Destroy(obj);
    }



}
