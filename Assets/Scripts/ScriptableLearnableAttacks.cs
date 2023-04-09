using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ScriptableLearnableAttacks : ScriptableObject
{
    public static List<LearnableAttackStruct> attacksToLearn = new List<LearnableAttackStruct>();

    public void AddLearnableAttack()
    {
        attacksToLearn.Add(new LearnableAttackStruct());
    }

    public void RemoveLearnableAttack(int i)
    {
        attacksToLearn.Remove(attacksToLearn[i]);
    }

    [System.Serializable]
    public struct LearnableAttackStruct
    {
        public Attack attack;
        public int level;
    }

    public List<Attack> attacks
    {
        get
        {
            List<Attack> attackListToReturn = new List<Attack>();
            foreach (LearnableAttackStruct i in attacksToLearn)
            {
                attackListToReturn.Add(i.attack);
            }
            return attackListToReturn;
        }
        set
        {
            attacks = value;
        }
    }

    public List<int> attackLevels
    {
        get
        {
            List<int> attackLevelListToReturn = new List<int>();
            foreach (LearnableAttackStruct i in attacksToLearn)
            {
                attackLevelListToReturn.Add(i.level);
            }
            return attackLevelListToReturn;
        }
    }


}
