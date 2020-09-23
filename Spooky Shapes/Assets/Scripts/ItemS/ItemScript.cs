using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class ItemScript : MonoBehaviour
{
    public Sprite itemSprite;
    public Image itemImage;
    public float count;
    public string itemName;

    public event Action CountEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetUpItem() {

    }

    public virtual void reset() {
        count = 0;
    }



    public virtual void effector() {
        Debug.Log("Item has no effect.");
    }


    public void AddItem() {
        count++;
    }

    public virtual void OnPickUp() {

    }

    public void SetSprites() {
        itemSprite = itemImage.sprite;

    }

}
