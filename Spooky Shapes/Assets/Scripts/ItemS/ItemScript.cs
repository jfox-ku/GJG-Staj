using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScript : MonoBehaviour
{
    public Sprite itemSprite;

    public float count;
    public string itemName;
    
    // Start is called before the first frame update
    void Start()
    {
        count = 1;
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


}
