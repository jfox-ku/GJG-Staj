using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform shootPos;
    public Transform nextBulletPos;

    public float rotateSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1) {
            Debug.Log("Touch recognized at pos."+Input.GetTouch(0).position);
            StopAllCoroutines();
            StartCoroutine(rotateTowards(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)));
        }
    }

    private IEnumerator rotateTowards(Vector3 target) {
        Debug.Log("Touch is at position " + target + " in the gameworld.");
        target.z = 0;
        var startDirection = transform.forward;
        var targetDirection = (target - this.transform.position).normalized;
        var newDirection = Vector3.RotateTowards(startDirection, targetDirection, rotateSpeed, 0f);


        while (newDirection != targetDirection) {


            Debug.Log("\n startDir = " + startDirection + "\n newDir = " + newDirection + "\n targetDir = " + targetDirection);

            Debug.DrawRay(transform.position, newDirection, Color.red);
            transform.rotation = Quaternion.LookRotation(new Vector3(0,0,newDirection.z));
            

            yield return new WaitForEndOfFrame();
            newDirection = Vector3.RotateTowards(newDirection, targetDirection, rotateSpeed, 0f);
        }

        Debug.Log("Reached end of rotation.");

    }



}
