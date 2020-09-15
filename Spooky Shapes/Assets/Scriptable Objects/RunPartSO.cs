using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Run/Part",fileName ="New RunPart")]
public class RunPartSO : ScriptableObject
{

    public GameObject runPartPrefab;
    public List<PieceCreatorSO> pieceC;
    public float width = 10;
    public float height = 22;
    [Range(0, 10)]
    public float difficulty;

    

    


    




}
