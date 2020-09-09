using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {triangle, circle, square};

[CreateAssetMenu(menuName = "Enemy Base")]
public class EnemyScriptable : ScriptableObject
{

    public Sprite sprite;
    public int health;
    public Type typ; 

}
