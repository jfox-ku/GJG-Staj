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
        //Debug.Log("Start on Pool called");
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

                //js.DestEvent += Deactivate;

                obj.SetActive(false);
                queue.Enqueue(obj);
            }


        }
        //printQueues();
        
    }

    // Update is called once per frame
    void Update()
    {
        childCount = this.transform.childCount;
    }


    public GameObject Retrieve(Type tip) {
        Queue<GameObject> queue = ListOfQueues[(int)tip];
        if (!GameObject.Equals(ListOfQueues[(int)tip], queue)) {
            Debug.LogError("Queue reference is not held!");
            
        }

        if (queue.Count == 0) {
            FindUnused(tip);
        }
       
        if (queue.Count!=0) {
            GameObject obj = null;
            int loopCount = 0;
            while (queue.Count >0) {
                loopCount++;
                //Debug.LogError("Object was already active!!!");
                
                obj = queue.Dequeue();
                if (obj == null) {
                    Debug.LogError("Qeueue depleted. Shouldn't happen!");
                }
                if (!obj.activeSelf) {
                    //Debug.Log("Found an inactive object of type "+tip+" in "+loopCount+" tries. Queue has " + queue.Count+" elements left");
                    break;

                }
                Debug.LogError("First object dequed was already active!");

                
            }
            //Debug.Log("Queue Count:" + queue.Count + "  Type of Queues: " + obj.GetComponent<JumpableScript>().tip);
            if (obj != null) {
                obj.gameObject.SetActive(true);
                obj.GetComponent<JumpableScript>().DestEvent += Deactivate;
                obj.transform.rotation = Quaternion.identity;
                obj.GetComponent<JumpableScript>().respawning = false;
                return obj;

            }




        } else {
      
   


        }

        return FindUnused(tip);






    }

    private void CreatePiece(Type tip) {
        Queue<GameObject> queue = ListOfQueues[(int)tip];
        var obj = Instantiate(JumpablePrefabs[(int)tip], this.transform);
        var js = obj.GetComponent<JumpableScript>();

        //js.DestEvent += Deactivate;

        obj.SetActive(false);
        queue.Enqueue(obj);

    }


    private GameObject FindUnused(Type tip) {
        
        var children = GetComponentsInChildren<JumpableScript>();
        foreach(JumpableScript js in children) {
            if (js.tip == tip) {
                if (!js.gameObject.activeInHierarchy) {
                    js.DestEvent += Deactivate;
                    Debug.LogError("Found unused of type "+js.tip+". How did you get out my boy?");
                    return js.gameObject;
                }
                
            }
        }

        Debug.Log("Couldn't find inactive piece of type " + tip + " . Creating new one..");
        //printQueues();
        var obje = Instantiate(JumpablePrefabs[(int)tip], this.transform);
        obje.GetComponent<JumpableScript>().DestEvent += Deactivate;
        obje.GetComponent<JumpableScript>().respawning = false;
        obje.SetActive(true);
        obje.transform.rotation = Quaternion.identity;
        return obje;

    }


    public static void Deactivate(GameObject jj) {
        //Debug.Log(jj.name+" is deactivated and put back in pool.");
        if (!jj.activeInHierarchy) return;
        var js = jj.GetComponent<JumpableScript>();
        js.DestEvent -= Deactivate;
        jj.SetActive(false);
        var queue = ListOfQueues[(int)js.tip];
        //Debug.Log("Back in the queue! " + js.tip+" Size: "+queue.Count);
        queue.Enqueue(js.gameObject);

        


    }


    private void printQueues() {
        foreach(Queue<GameObject> queue in ListOfQueues) {
            if(queue.Count != 0) {
                Type tip = queue.Peek().GetComponent<JumpableScript>().tip;
                Debug.Log("Queue of "+tip+" and size "+queue.Count );
                string output = "";
                foreach(var x in queue.ToArray()) {
                    if (x.activeInHierarchy) output += " 1 ";
                    else {
                        output += 0;
                    }
                }
                Debug.Log(output);


            } else {
                Debug.Log("Queue is empty");

            }




        }



    }

}
