using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryDisplayUI : MonoBehaviour
{
    public GameObject DisplayPrefab;
    public List<ItemDisplayPieceUI> PiecesList;
    [Range(0f,200f)]
    public float offset;

    public event Action<int,string> UpdateCountEvent;

    // Start is called before the first frame update
    void Awake()
    {
        PiecesList = new List<ItemDisplayPieceUI>();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player.InventoryEvent += UpdateInventory;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateInventory(List<ItemScript> ItemList) {
        //Debug.Log("Listened inventory change");
        
        foreach(ItemScript item in ItemList) {
            var found = false;

                for (int j = 0; j < PiecesList.Count; j++) {
                    if (PiecesList[j].itemName.Equals(item.itemName)) {
                        UpdateCountEvent?.Invoke((int)item.count,item.itemName);
                        //Debug.Log("Updating piece "+PiecesList[j].itemName+". Increasing count to "+item.count);
                        found = true;
                        break;
                    }
                
                
                }

            if (!found) {
               // Debug.Log("Piece not found. Creating new.");
                CreatePiece(item, this.transform.position + new Vector3(PiecesList.Count * offset, 0, 0));
            }
            





        }


    }





    private void OnDestroy() {
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player) player.GetComponent<PlayerScript>().InventoryEvent -= UpdateInventory;
    }

    void CreatePiece(ItemScript item,Vector2 pos) {
        //Debug.Log("Creating UI Piece for "+item.itemName+" at pos: "+pos);
        Vector2 positionNext = pos;
        var piece = Instantiate(DisplayPrefab,positionNext,Quaternion.identity,this.transform);
        var pieceScript = piece.GetComponent<ItemDisplayPieceUI>();
        PiecesList.Add(pieceScript);
        pieceScript.Initialize(item.itemImage,1,item.itemName);
        pieceScript.Listen(this,item.itemName);

    }

}
