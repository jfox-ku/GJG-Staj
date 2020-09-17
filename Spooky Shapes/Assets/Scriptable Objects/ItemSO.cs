using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {SIZE , BOUNCE, SAVER,};

[CreateAssetMenu(menuName = "Items/Item", fileName = "New Item")]
public class ItemSO : ScriptableObject
{
    public List<GameObject> itemPrefabs;
    public ItemType itemType;
    
}
