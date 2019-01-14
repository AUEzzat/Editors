using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Range,
    Melee,
    Magic
}

[System.Serializable]
public class Enemy {
    public int damage;
    public int range;
    public AttackType type;
    public string enemyName;
}
