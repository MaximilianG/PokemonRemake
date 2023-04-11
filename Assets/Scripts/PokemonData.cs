using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPokemonData", menuName = "ScriptableObjects/PokemonData", order = 1)]
public class PokemonData : ScriptableObject
{
    public int Id;
    public string Name;
    public int Level;
    public int HealthPoints;
    public int Attack;
    public int Defense;
    public int Speed;
    public int SpecialAttack;
    public int SpecialDefense;
    public Sprite FrontSprite;
    public Sprite BackSprite;
    public Attack[] DefaultAttacks = new Attack[4];
    public List<ScriptableEvolves.EvolveStruct> Evolves = new List<ScriptableEvolves.EvolveStruct>();
    public List<ScriptableLearnableAttacks.LearnableAttackStruct> LearnableAttacks;
}
