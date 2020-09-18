using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardFrameScript : MonoBehaviour
{
    public ItemScript item;
    private Type tip;
    public Sprite itemSprite;

    public SpriteRenderer rewardSpriteRenderer;

    private RewardPartScript rewardPart;

    private void Awake() {
        rewardSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rewardPart = GetComponentInParent<RewardPartScript>();
    }


    //Ok this is weird. I'm doing this because I made the items only 1 script and I couldn't keep the frames type in that item. 
    //I needed something else to remember the type, so its the frame. Yet, while passing it in
    //i change the original item.
    //definetely not the best way of doing this.
    public void giveItem() {
        //Debug.Log("giveItem called on " + this.name);
        var reward = item.gameObject.GetComponent<MatchboostItem>();
        if (reward != null) {
            reward.ChangeType(this.tip);
        }
        rewardPart.GiveItemPlayer(item);

    }


    //Remember item type while setting it!
    public void SetItem(ItemScript item) {
        //Debug.Log("Item set to" + item.itemName);
        
        itemSprite = item.itemSprite;
        this.item = item;
        var reward = item.gameObject.GetComponent<MatchboostItem>();
        if (reward != null) {
            this.tip = reward.pieceType;
        }
        

        rewardSpriteRenderer.sprite = itemSprite;
    }


}
