using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchboostItem : ItemScript
{

    public Type pieceType;
    public int[] pieceCounts;
    public Sprite[] sprites;

    // Start is called before the first frame update
    private void Awake() {

        pieceType = GetRandomEnum<Type>();
        pieceCounts = new int[getEnumCount<Type>()];
        Debug.Log("Piece count: "+pieceCounts.Length);

        itemName = "matchBoostItem"+pieceType.ToString();
        

    }

    public void ChangeType(Type tip) {
        pieceType = tip;
        itemName = "matchBoostItem" + pieceType.ToString();
        itemSprite = sprites[(int)pieceType];
    }

    public override void OnPickUp() {
        itemName = "matchBoostItem" + pieceType.ToString();
        //Debug.Log("PieceType index: "+(int)pieceType);
        pieceCounts[(int)pieceType]++;
        Debug.Log(itemName + " was picked up. Currently has "+ pieceCounts[(int)pieceType]+" of this type "+pieceType);
        effector();
    }


    public override void effector() {
        if (count > 30) return;

        var player = GameObject.FindGameObjectWithTag("Player");
        var playerScript = player.GetComponent<PlayerScript>();

        playerScript.itemMultipliers[(int)pieceType] = GetItemMultiplier(pieceType);

    }

    public float GetItemMultiplier(Type tip) {
        return pieceCounts[(int)tip] * 0.1f;
    }

    public override void SetUpItem() {
        base.SetUpItem();
        pieceType = GetRandomEnum<Type>();
        itemName = "matchBoostItem" + pieceType.ToString();
        itemSprite = sprites[(int)pieceType];


    }

    public override void reset() {
        base.reset();
        pieceCounts = new int[getEnumCount<Type>()];

    }


    //Helper from stackoverflow :) Picks random enum, might need to put this is a static class
    static T GetRandomEnum<T>() {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
    }
    
    //used similar method to get total count in enum
    static int getEnumCount<T>() {
        System.Array A = System.Enum.GetValues(typeof(T));
        return A.Length;
    }



}
