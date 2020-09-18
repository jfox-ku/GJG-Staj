using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RunManagerScript : MonoBehaviour
{

    //Not sure yet if this should be a singleton
    //public static RunManagerScript _instance;

    public SetSO set;
    private GameObject player;
    public GameObject testingPiece;

    private CinemachineVirtualCamera CVM;
    private CinemachineConfiner CCF;

    public List<RunPartSO> RunParts;
    private int toLoadIndex;
    public int totalLoaded;
    public Vector2 spawnPos;

    [SerializeField] private Queue<GameObject> loadedRunParts;
    public bool allowLoad = true;
    



    public delegate void PartsDelegate();
    public PartsDelegate upTrigger;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Initialize() {

        player = GameObject.FindGameObjectWithTag("Player");
        Instantiate(testingPiece, new Vector2(0,-2f),Quaternion.identity);

        CVM = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        CVM.Follow = player.transform;

        loadedRunParts = new Queue<GameObject>();
        set.ResetItemCount();

        toLoadIndex = 0;
        SpawnPart();



    }


    //Also despawns pervious parts
   private void SpawnPart() {
        if(totalLoaded >= 6) {
            //Debug.Log("Loaded all Parts. Now getting random parts.");
            //allowLoad = false;
            toLoadIndex = Random.Range(0, RunParts.Count);


            if(player.transform.position.y > loadedRunParts.Peek().GetComponent<RunPartScript>().GetTopPos().y)
            Destroy(loadedRunParts.Dequeue());
        }

        if(toLoadIndex >= RunParts.Count) toLoadIndex = Random.Range(0, RunParts.Count);

        RunPartSO sp;
        if(totalLoaded > 4 && totalLoaded % 5 == 0) {
            sp = set.RewardPart;
        } else {
            sp = RunParts[toLoadIndex];
        }
        
        
        if(loadedRunParts.Count < 6) {

            var part = Instantiate(sp.runPartPrefab, spawnPos, Quaternion.identity);
            var partScript = part.GetComponent<RunPartScript>();
            part.transform.position += new Vector3(0, partScript.GetSpawnHeight(), 0);
            loadedRunParts.Enqueue(part);
            partScript.SetUpCreators();

            var reward = part.GetComponent<RewardPartScript>();
            if (reward != null) {
                reward.SetUpItems();
            }
            spawnPos.y = partScript.GetTopPos().y;
            toLoadIndex++;
            totalLoaded++;

        }
        


        
    }


    private int lastId = -1;
    public void LoadNextPart(int id) {
        //Instantiate(testingPiece, new Vector2(player.transform.position.x, player.transform.position.y-2f), Quaternion.identity);
        if (id != lastId) {
            lastId = id;
            //Debug.Log("Loading next run part");
            SpawnPart();
        }

        
    }

    public void LoadZoneTrigger(GameObject obje) {
        var parentRunPart = obje.transform.parent.GetComponent<RunPartScript>();
        //Debug.Log("Load Trigger ID: " + parentRunPart.id);
        //If different Part ID, load new part
        LoadNextPart(parentRunPart.id);


    }



}
