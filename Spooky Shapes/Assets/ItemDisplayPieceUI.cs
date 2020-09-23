using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayPieceUI : MonoBehaviour
{
    InventoryDisplayUI IUI;
    public Image itemImage; 
    public Text itemCount;
    public string itemName;

    public void Initialize(Image img,int count,string name) {
        itemCount.text = "" + count;
        itemImage.sprite = img.sprite;
        itemName = name;

    }

    public void UpdateCount(int count,string check) {
        //Debug.Log("Update Count called.");
        if (itemName.Equals(check)) {
            //Debug.Log("Updating count on" + check);
            itemCount.text = "" + count;
        }
    }

    public void Listen(InventoryDisplayUI inv,string iName) {
        IUI = inv;
        IUI.UpdateCountEvent += UpdateCount;

    }




}
