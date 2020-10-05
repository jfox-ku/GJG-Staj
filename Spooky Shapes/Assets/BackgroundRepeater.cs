using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{

    public List<SpriteRenderer> Backgrounds;
    public GameObject backgroundPrefab;
    private GameObject Player;
    //Flipping it through quaternions proved more difficult
    public GameObject backgroundPrefabFlipped;
    public Queue<GameObject> bgQueue;

    private int count;
    private float height;
    private float offset;

    void Start()
    {
        height = 0;
        offset = 0;
        count = 0;
        bgQueue = new Queue<GameObject>();
        Player = GameObject.FindGameObjectWithTag("Player");

        Initialize();

    }

    void Update()
    {
        if (!CheckBounds()) {
            AddOnTop();
            RemoveFromBottom();


        }
    }



    private void AddOnTop() {
        GameObject bck;
        if (count % 2 == 0) {
            bck = Instantiate(backgroundPrefabFlipped);
        } else {

            bck = Instantiate(backgroundPrefab);
        }

        count++;
        height += bck.GetComponent<SpriteRenderer>().bounds.extents.y * 2;
        bck.transform.position = new Vector3(this.transform.position.x, height-offset, 0);
        bck.transform.SetParent(this.transform);
        bgQueue.Enqueue(bck);


    }

    private void RemoveFromBottom() {
        GameObject bg = bgQueue.Dequeue();
        Destroy(bg);

    }


    private void Initialize() {

        for (int i = 0; i < 3; i++) {
            GameObject bck;
            if (i % 2 == 0) {
                bck = Instantiate(backgroundPrefabFlipped);
            } else {

                bck = Instantiate(backgroundPrefab);
            }

            count++;
            height += bck.GetComponent<SpriteRenderer>().bounds.extents.y * 2;
            if (offset == 0) offset = height;

            bck.transform.SetParent(this.transform);
            bck.transform.position = new Vector3(this.transform.position.x, height-offset, 0);
            bgQueue.Enqueue(bck);


        }
    }


    public bool CheckBounds() {
        if (Player.transform.position.y < height - offset) {
            return true;


        }
        return false;

    }


    public Queue<GameObject> GetCurrentBackgrounds() {
        return bgQueue;
    }
}
