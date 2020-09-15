﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Set/Basic Set", fileName = "New Set")]
public class SetSO : ScriptableObject
{
    //public List<GameObject> CreatorsLst;
    public List<GameObject> JumpablesLst;

    public Color BGstartColor;
    public Color BGendColor;
    public float colorSwapDistanceMax;

    public GameObject getRandomJumpable() {
        return JumpablesLst[Random.Range(0,JumpablesLst.Count)];

    }


}
