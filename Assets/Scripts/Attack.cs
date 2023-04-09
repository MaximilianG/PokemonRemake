using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttack", menuName = "ScriptableObjects/Attack", order = 2)]
public class Attack : ScriptableObject
{
    public string Name;
    public string Description;
    public float AttackRatio;
}
