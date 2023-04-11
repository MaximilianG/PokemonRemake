using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ScriptableLearnableAttacks : ScriptableObject
{
    [SerializeField] private List<LearnableAttackStruct> LearnableAttacksList = new List<LearnableAttackStruct>();

    public void AddLearnableAttack()
    {
        LearnableAttacksList.Add(new LearnableAttackStruct());
    }

    public void RemoveLearnableAttack(int i)
    {
        LearnableAttacksList.Remove(LearnableAttacksList[i]);
    }

    public void RemoveLearnableAttack(LearnableAttackStruct _learnableAttackStruct)
    {
        LearnableAttacksList.Remove(_learnableAttackStruct);
    }

    [System.Serializable]
    public struct LearnableAttackStruct
    {
        public Attack attack;
        public int level;
    }

    public List<LearnableAttackStruct> LearnableAttacksStructList
    {
        get { return LearnableAttacksList; }
        set { LearnableAttacksList = value; }
    }

    public List<Attack> attacksList
    {
        get
        {
            List<Attack> attackListToReturn = new List<Attack>();
            foreach (LearnableAttackStruct learnableAttack in LearnableAttacksList)
            {
                attackListToReturn.Add(learnableAttack.attack);
            }
            return attackListToReturn;
        }
        set
        {
            attacksList = value;
        }
    }

    public List<int> attacksLevelsList
    {
        get
        {
            List<int> attackLevelListToReturn = new List<int>();
            foreach (LearnableAttackStruct learnableAttack in LearnableAttacksList)
            {
                attackLevelListToReturn.Add(learnableAttack.level);
            }
            return attackLevelListToReturn;
        }
    }
}
