using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpablePoolManager : MonoBehaviour
{
    public List<GameObject> JumpablePrefabs;
    [SerializeField]
    public static List<Queue<GameObject>> ListOfQueues;
    public int maxPiecePerType = 100;
    public int childCount;

    private static JumpablePoolManager internalInstance;
    public static JumpablePoolManager instance {
        get {
            if (internalInstance == null) {
                internalInstance = GameObject.FindObjectOfType<JumpablePoolManager>();
                return internalInstance;
            } else {
                return internalInstance;
            }


        }
    }

     

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Start on Pool called");
        ListOfQueues = new List<Queue<GameObject>>(JumpablePrefabs.Count);

            foreach(GameObject pref in JumpablePrefabs) {
            var prefabType = pref.GetComponent<JumpableScript>().tip;

            //Debug.Log("Adding "+prefabType);
            int index = (int)prefabType;
            ListOfQueues.Insert(index,new Queue<GameObject>());
            //Debug.Log(ListOfQueues.Count + " // " + index);
            var queue = ListOfQueues[index];
            for(int i = 0; i < maxPiecePerType; i++) {
                var obj = Instantiate(pref,this.transform);
                var js = obj.GetComponent<JumpableScript>();

                js.DestEvent += Deactivate;

                obj.SetActive(false);
                queue.Enqueue(obj);
            }


        }
        
    }

    // Update is called once per frame
    void Update()
    {
        childCount = this.transform.childCount;
    }


    public GameObject Retrieve(Type tip) {
        var queue = ListOfQueues[(int)tip];

       
        if (queue.Count!=0) {
            var obj = queue.Dequeue();
            while (obj.activeInHierarchy) {
                //Debug.LogError("Object was already active!!!");
                
                obj = queue.Dequeue();
                if (obj == null) break;
            }
            //Debug.Log("Queue Count:" + queue.Count + "  Type of Queues: " + obj.GetComponent<JumpableScript>().tip);
            if (obj != null) {
                obj.gameObject.SetActive(true);
                obj.GetComponent<JumpableScript>().DestEvent += Deactivate;
                obj.transform.rotation = Quaternion.identity;

                return obj;

            }


        } 

            
            Debug.LogError("Max Num of "+tip+ " reached. If this happens, pieces decide to randomly disappear.");
            var obje = Instantiate(JumpablePrefabs[(int)tip],this.transform);
            obje.GetComponent<JumpableScript>().DestEvent += Deactivate;
            obje.SetActive(true);
            obje.transform.rotation = Quaternion.identity;
            return obje;

        

   
    }


    public static void Deactivate(GameObject jj) {
        //Debug.Log(jj.name+" is deactivated and put back in pool.");
        var js = jj.GetComponent<JumpableScript>();
        
        js.DestEvent -= Deactivate;
        jj.SetActive(false);
        if (js.respawning) return;
        else {
            var queue = ListOfQueues[(int)js.tip];
            queue.Enqueue(js.gameObject);
        }
        


    }

}
