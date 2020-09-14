using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Piece/PieceCreator",fileName ="New PieceCreator")]
public class PieceCreatorSO : ScriptableObject
{

    public GameObject prefab;
    public int numToGen;
    public float genCooldown;
    public float travelTime;
    public float nextGenCooldown;

    
}
