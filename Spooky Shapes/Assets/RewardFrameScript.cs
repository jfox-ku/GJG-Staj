using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardFrameScript : MonoBehaviour
{
    public ItemScript item;
    public Sprite itemSprite;

    public SpriteRenderer rewardSpriteRenderer;

    private RewardPartScript rewardPart;

    private void Awake() {
        rewardSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rewardPart = GetComponentInParent<RewardPartScript>();
    }

    
    public void giveItem() {
        //Debug.Log("giveItem called on " + this.name);
        rewardPart.GiveItemPlayer(item);

    }

    public void SetItem(ItemScript item) {
        //Debug.Log("Item set to" + item.itemName);
        itemSprite = item.itemSprite;
        this.item = item;

        rewardSpriteRenderer.sprite = itemSprite;
    }


}
