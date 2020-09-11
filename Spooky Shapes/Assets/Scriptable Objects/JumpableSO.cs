using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {TRI,RECT,CIRC };

[CreateAssetMenu(menuName = "Jump/Jumpable")]
public class JumpableSO : ScriptableObject
{
    public Type geoType;
    public float basePushForce;
    public float drag;


}
